using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using SystemModule;
using SystemModule.Common;

namespace LoginSvr
{
    public class ThreadParseList
    {
        private readonly LogQueue _logQueue;
        private StringList AccountLoadList = null;
        private StringList IPaddrLoadList = null;
        private IList<AccountConst> AccountCostList = null;
        private IList<AccountConst> IPaddrCostList = null;
        private readonly LoginService _loginService;
        private readonly ConfigManager _configManager;
        private int _threadExecuteTick = 0;

        public ThreadParseList(LogQueue logQueue, LoginService loginService, ConfigManager configManager)
        {
            _logQueue = logQueue;
            _loginService = loginService;
            _configManager = configManager;
            AccountLoadList = new StringList();
            IPaddrLoadList = new StringList();
            AccountCostList = new List<AccountConst>();
            IPaddrCostList = new List<AccountConst>();
        }

        public void Execute()
        {
            if ((HUtil32.GetTickCount() - _threadExecuteTick) > 5 * 60 * 1000)
            {
                _threadExecuteTick = HUtil32.GetTickCount();
                string s18 = String.Empty;
                string s1C = String.Empty;
                string s24 = String.Empty;
                string s28 = String.Empty;
                int nC;
                int n10;
                int n14;
                Config Config = _configManager.Config;
                try
                {
                    if (File.Exists(Config.sFeedIDList))
                    {
                        AccountLoadList.Clear();
                        AccountLoadList.LoadFromFile(Config.sFeedIDList);
                        if (AccountLoadList.Count > 0)
                        {
                            AccountCostList.Clear();
                            for (var i = 0; i < AccountLoadList.Count; i++)
                            {
                                s18 = AccountLoadList[i].Trim();
                                s18 = HUtil32.GetValidStr3(s18, ref s1C, new string[] { " ", "\09" });//sid
                                s18 = HUtil32.GetValidStr3(s18, ref s24, new string[] { " ", "\09" });//sday
                                s18 = HUtil32.GetValidStr3(s18, ref s28, new string[] { " ", "\09" });//shour
                                n10 = HUtil32.Str_ToInt(s24, 0);
                                n14 = HUtil32.Str_ToInt(s28, 0);
                                nC = HUtil32.MakeLong(HUtil32._MAX(n14, 0), HUtil32._MAX(n10, 0));
                                _loginService.LoadAccountCostList(Config, new AccountConst(s1C, nC));
                                if ((i % 100) == 0)
                                {
                                    Thread.Sleep(1);
                                }
                            }
                        }
                    }
                }
                catch
                {
                    _logQueue.Enqueue("Exception] loading on IDStrList.");
                }
                try
                {
                    if (File.Exists(Config.sFeedIPList))
                    {
                        IPaddrLoadList.Clear();
                        IPaddrLoadList.LoadFromFile(Config.sFeedIPList);
                        if (IPaddrLoadList.Count > 0)
                        {
                            IPaddrCostList.Clear();
                            for (var i = 0; i < IPaddrLoadList.Count; i++)
                            {
                                s18 = IPaddrLoadList[i].Trim();
                                s18 = HUtil32.GetValidStr3(s18, ref s1C, new string[] { " ", "\09" });
                                s18 = HUtil32.GetValidStr3(s18, ref s24, new string[] { " ", "\09" });
                                s18 = HUtil32.GetValidStr3(s18, ref s28, new string[] { " ", "\09" });
                                n10 = HUtil32.Str_ToInt(s24, 0);
                                n14 = HUtil32.Str_ToInt(s28, 0);
                                nC = HUtil32.MakeLong(HUtil32._MAX(n14, 0), HUtil32._MAX(n10, 0));
                                _loginService.LoadIPaddrCostList(Config, new AccountConst(s1C, nC));
                                if ((i % 100) == 0)
                                {
                                    Thread.Sleep(1);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logQueue.Enqueue("Exception] loading on IPStrList.");
                    _logQueue.Enqueue(ex.StackTrace);
                }
            }
        }
    }
}