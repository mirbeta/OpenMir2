using System;
using System.IO;
using SystemModule.Common;

namespace GameGate.Filters
{
    public class ChatCommandFilter
    {
        public const string CHATCMDFILTERFILE = "CharCmdFilter.txt";
        private string ChatCommandFile;
        private static readonly ChatCommandFilter instance = new ChatCommandFilter();

        public static ChatCommandFilter Instance
        {
            get { return instance; }
        }

        public ChatCommandFilter()
        {
           
        }

        public void LoadChatCommandFilterList()
        {
            ChatCommandFile = Path.Combine(AppContext.BaseDirectory, "ChatFilter.txt");
            LoadChatCmdFilterList();
        }

        void LoadChatCmdFilterList()
        {
            var loadList = new StringList();
            if (!File.Exists(ChatCommandFile))
            {
                loadList.SaveToFile(ChatCommandFile);
                return;
            }
            GateShare.ChatCommandFilterMap.Clear();
            loadList.LoadFromFile(ChatCommandFile);
            for (int i = 0; i < loadList.Count; i++)
            {
                GateShare.ChatCommandFilterMap.TryAdd(loadList[i], 0);
            }
        }
    }
}
