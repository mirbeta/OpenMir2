using M2Server.RobotPlay;
using SystemModule;
using SystemModule.Data;
using SystemModule.Enums;

namespace M2Server.World
{
    public partial class WorldServer
    {
        public void AddRobotLogon(RoBotLogon ai)
        {
            RobotLogonQueue.Enqueue(ai);
        }

        private void RegenRobotPlayer(RoBotLogon ai)
        {
            var playObject = CreateRobotPlayObject(ai);
            if (playObject != null)
            {
                playObject.HomeMap = GetHomeInfo(ref playObject.HomeX, ref playObject.HomeY);
                playObject.MapFileName = playObject.HomeMap;
                playObject.UserAccount = "假人" + ai.sChrName;
                playObject.Start(FindPathType.Dynamic);
                BotPlayObjectList.Add(playObject);
            }
        }
        
        public void ProcessRobotPlayData()
        {
            const string sExceptionMsg = "[Exception] WorldServer::ProcessRobotPlayData";
            //人工智障开始登陆
            if (RobotLogonQueue.Count > 0)
            {
                if (HUtil32.GetTickCount() - RobotLogonTick > 1000)
                {
                    RobotLogonTick = HUtil32.GetTickCount();
                    if (RobotLogonQueue.Count > 0)
                    {
                        RegenRobotPlayer(RobotLogonQueue.Dequeue());
                    }
                }
            }
            try
            {
                var dwCurTick = HUtil32.GetTickCount();
                var nIdx = ProcBotHubIdx;
                var boCheckTimeLimit = false;
                var dwCheckTime = HUtil32.GetTickCount();
                while (true)
                {
                    if (BotPlayObjectList.Count <= nIdx) break;
                    var robotPlayer = BotPlayObjectList[nIdx];
                    if (dwCurTick - robotPlayer.RunTick > robotPlayer.RunTime)
                    {
                        robotPlayer.RunTick = dwCurTick;
                        if (!robotPlayer.Ghost)
                        {
                            if (!robotPlayer.LoginNoticeOk)
                            {
                                robotPlayer.RunNotice();
                            }
                            else
                            {
                                if (!robotPlayer.BoReadyRun)
                                {
                                    robotPlayer.BoReadyRun = true;
                                    robotPlayer.UserLogon();
                                }
                                else
                                {
                                    if ((HUtil32.GetTickCount() - robotPlayer.SearchTick) > robotPlayer.SearchTime)
                                    {
                                        robotPlayer.SearchTick = HUtil32.GetTickCount();
                                        robotPlayer.SearchViewRange();
                                        robotPlayer.GameTimeChanged();
                                    }
                                    robotPlayer.Run();
                                }
                            }
                        }
                        else
                        {
                            BotPlayObjectList.Remove(robotPlayer);
                            robotPlayer.Disappear();
                            //AddToHumanFreeList(robotPlayer);
                            //robotPlayer.DealCancelA();
                            //SaveHumanRcd(robotPlayer);
                            GameShare.SocketMgr.CloseUser(robotPlayer.GateIdx, robotPlayer.SocketId);
                            SendServerGroupMsg(Messages.SS_202, M2Share.ServerIndex, robotPlayer.ChrName);
                            continue;
                        }
                    }
                    nIdx++;
                    if ((HUtil32.GetTickCount() - dwCheckTime) > M2Share.HumLimit)
                    {
                        boCheckTimeLimit = true;
                        ProcBotHubIdx = nIdx;
                        break;
                    }
                }
                if (!boCheckTimeLimit) ProcBotHubIdx = 0;
            }
            catch (Exception ex)
            {
                _logger.Error(sExceptionMsg);
                _logger.Error(ex.StackTrace);
            }
        }
        
        private static RobotPlayer CreateRobotPlayObject(RoBotLogon ai)
        {
            var envirnoment = GameShare.MapMgr.FindMap(ai.sMapName);
            if (envirnoment == null)
            {
                return null;
            }
            var cert = new RobotPlayer();
            cert.Envir = envirnoment;
            cert.MapName = ai.sMapName;
            cert.CurrX = ai.nX;
            cert.CurrY = ai.nY;
            cert.Dir = (byte)M2Share.RandomNumber.Random(8);
            cert.ChrName = ai.sChrName;
            cert.WAbil = cert.Abil;
            if (M2Share.RandomNumber.Random(100) < cert.CoolEyeCode)
            {
                cert.CoolEye = true;
            }
            //Cert.m_sIPaddr = GetIPAddr;// Mac问题
            //Cert.m_sIPLocal = GetIPLocal(Cert.m_sIPaddr);
            cert.ConfigFileName = ai.sConfigFileName;
            cert.FilePath = ai.sFilePath;
            cert.ConfigListFileName = ai.sConfigListFileName;
            cert.HeroConfigListFileName = ai.sHeroConfigListFileName;// 英雄配置列表目录
            cert.Initialize();
            cert.RecalcLevelAbilitys();
            cert.RecalcAbilitys();
            cert.Abil.HP = cert.Abil.MaxHP;
            cert.Abil.MP = cert.Abil.MaxMP;
            if (cert.AddtoMapSuccess)
            {
                bool mapSuccess = false;
                int n20;
                if (cert.Envir.Width < 50)
                {
                    n20 = 2;
                }
                else
                {
                    n20 = 3;
                }
                int n24;
                if (cert.Envir.Height < 250)
                {
                    if (cert.Envir.Height < 30)
                    {
                        n24 = 2;
                    }
                    else
                    {
                        n24 = 20;
                    }
                }
                else
                {
                    n24 = 50;
                }
                var n1C = 0;
                while (true)
                {
                    if (!cert.Envir.CanWalk(cert.CurrX, cert.CurrY, false))
                    {
                        if ((cert.Envir.Width - n24 - 1) > cert.CurrX)
                        {
                            cert.CurrX += (short)n20;
                        }
                        else
                        {
                            cert.CurrX = (byte)(M2Share.RandomNumber.Random(cert.Envir.Width / 2) + n24);
                            if (cert.Envir.Height - n24 - 1 > cert.CurrY)
                            {
                                cert.CurrY += (short)n20;
                            }
                            else
                            {
                                cert.CurrY = (byte)(M2Share.RandomNumber.Random(cert.Envir.Height / 2) + n24);
                            }
                        }
                    }
                    else
                    {
                        mapSuccess = cert.Envir.AddMapObject(cert.CurrX, cert.CurrY, cert.CellType, cert.ActorId, cert);
                        break;
                    }
                    n1C++;
                    if (n1C >= 31)
                    {
                        break;
                    }
                }
                if (!mapSuccess)
                {
                    cert = null;
                }
            }
            return cert;
        }
    }
}
