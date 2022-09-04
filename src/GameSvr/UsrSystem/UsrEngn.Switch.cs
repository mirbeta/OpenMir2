using GameSvr.Actor;
using GameSvr.Player;
using SystemModule;
using SystemModule.Packet.ClientPackets;

namespace GameSvr.UsrSystem
{
    public partial class UserEngine
    {
        private TSwitchDataInfo GetSwitchData(string sChrName, int nCode)
        {
            TSwitchDataInfo result = null;
            TSwitchDataInfo SwitchData = null;
            for (var i = 0; i < _mChangeServerList.Count; i++)
            {
                SwitchData = _mChangeServerList[i];
                if (string.Compare(SwitchData.sChrName, sChrName, StringComparison.OrdinalIgnoreCase) == 0 && SwitchData.nCode == nCode)
                {
                    result = SwitchData;
                    break;
                }
            }
            return result;
        }

        private void LoadSwitchData(TSwitchDataInfo SwitchData, ref PlayObject PlayObject)
        {
            int nCount;
            TSlaveInfo SlaveInfo;
            if (SwitchData.boC70)
            {

            }
            PlayObject.BanShout = SwitchData.boBanShout;
            PlayObject.HearWhisper = SwitchData.boHearWhisper;
            PlayObject.BanGuildChat = SwitchData.boBanGuildChat;
            PlayObject.BanGuildChat = SwitchData.boBanGuildChat;
            PlayObject.AdminMode = SwitchData.boAdminMode;
            PlayObject.ObMode = SwitchData.boObMode;
            nCount = 0;
            while (true)
            {
                if (SwitchData.BlockWhisperArr[nCount] == "") break;
                PlayObject.m_BlockWhisperList.Add(SwitchData.BlockWhisperArr[nCount]);
                nCount++;
                if (nCount >= SwitchData.BlockWhisperArr.Count) break;
            }

            nCount = 0;
            while (true)
            {
                if (SwitchData.SlaveArr[nCount].SlaveName == "") break;
                SlaveInfo = SwitchData.SlaveArr[nCount];
                var slaveId = HUtil32.Sequence();
                M2Share.ActorMgr.AddOhter(slaveId, SlaveInfo);
                PlayObject.SendDelayMsg(PlayObject, Grobal2.RM_10401, 0, slaveId, 0, 0, "", 500);
                nCount++;
                if (nCount >= 5) break;
            }
            nCount = 0;
            while (true)
            {
                PlayObject.m_wStatusArrValue[nCount] = SwitchData.StatusValue[nCount];
                PlayObject.m_dwStatusArrTimeOutTick[nCount] = SwitchData.StatusTimeOut[nCount];
                nCount++;
                if (nCount >= 6) break;
            }
        }

        public void AddSwitchData(TSwitchDataInfo SwitchData)
        {
            SwitchData.dwWaitTime = HUtil32.GetTickCount();
            _mChangeServerList.Add(SwitchData);
        }

        private void DelSwitchData(TSwitchDataInfo SwitchData)
        {
            TSwitchDataInfo SwitchDataInfo;
            for (var i = 0; i < _mChangeServerList.Count; i++)
            {
                SwitchDataInfo = _mChangeServerList[i];
                if (SwitchDataInfo == SwitchData)
                {
                    SwitchDataInfo = null;
                    _mChangeServerList.RemoveAt(i);
                    break;
                }
            }
        }

        private bool SendSwitchData(PlayObject PlayObject, int nServerIndex)
        {
            TSwitchDataInfo SwitchData = null;
            MakeSwitchData(PlayObject, ref SwitchData);
            var flName = "$_" + M2Share.nServerIndex + "_$_" + M2Share.ShareFileNameNum + ".shr";
            PlayObject.m_sSwitchDataTempFile = flName;
            SendServerGroupMsg(Grobal2.ISM_USERSERVERCHANGE, nServerIndex, flName);//发送消息切换服务器
            M2Share.ShareFileNameNum++;
            return true;
        }

        private void MakeSwitchData(PlayObject PlayObject, ref TSwitchDataInfo SwitchData)
        {
            SwitchData = new TSwitchDataInfo();
            SwitchData.sChrName = PlayObject.CharName;
            SwitchData.sMap = PlayObject.MapName;
            SwitchData.wX = PlayObject.CurrX;
            SwitchData.wY = PlayObject.CurrY;
            SwitchData.Abil = PlayObject.Abil;
            SwitchData.nCode = PlayObject.m_nSessionID;
            SwitchData.boBanShout = PlayObject.BanShout;
            SwitchData.boHearWhisper = PlayObject.HearWhisper;
            SwitchData.boBanGuildChat = PlayObject.BanGuildChat;
            SwitchData.boBanGuildChat = PlayObject.BanGuildChat;
            SwitchData.boAdminMode = PlayObject.AdminMode;
            SwitchData.boObMode = PlayObject.ObMode;
            for (var i = 0; i < PlayObject.m_BlockWhisperList.Count; i++)
            {
                SwitchData.BlockWhisperArr.Add(PlayObject.m_BlockWhisperList[i]);
            }
            TBaseObject BaseObject = null;
            for (var i = 0; i < PlayObject.SlaveList.Count; i++)
            {
                BaseObject = PlayObject.SlaveList[i];
                if (i <= 4)
                {
                    SwitchData.SlaveArr[i].SlaveName = BaseObject.CharName;
                    SwitchData.SlaveArr[i].KillCount = BaseObject.KillMonCount;
                    SwitchData.SlaveArr[i].SalveLevel = BaseObject.SlaveMakeLevel;
                    SwitchData.SlaveArr[i].SlaveExpLevel = BaseObject.SlaveExpLevel;
                    SwitchData.SlaveArr[i].RoyaltySec = (BaseObject.MasterRoyaltyTick - HUtil32.GetTickCount()) / 1000;
                    SwitchData.SlaveArr[i].nHP = BaseObject.m_WAbil.HP;
                    SwitchData.SlaveArr[i].nMP = BaseObject.m_WAbil.MP;
                }
            }
            for (var i = 0; i < PlayObject.m_wStatusArrValue.Length; i++)
            {
                if (PlayObject.m_wStatusArrValue[i] > 0)
                {
                    SwitchData.StatusValue[i] = PlayObject.m_wStatusArrValue[i];
                    SwitchData.StatusTimeOut[i] = PlayObject.m_dwStatusArrTimeOutTick[i];
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