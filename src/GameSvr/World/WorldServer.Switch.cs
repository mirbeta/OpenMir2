using GameSvr.Actor;
using GameSvr.Player;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.World
{
    public partial class WorldServer
    {
        private SwitchDataInfo GetSwitchData(string sChrName, int nCode)
        {
            SwitchDataInfo result = null;
            for (var i = 0; i < _mChangeServerList.Count; i++)
            {
                SwitchDataInfo switchData = _mChangeServerList[i];
                if (string.Compare(switchData.sChrName, sChrName, StringComparison.OrdinalIgnoreCase) == 0 && switchData.nCode == nCode)
                {
                    result = switchData;
                    break;
                }
            }
            return result;
        }

        private void LoadSwitchData(SwitchDataInfo switchData, ref PlayObject playObject)
        {
            playObject.BanShout = switchData.boBanShout;
            playObject.HearWhisper = switchData.boHearWhisper;
            playObject.BanGuildChat = switchData.boBanGuildChat;
            playObject.BanGuildChat = switchData.boBanGuildChat;
            playObject.AdminMode = switchData.boAdminMode;
            playObject.ObMode = switchData.boObMode;
            var nCount = 0;
            while (true)
            {
                if (switchData.BlockWhisperArr[nCount] == "") break;
                playObject.LockWhisperList.Add(switchData.BlockWhisperArr[nCount]);
                nCount++;
                if (nCount >= switchData.BlockWhisperArr.Count) break;
            }
            nCount = 0;
            while (true)
            {
                if (switchData.SlaveArr[nCount].SlaveName == "") break;
                var slaveId = HUtil32.Sequence();
                M2Share.ActorMgr.AddOhter(slaveId,  switchData.SlaveArr[nCount]);
                playObject.SendDelayMsg(playObject, Grobal2.RM_10401, 0, slaveId, 0, 0, "", 500);
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
            _mChangeServerList.Add(switchData);
        }

        private void DelSwitchData(SwitchDataInfo switchData)
        {
            for (var i = 0; i < _mChangeServerList.Count; i++)
            {
                var switchDataInfo = _mChangeServerList[i];
                if (switchDataInfo == switchData)
                {
                    _mChangeServerList.RemoveAt(i);
                    break;
                }
            }
        }

        private bool SendSwitchData(PlayObject playObject, int nServerIndex)
        {
            SwitchDataInfo switchData = null;
            MakeSwitchData(playObject, ref switchData);
            var flName = "$_" + M2Share.ServerIndex + "_$_" + M2Share.ShareFileNameNum + ".shr";
            playObject.MSSwitchDataTempFile = flName;
            SendServerGroupMsg(Grobal2.ISM_USERSERVERCHANGE, nServerIndex, flName);//发送消息切换服务器
            M2Share.ShareFileNameNum++;
            return true;
        }

        private void MakeSwitchData(PlayObject playObject, ref SwitchDataInfo switchData)
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
            for (var i = 0; i < playObject.LockWhisperList.Count; i++)
            {
                switchData.BlockWhisperArr.Add(playObject.LockWhisperList[i]);
            }

            for (var i = 0; i < playObject.SlaveList.Count; i++)
            {
                BaseObject baseObject = playObject.SlaveList[i];
                if (i <= 4)
                {
                    switchData.SlaveArr[i].SlaveName = baseObject.ChrName;
                    switchData.SlaveArr[i].KillCount = baseObject.KillMonCount;
                    switchData.SlaveArr[i].SalveLevel = baseObject.SlaveMakeLevel;
                    switchData.SlaveArr[i].SlaveExpLevel = baseObject.SlaveExpLevel;
                    switchData.SlaveArr[i].RoyaltySec = (baseObject.MasterRoyaltyTick - HUtil32.GetTickCount()) / 1000;
                    switchData.SlaveArr[i].nHP = baseObject.Abil.HP;
                    switchData.SlaveArr[i].nMP = baseObject.Abil.MP;
                }
            }
            for (var i = 0; i < playObject.ExtraAbil.Length; i++)
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
            for (var i = _mChangeServerList.Count - 1; i >= 0; i--)
            {
                if ((HUtil32.GetTickCount() - _mChangeServerList[i].dwWaitTime) > 30 * 1000)
                {
                    _mChangeServerList[i] = null;
                    _mChangeServerList.RemoveAt(i);
                }
            }
        }

    }
}