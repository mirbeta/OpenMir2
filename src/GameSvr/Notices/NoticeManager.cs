using SystemModule.Common;

namespace GameSvr
{
    public class NoticeManager
    {
        private readonly NoticeMsg[] NoticeList = new NoticeMsg[100];

        public NoticeManager()
        {
            for (var i = NoticeList.GetLowerBound(0); i <= NoticeList.GetUpperBound(0); i++)
            {
                NoticeList[i].sMsg = string.Empty;
                NoticeList[i].sList = null;
                NoticeList[i].bo0C = true;
            }
        }

        public void LoadingNotice()
        {
            for (var i = NoticeList.GetLowerBound(0); i <= NoticeList.GetUpperBound(0); i++)
            {
                if (NoticeList[i].sMsg == "")
                {
                    continue;
                }
                var fileName = Path.Combine(M2Share.sConfigPath, M2Share.g_Config.sNoticeDir, $"{NoticeList[i].sMsg}.txt");
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
                    M2Share.ErrorMessage("Error in loading notice text. file name is " + fileName);
                }
            }
        }

        public void GetNoticeMsg(string sStr, IList<string> LoadList)
        {
            var bo15 = true;
            for (var i = NoticeList.GetLowerBound(0); i <= NoticeList.GetUpperBound(0); i++)
            {
                if (string.Compare(NoticeList[i].sMsg, sStr, StringComparison.OrdinalIgnoreCase) != 0) continue;
                if (NoticeList[i].sList != null)
                {
                    for (var j = 0; j < NoticeList[i].sList.Count; j++)
                    {
                        LoadList.Add(NoticeList[i].sList[j]);
                    }
                }
                bo15 = false;
            }
            if (!bo15)
            {
                return;
            }
            for (var i = NoticeList.GetLowerBound(0); i <= NoticeList.GetUpperBound(0); i++)
            {
                if (string.IsNullOrEmpty(NoticeList[i].sMsg))
                {
                    var fileName = Path.Combine(M2Share.sConfigPath, M2Share.g_Config.sNoticeDir, sStr + ".txt");
                    if (File.Exists(fileName))
                    {
                        try
                        {
                            if (NoticeList[i].sList == null)
                            {
                                NoticeList[i].sList = new StringList();
                            }
                            NoticeList[i].sList.LoadFromFile(fileName, true);
                            for (var j = 0; j < NoticeList[i].sList.Count; j++)
                            {
                                LoadList.Add(NoticeList[i].sList[j]);
                            }
                        }
                        catch (Exception)
                        {
                            M2Share.ErrorMessage("Error in loading notice text. file name is " + fileName);
                        }
                        NoticeList[i].sMsg = sStr;
                        break;
                    }
                }
            }
        }
    }
}