using GameSvr.Player;
using System.Text;

namespace GameSvr.Command.Commands
{
    [GameCommand("ServerInfo", "查看服务器信息", 10)]
    public class ServerInfoCommand : BaseCommond
    {
        [DefaultCommand]
        public void ServerInfo(string[] @Params, TPlayObject PlayObject)
        {
            var sb = new StringBuilder();
            //sb.AppendLine(string.Format("({0}) [{1}/{2}] [{3}/{4}] [{5}/{6}]", M2Share.UserEngine.MonsterCount,
            //       TRunSocket.g_nGateRecvMsgLenMin, TRunSocket.g_nGateRecvMsgLenMax, M2Share.UserEngine.OnlinePlayObject,
            //       M2Share.UserEngine.PlayObjectCount, M2Share.UserEngine.LoadPlayCount, M2Share.UserEngine.m_PlayObjectFreeList.Count));
            //sb.AppendLine(string.Format("Run({0}/{1}) Soc({2}/{3}) Usr({4}/{5})", M2Share.nRunTimeMin, M2Share.nRunTimeMax, M2Share.g_nSockCountMin,
            //        M2Share.g_nSockCountMax, M2Share.g_nUsrTimeMin, M2Share.g_nUsrTimeMax));
            //sb.AppendLine(string.Format("Hum{0}/{1} Usr{2}/{3} Mer{4}/{5} Npc{6}/{7}", M2Share.g_nHumCountMin, M2Share.g_nHumCountMax,
            //        M2Share.dwUsrRotCountMin, M2Share.dwUsrRotCountMax, M2Share.UserEngine.dwProcessMerchantTimeMin,
            //        M2Share.UserEngine.dwProcessMerchantTimeMax, M2Share.UserEngine.dwProcessNpcTimeMin, M2Share.UserEngine.dwProcessNpcTimeMax,
            //        M2Share.g_nProcessHumanLoopTime));
            //sb.AppendLine(string.Format("MonG({0}/{1}/{2}) MonP({3}/{4}/{5}) ObjRun({6}/{7})", M2Share.g_nMonGenTime, M2Share.g_nMonGenTimeMin,
            //      M2Share.g_nMonGenTimeMax, M2Share.g_nMonProcTime, M2Share.g_nMonProcTimeMin, M2Share.g_nMonProcTimeMax, M2Share.g_nBaseObjTimeMin,
            //      M2Share.g_nBaseObjTimeMax));
            //if (M2Share.dwStartTimeTick == 0)
            //{
            //    M2Share.dwStartTimeTick = HUtil32.GetTickCount();
            //}
            //M2Share.MainOutMessage(sb.ToString());

            //TGateInfo GateInfo;
            //sb.Clear();
            //for (int i = TRunSocket.g_GateArr.GetLowerBound(0); i <= TRunSocket.g_GateArr.GetUpperBound(0); i++)
            //{
            //    GateInfo = TRunSocket.g_GateArr[i];
            //    if (GateInfo.boUsed && (GateInfo.Socket != null))
            //    {
            //        sb.Append(string.Format("Gate [0] ", i));
            //        sb.Append(string.Format("Gate [{0}]:[{1}] ", GateInfo.sAddr, GateInfo.nPort));
            //        sb.Append(string.Format("Gate SendMessage:[{0}] ", GateInfo.nSendedMsgCount));
            //        sb.Append(string.Format("Gate SendRemain:[{0}] ", GateInfo.nSendRemainCount));
            //        if (GateInfo.nSendMsgBytes < 1024)
            //        {
            //            sb.Append(string.Format("Gate SendBytes:[{0}] b ", GateInfo.nSendMsgBytes));
            //        }
            //        else
            //        {
            //            sb.Append(string.Format("Gate SendBytes:[{0}] kb ", GateInfo.nSendMsgBytes / 1024));
            //        }
            //        if (GateInfo.UserList != null)
            //        {
            //            sb.Append(string.Format("Gate Users:[{0}] / [{1}] ", GateInfo.nUserCount, GateInfo.UserList.Count));
            //        }
            //        else
            //        {
            //            sb.Append(string.Format("Gate Users:[{0}] ", GateInfo.nUserCount));
            //        }
            //        sb.AppendLine(Environment.NewLine);
            //    }
            //}
            //if (sb.Length > 0)
            //{
            //    M2Share.MainOutMessage(sb.ToString());
            //}
        }
    }
}
