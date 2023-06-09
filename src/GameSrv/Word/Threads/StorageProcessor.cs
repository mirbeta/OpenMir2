using GameSrv.Services;
using M2Server;
using NLog;
using SystemModule.Data;

namespace GameSrv.Word.Threads
{
    public class StorageProcessor : TimerScheduledService
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public StorageProcessor() : base(TimeSpan.FromMilliseconds(500), "StorageProcessor")
        {

        }

        public override void Initialize(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        protected override void Startup(CancellationToken stoppingToken)
        {
            _logger.Info("玩家数据读写线程启动...");
        }

        protected override void Stopping(CancellationToken stoppingToken)
        {
            _logger.Info("玩家数据读写线程停止...");
        }

        protected override Task ExecuteInternal(CancellationToken stoppingToken)
        {
            const string sExceptionMsg = "[Exception] StorageProcessor::ExecuteInternal";
            try
            {
                //M2Share.FrontEngine.ProcessGameDate();
                //if (!M2Share.DataServer.IsConnected && M2Share.FrontEngine.m_SaveRcdList.Count > 0)
                //{
                //    _logger.Error("DBServer 断开链接，保存玩家数据失败.");
                //    HUtil32.EnterCriticalSection(M2Share.FrontEngine.UserCriticalSection);
                //    try
                //    {
                //        for (int i = 0; i < M2Share.FrontEngine.m_SaveRcdList.Count; i++)
                //        {
                //            if (M2Share.FrontEngine.m_SaveRcdList[i] != null)
                //            {
                //                M2Share.FrontEngine.m_SaveRcdList[i] = null;
                //            }
                //        }
                //    }
                //    finally
                //    {
                //        HUtil32.LeaveCriticalSection(M2Share.FrontEngine.UserCriticalSection);
                //    }
                //    M2Share.FrontEngine.m_SaveRcdList.Clear();
                //}
                //else
                //{
                //    ProcessReadStorage();
                //    ProcessSaveStorage();
                //}
            }
            catch (Exception ex)
            {
                M2Share.Logger.Error(sExceptionMsg);
                M2Share.Logger.Error(ex.StackTrace);
            }
            return Task.CompletedTask;
        }

        private static void ProcessSaveStorage()
        {
            //for (var i = 0; i < M2Share.FrontEngine.m_SaveRcdTempList.Count; i++)
            //{
            //    SavePlayerRcd saveRcd = M2Share.FrontEngine.m_SaveRcdTempList[i];
            //    if (saveRcd == null)
            //    {
            //        continue;
            //    }
            //    if (saveRcd.IsSaveing)
            //    {
            //        continue;
            //    }
            //    saveRcd.IsSaveing = true;
            //    if (!PlayerDataService.SaveHumRcdToDB(saveRcd, ref saveRcd.QueryId) || saveRcd.ReTryCount > 50)
            //    {
            //        saveRcd.ReTryCount++;
            //    }
            //    else
            //    {
            //        if (saveRcd.PlayObject != null)
            //        {
            //            ((PlayObject)saveRcd.PlayObject).RcdSaved = true;
            //        }
            //    }
            //}
            //M2Share.FrontEngine.m_SaveRcdTempList.Clear();
            //PlayerDataService.ProcessSaveQueue();
        }

        private void ProcessReadStorage()
        {
            //bool boReTryLoadDb = false;
            //for (int i = 0; i < M2Share.FrontEngine.m_LoadRcdTempList.Count; i++)
            //{
            //    LoadDBInfo loadDbInfo = M2Share.FrontEngine.m_LoadRcdTempList[i];
            //    if (loadDbInfo.SessionID == 0)
            //    {
            //        continue;
            //    }
            //    if (!LoadPlayerFromDB(loadDbInfo, ref boReTryLoadDb))
            //    {
            //        GameShare.SocketMgr.CloseUser(loadDbInfo.GateIdx, loadDbInfo.SocketId);
            //        _logger.Debug("读取用户数据失败，踢出用户.");
            //    }
            //    else
            //    {
            //        if (boReTryLoadDb)// 如果读取人物数据失败(数据还没有保存),则重新加入队列
            //        {
            //            HUtil32.EnterCriticalSection(M2Share.FrontEngine.UserCriticalSection);
            //            try
            //            {
            //                M2Share.FrontEngine.m_LoadRcdList.Add(loadDbInfo);
            //            }
            //            finally
            //            {
            //                HUtil32.LeaveCriticalSection(M2Share.FrontEngine.UserCriticalSection);
            //            }
            //        }
            //    }
            //}
            //M2Share.FrontEngine.m_LoadRcdTempList.Clear();
            //PlayerDataService.ProcessQueryQueue();
        }

        private static bool LoadPlayerFromDB(LoadDBInfo loadUser, ref bool boReTry)
        {
            int queryId = 0;
            bool result = false;
            boReTry = false;
            //if (M2Share.FrontEngine.InSaveRcdList(loadUser.ChrName))
            //{
            //    boReTry = true;// 反回TRUE,则重新加入队列
            //    return false;
            //}
            /*if (M2Share.WorldEngine.GetPlayObjectEx(loadUser.ChrName) != null)
            {
                M2Share.WorldEngine.KickPlayObjectEx(loadUser.ChrName);
                boReTry = true;// 反回TRUE,则重新加入队列
                return false;
            }*/
            if (!PlayerDataService.LoadHumRcdFromDB(loadUser.Account, loadUser.ChrName, loadUser.sIPaddr, ref queryId, loadUser.SessionID))
            {
                GameShare.SocketMgr.SendOutConnectMsg(loadUser.GateIdx, loadUser.SocketId, loadUser.GSocketIdx);
            }
            else
            {
                UserOpenInfo userOpenInfo = new UserOpenInfo
                {
                    ChrName = loadUser.ChrName,
                    LoadUser = loadUser,
                    HumanRcd = null,
                    QueryId = queryId
                };
                //M2Share.WorldEngine.AddUserOpenInfo(userOpenInfo);
                result = true;
            }
            return result;
        }

    }
}