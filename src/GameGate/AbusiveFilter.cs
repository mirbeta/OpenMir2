using System;
using System.IO;
using SystemModule.Common;

namespace GameGate
{
    public class AbusiveFilter
    {
        private readonly StringList AbuseList;
        private readonly ConfigManager _configManager;

        public AbusiveFilter(ConfigManager configManager)
        {
            _configManager = configManager;
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
                    switch (_configManager.GateConfig.m_tChatFilterMethod)
                    {
                        case  TChatFilterMethod.ctDropconnect:
                            kick = false;
                            break;
                        case TChatFilterMethod.ctReplaceAll:
                            chatMsg = _configManager.GateConfig.m_szChatFilterReplace;
                            break;
                            case TChatFilterMethod.ctReplaceOne:
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