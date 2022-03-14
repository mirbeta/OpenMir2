using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SystemModule;

namespace LoginSvr
{
    public class TimedService : BackgroundService
    {
        private readonly LogQueue _logQueue;
        private readonly LoginService _loginService;
        private readonly MonSocService _monSocService;
        private readonly ThreadParseList _threadParseList;
        private readonly MasSocService _massocService;
        private int _processMonSocTick = 0;
        private int _processServerStatuTick = 0;

        public TimedService(LogQueue logQueue, LoginService loginService, ThreadParseList threadParseList, 
            MonSocService monSocService, MasSocService massocService)
        {
            _logQueue = logQueue;
            _loginService = loginService;
            _threadParseList = threadParseList;
            _monSocService = monSocService;
            _massocService = massocService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                OutMianMessage();
                LoginProcess();
                ProcessMonSoc();
                CheckServerStatus();
                _threadParseList.Execute();
                await Task.Delay(TimeSpan.FromMilliseconds(10), stoppingToken);
            }
        }

        private void OutMianMessage()
        {
            while (!_logQueue.MessageLog.IsEmpty)
            {
                string message;

                if (!_logQueue.MessageLog.TryDequeue(out message)) continue;

                Console.WriteLine(message);
            }

            while (!_logQueue.DebugLog.IsEmpty)
            {
                string message;

                if (!_logQueue.DebugLog.TryDequeue(out message)) continue;

                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine(message);
                Console.ResetColor();
            }
        }

        private void LoginProcess()
        {
            _loginService.SessionClearKick();
            _loginService.SessionClearNoPayMent();
        }

        private void ProcessMonSoc()
        {
            if (HUtil32.GetTickCount() - _processMonSocTick > 20000)
            {
                _processMonSocTick = HUtil32.GetTickCount();
                _monSocService.ProcessCleanSession();
            }
        }

        private void CheckServerStatus()
        {
            if (HUtil32.GetTickCount() - _processServerStatuTick > 20000)
            {
                _processServerStatuTick = HUtil32.GetTickCount();
                IList<TMsgServerInfo> ServerList = _massocService.ServerList;
                if (!ServerList.Any())
                {
                    return;
                }
                for (var i = 0; i < ServerList.Count; i++)
                {
                    TMsgServerInfo MsgServer = ServerList[i];
                    string sServerName = MsgServer.sServerName;
                    if (!string.IsNullOrEmpty(sServerName))
                    {
                        var tickTime = HUtil32.GetTickCount() - MsgServer.dwKeepAliveTick;
                        if (tickTime <= 60000) continue;
                        MsgServer.Socket.Close();
                        if (MsgServer.nServerIndex == 99)
                        {
                            if (string.IsNullOrEmpty(sServerName))
                            {
                                _logQueue.Enqueue($"数据库服务器[{MsgServer.sIPaddr}]响应超时,关闭链接.");
                            }
                            else
                            {
                                _logQueue.Enqueue($"[{sServerName}]数据库服务器响应超时,关闭链接.");
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(sServerName))
                            {
                                _logQueue.Enqueue($"游戏服务器[{MsgServer.sIPaddr}]响应超时,关闭链接.");
                            }
                            else
                            {
                                _logQueue.Enqueue($"[{sServerName}]游戏服务器响应超时,关闭链接.");
                            }
                        }
                    }
                }
            }
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}