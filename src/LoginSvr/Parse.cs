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
        private StringList AccountLoadList = null;
        private StringList IPaddrLoadList = null;
        private IList<AccountConst> AccountCostList = null;
        private IList<AccountConst> IPaddrCostList = null;
        private readonly Thread _parseThread;
        private readonly LoginService _loginSvr;

        public ThreadParseList(LoginService loginSvr)
        {
            AccountLoadList = new StringList();
            IPaddrLoadList = new StringList();
            AccountCostList = new List<AccountConst>();
            IPaddrCostList = new List<AccountConst>();
            _loginSvr = loginSvr;
            _parseThread = new Thread(Execute);
            _parseThread.IsBackground = true;
            _parseThread.Start();
        }

        private void Execute(object obj)
        {
            string s18 = String.Empty;
            string s1C = String.Empty;
            string s24 = String.Empty;
            string s28 = String.Empty;
            int nC;
            int n10;
            int n14;
            TConfig Config = LSShare.g_Config;
            int dwTick2C = 0;
            while (true)
            {
                if ((HUtil32.GetTickCount() - dwTick2C) > 5 * 60 * 1000)
                {
                    dwTick2C = HUtil32.GetTickCount();
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
                                    _loginSvr.LoadAccountCostList(Config, new AccountConst(s1C, nC));
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
                        LSShare.MainOutMessage("Exception] loading on IDStrList.");
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
                                    _loginSvr.LoadIPaddrCostList(Config, new AccountConst(s1C, nC));
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
                        LSShare.MainOutMessage("Exception] loading on IPStrList.");
                    }
                }
                Thread.Sleep(10);
                if (!_parseThread.IsAlive)
                {
                    break;
                }
            }
        }
    }
}