using CloudGate.Conf;
using System;
using System.IO;
using SystemModule.Common;

namespace CloudGate
{
    public class AbusiveFilter
    {
        private readonly StringList AbuseList;
        private static readonly AbusiveFilter instance = new AbusiveFilter();
        private ConfigManager _configManager => ConfigManager.Instance;

        public static AbusiveFilter Instance
        {
            get { return instance; }
        }

        public AbusiveFilter()
        {
            AbuseList = new StringList();
        }

        public void LoadChatFilterList()
        {
            AbuseList.LoadFromFile(Path.Combine(AppContext.BaseDirectory, "ChatFilter.txt"));
        }

        public byte CheckChatFilter(ref string chatMsg, ref bool kick)
        {
            var rplaceCount = 0;
            for (int i = 0; i < AbuseList.Count; i++)
            {
                if (AbuseList[i].Contains(chatMsg))
                {
                    switch (_configManager.GateConfig.ChatFilterMethod)
                    {
                        case TChatFilterMethod.Dropconnect:
                            kick = false;
                            break;
                        case TChatFilterMethod.ReplaceAll:
                            chatMsg = _configManager.GateConfig.m_szChatFilterReplace;
                            break;
                        case TChatFilterMethod.ReplaceOne:
                            var szRplace = string.Empty;
                            for (int j = 0; j < AbuseList[i].Length; j++)
                            {
                                szRplace = szRplace + "*";
                                chatMsg = chatMsg.Replace(chatMsg, szRplace);
                                rplaceCount++;
                                if (rplaceCount > 4)
                                {
                                    break;
                                }
                            }
                            break;
                    }
                }
            }
            return 0;
        }
    }
}