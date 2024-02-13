using M2Server;
using OpenMir2;
using OpenMir2.Data;
using SystemModule;
using SystemModule.Actors;
using SystemModule.Data;

namespace GameSrv.Word
{
    public partial class WorldServer
    {
        private SwitchDataInfo GetSwitchData(string sChrName, int nCode)
        {
            SwitchDataInfo result = null;
            for (int i = 0; i < ChangeServerList.Count; i++)
            {
                SwitchDataInfo switchData = ChangeServerList[i];
                if (string.Compare(switchData.sChrName, sChrName, StringComparison.OrdinalIgnoreCase) == 0 && switchData.nCode == nCode)
                {
                    result = switchData;
                    break;
                }
            }
            return result;
        }

        private static void LoadSwitchData(SwitchDataInfo switchData, ref IPlayerActor playObject)
        {
            playObject.BanShout = switchData.boBanShout;
            playObject.HearWhisper = switchData.boHearWhisper;
            playObject.BanGuildChat = switchData.boBanGuildChat;
            playObject.BanGuildChat = switchData.boBanGuildChat;
            playObject.AdminMode = switchData.boAdminMode;
            playObject.ObMode = switchData.boObMode;
            int nCount = 0;
            while (true)
            {
                if (string.IsNullOrEmpty(switchData.BlockWhisperArr[nCount])) break;
                playObject.LockWhisperList.Add(switchData.BlockWhisperArr[nCount]);
                nCount++;
                if (nCount >= switchData.BlockWhisperArr.Count) break;
            }
            nCount = 0;
            while (true)
            {
                if (string.IsNullOrEmpty(switchData.SlaveArr[nCount].SlaveName)) break;
                int slaveId = HUtil32.Sequence();
                SystemShare.ActorMgr.AddOhter(slaveId, switchData.SlaveArr[nCount]);
                playObject.SendSelfDelayMsg(Messages.RM_10401, 0, slaveId, 0, 0, "", 500);
                nCount++;
                if (nCount >= 5) break;
            }
            nCount = 0;
            while (true)
            {
                playObject.ExtraAbil[nCount] = switchData.StatusValue[nCount];
                playObject.ExtraAbilTimes[nCount] = switchData.StatusTimeOut[nCount];
                nCount++;
                if (nCount >= 6) break;
            }
        }

        public void AddSwitchData(SwitchDataInfo switchData)
        {
            switchData.dwWaitTime = HUtil32.GetTickCount();
            ChangeServerList.Add(switchData);
        }

        private void DelSwitchData(SwitchDataInfo switchData)
        {
            for (int i = 0; i < ChangeServerList.Count; i++)
            {
                SwitchDataInfo switchDataInfo = ChangeServerList[i];
                if (switchDataInfo == switchData)
                {
                    ChangeServerList.RemoveAt(i);
                    break;
                }
            }
        }

        private bool SendSwitchData(IPlayerActor playObject, int nServerIndex)
        {
            SwitchDataInfo switchData = null;
            MakeSwitchData(playObject, ref switchData);
            string flName = "$_" + M2Share.ServerIndex + "_$_" + M2Share.ShareFileNameNum + ".shr";
            playObject.SwitchDataTempFile = flName;
            SendServerGroupMsg(Messages.ISM_USERSERVERCHANGE, nServerIndex, flName);//发送消息切换服务器
            M2Share.ShareFileNameNum++;
            return true;
        }

        private static void MakeSwitchData(IPlayerActor playObject, ref SwitchDataInfo switchData)
        {
            switchData = new SwitchDataInfo();
            switchData.sChrName = playObject.ChrName;
            switchData.sMap = playObject.MapName;
            switchData.wX = playObject.CurrX;
            switchData.wY = playObject.CurrY;
            switchData.Abil = playObject.Abil;
            switchData.nCode = playObject.SessionId;
            switchData.boBanShout = playObject.BanShout;
            switchData.boHearWhisper = playObject.HearWhisper;
            switchData.boBanGuildChat = playObject.BanGuildChat;
            switchData.boBanGuildChat = playObject.BanGuildChat;
            switchData.boAdminMode = playObject.AdminMode;
            switchData.boObMode = playObject.ObMode;
            for (int i = 0; i < playObject.LockWhisperList.Count; i++)
            {
                switchData.BlockWhisperArr.Add(playObject.LockWhisperList[i]);
            }

            for (int i = 0; i < playObject.SlaveList.Count; i++)
            {
                IActor baseObject = playObject.SlaveList[i];
                if (i <= 4)
                {
                    //switchData.SlaveArr[i].SlaveName = baseObject.ChrName;
                    //switchData.SlaveArr[i].KillCount = baseObject.KillMonCount;
                    //switchData.SlaveArr[i].SalveLevel = baseObject.SlaveMakeLevel;
                    //switchData.SlaveArr[i].SlaveExpLevel = baseObject.SlaveExpLevel;
                    //switchData.SlaveArr[i].RoyaltySec = (baseObject.MasterRoyaltyTick - HUtil32.GetTickCount()) / 1000;
                    //switchData.SlaveArr[i].nHP = baseObject.Abil.HP;
                    //switchData.SlaveArr[i].nMP = baseObject.Abil.MP;
                }
            }
            for (int i = 0; i < playObject.ExtraAbil.Length; i++)
            {
                if (playObject.ExtraAbil[i] > 0)
                {
                    switchData.StatusValue[i] = playObject.ExtraAbil[i];
                    switchData.StatusTimeOut[i] = playObject.ExtraAbilTimes[i];
                }
            }
        }

        public void CheckSwitchServerTimeOut()
        {
            for (int i = ChangeServerList.Count - 1; i >= 0; i--)
            {
                if ((HUtil32.GetTickCount() - ChangeServerList[i].dwWaitTime) > 30 * 1000)
                {
                    ChangeServerList[i] = null;
                    ChangeServerList.RemoveAt(i);
                }
            }
        }

    }
}