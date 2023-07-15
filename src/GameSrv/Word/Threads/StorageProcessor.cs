using GameSrv.Services;
using M2Server;
using NLog;
using SystemModule;
using SystemModule.Data;

namespace GameSrv.Word.Threads
{
    public class StorageProcessor : TimerScheduledService
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly object UserCriticalSection = new object();

        public StorageProcessor() : base(TimeSpan.FromMilliseconds(500), "StorageProcessor")
        {

        }

        public override void Initialize(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        protected override void Startup(CancellationToken stoppingToken)
        {
            _logger.Info("玩家数据数据线程启动...");
        }

        protected override void Stopping(CancellationToken stoppingToken)
        {
            _logger.Info("玩家数据数据线程停止...");
        }

        protected override Task ExecuteInternal(CancellationToken stoppingToken)
        {
            const string sExceptionMsg = "[Exception] StorageProcessor::ExecuteInternal";
            try
            {
                M2Share.FrontEngine.ProcessGameDate();
                var saveRcdList = M2Share.FrontEngine.GetSaveRcdList();
                if (!GameShare.DataServer.IsConnected && saveRcdList.Count > 0)
                {
                    _logger.Error("DBServer 断开链接，保存玩家数据失败.");
                    HUtil32.EnterCriticalSection(UserCriticalSection);
                    try
                    {
                        M2Share.FrontEngine.ClearSaveList();
                    }
                    finally
                    {
                        HUtil32.LeaveCriticalSection(UserCriticalSection);
                    }
                }
                else
                {
                    ProcessReadStorage();
                    ProcessSaveStorage();
                }
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
            var saveRcdTempList = M2Share.FrontEngine.GetTempSaveRcdList();
            for (var i = 0; i < saveRcdTempList.Count; i++)
            {
                var saveRcd = saveRcdTempList[i];
                if (saveRcd == null)
                {
                    continue;
                }
                if (saveRcd.IsSaveing)
                {
                    continue;
                }
                saveRcd.IsSaveing = true;
                if (!CharacterDataService.SaveHumRcdToDB(saveRcd, ref saveRcd.QueryId) || saveRcd.ReTryCount > 50)
                {
                    saveRcd.ReTryCount++;
                }
                else
                {
                    if (saveRcd.PlayObject != null)
                    {
                        saveRcd.PlayObject.RcdSaved = true;
                    }
                }
            }
            M2Share.FrontEngine.ClearSaveRcdTempList();
            CharacterDataService.ProcessSaveQueue();
        }

        private void ProcessReadStorage()
        {
            var boReTryLoadDb = false;
            var loadRcdTempList = M2Share.FrontEngine.GetLoadTempList();
            for (var i = 0; i < loadRcdTempList.Count; i++)
            {
                var loadDbInfo = loadRcdTempList[i];
                if (loadDbInfo.SessionID == 0)
                {
                    continue;
                }
                if (!LoadCharacterData(loadDbInfo, ref boReTryLoadDb))
                {
                    M2Share.NetChannel.CloseUser(loadDbInfo.GateIdx, loadDbInfo.SocketId);
                    _logger.Debug("读取用户数据失败，踢出用户.");
                }
                else
                {
                    if (boReTryLoadDb)// 如果读取人物数据失败(数据还没有保存),则重新加入队列
                    {
                        HUtil32.EnterCriticalSection(UserCriticalSection);
                        try
                        {
                            M2Share.FrontEngine.AddToLoadRcdList(loadDbInfo);
                        }
                        finally
                        {
                            HUtil32.LeaveCriticalSection(UserCriticalSection);
                        }
                    }
                }
            }
            M2Share.FrontEngine.ClearLoadRcdTempList();
            CharacterDataService.ProcessQueryQueue();
        }

        private static bool LoadCharacterData(LoadDBInfo loadUser, ref bool reTry)
        {
            var queryId = 0;
            var result = false;
            reTry = false;
            if (M2Share.FrontEngine.InSaveRcdList(loadUser.ChrName))
            {
                reTry = true;// 反回TRUE,则重新加入队列
                return false;
            }
            /*if (SystemShare.WorldEngine.GetPlayObjectEx(loadUser.ChrName) != null)
            {
                SystemShare.WorldEngine.KickPlayObjectEx(loadUser.ChrName);
                boReTry = true;// 反回TRUE,则重新加入队列
                return false;
            }*/
            if (!CharacterDataService.QueryCharacterData(loadUser.Account, loadUser.ChrName, loadUser.sIPaddr, ref queryId, loadUser.SessionID))
            {
                M2Share.NetChannel.SendOutConnectMsg(loadUser.GateIdx, loadUser.SocketId, loadUser.GSocketIdx); // 获取数据失败,发送连接断开消息
            }
            else
            {
                var userOpenInfo = new UserOpenInfo
                {
                    ChrName = loadUser.ChrName,
                    LoadUser = loadUser,
                    HumanRcd = null,
                    QueryId = queryId
                };
                SystemShare.WorldEngine.AddUserOpenInfo(userOpenInfo);
                result = true;
            }
            return result;
        }
    }
}