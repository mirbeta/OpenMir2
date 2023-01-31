using SystemModule.Common;
using SystemModule.Data;

namespace GameSvr.Notices
{
    public class NoticeManager
    {
        private readonly NoticeMsg[] NoticeList = new NoticeMsg[100];

        public NoticeManager()
        {
            for (int i = 0; i < NoticeList.Length; i++)
            {
                NoticeList[i].sMsg = string.Empty;
                NoticeList[i].sList = null;
            }
        }

        public void LoadingNotice()
        {
            for (int i = 0; i < NoticeList.Length; i++)
            {
                if (string.IsNullOrEmpty(NoticeList[i].sMsg))
                {
                    continue;
                }
                string fileName = Path.Combine(M2Share.BasePath, M2Share.Config.NoticeDir, $"{NoticeList[i].sMsg}.txt");
                if (!File.Exists(fileName)) continue;
                try
                {
                    if (NoticeList[i].sList == null)
                    {
                        NoticeList[i].sList = new StringList();
                    }
                    NoticeList[i].sList.LoadFromFile(fileName);
                }
                catch
                {
                    M2Share.Logger.Error("Error in loading notice text. file name is " + fileName);
                }
            }
        }

        public void GetNoticeMsg(string sStr, IList<string> LoadList)
        {
            bool success = true;
            for (int i = 0; i < NoticeList.Length; i++)
            {
                if (string.Compare(NoticeList[i].sMsg, sStr, StringComparison.OrdinalIgnoreCase) != 0) continue;
                if (NoticeList[i].sList != null)
                {
                    for (int j = 0; j < NoticeList[i].sList.Count; j++)
                    {
                        LoadList.Add(NoticeList[i].sList[j]);
                    }
                }
                success = false;
            }
            if (!success)
            {
                return;
            }
            for (int i = 0; i < NoticeList.Length; i++)
            {
                if (string.IsNullOrEmpty(NoticeList[i].sMsg))
                {
                    string fileName = Path.Combine(M2Share.BasePath, M2Share.Config.NoticeDir, sStr + ".txt");
                    if (File.Exists(fileName))
                    {
                        try
                        {
                            if (NoticeList[i].sList == null)
                            {
                                NoticeList[i].sList = new StringList();
                            }
                            NoticeList[i].sList.LoadFromFile(fileName, true);
                            for (int j = 0; j < NoticeList[i].sList.Count; j++)
                            {
                                LoadList.Add(NoticeList[i].sList[j]);
                            }
                        }
                        catch (Exception)
                        {
                            M2Share.Logger.Error("Error in loading notice text. file name is " + fileName);
                        }
                        NoticeList[i].sMsg = sStr;
                        break;
                    }
                }
            }
        }
    }
}