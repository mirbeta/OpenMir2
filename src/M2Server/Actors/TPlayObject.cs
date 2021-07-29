using SystemModule;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using SystemModule.Common;
using SystemModule.Packages;

namespace M2Server
{
    public partial class TPlayObject : TAnimalObject
    {
        public TDefaultMessage m_DefMsg;
        public ArrayList TList55C = null;
        public string m_sOldSayMsg = string.Empty;
        public int m_nSayMsgCount = 0;
        public int m_dwSayMsgTick = 0;
        public bool m_boDisableSayMsg = false;
        public int m_dwDisableSayMsgTick = 0;
        public int m_dwCheckDupObjTick = 0;
        public int dwTick578 = 0;
        public int dwTick57C = 0;
        public bool m_boInSafeArea = false;
        public int n584 = 0;
        public int n588 = 0;
        /// <summary>
        /// 登录帐号名
        /// </summary>
        public string m_sUserID;
        /// <summary>
        /// 人物IP地址
        /// </summary>
        public string m_sIPaddr = string.Empty;
        public string m_sIPLocal = string.Empty;
        public int m_nSocket = 0;
        /// <summary>
        /// 人物连接到游戏网关SOCKETID
        /// </summary>
        public int m_nGSocketIdx = 0;
        /// <summary>
        /// 人物所在网关号
        /// </summary>
        public int m_nGateIdx = 0;
        public int m_nSoftVersionDate = 0;
        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime m_dLogonTime;
        /// <summary>
        /// 战领沙城时间
        /// </summary>
        public int m_dwLogonTick = 0;
        // 0x5B8  (Dword)
        /// <summary>
        /// 是否进入游戏完成
        /// </summary>
        public bool m_boReadyRun = false;
        /// <summary>
        /// 当前会话ID
        /// </summary>
        public int m_nSessionID = 0;
        /// <summary>
        /// 人物当前模式(测试/付费模式)
        /// </summary>
        public int m_nPayMent = 0;
        public int m_nPayMode = 0;
        /// <summary>
        /// 全局会话信息
        /// </summary>
        public TSessInfo m_SessInfo = null;
        public int m_dwLoadTick = 0;
        /// <summary>
        /// 人物当前所在服务器序号
        /// </summary>
        public int m_nServerIndex = 0;
        public bool m_boEmergencyClose = false;
        /// <summary>
        /// 掉线标志
        /// </summary>
        public bool m_boSoftClose = false;
        /// <summary>
        /// 断线标志(@kick 命令)
        /// </summary>
        public bool m_boKickFlag = false;
        public bool m_boReconnection = false;
        public bool m_boRcdSaved = false;
        public bool m_boSwitchData = false;
        public bool m_boSwitchDataOK = false;
        public string m_sSwitchDataTempFile = string.Empty;
        public int m_nWriteChgDataErrCount = 0;
        public string m_sSwitchMapName = string.Empty;
        public short m_nSwitchMapX = 0;
        public short m_nSwitchMapY = 0;
        public bool m_boSwitchDataSended = false;
        public int m_dwChgDataWritedTick = 0;
        public int m_dw5D4 = 0;
        public int n5F8 = 0;
        public int n5FC = 0;
        /// <summary>
        /// 攻击间隔
        /// </summary>
        public int m_dwHitIntervalTime = 0;
        /// <summary>
        /// 魔法间隔
        /// </summary>
        public int m_dwMagicHitIntervalTime = 0;
        /// <summary>
        /// 走路间隔
        /// </summary>
        public int m_dwRunIntervalTime = 0;
        /// <summary>
        /// 走路间隔
        /// </summary>
        public int m_dwWalkIntervalTime = 0;
        /// <summary>
        /// 换方向间隔
        /// </summary>
        public int m_dwTurnIntervalTime = 0;
        /// <summary>
        /// 组合操作间隔
        /// </summary>
        public int m_dwActionIntervalTime = 0;
        /// <summary>
        /// 移动刺杀间隔
        /// </summary>
        public int m_dwRunLongHitIntervalTime = 0;
        /// <summary>
        /// 跑位攻击间隔
        /// </summary>        
        public int m_dwRunHitIntervalTime = 0;
        /// <summary>
        /// 走位攻击间隔
        /// </summary>        
        public int m_dwWalkHitIntervalTime = 0;
        /// <summary>
        /// 跑位魔法间隔
        /// </summary>        
        public int m_dwRunMagicIntervalTime = 0;
        /// <summary>
        /// 魔法攻击时间
        /// </summary>        
        public int m_dwMagicAttackTick = 0;
        /// <summary>
        /// 魔法攻击间隔时间
        /// </summary>
        public int m_dwMagicAttackInterval = 0;
        /// <summary>
        /// 攻击时间
        /// </summary>
        public int m_dwAttackTick = 0;
        /// <summary>
        /// 人物跑动时间
        /// </summary>
        public int m_dwMoveTick = 0;
        /// <summary>
        /// 人物攻击计数
        /// </summary>
        public int m_dwAttackCount = 0;
        /// <summary>
        /// 人物攻击计数
        /// </summary>
        public int m_dwAttackCountA = 0;
        /// <summary>
        /// 魔法攻击计数
        /// </summary>
        public int m_dwMagicAttackCount = 0;
        /// <summary>
        /// 人物跑计数
        /// </summary>
        public int m_dwMoveCount = 0;
        /// <summary>
        /// 人物跑计数
        /// </summary>
        public int m_dwMoveCountA = 0;
        /// <summary>
        /// 超速计数
        /// </summary>
        public int m_nOverSpeedCount = 0;
        public bool m_boDieInFight3Zone = false;
        public string m_sGotoNpcLabel = string.Empty;
        public int m_nDelayCall = 0;
        public int m_dwDelayCallTick = 0;
        public bool m_boDelayCall = false;
        public int m_DelayCallNPC = 0;
        public string m_sDelayCallLabel = string.Empty;
        public TScript m_Script = null;
        public TBaseObject m_NPC = null;
        public int[] m_nVal;
        public int[] m_nMval;
        public int[] m_DyVal;
        public string[] m_nSval;
        public string m_sPlayDiceLabel = string.Empty;
        public bool m_boTimeRecall = false;
        public int m_dwTimeRecallTick = 0;
        public string m_sMoveMap = string.Empty;
        public short m_nMoveX = 0;
        public short m_nMoveY = 0;
        public bool bo698 = false;
        public int n69C = 0;
        /// <summary>
        /// 保存人物数据时间间隔
        /// </summary>
        public int m_dwSaveRcdTick = 0;
        public byte m_btBright = 0;
        public bool m_boNewHuman = false;
        public bool m_boSendNotice = false;
        public int m_dwWaitLoginNoticeOKTick = 0;
        public bool m_boLoginNoticeOK = false;
        public bool bo6AB = false;
        /// <summary>
        /// 帐号过期
        /// </summary>
        public bool m_boExpire = false;
        public int m_dwShowLineNoticeTick = 0;
        public int m_nShowLineNoticeIdx = 0;
        public int m_nSoftVersionDateEx = 0;
        public Hashtable m_CanJmpScriptLableList = null;
        public int m_nScriptGotoCount = 0;
        public string m_sScriptCurrLable = string.Empty;
        // 用于处理 @back 脚本命令
        public string m_sScriptGoBackLable = string.Empty;
        // 用于处理 @back 脚本命令
        public int m_dwTurnTick = 0;
        public short m_wOldIdent = 0;
        public byte m_btOldDir = 0;
        /// <summary>
        /// 第一个操作
        /// </summary>
        public bool m_boFirstAction = false;
        /// <summary>
        /// 二次操作之间间隔时间
        /// </summary>        
        public int m_dwActionTick = 0;
        /// <summary>
        /// 配偶名称
        /// </summary>
        public string m_sDearName;
        public TPlayObject m_DearHuman = null;
        /// <summary>
        /// 是否允许夫妻传送
        /// </summary>
        public bool m_boCanDearRecall = false;
        public bool m_boCanMasterRecall = false;
        /// <summary>
        /// 夫妻传送时间
        /// </summary>
        public int m_dwDearRecallTick = 0;
        public int m_dwMasterRecallTick = 0;
        /// <summary>
        /// 师徒名称
        /// </summary>
        public string m_sMasterName;
        public TPlayObject m_MasterHuman = null;
        public IList<TPlayObject> m_MasterList = null;
        public bool m_boMaster = false;
        /// <summary>
        /// 声望点
        /// </summary>
        public byte m_btCreditPoint = 0;
        /// <summary>
        /// 离婚次数
        /// </summary>        
        public byte m_btMarryCount = 0;
        public byte m_btReLevel = 0;
        // 转生等级
        public byte m_btReColorIdx = 0;
        public int m_dwReColorTick = 0;
        /// <summary>
        /// 杀怪经验倍数
        /// </summary>
        public int m_nKillMonExpMultiple = 0;
        /// <summary>
        /// 处理消息循环时间控制
        /// </summary>        
        public int m_dwGetMsgTick = 0;
        public bool m_boSetStoragePwd = false;
        public bool m_boReConfigPwd = false;
        public bool m_boCheckOldPwd = false;
        public bool m_boUnLockPwd = false;
        public bool m_boUnLockStoragePwd = false;
        public bool m_boPasswordLocked = false;
        // 锁密码
        public byte m_btPwdFailCount = 0;
        public bool m_boLockLogon = false;
        // 是否启用锁登录功能
        public bool m_boLockLogoned = false;
        // 是否打开登录锁
        public string m_sTempPwd;
        public string m_sStoragePwd;
        public TBaseObject m_PoseBaseObject = null;
        public bool m_boStartMarry = false;
        public bool m_boStartMaster = false;
        public bool m_boStartUnMarry = false;
        public bool m_boStartUnMaster = false;
        public bool m_boFilterSendMsg = false;
        // 禁止发方字(发的文字只能自己看到)
        public int m_nKillMonExpRate = 0;
        // 杀怪经验倍数(此数除以 100 为真正倍数)
        public int m_nPowerRate = 0;
        // 人物攻击力倍数(此数除以 100 为真正倍数)
        public int m_dwKillMonExpRateTime = 0;
        public int m_dwPowerRateTime = 0;
        public int m_dwRateTick = 0;
        public bool m_boCanUseItem = false;
        // 是否允许使用物品
        public bool m_boCanDeal = false;
        public bool m_boCanDrop = false;
        public bool m_boCanGetBackItem = false;
        public bool m_boCanWalk = false;
        public bool m_boCanRun = false;
        public bool m_boCanHit = false;
        public bool m_boCanSpell = false;
        public bool m_boCanSendMsg = false;
        public int m_nMemberType = 0;
        // 会员类型
        public int m_nMemberLevel = 0;
        // 会员等级
        public bool m_boSendMsgFlag = false;
        // 发祝福语标志
        public bool m_boChangeItemNameFlag = false;
        public int m_nGameGold = 0;
        // 游戏币
        public bool m_boDecGameGold = false;
        // 是否自动减游戏币
        public int m_dwDecGameGoldTime = 0;
        public int m_dwDecGameGoldTick = 0;
        public int m_nDecGameGold = 0;
        // 一次减点数
        public bool m_boIncGameGold = false;
        // 是否自动加游戏币
        public int m_dwIncGameGoldTime = 0;
        public int m_dwIncGameGoldTick = 0;
        public int m_nIncGameGold = 0;
        // 一次减点数
        public int m_nGamePoint = 0;
        // 游戏点数
        public int m_dwIncGamePointTick = 0;
        public int m_nPayMentPoint = 0;
        public int m_dwPayMentPointTick = 0;
        public int m_dwDecHPTick = 0;
        public int m_dwIncHPTick = 0;
        public TPlayObject m_GetWhisperHuman = null;
        public int m_dwClearObjTick = 0;
        public short m_wContribution = 0;
        // 贡献度
        public string m_sRankLevelName = string.Empty;
        // 显示名称格式串
        public bool m_boFilterAction = false;
        public bool m_boClientFlag = false;
        public byte m_nStep = 0;
        public int m_nClientFlagMode = 0;
        public int m_dwAutoGetExpTick = 0;
        public int m_nAutoGetExpTime = 0;
        public int m_nAutoGetExpPoint = 0;
        public TEnvirnoment m_AutoGetExpEnvir = null;
        public bool m_boAutoGetExpInSafeZone = false;
        public IList<TDynamicVar> m_DynamicVarList = null;
        public short m_dwClientTick = 0;
        /// <summary>
        /// 进入速度测试模式
        /// </summary>
        public bool m_boTestSpeedMode = false;
        public int nRunCount = 0;
        public int dwRunTimeCount = 0;
        public int m_dwDelayTime = 0;
        public string m_sRandomNo = string.Empty;
        public int m_dwQueryBagItemsTick = 0;
        /// <summary>
        /// 包裹刷新时间
        /// </summary>
        public int m_dwClickNpcTime = 0;

        private bool ClientPickUpItem_IsSelf(TBaseObject BaseObject)
        {
            bool result;
            if (BaseObject == null || this == BaseObject)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        private bool ClientPickUpItem_IsOfGroup(TBaseObject BaseObject)
        {
            TBaseObject GroupMember;
            var result = false;
            if (m_GroupOwner == null)
            {
                return result;
            }
            for (var i = 0; i < m_GroupOwner.m_GroupMembers.Count; i++)
            {
                GroupMember = m_GroupOwner.m_GroupMembers[i];
                if (GroupMember == BaseObject)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        private bool ClientPickUpItem()
        {
            var result = false;
            if (m_boDealing)
            {
                return result;
            }
            var mapItem = m_PEnvir.GetItem(m_nCurrX, m_nCurrY);
            if (mapItem == null)
            {
                return result;
            }
            if (HUtil32.GetTickCount() - mapItem.dwCanPickUpTick > M2Share.g_Config.dwFloorItemCanPickUpTime)// 2 * 60 * 1000
            {
                mapItem.OfBaseObject = null;
            }
            if (!ClientPickUpItem_IsSelf(mapItem.OfBaseObject as TBaseObject) && !ClientPickUpItem_IsOfGroup(mapItem.OfBaseObject as TBaseObject))
            {
                SysMsg(M2Share.g_sCanotPickUpItem, TMsgColor.c_Red, TMsgType.t_Hint);
                return result;
            }
            if (mapItem.Name.Equals(grobal2.sSTRING_GOLDNAME, StringComparison.OrdinalIgnoreCase))
            {
                if (m_PEnvir.DeleteFromMap(m_nCurrX, m_nCurrY, grobal2.OS_ITEMOBJECT, mapItem) == 1)
                {
                    if (IncGold(mapItem.Count))
                    {
                        SendRefMsg(grobal2.RM_ITEMHIDE, 0, mapItem.Id, m_nCurrX, m_nCurrY, "");
                        if (M2Share.g_boGameLogGold)
                        {
                            M2Share.AddGameDataLog('4' + "\t" + m_sMapName + "\t" + m_nCurrX.ToString() + "\t" + m_nCurrY.ToString() + "\t" + m_sCharName + "\t" + grobal2.sSTRING_GOLDNAME 
                                                   + "\t" + mapItem.Count.ToString() + "\t" + '1' + "\t" + '0');
                        }
                        GoldChanged();
                        Dispose(mapItem);
                    }
                    else
                    {
                        m_PEnvir.AddToMap(m_nCurrX, m_nCurrY, grobal2.OS_ITEMOBJECT, mapItem);
                    }
                }
                return result;
            }
            if (IsEnoughBag())
            {
                if (m_PEnvir.DeleteFromMap(m_nCurrX, m_nCurrY, grobal2.OS_ITEMOBJECT, mapItem) == 1)
                {
                    var UserItem = mapItem.UserItem;
                    var StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                    if (StdItem != null && IsAddWeightAvailable(M2Share.UserEngine.GetStdItemWeight(UserItem.wIndex)))
                    {
                        SendMsg(this, grobal2.RM_ITEMHIDE, 0, mapItem.Id, m_nCurrX, m_nCurrY, "");
                        AddItemToBag(UserItem);
                        if (!M2Share.IsCheapStuff(StdItem.StdMode))
                        {
                            if (StdItem.NeedIdentify == 1)
                            {
                                M2Share.AddGameDataLog('4' + "\t" + m_sMapName + "\t" + m_nCurrX.ToString() + "\t" + m_nCurrY.ToString() + "\t" + m_sCharName + "\t" + StdItem.Name 
                                                       + "\t" + UserItem.MakeIndex.ToString() + "\t" + '1' + "\t" + '0');
                            }
                        }
                        Dispose(mapItem);
                        if (m_btRaceServer == grobal2.RC_PLAYOBJECT)
                        {
                            this.SendAddItem(UserItem);
                        }
                        result = true;
                    }
                    else
                    {
                        Dispose(UserItem);
                        m_PEnvir.AddToMap(m_nCurrX, m_nCurrY, grobal2.OS_ITEMOBJECT, mapItem);
                    }
                }
            }
            return result;
        }

        private void WinExp(int dwExp)
        {
            if (m_Abil.Level > M2Share.g_Config.nLimitExpLevel)
            {
                dwExp = M2Share.g_Config.nLimitExpValue;
                GetExp(dwExp);
            }
            else if (dwExp > 0)
            {
                dwExp = M2Share.g_Config.dwKillMonExpMultiple * dwExp; // 系统指定杀怪经验倍数
                dwExp = m_nKillMonExpMultiple * dwExp; // 人物指定的杀怪经验倍数
                dwExp = HUtil32.Round(m_nKillMonExpRate / 100 * dwExp);// 人物指定的杀怪经验倍数
                if (m_PEnvir.Flag.boEXPRATE)
                {
                    dwExp = HUtil32.Round(m_PEnvir.Flag.nEXPRATE / 100 * dwExp);// 地图上指定杀怪经验倍数
                }
                if (m_boExpItem) // 物品经验倍数
                {
                    dwExp = HUtil32.Round(m_rExpItem * dwExp);
                }
                GetExp(dwExp);
            }
        }

        private void GetExp(int dwExp)
        {
            m_Abil.Exp += dwExp;
            AddBodyLuck(dwExp * 0.002);
            SendMsg(this, grobal2.RM_WINEXP, 0, dwExp, 0, 0, "");
            if (m_Abil.Exp >= m_Abil.MaxExp)
            {
                m_Abil.Exp -= m_Abil.MaxExp;
                if (m_Abil.Level < M2Share.MAXUPLEVEL)
                {
                    m_Abil.Level++;
                }
                HasLevelUp(m_Abil.Level - 1);
                AddBodyLuck(100);
                M2Share.AddGameDataLog("12" + "\t" + m_sMapName + "\t" + m_Abil.Level.ToString() + "\t" + m_Abil.Exp.ToString() + "\t" + m_sCharName + "\t" + '0' + "\t" + '0' + "\t" + '1' + "\t" + '0');
                IncHealthSpell(2000, 2000);
            }
        }

        public bool IncGold(int tGold)
        {
            var result = false;
            if (m_nGold + tGold <= M2Share.g_Config.nHumanMaxGold)
            {
                m_nGold += tGold;
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 检查包裹是否满了
        /// </summary>
        /// <returns></returns>
        public bool IsEnoughBag()
        {
            return m_ItemList.Count < grobal2.MAXBAGITEM;
        }

        public bool IsAddWeightAvailable(int nWeight)
        {
            return m_WAbil.Weight + nWeight <= m_WAbil.MaxWeight;
        }

        public void SendAddItem(TUserItem UserItem)
        {
            TItem Item;
            TStdItem StdItem = null;
            TClientItem ClientItem = null;
            TOClientItem OClientItem = null;
            if (m_nSoftVersionDateEx == 0)
            {
                Item = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                if (Item == null)
                {
                    return;
                }
                OClientItem = new TOClientItem();
                Item.GetStandardItem(ref StdItem);
                Item.GetItemAddValue(UserItem, ref StdItem);
                StdItem.Name = ItmUnit.GetItemName(UserItem);
                M2Share.CopyStdItemToOStdItem(StdItem, OClientItem.S);
                OClientItem.MakeIndex = UserItem.MakeIndex;
                OClientItem.Dura = UserItem.Dura;
                OClientItem.DuraMax = UserItem.DuraMax;
                if (StdItem.StdMode == 50)
                {
                    OClientItem.S.Name = OClientItem.S.Name + " #" + UserItem.Dura;
                }
                if (new ArrayList(new object[] { 15, 19, 20, 21, 22, 23, 24, 26 }).Contains(StdItem.StdMode))
                {
                    if (UserItem.btValue[8] == 0)
                    {
                        OClientItem.S.Shape = 0;
                    }
                    else
                    {
                        OClientItem.S.Shape = 130;
                    }
                }
                m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_ADDITEM, ObjectId, 0, 0, 1);
                SendSocket(m_DefMsg, EDcode.EncodeBuffer(OClientItem));
            }
            else
            {
                Item = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                if (Item == null)
                {
                    return;
                }
                ClientItem = new TClientItem();
                Item.GetStandardItem(ref ClientItem.S);
                Item.GetItemAddValue(UserItem, ref ClientItem.S);
                ClientItem.S.Name = ItmUnit.GetItemName(UserItem);
                ClientItem.MakeIndex = UserItem.MakeIndex;
                ClientItem.Dura = UserItem.Dura;
                ClientItem.DuraMax = UserItem.DuraMax;
                StdItem = ClientItem.S;
                if (StdItem.StdMode == 50)
                {
                    ClientItem.S.Name = ClientItem.S.Name + " #" + UserItem.Dura;
                }
                if (new ArrayList(new byte[] { 15, 19, 20, 21, 22, 23, 24, 26 }).Contains(StdItem.StdMode))
                {
                    if (UserItem.btValue[8] == 0)
                    {
                        ClientItem.S.Shape = 0;
                    }
                    else
                    {
                        ClientItem.S.Shape = 130;
                    }
                }
                m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_ADDITEM, ObjectId, 0, 0, 1);
                SendSocket(m_DefMsg, EDcode.EncodeBuffer(ClientItem));
            }
        }

        protected virtual void Whisper(string whostr, string saystr)
        {
            var svidx = 0;
            var PlayObject = M2Share.UserEngine.GetPlayObject(whostr);
            if (PlayObject != null)
            {
                if (!PlayObject.m_boReadyRun)
                {
                    SysMsg(whostr + M2Share.g_sCanotSendmsg, TMsgColor.c_Red, TMsgType.t_Hint);
                    return;
                }
                if (!PlayObject.m_boHearWhisper || PlayObject.IsBlockWhisper(m_sCharName))
                {
                    SysMsg(whostr + M2Share.g_sUserDenyWhisperMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                    return;
                }
                if (!m_boOffLineFlag && PlayObject.m_boOffLineFlag)
                {
                    if (PlayObject.m_sOffLineLeaveword != "")
                    {
                        PlayObject.Whisper(m_sCharName, PlayObject.m_sOffLineLeaveword);
                    }
                    else
                    {
                        PlayObject.Whisper(m_sCharName, M2Share.g_Config.sServerName + '[' + M2Share.g_Config.sServerIPaddr + "]提示您");
                    }
                    return;
                }
                if (m_btPermission > 0)
                {
                    PlayObject.SendMsg(PlayObject, grobal2.RM_WHISPER, 0, M2Share.g_Config.btGMWhisperMsgFColor, M2Share.g_Config.btGMWhisperMsgBColor, 0, m_sCharName + "=> " + saystr);
                    if (m_GetWhisperHuman != null && !m_GetWhisperHuman.m_boGhost)
                    {
                        m_GetWhisperHuman.SendMsg(m_GetWhisperHuman, grobal2.RM_WHISPER, 0, M2Share.g_Config.btGMWhisperMsgFColor, M2Share.g_Config.btGMWhisperMsgBColor, 0, m_sCharName + "=>" + PlayObject.m_sCharName + ' ' + saystr);
                    }
                    if (PlayObject.m_GetWhisperHuman != null && !PlayObject.m_GetWhisperHuman.m_boGhost)
                    {
                        PlayObject.m_GetWhisperHuman.SendMsg(PlayObject.m_GetWhisperHuman, grobal2.RM_WHISPER, 0, M2Share.g_Config.btGMWhisperMsgFColor, M2Share.g_Config.btGMWhisperMsgBColor, 0, m_sCharName + "=>" + PlayObject.m_sCharName + ' ' + saystr);
                    }
                }
                else
                {
                    PlayObject.SendMsg(PlayObject, grobal2.RM_WHISPER, 0, M2Share.g_Config.btWhisperMsgFColor, M2Share.g_Config.btWhisperMsgBColor, 0, m_sCharName + "=> " + saystr);
                    if (m_GetWhisperHuman != null && !m_GetWhisperHuman.m_boGhost)
                    {
                        m_GetWhisperHuman.SendMsg(m_GetWhisperHuman, grobal2.RM_WHISPER, 0, M2Share.g_Config.btWhisperMsgFColor, M2Share.g_Config.btWhisperMsgBColor, 0, m_sCharName + "=>" + PlayObject.m_sCharName + ' ' + saystr);
                    }
                    if (PlayObject.m_GetWhisperHuman != null && !PlayObject.m_GetWhisperHuman.m_boGhost)
                    {
                        PlayObject.m_GetWhisperHuman.SendMsg(PlayObject.m_GetWhisperHuman, grobal2.RM_WHISPER, 0, M2Share.g_Config.btWhisperMsgFColor, M2Share.g_Config.btWhisperMsgBColor, 0, m_sCharName + "=>" + PlayObject.m_sCharName + ' ' + saystr);
                    }
                }
            }
            else
            {
                if (M2Share.UserEngine.FindOtherServerUser(whostr, ref svidx))
                {
                    M2Share.UserEngine.SendServerGroupMsg(grobal2.SS_WHISPER, svidx, whostr + '/' + m_sCharName + "=> " + saystr);
                }
                else
                {
                    SysMsg(whostr + M2Share.g_sUserNotOnLine, TMsgColor.c_Red, TMsgType.t_Hint);
                }
            }
        }

        public void WhisperRe(string SayStr, byte MsgType)
        {
            var sendwho = string.Empty;
            HUtil32.GetValidStr3(SayStr, ref sendwho, new string[] {"[", " ", "=", ">"});
            if (m_boHearWhisper && !IsBlockWhisper(sendwho))
            {
                switch (MsgType)
                {
                    case 0:
                        SendMsg(this, grobal2.RM_WHISPER, 0, M2Share.g_Config.btGMWhisperMsgFColor,
                            M2Share.g_Config.btGMWhisperMsgBColor, 0, SayStr);
                        break;
                    case 1:
                        SendMsg(this, grobal2.RM_WHISPER, 0, M2Share.g_Config.btWhisperMsgFColor,
                            M2Share.g_Config.btWhisperMsgBColor, 0, SayStr);
                        break;
                    case 2:
                        SendMsg(this, grobal2.RM_WHISPER, 0, M2Share.g_Config.btPurpleMsgFColor,
                            M2Share.g_Config.btPurpleMsgBColor, 0, SayStr);
                        break;
                }
            }
        }

        public bool IsBlockWhisper(string sName)
        {
            var result = false;
            for (var i = 0; i < this.m_BlockWhisperList.Count; i++)
            {
                if (string.Compare(sName, this.m_BlockWhisperList[i], StringComparison.Ordinal) == 0)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        public void SendSocket(string sMsg)
        {
            TMsgHeader MsgHdr;
            int nSendBytes;
            const string sExceptionMsg = "[Exception] TPlayObject::SendSocket..";
            if (m_boOffLineFlag)
            {
                return;
            }
            byte[] Buff = null;
            try
            {
                MsgHdr = new TMsgHeader
                {
                    dwCode = grobal2.RUNGATECODE,
                    nSocket = m_nSocket,
                    wGSocketIdx = (ushort)m_nGSocketIdx,
                    wIdent = grobal2.GM_DATA
                };
                if (!string.IsNullOrEmpty(sMsg))
                {
                    var bMsg = HUtil32.StringToByteAry(sMsg);
                    MsgHdr.nLength = -(bMsg.Length + 1);
                    nSendBytes = Math.Abs(MsgHdr.nLength) + 20;
                    //Buff = new byte[nSendBytes + sizeof(int)];
                    using var memoryStream = new MemoryStream();
                    var backingStream = new BinaryWriter(memoryStream);
                    backingStream.Write(nSendBytes);
                    backingStream.Write(MsgHdr.ToByte());
                    if (!string.IsNullOrEmpty(sMsg))
                    {
                        backingStream.Write(bMsg);
                        backingStream.Write((byte)0);
                    }
                    var stream = backingStream.BaseStream as MemoryStream;
                    Buff = stream.ToArray();
                }
                M2Share.RunSocket.AddGateBuffer(m_nGateIdx, Buff);
            }
            catch
            {
                M2Share.MainOutMessage(sExceptionMsg, MessageType.Error);
            }
        }
        
        private void SendSocket(TDefaultMessage DefMsg)
        {
            SendSocket(DefMsg, "");
        }

        public virtual void SendSocket(TDefaultMessage DefMsg, string sMsg)
        {
            TMsgHeader MsgHdr;
            int nSendBytes;
            const string sExceptionMsg = "[Exception] TPlayObject::SendSocket..";
            if (m_boOffLineFlag && DefMsg != null && DefMsg.Ident != grobal2.SM_OUTOFCONNECTION)
            {
                return;
            }
            byte[] Buff = null;
            try
            {
                MsgHdr = new TMsgHeader
                {
                    dwCode = grobal2.RUNGATECODE,
                    nSocket = m_nSocket,
                    wGSocketIdx = (ushort)m_nGSocketIdx,
                    wIdent = grobal2.GM_DATA
                };
                if (DefMsg != null)
                {
                    var bMsg = HUtil32.StringToByteAry(sMsg);
                    if (!string.IsNullOrEmpty(sMsg))
                    {
                        MsgHdr.nLength = bMsg.Length + 12 + 1;
                    }
                    else
                    {
                        MsgHdr.nLength = 12;
                    }
                    nSendBytes = MsgHdr.nLength + 20;
                    //Buff = new byte[nSendBytes + sizeof(int)];
                    using var memoryStream = new MemoryStream();
                    var backingStream = new BinaryWriter(memoryStream);
                    backingStream.Write(nSendBytes);
                    backingStream.Write(MsgHdr.ToByte());
                    backingStream.Write(DefMsg.ToByte());
                    if (!string.IsNullOrEmpty(sMsg))
                    {
                        backingStream.Write(bMsg);
                        backingStream.Write((byte)0);
                    }
                    var stream = backingStream.BaseStream as MemoryStream;
                    Buff = stream.ToArray();
                    // fixed (byte* pb = Buff)
                    // {
                    //     *(int*)pb = nSendBytes;
                    //     *(TMsgHeader*)(pb + sizeof(int)) = MsgHdr;
                    //     *(TDefaultMessage*)(pb + sizeof(int) + sizeof(TMsgHeader)) = DefMsg;
                    // }
                    //
                    // if (!string.IsNullOrEmpty(sMsg))
                    // {
                    //     Buffer.BlockCopy(bMsg, 0, Buff, sizeof(TDefaultMessage) + sizeof(TMsgHeader) + sizeof(int), bMsg.Length);
                    //     Buff[Buff.Length - 1] = 0;
                    // }
                }
                else
                {
                    if (!string.IsNullOrEmpty(sMsg))
                    {
                        var bMsg = HUtil32.StringToByteAry(sMsg);
                        MsgHdr.nLength = -(bMsg.Length + 1);
                        nSendBytes = Math.Abs(MsgHdr.nLength) + 20;
                        //Buff = new byte[nSendBytes + sizeof(int)];
                        using var memoryStream = new MemoryStream();
                        var backingStream = new BinaryWriter(memoryStream);
                        backingStream.Write(nSendBytes);
                        backingStream.Write(MsgHdr.ToByte());
                        if (!string.IsNullOrEmpty(sMsg))
                        {
                            backingStream.Write(bMsg);
                            backingStream.Write((byte)0);
                        }
                        var stream = backingStream.BaseStream as MemoryStream;
                        Buff = stream.ToArray();
                    }
                }
                M2Share.RunSocket.AddGateBuffer(m_nGateIdx, Buff);
            }
            catch
            {
                M2Share.MainOutMessage(sExceptionMsg, MessageType.Error);
            }
        }

        public void SendDefMessage(short wIdent, int nRecog, int nParam, int nTag, int nSeries, string sMsg)
        {
            m_DefMsg = grobal2.MakeDefaultMsg(wIdent, nRecog, nParam, nTag, nSeries);
            if (!string.IsNullOrEmpty(sMsg))
            {
                SendSocket(m_DefMsg, EDcode.EncodeString(sMsg));
            }
            else
            {
                SendSocket(m_DefMsg);
            }
        }

        private void ClientQueryUserName(int target, int x, int y)
        {
            var BaseObject = M2Share.ObjectSystem.Get(target);
            if (CretInNearXY(BaseObject, x, y))
            {
                int TagColor = GetCharColor(BaseObject);
                var Def = grobal2.MakeDefaultMsg(grobal2.SM_USERNAME, BaseObject.ObjectId, (short)TagColor, 0, 0);
                var uname = BaseObject.GetShowName();
                SendSocket(Def, EDcode.EncodeString(uname));
            }
            else
            {
                SendDefMessage(grobal2.SM_GHOST, BaseObject.ObjectId, x, y, 0, "");
            }
        }

        private byte DayBright()
        {
            byte result;
            if (m_PEnvir.Flag.boDarkness)
            {
                result = 1;
            }
            else if (m_btBright == 1)
            {
                result = 0;
            }
            else if (m_btBright == 3)
            {
                result = 1;
            }
            else
            {
                result = 2;
            }
            if (m_PEnvir.Flag.boDayLight)
            {
                result = 0;
            }
            return result;
        }

        public void RefUserState()
        {
            var n8 = 0;
            if (m_PEnvir.Flag.boFightZone)
            {
                n8 = n8 | 1;
            }
            if (m_PEnvir.Flag.boSAFE)
            {
                n8 = n8 | 2;
            }
            if (m_boInFreePKArea)
            {
                n8 = n8 | 4;
            }
            SendDefMessage(grobal2.SM_AREASTATE, n8, 0, 0, 0, "");
        }

        public void RefMyStatus()
        {
            RecalcAbilitys();
            SendMsg(this, grobal2.RM_MYSTATUS, 0, 0, 0, 0, "");
        }

        /// <summary>
        /// 祈祷套装生效
        /// </summary>
        private void ProcessSpiritSuite()
        {
            TItem StdItem;
            TUserItem UseItem;
            if (!M2Share.g_Config.boSpiritMutiny || !m_bopirit)
            {
                return;
            }
            m_bopirit = false;
            for (var i = m_UseItems.GetLowerBound(0); i <= m_UseItems.GetUpperBound(0); i++)
            {
                UseItem = m_UseItems[i];
                if (UseItem == null)
                {
                    continue;
                }
                if (UseItem.wIndex <= 0)
                {
                    continue;
                }
                StdItem = M2Share.UserEngine.GetStdItem(UseItem.wIndex);
                if (StdItem != null)
                {
                    if (StdItem.Shape == 126 || StdItem.Shape == 127 || StdItem.Shape == 128 || StdItem.Shape == 129)
                    {
                        SendDelItems(UseItem);
                        UseItem.wIndex = 0;
                    }
                }
            }
            RecalcAbilitys();
            M2Share.g_dwSpiritMutinyTick = HUtil32.GetTickCount() + M2Share.g_Config.dwSpiritMutinyTime;
            M2Share.UserEngine.SendBroadCastMsg("神之祈祷，天地震怒，尸横遍野...", TMsgType.t_System);
            SysMsg("祈祷发出强烈的宇宙效应", TMsgColor.c_Green, TMsgType.t_Hint);
        }

        private void LogonTimcCost()
        {
            int n08;
            string sC;
            if (m_nPayMent == 2 || M2Share.g_Config.boTestServer)
            {
                n08 = (HUtil32.GetTickCount() - m_dwLogonTick) / 1000;
            }
            else
            {
                n08 = 0;
            }
            sC = m_sIPaddr + "\t" + m_sUserID + "\t" + m_sCharName + "\t" + n08.ToString() + "\t" + m_dLogonTime.ToString("yyyy-mm-dd hh:mm:ss") + "\t" + DateTime.Now.ToString("yyyy-mm-dd hh:mm:ss") + "\t" + m_nPayMode.ToString();
            M2Share.AddLogonCostLog(sC);
            if (m_nPayMode == 2)
            {
                IdSrvClient.Instance.SendLogonCostMsg(m_sUserID, n08 / 60);
            }
        }

        private bool ClientHitXY(short wIdent, int nX, int nY, byte nDir, bool boLateDelivery, ref int dwDelayTime)
        {
            var result = false;
            short n14 = 0;
            short n18 = 0;
            TItem StdItem;
            int dwAttackTime;
            int dwCheckTime;
            const string sExceptionMsg = "[Exception] TPlayObject::ClientHitXY";
            dwDelayTime = 0;
            try
            {
                if (!m_boCanHit)
                {
                    return result;
                }
                if (m_boDeath || m_wStatusTimeArr[grobal2.POISON_STONE] != 0 && !M2Share.g_Config.ClientConf.boParalyCanHit)
                {
                    return result;
                }
                // 防麻
                if (!boLateDelivery)
                {
                    if (!CheckActionStatus(wIdent, ref dwDelayTime))
                    {
                        m_boFilterAction = false;
                        return result;
                    }
                    m_boFilterAction = true;
                    dwAttackTime = HUtil32._MAX(0, (M2Share.g_Config.dwHitIntervalTime - m_nHitSpeed) * M2Share.g_Config.ClientConf.btItemSpeed);// 防止负数出错 武器速度控制
                    dwCheckTime = HUtil32.GetTickCount() - m_dwAttackTick;
                    if (dwCheckTime < dwAttackTime)
                    {
                        m_dwAttackCount++;
                        dwDelayTime = dwAttackTime - dwCheckTime;
                        if (dwDelayTime > M2Share.g_Config.dwDropOverSpeed)
                        {
                            if (m_dwAttackCount >= 4)
                            {
                                m_dwAttackTick = HUtil32.GetTickCount();
                                m_dwAttackCount = 0;
                                dwDelayTime = M2Share.g_Config.dwDropOverSpeed;
                                if (m_boTestSpeedMode)
                                {
                                    SysMsg("攻击忙复位！！！" + dwDelayTime, TMsgColor.c_Red, TMsgType.t_Hint);
                                }
                            }
                            else
                            {
                                m_dwAttackCount = 0;
                            }
                            return result;
                        }
                        else
                        {
                            if (m_boTestSpeedMode)
                            {
                                SysMsg("攻击步忙！！！" + dwDelayTime, TMsgColor.c_Red, TMsgType.t_Hint);
                            }
                            return result;
                        }
                    }
                }
                if (nX == m_nCurrX && nY == m_nCurrY)
                {
                    result = true;
                    m_dwAttackTick = HUtil32.GetTickCount();
                    if (wIdent == grobal2.CM_HEAVYHIT && m_UseItems[grobal2.U_WEAPON] != null && m_UseItems[grobal2.U_WEAPON].Dura > 0)// 挖矿
                    {
                        if (GetFrontPosition(ref n14, ref n18) && !m_PEnvir.CanWalk(n14, n18, false))
                        {
                            StdItem = M2Share.UserEngine.GetStdItem(m_UseItems[grobal2.U_WEAPON].wIndex);
                            if (StdItem != null && StdItem.Shape == 19)
                            {
                                if (PileStones(n14, n18))
                                {
                                    SendSocket("=DIG");
                                }
                                m_nHealthTick -= 30;
                                m_nSpellTick -= 50;
                                m_nSpellTick = HUtil32._MAX(0, m_nSpellTick);
                                m_nPerHealth -= 2;
                                m_nPerSpell -= 2;
                                return result;
                            }
                        }
                    }
                    if (wIdent == grobal2.CM_HIT)
                    {
                        AttackDir(null, 0, nDir);
                    }
                    if (wIdent == grobal2.CM_HEAVYHIT)
                    {
                        AttackDir(null, 1, nDir);
                    }
                    if (wIdent == grobal2.CM_BIGHIT)
                    {
                        AttackDir(null, 2, nDir);
                    }
                    if (wIdent == grobal2.CM_POWERHIT)
                    {
                        AttackDir(null, 3, nDir);
                    }
                    if (wIdent == grobal2.CM_LONGHIT)
                    {
                        AttackDir(null, 4, nDir);
                    }
                    if (wIdent == grobal2.CM_WIDEHIT)
                    {
                        AttackDir(null, 5, nDir);
                    }
                    if (wIdent == grobal2.CM_FIREHIT)
                    {
                        AttackDir(null, 7, nDir);
                    }
                    if (wIdent == grobal2.CM_CRSHIT)
                    {
                        AttackDir(null, 8, nDir);
                    }
                    if (wIdent == grobal2.CM_TWINHIT)
                    {
                        AttackDir(null, 9, nDir);
                    }
                    if (wIdent == grobal2.CM_42HIT)
                    {
                        AttackDir(null, 10, nDir);
                    }
                    if (wIdent == grobal2.CM_42HIT)
                    {
                        AttackDir(null, 11, nDir);
                    }
                    if (m_MagicPowerHitSkill != null && m_UseItems[grobal2.U_WEAPON].Dura > 0)
                    {
                        m_btAttackSkillCount -= 1;
                        if (m_btAttackSkillPointCount == m_btAttackSkillCount)
                        {
                            m_boPowerHit = true;
                            SendSocket("+PWR");
                        }
                        if (m_btAttackSkillCount <= 0)
                        {
                            m_btAttackSkillCount = (byte)(7 - m_MagicPowerHitSkill.btLevel);
                            m_btAttackSkillPointCount = (byte)M2Share.RandomNumber.Random(m_btAttackSkillCount);
                        }
                    }
                    m_nHealthTick -= 30;
                    m_nSpellTick -= 100;
                    m_nSpellTick = HUtil32._MAX(0, m_nSpellTick);
                    m_nPerHealth -= 2;
                    m_nPerSpell -= 2;
                }
            }
            catch (Exception e)
            {
                M2Share.ErrorMessage(sExceptionMsg);
                M2Share.ErrorMessage(e.Message);
            }
            return result;
        }

        private bool ClientHorseRunXY(short wIdent, int nX, int nY, bool boLateDelivery, ref int dwDelayTime)
        {
            var result = false;
            byte n14;
            int dwCheckTime;
            dwDelayTime = 0;
            if (!m_boCanRun)
            {
                return result;
            }
            if (m_boDeath || m_wStatusTimeArr[grobal2.POISON_STONE] != 0 && !M2Share.g_Config.ClientConf.boParalyCanRun)// 防麻
            {
                return result;
            }
            if (!boLateDelivery)
            {
                if (!CheckActionStatus(wIdent, ref dwDelayTime))
                {
                    m_boFilterAction = false;
                    return result;
                }
                m_boFilterAction = true;
                dwCheckTime = HUtil32.GetTickCount() - m_dwMoveTick;
                if (dwCheckTime < M2Share.g_Config.dwRunIntervalTime)
                {
                    m_dwMoveCount++;
                    dwDelayTime = M2Share.g_Config.dwRunIntervalTime - dwCheckTime;
                    if (dwDelayTime > M2Share.g_Config.dwDropOverSpeed)
                    {
                        if (m_dwMoveCount >= 4)
                        {
                            m_dwMoveTick = HUtil32.GetTickCount();
                            m_dwMoveCount = 0;
                            dwDelayTime = M2Share.g_Config.dwDropOverSpeed;
                            if (m_boTestSpeedMode)
                            {
                                SysMsg("马跑步忙复位！！！" + dwDelayTime, TMsgColor.c_Red, TMsgType.t_Hint);
                            }
                        }
                        else
                        {
                            m_dwMoveCount = 0;
                        }
                        return result;
                    }
                    else
                    {
                        if (m_boTestSpeedMode)
                        {
                            SysMsg("马跑步忙！！！" + dwDelayTime, TMsgColor.c_Red, TMsgType.t_Hint);
                        }
                        return result;
                    }
                }
            }
            m_dwMoveTick = HUtil32.GetTickCount();
            m_bo316 = false;
#if Debug
            Debug.WriteLine(format("当前X:{0} 当前Y:{1} 目标X:{2} 目标Y:{3}", new object[] {this.m_nCurrX, this.m_nCurrY, nX, nY}), TMsgColor.c_Green, TMsgType.t_Hint);
#endif
            n14 = M2Share.GetNextDirection(m_nCurrX, m_nCurrY, nX, nY);
            if (HorseRunTo(n14, false))
            {
                if (m_boTransparent && m_boHideMode)
                {
                    m_wStatusTimeArr[grobal2.STATE_TRANSPARENT] = 1;
                }
                if (m_bo316 || m_nCurrX == nX && m_nCurrY == nY)
                {
                    result = true;
                }
                m_nHealthTick -= 60;
                m_nSpellTick -= 10;
                m_nSpellTick = HUtil32._MAX(0, m_nSpellTick);
                m_nPerHealth -= 1;
                m_nPerSpell -= 1;
            }
            else
            {
                m_dwMoveCount = 0;
                m_dwMoveCountA = 0;
            }
            return result;
        }

        private bool ClientSpellXY(short wIdent, int nKey, int nTargetX, int nTargetY, TBaseObject TargeTBaseObject, bool boLateDelivery, ref int dwDelayTime)
        {
            var result = false;
            dwDelayTime = 0;
            if (!m_boCanSpell)
            {
                return result;
            }
            if (m_boDeath || m_wStatusTimeArr[grobal2.POISON_STONE] != 0 && !M2Share.g_Config.ClientConf.boParalyCanSpell)// 防麻
            {
                return result;
            }
            var UserMagic = GetMagicInfo(nKey);
            if (UserMagic == null)
            {
                return result;
            }
            var boIsWarrSkill = M2Share.MagicManager.IsWarrSkill(UserMagic.wMagIdx);
            if (!boLateDelivery && !boIsWarrSkill)
            {
                if (!CheckActionStatus(wIdent, ref dwDelayTime))
                {
                    m_boFilterAction = false;
                    return result;
                }
                m_boFilterAction = true;
                var dwCheckTime = HUtil32.GetTickCount() - m_dwMagicAttackTick;
                if (dwCheckTime < m_dwMagicAttackInterval)
                {
                    m_dwMagicAttackCount++;
                    dwDelayTime = m_dwMagicAttackInterval - dwCheckTime;
                    if (dwDelayTime > M2Share.g_Config.dwMagicHitIntervalTime / 3)
                    {
                        if (m_dwMagicAttackCount >= 4)
                        {
                            m_dwMagicAttackTick = HUtil32.GetTickCount();
                            m_dwMagicAttackCount = 0;
                            dwDelayTime = M2Share.g_Config.dwMagicHitIntervalTime / 3;
                            if (m_boTestSpeedMode)
                            {
                                SysMsg("魔法忙复位！！！" + dwDelayTime, TMsgColor.c_Red, TMsgType.t_Hint);
                            }
                        }
                        else
                        {
                            m_dwMagicAttackCount = 0;
                        }
                        return result;
                    }
                    else
                    {
                        if (m_boTestSpeedMode)
                        {
                            SysMsg("魔法忙！！！" + dwDelayTime, TMsgColor.c_Red, TMsgType.t_Hint);
                        }
                        return result;
                    }
                }
            }
            m_nSpellTick -= 450;
            m_nSpellTick = HUtil32._MAX(0, m_nSpellTick);
            if (boIsWarrSkill)
            {
                // m_dwMagicAttackInterval:=0;
                // m_dwMagicAttackInterval:=g_Config.dwMagicHitIntervalTime; //01/21 改成此行
            }
            else
            {
                m_dwMagicAttackInterval = UserMagic.MagicInfo.dwDelayTime + M2Share.g_Config.dwMagicHitIntervalTime;
            }
            m_dwMagicAttackTick = HUtil32.GetTickCount();
            ushort nSpellPoint;
            switch (UserMagic.wMagIdx)
            {
                case grobal2.SKILL_ERGUM:
                    if (m_MagicErgumSkill != null)
                    {
                        if (!m_boUseThrusting)
                        {
                            ThrustingOnOff(true);
                            SendSocket("+LNG");
                        }
                        else
                        {
                            ThrustingOnOff(false);
                            SendSocket("+ULNG");
                        }
                    }
                    result = true;
                    break;
                case grobal2.SKILL_BANWOL:
                    if (m_MagicBanwolSkill != null)
                    {
                        if (!m_boUseHalfMoon)
                        {
                            HalfMoonOnOff(true);
                            SendSocket("+WID");
                        }
                        else
                        {
                            HalfMoonOnOff(false);
                            SendSocket("+UWID");
                        }
                    }
                    result = true;
                    break;
                case grobal2.SKILL_REDBANWOL:
                    if (m_MagicRedBanwolSkill != null)
                    {
                        if (!m_boRedUseHalfMoon)
                        {
                            RedHalfMoonOnOff(true);
                            SendSocket("+WID");
                        }
                        else
                        {
                            RedHalfMoonOnOff(false);
                            SendSocket("+UWID");
                        }
                    }
                    result = true;
                    break;
                case grobal2.SKILL_FIRESWORD:
                    if (m_MagicFireSwordSkill != null)
                    {
                        if (AllowFireHitSkill())
                        {
                            nSpellPoint = GetSpellPoint(UserMagic);
                            if (m_WAbil.MP >= nSpellPoint)
                            {
                                if (nSpellPoint > 0)
                                {
                                    DamageSpell(nSpellPoint);
                                    HealthSpellChanged();
                                }
                                SendSocket("+FIR");
                            }
                        }
                    }
                    result = true;
                    break;
                case grobal2.SKILL_MOOTEBO:
                    result = true;
                    if (HUtil32.GetTickCount() - m_dwDoMotaeboTick > 3 * 1000)
                    {
                        m_dwDoMotaeboTick = HUtil32.GetTickCount();
                        m_btDirection = (byte)nTargetX;
                        nSpellPoint = GetSpellPoint(UserMagic);
                        if (m_WAbil.MP >= nSpellPoint)
                        {
                            if (nSpellPoint > 0)
                            {
                                DamageSpell(nSpellPoint);
                                HealthSpellChanged();
                            }
                            if (DoMotaebo(m_btDirection, UserMagic.btLevel))
                            {
                                if (UserMagic.btLevel < 3)
                                {
                                    if (UserMagic.MagicInfo.TrainLevel[UserMagic.btLevel] < m_Abil.Level)
                                    {
                                        TrainSkill(UserMagic, M2Share.RandomNumber.Random(3) + 1);
                                        if (!CheckMagicLevelup(UserMagic))
                                        {
                                            SendDelayMsg(this, grobal2.RM_MAGIC_LVEXP, 0, UserMagic.MagicInfo.wMagicID, UserMagic.btLevel, UserMagic.nTranPoint, "", 1000);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
                case grobal2.SKILL_CROSSMOON:
                    if (m_MagicCrsSkill != null)
                    {
                        if (!m_boCrsHitkill)
                        {
                            SkillCrsOnOff(true);
                            SendSocket("+CRS");
                        }
                        else
                        {
                            SkillCrsOnOff(false);
                            SendSocket("+UCRS");
                        }
                    }
                    result = true;
                    break;
                case grobal2.SKILL_TWINBLADE:// 狂风斩
                    if (m_MagicTwnHitSkill != null)
                    {
                        if (AllowTwinHitSkill())
                        {
                            nSpellPoint = GetSpellPoint(UserMagic);
                            if (m_WAbil.MP >= nSpellPoint)
                            {
                                if (nSpellPoint > 0)
                                {
                                    DamageSpell(nSpellPoint);
                                    HealthSpellChanged();
                                }
                                SendSocket("+TWN");
                            }
                        }
                    }
                    result = true;
                    break;
                case 43:// 破空剑
                    if (m_Magic43Skill != null)
                    {
                        if (!m_bo43kill)
                        {
                            Skill43OnOff(true);
                            SendSocket("+CID");
                        }
                        else
                        {
                            Skill43OnOff(false);
                            SendSocket("+UCID");
                        }
                    }
                    result = true;
                    break;
                default:
                    m_btDirection = M2Share.GetNextDirection(m_nCurrX, m_nCurrY, nTargetX, nTargetY); ;
                    TBaseObject BaseObject = null;
                    if (CretInNearXY(TargeTBaseObject, nTargetX, nTargetY)) // 检查目标角色，与目标座标误差范围，如果在误差范围内则修正目标座标
                    {
                        BaseObject = TargeTBaseObject;
                        nTargetX = BaseObject.m_nCurrX;
                        nTargetY = BaseObject.m_nCurrY;
                    }
                    if (!DoSpell(UserMagic, (short)nTargetX, (short)nTargetY, BaseObject))
                    {
                        SendRefMsg(grobal2.RM_MAGICFIREFAIL, 0, 0, 0, 0, "");
                    }
                    result = true;
                    break;
            }
            return result;
        }

        public bool RunTo(byte btDir, bool boFlag, int nDestX, int nDestY)
        {
            const string sExceptionMsg = "[Exception] TBaseObject::RunTo";
            var result = false;
            try
            {
                int nOldX = m_nCurrX;
                int nOldY = m_nCurrY;
                m_btDirection = btDir;
                switch (btDir)
                {
                    case grobal2.DR_UP:
                        if (m_nCurrY > 1 && m_PEnvir.CanWalkEx(m_nCurrX, m_nCurrY - 1, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.CanWalkEx(m_nCurrX, m_nCurrY - 2, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.MoveToMovingObject(m_nCurrX, m_nCurrY, this, m_nCurrX, m_nCurrY - 2, true) > 0)
                        {
                            m_nCurrY -= 2;
                        }
                        break;
                    case grobal2.DR_UPRIGHT:
                        if (m_nCurrX < m_PEnvir.wWidth - 2 && m_nCurrY > 1 && m_PEnvir.CanWalkEx(m_nCurrX + 1, m_nCurrY - 1, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.CanWalkEx(m_nCurrX + 2, m_nCurrY - 2, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.MoveToMovingObject(m_nCurrX, m_nCurrY, this, m_nCurrX + 2, m_nCurrY - 2, true) > 0)
                        {
                            m_nCurrX += 2;
                            m_nCurrY -= 2;
                        }
                        break;
                    case grobal2.DR_RIGHT:
                        if (m_nCurrX < m_PEnvir.wWidth - 2 && m_PEnvir.CanWalkEx(m_nCurrX + 1, m_nCurrY, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.CanWalkEx(m_nCurrX + 2, m_nCurrY, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.MoveToMovingObject(m_nCurrX, m_nCurrY, this, m_nCurrX + 2, m_nCurrY, true) > 0)
                        {
                            m_nCurrX += 2;
                        }
                        break;
                    case grobal2.DR_DOWNRIGHT:
                        if (m_nCurrX < m_PEnvir.wWidth - 2 && m_nCurrY < m_PEnvir.wHeight - 2 && m_PEnvir.CanWalkEx(m_nCurrX + 1, m_nCurrY + 1, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.CanWalkEx(m_nCurrX + 2, m_nCurrY + 2, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.MoveToMovingObject(m_nCurrX, m_nCurrY, this, m_nCurrX + 2, m_nCurrY + 2, true) > 0)
                        {
                            m_nCurrX += 2;
                            m_nCurrY += 2;
                        }
                        break;
                    case grobal2.DR_DOWN:
                        if (m_nCurrY < m_PEnvir.wHeight - 2 && m_PEnvir.CanWalkEx(m_nCurrX, m_nCurrY + 1, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.CanWalkEx(m_nCurrX, m_nCurrY + 2, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.MoveToMovingObject(m_nCurrX, m_nCurrY, this, m_nCurrX, m_nCurrY + 2, true) > 0)
                        {
                            m_nCurrY += 2;
                        }
                        break;
                    case grobal2.DR_DOWNLEFT:
                        if (m_nCurrX > 1 && m_nCurrY < m_PEnvir.wHeight - 2 && m_PEnvir.CanWalkEx(m_nCurrX - 1, m_nCurrY + 1, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.CanWalkEx(m_nCurrX - 2, m_nCurrY + 2, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.MoveToMovingObject(m_nCurrX, m_nCurrY, this, m_nCurrX - 2, m_nCurrY + 2, true) > 0)
                        {
                            m_nCurrX -= 2;
                            m_nCurrY += 2;
                        }
                        break;
                    case grobal2.DR_LEFT:
                        if (m_nCurrX > 1 && m_PEnvir.CanWalkEx(m_nCurrX - 1, m_nCurrY, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.CanWalkEx(m_nCurrX - 2, m_nCurrY, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.MoveToMovingObject(m_nCurrX, m_nCurrY, this, m_nCurrX - 2, m_nCurrY, true) > 0)
                        {
                            m_nCurrX -= 2;
                        }
                        break;
                    case grobal2.DR_UPLEFT:
                        if (m_nCurrX > 1 && m_nCurrY > 1 && m_PEnvir.CanWalkEx(m_nCurrX - 1, m_nCurrY - 1, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.CanWalkEx(m_nCurrX - 2, m_nCurrY - 2, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.MoveToMovingObject(m_nCurrX, m_nCurrY, this, m_nCurrX - 2, m_nCurrY - 2, true) > 0)
                        {
                            m_nCurrX -= 2;
                            m_nCurrY -= 2;
                        }
                        break;
                }
                if (m_nCurrX != nOldX || m_nCurrY != nOldY)
                {
                    if (Walk(grobal2.RM_RUN))
                    {
                        result = true;
                    }
                    else
                    {
                        m_nCurrX = (short)nOldX;
                        m_nCurrY = (short)nOldY;
                        m_PEnvir.MoveToMovingObject(nOldX, nOldY, this, m_nCurrX, m_nCurrX, true);
                    }
                }
            }
            catch
            {
                M2Share.ErrorMessage(sExceptionMsg);
            }
            return result;
        }

        private bool HorseRunTo(byte btDir, bool boFlag)
        {
            int n10;
            int n14;
            const string sExceptionMsg = "[Exception] TPlayObject::HorseRunTo";
            var result = false;
            try
            {
                n10 = m_nCurrX;
                n14 = m_nCurrY;
                m_btDirection = btDir;
                switch (btDir)
                {
                    case grobal2.DR_UP:
                        if (m_nCurrY > 2 && m_PEnvir.CanWalkEx(m_nCurrX, m_nCurrY - 1, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.CanWalkEx(m_nCurrX, m_nCurrY - 2, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.CanWalkEx(m_nCurrX, m_nCurrY - 3, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.MoveToMovingObject(m_nCurrX, m_nCurrY, this, m_nCurrX, m_nCurrY - 3, true) > 0)
                        {
                            m_nCurrY -= 3;
                        }
                        break;
                    case grobal2.DR_UPRIGHT:
                        if (m_nCurrX < m_PEnvir.wWidth - 3 && m_nCurrY > 2 && m_PEnvir.CanWalkEx(m_nCurrX + 1, m_nCurrY - 1, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.CanWalkEx(m_nCurrX + 2, m_nCurrY - 2, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.CanWalkEx(m_nCurrX + 3, m_nCurrY - 3, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.MoveToMovingObject(m_nCurrX, m_nCurrY, this, m_nCurrX + 3, m_nCurrY - 3, true) > 0)
                        {
                            m_nCurrX += 3;
                            m_nCurrY -= 3;
                        }
                        break;
                    case grobal2.DR_RIGHT:
                        if (m_nCurrX < m_PEnvir.wWidth - 3 && m_PEnvir.CanWalkEx(m_nCurrX + 1, m_nCurrY, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.CanWalkEx(m_nCurrX + 2, m_nCurrY, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.CanWalkEx(m_nCurrX + 3, m_nCurrY, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.MoveToMovingObject(m_nCurrX, m_nCurrY, this, m_nCurrX + 3, m_nCurrY, true) > 0)
                        {
                            m_nCurrX += 3;
                        }
                        break;
                    case grobal2.DR_DOWNRIGHT:
                        if (m_nCurrX < m_PEnvir.wWidth - 3 && m_nCurrY < m_PEnvir.wHeight - 3 && m_PEnvir.CanWalkEx(m_nCurrX + 1, m_nCurrY + 1, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.CanWalkEx(m_nCurrX + 2, m_nCurrY + 2, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.CanWalkEx(m_nCurrX + 3, m_nCurrY + 3, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.MoveToMovingObject(m_nCurrX, m_nCurrY, this, m_nCurrX + 3, m_nCurrY + 3, true) > 0)
                        {
                            m_nCurrX += 3;
                            m_nCurrY += 3;
                        }
                        break;
                    case grobal2.DR_DOWN:
                        if (m_nCurrY < m_PEnvir.wHeight - 3 && m_PEnvir.CanWalkEx(m_nCurrX, m_nCurrY + 1, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.CanWalkEx(m_nCurrX, m_nCurrY + 2, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.CanWalkEx(m_nCurrX, m_nCurrY + 3, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.MoveToMovingObject(m_nCurrX, m_nCurrY, this, m_nCurrX, m_nCurrY + 3, true) > 0)
                        {
                            m_nCurrY += 3;
                        }
                        break;
                    case grobal2.DR_DOWNLEFT:
                        if (m_nCurrX > 2 && m_nCurrY < m_PEnvir.wHeight - 3 && m_PEnvir.CanWalkEx(m_nCurrX - 1, m_nCurrY + 1, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.CanWalkEx(m_nCurrX - 2, m_nCurrY + 2, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.CanWalkEx(m_nCurrX - 3, m_nCurrY + 3, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.MoveToMovingObject(m_nCurrX, m_nCurrY, this, m_nCurrX - 3, m_nCurrY + 3, true) > 0)
                        {
                            m_nCurrX -= 3;
                            m_nCurrY += 3;
                        }
                        break;
                    case grobal2.DR_LEFT:
                        if (m_nCurrX > 2 && m_PEnvir.CanWalkEx(m_nCurrX - 1, m_nCurrY, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.CanWalkEx(m_nCurrX - 2, m_nCurrY, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.CanWalkEx(m_nCurrX - 3, m_nCurrY, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.MoveToMovingObject(m_nCurrX, m_nCurrY, this, m_nCurrX - 3, m_nCurrY, true) > 0)
                        {
                            m_nCurrX -= 3;
                        }
                        break;
                    case grobal2.DR_UPLEFT:
                        if (m_nCurrX > 2 && m_nCurrY > 2 && m_PEnvir.CanWalkEx(m_nCurrX - 1, m_nCurrY - 1, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.CanWalkEx(m_nCurrX - 2, m_nCurrY - 2, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.CanWalkEx(m_nCurrX - 3, m_nCurrY - 3, M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll) && m_PEnvir.MoveToMovingObject(m_nCurrX, m_nCurrY, this, m_nCurrX - 3, m_nCurrY - 3, true) > 0)
                        {
                            m_nCurrX -= 3;
                            m_nCurrY -= 3;
                        }
                        break;
                }
                if (m_nCurrX != n10 || m_nCurrY != n14)
                {
                    if (Walk(grobal2.RM_HORSERUN))
                    {
                        result = true;
                    }
                    else
                    {
                        m_nCurrX = (short)n10;
                        m_nCurrY = (short)n14;
                        m_PEnvir.MoveToMovingObject(n10, n14, this, m_nCurrX, m_nCurrX, true);
                    }
                }
            }
            catch
            {
                M2Share.ErrorMessage(sExceptionMsg);
            }
            return result;
        }

        private bool ClientRunXY(short wIdent, int nX, int nY, int nFlag, ref int dwDelayTime)
        {
            bool result = false;
            byte nDir;
            dwDelayTime = 0;
            if (!m_boCanRun)
            {
                return result;
            }
            if (m_boDeath || m_wStatusTimeArr[grobal2.POISON_STONE] != 0 && !M2Share.g_Config.ClientConf.boParalyCanRun)
            {
                return result;
            }
            // 防麻
            if (nFlag != wIdent)
            {
                if (!CheckActionStatus(wIdent, ref dwDelayTime))
                {
                    m_boFilterAction = false;
                    return result;
                }
                m_boFilterAction = true;
                int dwCheckTime = HUtil32.GetTickCount() - m_dwMoveTick;
                if (dwCheckTime < M2Share.g_Config.dwRunIntervalTime)
                {
                    m_dwMoveCount++;
                    dwDelayTime = M2Share.g_Config.dwRunIntervalTime - dwCheckTime;
                    if (dwDelayTime > M2Share.g_Config.dwRunIntervalTime / 3)
                    {
                        if (m_dwMoveCount >= 4)
                        {
                            m_dwMoveTick = HUtil32.GetTickCount();
                            m_dwMoveCount = 0;
                            dwDelayTime = M2Share.g_Config.dwRunIntervalTime / 3;
                            if (m_boTestSpeedMode)
                            {
                                SysMsg("跑步忙复位！！！" + dwDelayTime, TMsgColor.c_Red, TMsgType.t_Hint);
                            }
                        }
                        else
                        {
                            m_dwMoveCount = 0;
                        }
                        return result;
                    }
                    else
                    {
                        if (m_boTestSpeedMode)
                        {
                            SysMsg("跑步忙！！！" + dwDelayTime, TMsgColor.c_Red, TMsgType.t_Hint);
                        }
                        return result;
                    }
                }
            }
            m_dwMoveTick = HUtil32.GetTickCount();
            m_bo316 = false;
            nDir = M2Share.GetNextDirection(m_nCurrX, m_nCurrY, nX, nY);
            if (RunTo(nDir, false, nX, nY))
            {
                if (m_boTransparent && m_boHideMode)
                {
                    m_wStatusTimeArr[grobal2.STATE_TRANSPARENT] = 1;
                }
                if (m_bo316 || m_nCurrX == nX && m_nCurrY == nY)
                {
                    result = true;
                }
                m_nHealthTick -= 60;
                m_nSpellTick -= 10;
                m_nSpellTick = HUtil32._MAX(0, m_nSpellTick);
                m_nPerHealth -= 1;
                m_nPerSpell -= 1;
            }
            else
            {
                m_dwMoveCount = 0;
                m_dwMoveCountA = 0;
            }
            return result;
        }

        private bool ClientWalkXY(short wIdent, int nX, int nY, bool boLateDelivery, ref int dwDelayTime)
        {
            bool result = false;
            int n14;
            int n18;
            int n1C;
            dwDelayTime = 0;
            if (!m_boCanWalk)
            {
                return result;
            }
            if (m_boDeath || m_wStatusTimeArr[grobal2.POISON_STONE] != 0 && !M2Share.g_Config.ClientConf.boParalyCanWalk)
            {
                return result;
            }
            // 防麻
            if (!boLateDelivery)
            {
                if (!CheckActionStatus(wIdent, ref dwDelayTime))
                {
                    m_boFilterAction = false;
                    return result;
                }
                m_boFilterAction = true;
                int dwCheckTime = HUtil32.GetTickCount() - m_dwMoveTick;
                if (dwCheckTime < M2Share.g_Config.dwWalkIntervalTime)
                {
                    m_dwMoveCount++;
                    dwDelayTime = M2Share.g_Config.dwWalkIntervalTime - dwCheckTime;
                    if (dwDelayTime > M2Share.g_Config.dwWalkIntervalTime / 3)
                    {
                        if (m_dwMoveCount >= 4)
                        {
                            m_dwMoveTick = HUtil32.GetTickCount();
                            m_dwMoveCount = 0;
                            dwDelayTime = M2Share.g_Config.dwWalkIntervalTime / 3;
                            if (m_boTestSpeedMode)
                            {
                                SysMsg("走路忙复位！！！" + dwDelayTime, TMsgColor.c_Red, TMsgType.t_Hint);
                            }
                        }
                        else
                        {
                            m_dwMoveCount = 0;
                        }
                        return result;
                    }
                    else
                    {
                        if (m_boTestSpeedMode)
                        {
                            SysMsg("走路忙！！！" + dwDelayTime, TMsgColor.c_Red, TMsgType.t_Hint);
                        }
                        return result;
                    }
                }
            }
            m_dwMoveTick = HUtil32.GetTickCount();
            m_bo316 = false;
            n18 = m_nCurrX;
            n1C = m_nCurrY;
            n14 = M2Share.GetNextDirection(m_nCurrX, m_nCurrY, nX, nY);
            if (!m_boClientFlag)
            {
                if (n14 == 0 && m_nStep == 0)
                {
                    m_nStep++;
                }
                else if (n14 == 4 && m_nStep == 1)
                {
                    m_nStep++;
                }
                else if (n14 == 6 && m_nStep == 2)
                {
                    m_nStep++;
                }
                else if (n14 == 2 && m_nStep == 3)
                {
                    m_nStep++;
                }
                else if (n14 == 1 && m_nStep == 4)
                {
                    m_nStep++;
                }
                else if (n14 == 5 && m_nStep == 5)
                {
                    m_nStep++;
                }
                else if (n14 == 7 && m_nStep == 6)
                {
                    m_nStep++;
                }
                else if (n14 == 3 && m_nStep == 7)
                {
                    m_nStep++;
                }
                else
                {
                    m_nGameGold -= m_nStep;
                    GameGoldChanged();
                    m_nStep = 0;
                }
                if (m_nStep != 0)
                {
                    m_nGameGold++;
                    GameGoldChanged();
                }
            }
            if (WalkTo((byte)n14, false))
            {
                if (m_bo316 || m_nCurrX == nX && m_nCurrY == nY)
                {
                    result = true;
                }
                m_nHealthTick -= 10;
            }
            else
            {
                m_dwMoveCount = 0;
                m_dwMoveCountA = 0;
            }
            return result;
        }

        protected void ThrustingOnOff(bool boSwitch)
        {
            m_boUseThrusting = boSwitch;
            if (m_boUseThrusting)
            {
                SysMsg(M2Share.sThrustingOn, TMsgColor.c_Green, TMsgType.t_Hint);
            }
            else
            {
                SysMsg(M2Share.sThrustingOff, TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        protected void HalfMoonOnOff(bool boSwitch)
        {
            m_boUseHalfMoon = boSwitch;
            if (m_boUseHalfMoon)
            {
                SysMsg(M2Share.sHalfMoonOn, TMsgColor.c_Green, TMsgType.t_Hint);
            }
            else
            {
                SysMsg(M2Share.sHalfMoonOff, TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        protected void RedHalfMoonOnOff(bool boSwitch)
        {
            m_boRedUseHalfMoon = boSwitch;
            if (m_boRedUseHalfMoon)
            {
                SysMsg(M2Share.sRedHalfMoonOn, TMsgColor.c_Green, TMsgType.t_Hint);
            }
            else
            {
                SysMsg(M2Share.sRedHalfMoonOff, TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        protected void SkillCrsOnOff(bool boSwitch)
        {
            m_boCrsHitkill = boSwitch;
            if (m_boCrsHitkill)
            {
                SysMsg(M2Share.sCrsHitOn, TMsgColor.c_Green, TMsgType.t_Hint);
            }
            else
            {
                SysMsg(M2Share.sCrsHitOff, TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        protected void SkillTwinOnOff(bool boSwitch)
        {
            m_boTwinHitSkill = boSwitch;
            if (m_boTwinHitSkill)
            {
                SysMsg(M2Share.sTwinHitOn, TMsgColor.c_Green, TMsgType.t_Hint);
            }
            else
            {
                SysMsg(M2Share.sTwinHitOff, TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        private void Skill43OnOff(bool boSwitch)
        {
            m_bo43kill = boSwitch;
            if (m_bo43kill)
            {
                SysMsg("开启破空剑", TMsgColor.c_Green, TMsgType.t_Hint);
            }
            else
            {
                SysMsg("关闭破空剑", TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        private bool AllowFireHitSkill()
        {
            var result = false;
            if (HUtil32.GetTickCount() - m_dwLatestFireHitTick > 10 * 1000)
            {
                m_dwLatestFireHitTick = HUtil32.GetTickCount();
                m_boFireHitSkill = true;
                SysMsg(M2Share.sFireSpiritsSummoned, TMsgColor.c_Green, TMsgType.t_Hint);
                result = true;
            }
            else
            {
                SysMsg(M2Share.sFireSpiritsFail, TMsgColor.c_Red, TMsgType.t_Hint);
            }
            return result;
        }

        private bool AllowTwinHitSkill()
        {
            m_dwLatestTwinHitTick = HUtil32.GetTickCount();
            m_boTwinHitSkill = true;
            SysMsg("twin hit skill charged", TMsgColor.c_Green, TMsgType.t_Hint);
            return true;
        }

        private void ClientClickNPC(int NPC)
        {
            if (!m_boCanDeal)
            {
                SendMsg(M2Share.g_ManageNPC, grobal2.RM_MENU_OK, 0, ObjectId, 0, 0, M2Share.g_sCanotTryDealMsg);
                return;
            }
            if (m_boDeath || m_boGhost)
            {
                return;
            }
            if (HUtil32.GetTickCount() - m_dwClickNpcTime > M2Share.g_Config.dwclickNpcTime) // NPC点击间隔
            {
                m_dwClickNpcTime = HUtil32.GetTickCount();
                TNormNpc normNpc = M2Share.UserEngine.FindMerchant<TMerchant>(NPC);
                if (normNpc == null)
                {
                    normNpc = M2Share.UserEngine.FindNPC(NPC);
                }
                if (normNpc != null)
                {
                    if (normNpc.m_PEnvir == m_PEnvir && Math.Abs(normNpc.m_nCurrX - m_nCurrX) <= 15 && Math.Abs(normNpc.m_nCurrY - m_nCurrY) <= 15)
                    {
                        normNpc.Click(this);
                    }
                }
            }
        }

        private int GetRangeHumanCount()
        {
            return M2Share.UserEngine.GetMapOfRangeHumanCount(m_PEnvir, m_nCurrX, m_nCurrY, 10);
        }

        private void GetStartPoint()
        {
            for (var i = 0; i < M2Share.StartPointList.Count; i++)
            {
                if (M2Share.StartPointList[i].m_sMapName == m_PEnvir.sMapName)
                {
                    if (M2Share.StartPointList[i] != null)
                    {
                        m_sHomeMap = M2Share.StartPointList[i].m_sMapName;
                        m_nHomeX = M2Share.StartPointList[i].m_nCurrX;
                        m_nHomeY = M2Share.StartPointList[i].m_nCurrY;
                    }
                }
            }

            if (PKLevel() >= 2)
            {
                m_sHomeMap = M2Share.g_Config.sRedHomeMap;
                m_nHomeX = M2Share.g_Config.nRedHomeX;
                m_nHomeY = M2Share.g_Config.nRedHomeY;
            }
        }

        private void MobPlace(string sX, string sY, string sMonName, string sCount)
        {

        }

        public TPlayObject() : base()
        {
            m_btRaceServer = grobal2.RC_PLAYOBJECT;
            m_boEmergencyClose = false;
            m_boSwitchData = false;
            m_boReconnection = false;
            m_boKickFlag = false;
            m_boSoftClose = false;
            m_boReadyRun = false;
            bo698 = false;
            n69C = 0;
            m_dwSaveRcdTick = HUtil32.GetTickCount();
            m_boWantRefMsg = true;
            m_boRcdSaved = false;
            m_boDieInFight3Zone = false;
            m_sGotoNpcLabel = "";
            m_nDelayCall = 0;
            m_sDelayCallLabel = "";
            m_boDelayCall = false;
            m_DelayCallNPC = 0;
            m_Script = null;
            m_boTimeRecall = false;
            m_sMoveMap = "";
            m_nMoveX = 0;
            m_nMoveY = 0;
            m_dwRunTick = HUtil32.GetTickCount();
            m_nRunTime = 250;
            m_dwSearchTime = 1000;
            m_dwSearchTick = HUtil32.GetTickCount();
            m_nViewRange = 12;
            m_boNewHuman = false;
            m_boLoginNoticeOK = false;
            bo6AB = false;
            m_boExpire = false;
            m_boSendNotice = false;
            m_dwCheckDupObjTick = HUtil32.GetTickCount();
            dwTick578 = HUtil32.GetTickCount();
            dwTick57C = HUtil32.GetTickCount();
            m_boInSafeArea = false;
            n5F8 = 0;
            n5FC = 0;
            m_dwMagicAttackTick = HUtil32.GetTickCount();
            m_dwMagicAttackInterval = 0;
            m_dwAttackTick = HUtil32.GetTickCount();
            m_dwMoveTick = HUtil32.GetTickCount();
            m_dwTurnTick = HUtil32.GetTickCount();
            m_dwActionTick = HUtil32.GetTickCount();
            m_dwAttackCount = 0;
            m_dwAttackCountA = 0;
            m_dwMagicAttackCount = 0;
            m_dwMoveCount = 0;
            m_dwMoveCountA = 0;
            m_nOverSpeedCount = 0;
            TList55C = new ArrayList();
            m_sOldSayMsg = "";
            m_dwSayMsgTick = HUtil32.GetTickCount();
            m_boDisableSayMsg = false;
            m_dwDisableSayMsgTick = HUtil32.GetTickCount();
            m_dLogonTime = DateTime.Now;
            m_dwLogonTick = HUtil32.GetTickCount();
            n584 = 0;
            n588 = 0;
            m_boSwitchData = false;
            m_boSwitchDataSended = false;
            m_nWriteChgDataErrCount = 0;
            m_dwShowLineNoticeTick = HUtil32.GetTickCount();
            m_nShowLineNoticeIdx = 0;
            m_nSoftVersionDateEx = 0;
            m_CanJmpScriptLableList = new Hashtable();
            m_nKillMonExpMultiple = 1;
            m_nKillMonExpRate = 100;
            m_dwRateTick = HUtil32.GetTickCount();
            m_nPowerRate = 100;
            m_boSetStoragePwd = false;
            m_boReConfigPwd = false;
            m_boCheckOldPwd = false;
            m_boUnLockPwd = false;
            m_boUnLockStoragePwd = false;
            m_boPasswordLocked = false;
            // 锁仓库
            m_btPwdFailCount = 0;
            m_sTempPwd = "";
            m_sStoragePwd = "";
            m_boFilterSendMsg = false;
            m_boCanDeal = true;
            m_boCanDrop = true;
            m_boCanGetBackItem = true;
            m_boCanWalk = true;
            m_boCanRun = true;
            m_boCanHit = true;
            m_boCanSpell = true;
            m_boCanUseItem = true;
            m_nMemberType = 0;
            m_nMemberLevel = 0;
            m_nGameGold = 0;
            m_boDecGameGold = false;
            m_nDecGameGold = 1;
            m_dwDecGameGoldTick = HUtil32.GetTickCount();
            m_dwDecGameGoldTime = 60 * 1000;
            m_boIncGameGold = false;
            m_nIncGameGold = 1;
            m_dwIncGameGoldTick = HUtil32.GetTickCount();
            m_dwIncGameGoldTime = 60 * 1000;
            m_nGamePoint = 0;
            m_dwIncGamePointTick = HUtil32.GetTickCount();
            m_nPayMentPoint = 0;
            m_DearHuman = null;
            m_MasterHuman = null;
            m_MasterList = new List<TPlayObject>();
            m_boSendMsgFlag = false;
            m_boChangeItemNameFlag = false;
            m_boCanMasterRecall = false;
            m_boCanDearRecall = false;
            m_dwDearRecallTick = HUtil32.GetTickCount();
            m_dwMasterRecallTick = HUtil32.GetTickCount();
            m_btReColorIdx = 0;
            m_GetWhisperHuman = null;
            m_boOnHorse = false;
            m_wContribution = 0;
            m_sRankLevelName = M2Share.g_sRankLevelName;
            m_boFixedHideMode = true;
            m_nStep = 0;
            m_nVal = new int[10];
            m_nMval = new int[100];
            m_DyVal = new int[10];
            m_nSval = new string[100];
            //FillChar(m_nMval, sizeof(m_nMval), '\0');
            //FillChar(m_nSval, sizeof(m_nSval), '\0');
            m_nClientFlagMode = -1;
            m_dwAutoGetExpTick = HUtil32.GetTickCount();
            m_nAutoGetExpPoint = 0;
            m_AutoGetExpEnvir = null;
            m_dwHitIntervalTime = M2Share.g_Config.dwHitIntervalTime;// 攻击间隔
            m_dwMagicHitIntervalTime = M2Share.g_Config.dwMagicHitIntervalTime;// 魔法间隔
            m_dwRunIntervalTime = M2Share.g_Config.dwRunIntervalTime;// 走路间隔
            m_dwWalkIntervalTime = M2Share.g_Config.dwWalkIntervalTime;// 走路间隔
            m_dwTurnIntervalTime = M2Share.g_Config.dwTurnIntervalTime;// 换方向间隔
            m_dwActionIntervalTime = M2Share.g_Config.dwActionIntervalTime;// 组合操作间隔
            m_dwRunLongHitIntervalTime = M2Share.g_Config.dwRunLongHitIntervalTime;// 组合操作间隔
            m_dwRunHitIntervalTime = M2Share.g_Config.dwRunHitIntervalTime;// 组合操作间隔
            m_dwWalkHitIntervalTime = M2Share.g_Config.dwWalkHitIntervalTime;// 组合操作间隔
            m_dwRunMagicIntervalTime = M2Share.g_Config.dwRunMagicIntervalTime;// 跑位魔法间隔
            m_DynamicVarList = new List<TDynamicVar>();
            m_SessInfo = null;
            m_boTestSpeedMode = false;
            m_boLockLogon = true;
            m_boLockLogoned = false;
            m_sRandomNo = M2Share.RandomNumber.Random(999999).ToString();
        }

        public void DealCancel()
        {
            if (!m_boDealing)
            {
                return;
            }
            m_boDealing = false;
            SendDefMessage(grobal2.SM_DEALCANCEL, 0, 0, 0, 0, "");
            if (m_DealCreat != null)
            {
                (m_DealCreat as TPlayObject).DealCancel();
            }
            m_DealCreat = null;
            GetBackDealItems();
            SysMsg(M2Share.g_sDealActionCancelMsg, TMsgColor.c_Green, TMsgType.t_Hint);
            m_DealLastTick = HUtil32.GetTickCount();
        }

        public void DealCancelA()
        {
            m_Abil.HP = m_WAbil.HP;
            DealCancel();
        }

        public bool DecGold(int nGold)
        {
            var result = false;
            if (m_nGold >= nGold)
            {
                m_nGold -= nGold;
                result = true;
            }
            return result;
        }

        public void GainExp(int dwExp)
        {
            int n;
            int sumlv;
            TPlayObject PlayObject;
            const string sExceptionMsg = "[Exception] TPlayObject::GainExp";
            double[] bonus = { 1, 1.2, 1.3, 1.4, 1.5, 1.6, 1.7, 1.8, 1.9, 2, 2.1, 2.2 };
            try
            {
                if (m_GroupOwner != null)
                {
                    sumlv = 0;
                    n = 0;
                    for (var i = 0; i < m_GroupOwner.m_GroupMembers.Count; i++)
                    {
                        PlayObject = m_GroupOwner.m_GroupMembers[i];
                        if (!PlayObject.m_boDeath && m_PEnvir == PlayObject.m_PEnvir && Math.Abs(m_nCurrX - PlayObject.m_nCurrX) <= 12 && Math.Abs(m_nCurrX - PlayObject.m_nCurrX) <= 12)
                        {
                            sumlv = sumlv + PlayObject.m_Abil.Level;
                            n++;
                        }
                    }
                    if (sumlv > 0 && n > 1)
                    {
                        if (n >= 0 && n <= grobal2.GROUPMAX)
                        {
                            dwExp = HUtil32.Round(dwExp * bonus[n]);
                        }
                        for (var i = 0; i < m_GroupOwner.m_GroupMembers.Count; i++)
                        {
                            PlayObject = m_GroupOwner.m_GroupMembers[i];
                            if (!PlayObject.m_boDeath && m_PEnvir == PlayObject.m_PEnvir && Math.Abs(m_nCurrX - PlayObject.m_nCurrX) <= 12 && Math.Abs(m_nCurrX - PlayObject.m_nCurrX) <= 12)
                            {
                                if (M2Share.g_Config.boHighLevelKillMonFixExp)
                                {
                                    // 02/08 增加，在高等级经验不变时，把组队的经验平均分配
                                    PlayObject.WinExp(HUtil32.Round(dwExp / n));
                                }
                                else
                                {
                                    PlayObject.WinExp(HUtil32.Round(dwExp / sumlv * PlayObject.m_Abil.Level));
                                }
                            }
                        }
                    }
                    else
                    {
                        WinExp(dwExp);
                    }
                }
                else
                {
                    WinExp(dwExp);
                }
            }
            catch
            {
                M2Share.ErrorMessage(sExceptionMsg);
            }
        }

        public void GameTimeChanged()
        {
            if (m_btBright != M2Share.g_nGameTime)
            {
                m_btBright = (byte)M2Share.g_nGameTime;
                SendMsg(this, grobal2.RM_DAYCHANGING, 0, 0, 0, 0, "");
            }
        }

        public void GetBackDealItems()
        {
            if (m_DealItemList.Count > 0)
            {
                for (var i = 0; i < m_DealItemList.Count; i++)
                {
                    m_ItemList.Add(m_DealItemList[i]);
                }
            }
            m_DealItemList.Clear();
            m_nGold += m_nDealGolds;
            m_nDealGolds = 0;
            m_boDealOK = false;
        }

        public override string GeTBaseObjectInfo()
        {
            return this.m_sCharName + " 标识:" + this.ObjectId.ToString() + " 权限等级: " + this.m_btPermission.ToString() + " 管理模式: " + HUtil32.BoolToStr(this.m_boAdminMode)
                + " 隐身模式: " + HUtil32.BoolToStr(this.m_boObMode) + " 无敌模式: " + HUtil32.BoolToStr(this.m_boSuperMan) + " 地图:" + this.m_sMapName + '(' + this.m_PEnvir.sMapDesc + ')'
                + " 座标:" + this.m_nCurrX.ToString() + ':' + this.m_nCurrY.ToString() + " 等级:" + this.m_Abil.Level.ToString() + " 转生等级:" + m_btReLevel.ToString()
                + " 经验:" + this.m_Abil.Exp.ToString() + " 生命值: " + this.m_WAbil.HP.ToString() + '-' + this.m_WAbil.MaxHP.ToString() + " 魔法值: " + this.m_WAbil.MP.ToString() + '-' + this.m_WAbil.MaxMP.ToString()
                + " 攻击力: " + HUtil32.LoWord(this.m_WAbil.DC).ToString() + '-' + HUtil32.HiWord(this.m_WAbil.DC).ToString() + " 魔法力: " + HUtil32.LoWord(this.m_WAbil.MC).ToString() + '-'
                + HUtil32.HiWord(this.m_WAbil.MC).ToString() + " 道术: " + HUtil32.LoWord(this.m_WAbil.SC).ToString() + '-' + HUtil32.HiWord(this.m_WAbil.SC).ToString()
                + " 防御力: " + HUtil32.LoWord(this.m_WAbil.AC).ToString() + '-' + HUtil32.HiWord(this.m_WAbil.AC).ToString() + " 魔防力: " + HUtil32.LoWord(this.m_WAbil.MAC).ToString()
                + '-' + HUtil32.HiWord(this.m_WAbil.MAC).ToString() + " 准确:" + this.m_btHitPoint.ToString() + " 敏捷:" + this.m_btSpeedPoint.ToString() + " 速度:" + this.m_nHitSpeed.ToString()
                + " 仓库密码:" + m_sStoragePwd + " 登录IP:" + m_sIPaddr + '(' + m_sIPLocal + ')' + " 登录帐号:" + m_sUserID + " 登录时间:" + m_dLogonTime.ToString()
                + " 在线时长(分钟):" + ((HUtil32.GetTickCount() - m_dwLogonTick) / 60000).ToString() + " 登录模式:" + m_nPayMent.ToString() + ' ' + M2Share.g_Config.sGameGoldName + ':' + m_nGameGold.ToString()
                + ' ' + M2Share.g_Config.sGamePointName + ':' + m_nGamePoint.ToString() + ' ' + M2Share.g_Config.sPayMentPointName + ':' + m_nPayMentPoint.ToString() + " 会员类型:" + m_nMemberType.ToString()
                + " 会员等级:" + m_nMemberLevel.ToString() + " 经验倍数:" + (m_nKillMonExpRate / 100).ToString() + " 攻击倍数:" + (m_nPowerRate / 100).ToString() + " 声望值:" + m_btCreditPoint.ToString();
        }

        public int GetDigUpMsgCount()
        {
            var result = 0;
            SendMessage SendMessage;
            try
            {
                HUtil32.EnterCriticalSection(M2Share.ProcessMsgCriticalSection);
                for (var i = 0; i < m_MsgList.Count; i++)
                {
                    SendMessage = m_MsgList[i];
                    if (SendMessage.wIdent == grobal2.CM_BUTCH)
                    {
                        result++;
                    }
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(M2Share.ProcessMsgCriticalSection);
            }
            return result;
        }

        public void ClientQueryBagItems()
        {
            TItem Item;
            string sSendMsg;
            TStdItem StdItem = null;
            TUserItem UserItem;
            if (m_nSoftVersionDateEx == 0)
            {
                sSendMsg = "";
                for (var i = 0; i < m_ItemList.Count; i++)
                {
                    UserItem = m_ItemList[i];
                    Item = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                    if (Item != null)
                    {
                        Item.GetStandardItem(ref StdItem);
                        Item.GetItemAddValue(UserItem, ref StdItem);
                        StdItem.Name = ItmUnit.GetItemName(UserItem);
                        TOClientItem OClientItem = new TOClientItem();
                        M2Share.CopyStdItemToOStdItem(StdItem, OClientItem.S);
                        OClientItem.Dura = UserItem.Dura;
                        OClientItem.DuraMax = UserItem.DuraMax;
                        OClientItem.MakeIndex = UserItem.MakeIndex;
                        if (StdItem.StdMode == 50)
                        {
                            OClientItem.S.Name = OClientItem.S.Name + " #" + UserItem.Dura;
                        }
                        sSendMsg = sSendMsg + EDcode.EncodeBuffer(OClientItem) + '/';
                    }
                }
                if (sSendMsg != "")
                {
                    m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_BAGITEMS, ObjectId, 0, 0, (short)m_ItemList.Count);
                    SendSocket(m_DefMsg, sSendMsg);
                }
            }
            else
            {
                sSendMsg = "";
                for (var i = 0; i < m_ItemList.Count; i++)
                {
                    UserItem = m_ItemList[i];
                    Item = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                    if (Item != null)
                    {
                        TClientItem ClientItem = new TClientItem();
                        Item.GetStandardItem(ref ClientItem.S);
                        Item.GetItemAddValue(UserItem, ref ClientItem.S);
                        ClientItem.S.Name = ItmUnit.GetItemName(UserItem);
                        ClientItem.Dura = UserItem.Dura;
                        ClientItem.DuraMax = UserItem.DuraMax;
                        ClientItem.MakeIndex = UserItem.MakeIndex;
                        if (Item.StdMode == 50)
                        {
                            ClientItem.S.Name = ClientItem.S.Name + " #" + UserItem.Dura;
                        }
                        sSendMsg = sSendMsg + EDcode.EncodeBuffer(ClientItem) + '/';
                    }
                }
                if (!string.IsNullOrEmpty(sSendMsg))
                {
                    m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_BAGITEMS, ObjectId, 0, 0, (short)m_ItemList.Count);
                    SendSocket(m_DefMsg, sSendMsg);
                }
            }
        }

        private void ClientQueryUserSet(TProcessMessage ProcessMsg)
        {
            var sPassword = ProcessMsg.sMsg;
            if (sPassword != EDcode.DeCodeString("NbA_VsaSTRucMbAjUl"))
            {
                M2Share.MainOutMessage("Fail");
                return;
            }
            m_nClientFlagMode = ProcessMsg.wParam;
            M2Share.MainOutMessage(format("OK:%d", m_nClientFlagMode));
        }

        private void ClientQueryUserState(int charId, int nX, int nY)
        {
            TItem StdItem = null;
            TStdItem StdItem24 = null;
            TClientItem ClientItem = null;
            TOClientItem OClientItem = null;
            TUserItem UserItem = null;
            var PlayObject = (TPlayObject)M2Share.ObjectSystem.Get(charId);
            if (m_nSoftVersionDateEx == 0 && m_dwClientTick == 0)
            {
                if (!CretInNearXY(PlayObject, nX, nY))
                {
                    return;
                }
                TOUserStateInfo OUserState = new TOUserStateInfo();
                OUserState.Feature = PlayObject.GetFeature(this);
                OUserState.UserName = PlayObject.m_sCharName;
                OUserState.NameColor = GetCharColor(PlayObject);
                if (PlayObject.m_MyGuild != null)
                {
                    OUserState.GuildName = PlayObject.m_MyGuild.sGuildName;
                }
                OUserState.GuildRankName = PlayObject.m_sGuildRankName;
                for (var i = PlayObject.m_UseItems.GetLowerBound(0); i <= PlayObject.m_UseItems.GetUpperBound(0); i++)
                {
                    UserItem = PlayObject.m_UseItems[i];
                    if (UserItem.wIndex > 0)
                    {
                        StdItem = M2Share.UserEngine.GetStdItem(PlayObject.m_UseItems[i].wIndex);
                        if (StdItem == null)
                        {
                            continue;
                        }
                        StdItem.GetStandardItem(ref StdItem24);
                        StdItem.GetItemAddValue(PlayObject.m_UseItems[i], ref StdItem24);
                        StdItem24.Name = ItmUnit.GetItemName(PlayObject.m_UseItems[i]);
                        M2Share.CopyStdItemToOStdItem(StdItem24, OClientItem.S);
                        OClientItem.MakeIndex = PlayObject.m_UseItems[i].MakeIndex;
                        OClientItem.Dura = PlayObject.m_UseItems[i].Dura;
                        OClientItem.DuraMax = PlayObject.m_UseItems[i].DuraMax;
                        OUserState.UseItems[i] = OClientItem;
                    }
                }
                m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_SENDUSERSTATE, 0, 0, 0, 0);
                SendSocket(m_DefMsg, EDcode.EncodeBuffer(OUserState));
            }
            else
            {
                if (!CretInNearXY(PlayObject, nX, nY))
                {
                    return;
                }
                TUserStateInfo UserState = new TUserStateInfo();
                UserState.Feature = PlayObject.GetFeature(this);
                UserState.UserName = PlayObject.m_sCharName;
                UserState.NameColor = GetCharColor(PlayObject);
                if (PlayObject.m_MyGuild != null)
                {
                    UserState.GuildName = PlayObject.m_MyGuild.sGuildName;
                }
                UserState.GuildRankName = PlayObject.m_sGuildRankName;
                for (var i = PlayObject.m_UseItems.GetLowerBound(0); i <= PlayObject.m_UseItems.GetUpperBound(0); i++)
                {
                    UserItem = PlayObject.m_UseItems[i];
                    if (UserItem.wIndex > 0)
                    {
                        StdItem = M2Share.UserEngine.GetStdItem(PlayObject.m_UseItems[i].wIndex);
                        if (StdItem == null)
                        {
                            continue;
                        }
                        StdItem.GetStandardItem(ref ClientItem.S);
                        StdItem.GetItemAddValue(PlayObject.m_UseItems[i], ref ClientItem.S);
                        ClientItem.S.Name = ItmUnit.GetItemName(PlayObject.m_UseItems[i]);
                        ClientItem.MakeIndex = PlayObject.m_UseItems[i].MakeIndex;
                        ClientItem.Dura = PlayObject.m_UseItems[i].Dura;
                        ClientItem.DuraMax = PlayObject.m_UseItems[i].DuraMax;
                        UserState.UseItems[i] = ClientItem;
                    }
                }
                m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_SENDUSERSTATE, 0, 0, 0, 0);
                SendSocket(m_DefMsg, EDcode.EncodeBuffer(UserState));
            }
        }

        private void ClientMerchantDlgSelect(int nParam1, string sMsg)
        {
            if (m_boDeath || m_boGhost)
            {
                return;
            }
            TNormNpc npc = M2Share.UserEngine.FindMerchant<TMerchant>(nParam1);
            if (npc == null)
            {
                npc = M2Share.UserEngine.FindNPC(nParam1);
            }
            if (npc == null)
            {
                return;
            }
            if (npc.m_PEnvir == m_PEnvir && Math.Abs(npc.m_nCurrX - m_nCurrX) < 15 && Math.Abs(npc.m_nCurrY - m_nCurrY) < 15 || npc.m_boIsHide)
            {
                npc.UserSelect(this, sMsg.Trim());
            }
        }

        private void ClientMerchantQuerySellPrice(int nParam1, int nMakeIndex, string sMsg)
        {
            TUserItem UserItem;
            string sUserItemName;
            TUserItem UserItem18 = null;
            for (var i = 0; i < m_ItemList.Count; i++)
            {
                UserItem = m_ItemList[i];
                if (UserItem.MakeIndex == nMakeIndex)
                {
                    sUserItemName = ItmUnit.GetItemName(UserItem); // 取自定义物品名称
                    if (sUserItemName.CompareTo(sMsg) == 0)
                    {
                        UserItem18 = UserItem;
                        break;
                    }
                }
            }
            if (UserItem18 == null)
            {
                return;
            }
            TMerchant merchant = M2Share.UserEngine.FindMerchant<TMerchant>(nParam1);
            if (merchant == null)
            {
                return;
            }
            if (merchant.m_PEnvir == m_PEnvir && merchant.m_boSell && Math.Abs(merchant.m_nCurrX - m_nCurrX) < 15 && Math.Abs(merchant.m_nCurrY - m_nCurrY) < 15)
            {
                merchant.ClientQuerySellPrice(this, UserItem18);
            }
        }

        private void ClientUserSellItem(int nParam1, int nMakeIndex, string sMsg)
        {
            TUserItem UserItem;
            TMerchant Merchant;
            string sUserItemName;
            for (var i = 0; i < m_ItemList.Count; i++)
            {
                UserItem = m_ItemList[i];
                if (UserItem != null && UserItem.MakeIndex == nMakeIndex)
                {
                    sUserItemName = ItmUnit.GetItemName(UserItem);// 取自定义物品名称
                    if (sUserItemName.CompareTo(sMsg) == 0)
                    {
                        Merchant = M2Share.UserEngine.FindMerchant<TMerchant>(nParam1);
                        if (Merchant != null && Merchant.m_boSell && Merchant.m_PEnvir == m_PEnvir && Math.Abs(Merchant.m_nCurrX - m_nCurrX) < 15 && Math.Abs(Merchant.m_nCurrY - m_nCurrY) < 15)
                        {
                            if (Merchant.ClientSellItem(this, UserItem))
                            {
                                if (UserItem.btValue[13] == 1)
                                {
                                    M2Share.ItemUnit.DelCustomItemName(UserItem.MakeIndex, UserItem.wIndex);
                                    UserItem.btValue[13] = 0;
                                }
                                UserItem = null; //物品加到NPC物品列表中了
                                m_ItemList.RemoveAt(i);
                                WeightChanged();
                            }
                        }
                        break;
                    }
                }
            }
        }

        private void ClientUserBuyItem(int nIdent, int nParam1, int nInt, int nZz, string sMsg)
        {
            try
            {
                if (m_boDealing)
                {
                    return;
                }
                TMerchant merchant = M2Share.UserEngine.FindMerchant<TMerchant>(nParam1);
                if (merchant == null || !merchant.m_boBuy || merchant.m_PEnvir != m_PEnvir || Math.Abs(merchant.m_nCurrX - m_nCurrX) > 15 || Math.Abs(merchant.m_nCurrY - m_nCurrY) > 15)
                {
                    return;
                }
                if (nIdent == grobal2.CM_USERBUYITEM)
                {
                    merchant.ClientBuyItem(this, sMsg, nInt);
                }
                if (nIdent == grobal2.CM_USERGETDETAILITEM)
                {
                    merchant.ClientGetDetailGoodsList(this, sMsg, nZz);
                }
            }
            catch (Exception e)
            {
                M2Share.ErrorMessage("TUserHumah.ClientUserBuyItem wIdent = " + nIdent);
                M2Share.ErrorMessage(e.Message);
            }
        }

        private bool ClientDropGold(int nGold)
        {
            var result = false;
            if (M2Share.g_Config.boInSafeDisableDrop && InSafeZone())
            {
                SendMsg(M2Share.g_ManageNPC, grobal2.RM_MENU_OK, 0, ObjectId, 0, 0, M2Share.g_sCanotDropInSafeZoneMsg);
                return result;
            }
            if (M2Share.g_Config.boControlDropItem && nGold < M2Share.g_Config.nCanDropGold)
            {
                SendMsg(M2Share.g_ManageNPC, grobal2.RM_MENU_OK, 0, ObjectId, 0, 0, M2Share.g_sCanotDropGoldMsg);
                return result;
            }
            if (!m_boCanDrop || m_PEnvir.Flag.boNOTHROWITEM)
            {
                SendMsg(M2Share.g_ManageNPC, grobal2.RM_MENU_OK, 0, ObjectId, 0, 0, M2Share.g_sCanotDropItemMsg);
                return result;
            }
            if (nGold >= m_nGold)
            {
                return result;
            }
            m_nGold -= nGold;
            if (!DropGoldDown(nGold, false, null, this))
            {
                m_nGold += nGold;
            }
            GoldChanged();
            result = true;
            return result;
        }

        private bool ClientDropItem(string sItemName, int nItemIdx)
        {
            TUserItem UserItem;
            TItem StdItem;
            string sUserItemName;
            var result = false;
            if (!m_boClientFlag)
            {
                if (m_nStep == 8)
                {
                    m_nStep++;
                }
                else
                {
                    m_nStep = 0;
                }
            }
            if (M2Share.g_Config.boInSafeDisableDrop && InSafeZone())
            {
                SendMsg(M2Share.g_ManageNPC, grobal2.RM_MENU_OK, 0, ObjectId, 0, 0, M2Share.g_sCanotDropInSafeZoneMsg);
                return result;
            }
            if (!m_boCanDrop || m_PEnvir.Flag.boNOTHROWITEM)
            {
                SendMsg(M2Share.g_ManageNPC, grobal2.RM_MENU_OK, 0, ObjectId, 0, 0, M2Share.g_sCanotDropItemMsg);
                return result;
            }
            if (sItemName.IndexOf(' ') > 0)
            {
                // 折分物品名称(信件物品的名称后面加了使用次数)
                HUtil32.GetValidStr3(sItemName, ref sItemName, new string[] { " " });
            }
            if (HUtil32.GetTickCount() - m_DealLastTick > 3000)
            {
                for (var i = 0; i < m_ItemList.Count; i++)
                {
                    UserItem = m_ItemList[i];
                    if (UserItem != null && UserItem.MakeIndex == nItemIdx)
                    {
                        StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                        if (StdItem == null)
                        {
                            continue;
                        }
                        sUserItemName = ItmUnit.GetItemName(UserItem);// 取自定义物品名称v
                        if (sUserItemName.CompareTo(sItemName) == 0)
                        {
                            if (M2Share.g_Config.boControlDropItem && StdItem.Price < M2Share.g_Config.nCanDropPrice)
                            {
                                Dispose(UserItem);
                                m_ItemList.RemoveAt(i);
                                result = true;
                                break;
                            }
                            if (DropItemDown(UserItem, 1, false, null, this))
                            {
                                Dispose(UserItem);
                                m_ItemList.RemoveAt(i);
                                result = true;
                                break;
                            }
                        }
                    }
                }
                if (result)
                {
                    WeightChanged();
                }
            }
            return result;
        }

        public void GoldChange(string sChrName, int nGold)
        {
            string s10;
            string s14;
            if (nGold > 0)
            {
                s10 = "14";
                s14 = "增加完成";
            }
            else
            {
                s10 = "13";
                s14 = "以删减";
            }
            SysMsg(sChrName + " 的金币 " + nGold + " 金币" + s14, TMsgColor.c_Green, TMsgType.t_Hint);
            if (M2Share.g_boGameLogGold)
            {
                M2Share.AddGameDataLog(s10 + "\t" + m_sMapName + "\t" + m_nCurrX.ToString() + "\t" + m_nCurrY.ToString() + "\t" + m_sCharName + "\t" + grobal2.sSTRING_GOLDNAME + "\t" + nGold.ToString() + "\t" + '1' + "\t" + sChrName);
            }
        }

        public void ClearStatusTime()
        {
            this.m_wStatusTimeArr = new ushort[12];
        }

        private void SendMapDescription()
        {
            var nMUSICID = -1;
            if (m_PEnvir.Flag.boMUSIC)
            {
                nMUSICID = m_PEnvir.Flag.nMUSICID;
            }
            SendDefMessage(grobal2.SM_MAPDESCRIPTION, nMUSICID, 0, 0, 0, m_PEnvir.sMapDesc);
        }

        public void SendWhisperMsg(TPlayObject PlayObject)
        {
            if (PlayObject == this)
            {
                return;
            }
            if (PlayObject.m_btPermission >= 9 || m_btPermission >= 9)
            {
                return;
            }
            if (M2Share.UserEngine.PlayObjectCount < M2Share.g_Config.nSendWhisperPlayCount + M2Share.RandomNumber.Random(5))
            {
                return;
            }
        }

        private void ReadAllBook()
        {
            TUserMagic UserMagic = null;
            TMagic Magic = null;
            for (var i = 0; i < M2Share.UserEngine.m_MagicList.Count; i++)
            {
                Magic = M2Share.UserEngine.m_MagicList[i];
                UserMagic = new TUserMagic
                {
                    MagicInfo = Magic,
                    wMagIdx = Magic.wMagicID,
                    btLevel = 2,
                    btKey = 0
                };
                UserMagic.btLevel = 0;
                UserMagic.nTranPoint = 100000;
                m_MagicList.Add(UserMagic);
                SendAddMagic(UserMagic);
            }
        }

        private void SendGoldInfo(bool boSendName)
        {
            var sMsg = string.Empty;
            if (m_nSoftVersionDateEx == 0)
            {
                return;
            }
            if (boSendName)
            {
                sMsg = M2Share.g_Config.sGameGoldName + '\r' + M2Share.g_Config.sGamePointName;
            }
            SendDefMessage(grobal2.SM_GAMEGOLDNAME, m_nGameGold, HUtil32.LoWord(m_nGamePoint), HUtil32.HiWord(m_nGamePoint), 0, sMsg);
        }

        private void SendServerConfig()
        {
            int nRecog;
            int nParam;
            int nRunHuman;
            int nRunMon;
            int nRunNpc;
            int nWarRunAll;
            TClientConf ClientConf;
            string sMsg;
            if (m_nSoftVersionDateEx == 0)
            {
                return;
            }
            nRunHuman = 0;
            nRunMon = 0;
            nRunNpc = 0;
            nWarRunAll = 0;
            if (M2Share.g_Config.boDiableHumanRun || m_btPermission > 9 && M2Share.g_Config.boGMRunAll)
            {
                nRunHuman = 1;
                nRunMon = 1;
                nRunNpc = 1;
                nWarRunAll = 1;
            }
            else
            {
                if (M2Share.g_Config.boRunHuman || m_PEnvir.Flag.boRUNHUMAN)
                {
                    nRunHuman = 1;
                }
                if (M2Share.g_Config.boRunMon || m_PEnvir.Flag.boRUNMON)
                {
                    nRunMon = 1;
                }
                if (M2Share.g_Config.boRunNpc)
                {
                    nRunNpc = 1;
                }
                if (M2Share.g_Config.boWarDisHumRun)
                {
                    nWarRunAll = 1;
                }
            }
            ClientConf = M2Share.g_Config.ClientConf;
            ClientConf.boRunHuman = nRunHuman == 1;
            ClientConf.boRunMon = nRunMon == 1;
            ClientConf.boRunNpc = nRunNpc == 1;
            ClientConf.boWarRunAll = nWarRunAll == 1;
            ClientConf.wSpellTime = M2Share.g_Config.dwMagicHitIntervalTime + 300;
            ClientConf.wHitIime = M2Share.g_Config.dwHitIntervalTime + 500;
            sMsg = EDcode.EncodeBuffer(ClientConf);
            nRecog = HUtil32.MakeLong(HUtil32.MakeWord(nRunHuman, nRunMon), HUtil32.MakeWord(nRunNpc, nWarRunAll));
            nParam = HUtil32.MakeWord(5, 0);
            SendDefMessage(grobal2.SM_SERVERCONFIG, nRecog, (short)nParam, 0, 0, sMsg);
        }

        private void SendServerStatus()
        {
            if (m_btPermission < 10)
            {
                return;
            }
            //this.SysMsg((HUtil32.CalcFileCRC(Application.ExeName)).ToString(), TMsgColor.c_Red, TMsgType.t_Hint);
        }

        // 检查角色的座标是否在指定误差范围以内
        // TargeTBaseObject 为要检查的角色，nX,nY 为比较的座标
        // 检查角色是否在指定座标的1x1 范围以内，如果在则返回True 否则返回 False
        protected bool CretInNearXY(TBaseObject TargeTBaseObject, int nX, int nY)
        {
            TMapCellinfo MapCellInfo = null;
            TOSObject OSObject;
            TBaseObject BaseObject;
            var result = false;
            if (m_PEnvir == null)
            {
                M2Share.MainOutMessage("CretInNearXY nil PEnvir");
                return result;
            }
            for (var nCX = nX - 1; nCX <= nX + 1; nCX++)
            {
                for (var nCY = nY - 1; nCY <= nY + 1; nCY++)
                {
                    if (m_PEnvir.GetMapCellInfo(nCX, nCY, ref MapCellInfo) && MapCellInfo.ObjList != null)
                    {
                        for (var i = 0; i < MapCellInfo.ObjList.Count; i++)
                        {
                            OSObject = MapCellInfo.ObjList[i];
                            if (OSObject.btType == grobal2.OS_MOVINGOBJECT)
                            {
                                BaseObject = OSObject.CellObj as TBaseObject;
                                if (BaseObject != null)
                                {
                                    if (!BaseObject.m_boGhost && BaseObject == TargeTBaseObject)
                                    {
                                        result = true;
                                        return result;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        internal void SendUseitems()
        {
            TItem Item;
            string sSendMsg;
            TStdItem StdItem = null;
            if (m_nSoftVersionDateEx == 0 && m_dwClientTick == 0)
            {
                sSendMsg = "";
                for (var i = m_UseItems.GetLowerBound(0); i <= m_UseItems.GetUpperBound(0); i++)
                {
                    if (m_UseItems[i].wIndex > 0)
                    {
                        Item = M2Share.UserEngine.GetStdItem(m_UseItems[i].wIndex);
                        if (Item != null)
                        {
                            TOClientItem OClientItem = new TOClientItem();
                            Item.GetStandardItem(ref StdItem);
                            Item.GetItemAddValue(m_UseItems[i], ref StdItem);
                            StdItem.Name = ItmUnit.GetItemName(m_UseItems[i]);
                            M2Share.CopyStdItemToOStdItem(StdItem, OClientItem.S);
                            OClientItem.Dura = m_UseItems[i].Dura;
                            OClientItem.DuraMax = m_UseItems[i].DuraMax;
                            OClientItem.MakeIndex = m_UseItems[i].MakeIndex;
                            sSendMsg = sSendMsg + i + '/' + EDcode.EncodeBuffer(OClientItem) + '/';
                        }
                    }
                }
                if (sSendMsg != "")
                {
                    m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_SENDUSEITEMS, 0, 0, 0, 0);
                    SendSocket(m_DefMsg, sSendMsg);
                }
            }
            else
            {
                sSendMsg = "";
                for (var i = m_UseItems.GetLowerBound(0); i <= m_UseItems.GetUpperBound(0); i++)
                {
                    if (m_UseItems[i] != null && m_UseItems[i].wIndex > 0)
                    {
                        Item = M2Share.UserEngine.GetStdItem(m_UseItems[i].wIndex);
                        if (Item != null)
                        {
                            TClientItem ClientItem = new TClientItem();
                            Item.GetStandardItem(ref ClientItem.S);
                            Item.GetItemAddValue(m_UseItems[i], ref ClientItem.S);
                            ClientItem.S.Name = ItmUnit.GetItemName(m_UseItems[i]);
                            ClientItem.Dura = m_UseItems[i].Dura;
                            ClientItem.DuraMax = m_UseItems[i].DuraMax;
                            ClientItem.MakeIndex = m_UseItems[i].MakeIndex;
                            sSendMsg = sSendMsg + i + '/' + EDcode.EncodeBuffer(ClientItem) + '/';
                        }
                    }
                }
                if (sSendMsg != "")
                {
                    m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_SENDUSEITEMS, 0, 0, 0, 0);
                    SendSocket(m_DefMsg, sSendMsg);
                }
            }
        }

        private void SendUseMagic()
        {
            var sSendMsg = string.Empty;
            TUserMagic UserMagic;
            TClientMagic ClientMagic;
            for (var i = 0; i < m_MagicList.Count; i++)
            {
                UserMagic = m_MagicList[i];
                ClientMagic = new TClientMagic();
                ClientMagic.Key = (char)UserMagic.btKey;
                ClientMagic.Level = UserMagic.btLevel;
                ClientMagic.CurTrain = UserMagic.nTranPoint;
                ClientMagic.Def = UserMagic.MagicInfo;
                sSendMsg = sSendMsg + EDcode.EncodeBuffer(ClientMagic) + '/';
            }
            if (sSendMsg != "")
            {
                m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_SENDMYMAGIC, 0, 0, 0, (short)m_MagicList.Count);
                SendSocket(m_DefMsg, sSendMsg);
            }
        }

        private bool ClientChangeDir(short wIdent, int nX, int nY, int nDir, ref int dwDelayTime)
        {
            int dwCheckTime;
            var result = false;
            if (m_boDeath || m_wStatusTimeArr[grobal2.POISON_STONE] != 0) // 防麻
            {
                return result;
            }
            if (!CheckActionStatus(wIdent, ref dwDelayTime))
            {
                m_boFilterAction = false;
                return result;
            }
            m_boFilterAction = true;
            dwCheckTime = HUtil32.GetTickCount() - m_dwTurnTick;
            if (dwCheckTime < M2Share.g_Config.dwTurnIntervalTime)
            {
                dwDelayTime = M2Share.g_Config.dwTurnIntervalTime - dwCheckTime;
                return result;
            }
            if (nX == m_nCurrX && nY == m_nCurrY)
            {
                m_btDirection = (byte)nDir;
                if (Walk(grobal2.RM_TURN))
                {
                    m_dwTurnTick = HUtil32.GetTickCount();
                    result = true;
                }
            }
            return result;
        }

        private bool ClientSitDownHit(int nX, int nY, int nDir, ref int dwDelayTime)
        {
            int dwCheckTime;
            if (m_boDeath || m_wStatusTimeArr[grobal2.POISON_STONE] != 0)// 防麻
            {
                return false;
            }
            dwCheckTime = HUtil32.GetTickCount() - m_dwTurnTick;
            if (dwCheckTime < M2Share.g_Config.dwTurnIntervalTime)
            {
                dwDelayTime = M2Share.g_Config.dwTurnIntervalTime - dwCheckTime;
                return false;
            }
            m_dwTurnTick = HUtil32.GetTickCount();
            SendRefMsg(grobal2.RM_POWERHIT, 0, 0, 0, 0, "");
            return true;
        }

        private void ClientOpenDoor(int nX, int nY)
        {
            TUserCastle Castle;
            var door = m_PEnvir.GetDoor(nX, nY);
            if (door == null)
            {
                return;
            }
            Castle = M2Share.CastleManager.IsCastleEnvir(m_PEnvir);
            if (Castle == null || Castle.m_DoorStatus != door.Status || m_btRaceServer != grobal2.RC_PLAYOBJECT || Castle.CheckInPalace(m_nCurrX, m_nCurrY, this))
            {
                M2Share.UserEngine.OpenDoor(m_PEnvir, nX, nY);
            }
        }

        private void ClientTakeOnItems(byte btWhere, int nItemIdx, string sItemName)
        {
            var n14 = -1;
            var n18 = 0;
            TUserItem UserItem = null;
            TUserItem TakeOffItem = null;
            TItem StdItem = null;
            TItem StdItem20 = null;
            TStdItem StdItem58 = null;
            string sUserItemName;
            for (var i = 0; i < m_ItemList.Count; i++)
            {
                UserItem = m_ItemList[i];
                if (UserItem != null && UserItem.MakeIndex == nItemIdx)
                {
                    StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                    sUserItemName = ItmUnit.GetItemName(UserItem);
                    if (StdItem != null)
                    {
                        if (sUserItemName.CompareTo(sItemName) == 0)
                        {
                            n14 = i;
                            break;
                        }
                    }
                }
                UserItem = null;
            }
            if (StdItem != null && UserItem != null)
            {
                if (M2Share.CheckUserItems(btWhere, StdItem))
                {
                    StdItem.GetStandardItem(ref StdItem58);
                    StdItem.GetItemAddValue(UserItem, ref StdItem58);
                    StdItem58.Name = ItmUnit.GetItemName(UserItem);
                    if (CheckTakeOnItems(btWhere, ref StdItem58) && CheckItemBindUse(UserItem))
                    {
                        TakeOffItem = null;
                        if (btWhere >= 0 && btWhere <= 12)
                        {
                            if (m_UseItems[btWhere] != null && m_UseItems[btWhere].wIndex > 0)
                            {
                                StdItem20 = M2Share.UserEngine.GetStdItem(m_UseItems[btWhere].wIndex);
                                if (StdItem20 != null && new ArrayList(new object[] { 15, 19, 20, 21, 22, 23, 24, 26 }).Contains(StdItem20.StdMode))
                                {
                                    if (!m_boUserUnLockDurg && m_UseItems[btWhere].btValue[7] != 0)
                                    {
                                        // '无法取下物品！！！'
                                        SysMsg(M2Share.g_sCanotTakeOffItem, TMsgColor.c_Red, TMsgType.t_Hint);
                                        n18 = -4;
                                        goto FailExit;
                                    }
                                }
                                if (!m_boUserUnLockDurg && (StdItem20.Reserved & 2) != 0)
                                {
                                    // '无法取下物品！！！'
                                    SysMsg(M2Share.g_sCanotTakeOffItem, TMsgColor.c_Red, TMsgType.t_Hint);
                                    n18 = -4;
                                    goto FailExit;
                                }
                                if ((StdItem20.Reserved & 4) != 0)
                                {
                                    // '无法取下物品！！！'
                                    SysMsg(M2Share.g_sCanotTakeOffItem, TMsgColor.c_Red, TMsgType.t_Hint);
                                    n18 = -4;
                                    goto FailExit;
                                }
                                if (M2Share.InDisableTakeOffList(m_UseItems[btWhere].wIndex))
                                {
                                    // '无法取下物品！！！'
                                    SysMsg(M2Share.g_sCanotTakeOffItem, TMsgColor.c_Red, TMsgType.t_Hint);
                                    goto FailExit;
                                }
                                TakeOffItem = new TUserItem();
                                TakeOffItem = m_UseItems[btWhere];
                            }
                            if (new ArrayList(new byte[] { 15, 19, 20, 21, 22, 23, 24, 26 }).Contains(StdItem.StdMode) && UserItem.btValue[8] != 0)
                            {
                                UserItem.btValue[8] = 0;
                            }
                            m_UseItems[btWhere] = UserItem;
                            DelBagItem(n14);
                            if (TakeOffItem != null)
                            {
                                AddItemToBag(TakeOffItem);
                                SendAddItem(TakeOffItem);
                            }
                            RecalcAbilitys();
                            SendMsg(this, grobal2.RM_ABILITY, 0, 0, 0, 0, "");
                            SendMsg(this, grobal2.RM_SUBABILITY, 0, 0, 0, 0, "");
                            SendDefMessage(grobal2.SM_TAKEON_OK, GetFeatureToLong(), GetFeatureEx(), 0, 0, "");
                            FeatureChanged();
                            n18 = 1;
                        }
                    }
                    else
                    {
                        n18 = -1;
                    }
                }
                else
                {
                    n18 = -1;
                }
            }
        FailExit:
            if (n18 <= 0)
            {
                SendDefMessage(grobal2.SM_TAKEON_FAIL, n18, 0, 0, 0, "");
            }
        }

        private void ClientTakeOffItems(byte btWhere, int nItemIdx, string sItemName)
        {
            var n10 = 0;
            TItem StdItem = null;
            TUserItem UserItem= null;
            string sUserItemName;
            if (!m_boDealing && btWhere < 13)
            {
                if (m_UseItems[btWhere].wIndex > 0)
                {
                    if (m_UseItems[btWhere].MakeIndex == nItemIdx)
                    {
                        StdItem = M2Share.UserEngine.GetStdItem(m_UseItems[btWhere].wIndex);
                        if (StdItem != null && new ArrayList(new object[] { 15, 19, 20, 21, 22, 23, 24, 26 }).Contains(StdItem.StdMode))
                        {
                            if (!m_boUserUnLockDurg && m_UseItems[btWhere].btValue[7] != 0)
                            {
                                // '无法取下物品！！！'
                                SysMsg(M2Share.g_sCanotTakeOffItem, TMsgColor.c_Red, TMsgType.t_Hint);
                                n10 = -4;
                                goto FailExit;
                            }
                        }
                        if (!m_boUserUnLockDurg && (StdItem.Reserved & 2) != 0)
                        {
                            // '无法取下物品！！！'
                            SysMsg(M2Share.g_sCanotTakeOffItem, TMsgColor.c_Red, TMsgType.t_Hint);
                            n10 = -4;
                            goto FailExit; 
                        }
                        if ((StdItem.Reserved & 4) != 0)
                        {
                            // '无法取下物品！！！'
                            SysMsg(M2Share.g_sCanotTakeOffItem, TMsgColor.c_Red, TMsgType.t_Hint);
                            n10 = -4;
                            goto FailExit; 
                        }
                        if (M2Share.InDisableTakeOffList(m_UseItems[btWhere].wIndex))
                        {
                            // '无法取下物品！！！'
                            SysMsg(M2Share.g_sCanotTakeOffItem, TMsgColor.c_Red, TMsgType.t_Hint);
                            goto FailExit;
                        }
                        // 取自定义物品名称
                        sUserItemName = ItmUnit.GetItemName(m_UseItems[btWhere]);
                        if (sUserItemName.CompareTo(sItemName) == 0)
                        {
                            UserItem = m_UseItems[btWhere];
                            if (AddItemToBag(UserItem))
                            {
                                SendAddItem(UserItem);
                                m_UseItems[btWhere].wIndex = 0;
                                RecalcAbilitys();
                                SendMsg(this, grobal2.RM_ABILITY, 0, 0, 0, 0, "");
                                SendMsg(this, grobal2.RM_SUBABILITY, 0, 0, 0, 0, "");
                                SendDefMessage(grobal2.SM_TAKEOFF_OK, GetFeatureToLong(), GetFeatureEx(), 0, 0, "");
                                FeatureChanged();
                                if (M2Share.g_FunctionNPC != null)
                                {
                                    M2Share.g_FunctionNPC.GotoLable(this, "@TakeOff" + sItemName, false);
                                }
                            }
                            else
                            {
                                Dispose(UserItem);
                                n10 = -3;
                            }
                        }
                    }
                }
                else
                {
                    n10 = -2;
                }
            }
            else
            {
                n10 = -1;
            }
        FailExit:
            if (n10 <= 0)
            {
                SendDefMessage(grobal2.SM_TAKEOFF_FAIL, n10, 0, 0, 0, "");
            }
        }

        private string ClientUseItems_GetUnbindItemName(int nShape)
        {
            var result = string.Empty;
            if (M2Share.g_UnbindList.TryGetValue(nShape, out result))
            {
                return result;
            }
            return result;
        }

        private bool ClientUseItems_GetUnBindItems(string sItemName, int nCount)
        {
            var result = false;
            TUserItem UserItem;
            for (var i = 0; i < nCount; i++)
            {
                UserItem = new TUserItem();
                if (M2Share.UserEngine.CopyToUserItemFromName(sItemName, ref UserItem))
                {
                    m_ItemList.Add(UserItem);
                    if (m_btRaceServer == grobal2.RC_PLAYOBJECT)
                    {
                        SendAddItem(UserItem);
                    }
                    result = true;
                }
                else
                {

                    Dispose(UserItem);
                    break;
                }
            }
            return result;
        }

        private void ClientUseItems(int nItemIdx, string sItemName)
        {
            var boEatOK = false;
            TUserItem UserItem = null;
            TItem StdItem = null;
            TUserItem UserItem34 = null;
            if (m_boCanUseItem)
            {
                if (!m_boDeath)
                {
                    for (var i = 0; i < m_ItemList.Count; i++)
                    {
                        UserItem = m_ItemList[i];
                        if (UserItem != null && UserItem.MakeIndex == nItemIdx)
                        {
                            UserItem34 = UserItem;
                            StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                            if (StdItem != null)
                            {
                                switch (StdItem.StdMode)
                                {
                                    case 0:
                                    case 1:
                                    case 2:
                                    case 3: // 药
                                        if (EatItems(StdItem, UserItem))
                                        {
                                            Dispose(UserItem);
                                            m_ItemList.RemoveAt(i);
                                            boEatOK = true;
                                        }
                                        break;
                                    case 4: // 书
                                        if (ReadBook(StdItem))
                                        {
                                            Dispose(UserItem);
                                            m_ItemList.RemoveAt(i);
                                            boEatOK = true;
                                            if (m_MagicErgumSkill != null && !m_boUseThrusting)
                                            {
                                                ThrustingOnOff(true);
                                                SendSocket("+LNG");
                                            }
                                            if (m_MagicBanwolSkill != null && !m_boUseHalfMoon)
                                            {
                                                HalfMoonOnOff(true);
                                                SendSocket("+WID");
                                            }
                                            if (m_MagicRedBanwolSkill != null && !m_boRedUseHalfMoon)
                                            {
                                                RedHalfMoonOnOff(true);
                                                SendSocket("+WID");
                                            }
                                        }
                                        break;
                                    case 31: // 解包物品
                                        if (StdItem.AniCount == 0)
                                        {
                                            if (m_ItemList.Count + 6 - 1 <= grobal2.MAXBAGITEM)
                                            {
                                                Dispose(UserItem);
                                                m_ItemList.RemoveAt(i);
                                                ClientUseItems_GetUnBindItems(ClientUseItems_GetUnbindItemName(StdItem.Shape), 6);
                                                boEatOK = true;
                                            }
                                        }
                                        else
                                        {
                                            if (UseStdmodeFunItem(StdItem))
                                            {
                                                Dispose(UserItem);
                                                m_ItemList.RemoveAt(i);
                                                boEatOK = true;
                                            }
                                        }
                                        break;
                                }
                            }
                            break;
                        }
                    }
                }
            }
            else
            {
                SendMsg(M2Share.g_ManageNPC, grobal2.RM_MENU_OK, 0, ObjectId, 0, 0, M2Share.g_sCanotUseItemMsg);
            }
            if (boEatOK)
            {
                WeightChanged();
                SendDefMessage(grobal2.SM_EAT_OK, 0, 0, 0, 0, "");
                if (StdItem.NeedIdentify == 1)
                {
                    M2Share.AddGameDataLog("11" + "\t" + m_sMapName + "\t" + m_nCurrX.ToString() + "\t" + m_nCurrY.ToString() + "\t" + m_sCharName + "\t" + StdItem.Name + "\t" + UserItem34.MakeIndex.ToString() + "\t" + '1' + "\t" + '0');
                }
            }
            else
            {
                SendDefMessage(grobal2.SM_EAT_FAIL, 0, 0, 0, 0, "");
            }
        }

        private bool UseStdmodeFunItem(TItem StdItem)
        {
            var result = false;
            if (M2Share.g_FunctionNPC != null)
            {
                M2Share.g_FunctionNPC.GotoLable(this, "@StdModeFunc" + StdItem.AniCount, false);
                result = true;
            }
            return result;
        }

        private bool ClientGetButchItem(int charId, int nX, int nY, byte btDir, ref int dwDelayTime)
        {
            var result = false;
            dwDelayTime = 0;
            var BaseObject = M2Share.ObjectSystem.Get(charId);
            var dwCheckTime = HUtil32.GetTickCount() - m_dwTurnTick;
            if (dwCheckTime < M2Share.g_Config.dwTurnIntervalTime)
            {
                dwDelayTime = M2Share.g_Config.dwTurnIntervalTime - dwCheckTime;
                return result;
            }
            m_dwTurnTick = HUtil32.GetTickCount();
            if (Math.Abs(nX - m_nCurrX) <= 2 && Math.Abs(nY - m_nCurrY) <= 2)
            {
                if (m_PEnvir.IsValidObject(nX, nY, 2, BaseObject))
                {
                    if (BaseObject.m_boDeath && !BaseObject.m_boSkeleton && BaseObject.m_boAnimal)
                    {
                        var n10 = M2Share.RandomNumber.Random(16) + 5;
                        var n14 = M2Share.RandomNumber.Random(201) + 100;
                        BaseObject.m_nBodyLeathery -= n10;
                        BaseObject.m_nMeatQuality -= (ushort)n14;
                        if (BaseObject.m_nMeatQuality < 0)
                        {
                            BaseObject.m_nMeatQuality = 0;
                        }
                        if (BaseObject.m_nBodyLeathery <= 0)
                        {
                            if (BaseObject.m_btRaceServer >= grobal2.RC_ANIMAL && BaseObject.m_btRaceServer < grobal2.RC_MONSTER)
                            {
                                BaseObject.m_boSkeleton = true;
                                ApplyMeatQuality();
                                BaseObject.SendRefMsg(grobal2.RM_SKELETON, BaseObject.m_btDirection, BaseObject.m_nCurrX, BaseObject.m_nCurrY, 0, "");
                            }
                            if (!TakeBagItems(BaseObject))
                            {
                                SysMsg(M2Share.sYouFoundNothing, TMsgColor.c_Red, TMsgType.t_Hint);
                            }
                            BaseObject.m_nBodyLeathery = 50;
                        }
                        m_dwDeathTick = HUtil32.GetTickCount();
                    }
                }
                m_btDirection = btDir;
            }
            SendRefMsg(grobal2.RM_BUTCH, m_btDirection, m_nCurrX, m_nCurrY, 0, "");
            return result;
        }

        private void ClientChangeMagicKey(int nSkillIdx, int nKey)
        {
            TUserMagic UserMagic;
            for (var i = 0; i < m_MagicList.Count; i++)
            {
                UserMagic = m_MagicList[i];
                if (UserMagic.MagicInfo.wMagicID == nSkillIdx)
                {
                    UserMagic.btKey = (byte)nKey;
                    break;
                }
            }
        }

        private void ClientGroupClose()
        {
            if (m_GroupOwner == null)
            {
                m_boAllowGroup = false;
                return;
            }
            if (m_GroupOwner != this)
            {
                m_GroupOwner.DelMember(this);
                m_boAllowGroup = false;
            }
            else
            {
                SysMsg("如果你想退出，使用编组功能（删除按钮）", TMsgColor.c_Red, TMsgType.t_Hint);
            }
            if (M2Share.g_FunctionNPC != null)
            {
                M2Share.g_FunctionNPC.GotoLable(this, "@GroupClose", false);
            }
        }

        private void ClientCreateGroup(string sHumName)
        {
            var PlayObject = M2Share.UserEngine.GetPlayObject(sHumName);
            if (m_GroupOwner != null)
            {
                SendDefMessage(grobal2.SM_CREATEGROUP_FAIL, -1, 0, 0, 0, "");
                return;
            }
            if (PlayObject == null || PlayObject == this || PlayObject.m_boDeath || PlayObject.m_boGhost)
            {
                SendDefMessage(grobal2.SM_CREATEGROUP_FAIL, -2, 0, 0, 0, "");
                return;
            }
            if (PlayObject.m_GroupOwner != null)
            {
                SendDefMessage(grobal2.SM_CREATEGROUP_FAIL, -3, 0, 0, 0, "");
                return;
            }
            if (!PlayObject.m_boAllowGroup)
            {
                SendDefMessage(grobal2.SM_CREATEGROUP_FAIL, -4, 0, 0, 0, "");
                return;
            }
            m_GroupMembers.Clear();
            this.m_GroupMembers.Add(this);
            this.m_GroupMembers.Add(PlayObject);
            JoinGroup(this);
            PlayObject.JoinGroup(this);
            m_boAllowGroup = true;
            SendDefMessage(grobal2.SM_CREATEGROUP_OK, 0, 0, 0, 0, "");
            SendGroupMembers();
            if (M2Share.g_FunctionNPC != null)
            {
                M2Share.g_FunctionNPC.GotoLable(this, "@GroupCreate", false);// 创建小组时触发
            }
        }

        private void ClientAddGroupMember(string sHumName)
        {
            var PlayObject = M2Share.UserEngine.GetPlayObject(sHumName);
            if (m_GroupOwner != this)
            {
                SendDefMessage(grobal2.SM_GROUPADDMEM_FAIL, -1, 0, 0, 0, "");
                return;
            }
            if (m_GroupMembers.Count > M2Share.g_Config.nGroupMembersMax)
            {
                SendDefMessage(grobal2.SM_GROUPADDMEM_FAIL, -5, 0, 0, 0, "");
                return;
            }
            if (PlayObject == null || PlayObject == this || PlayObject.m_boDeath || PlayObject.m_boGhost)
            {
                SendDefMessage(grobal2.SM_GROUPADDMEM_FAIL, -2, 0, 0, 0, "");
                return;
            }
            if (PlayObject.m_GroupOwner != null)
            {
                SendDefMessage(grobal2.SM_GROUPADDMEM_FAIL, -3, 0, 0, 0, "");
                return;
            }
            if (!PlayObject.m_boAllowGroup)
            {
                SendDefMessage(grobal2.SM_GROUPADDMEM_FAIL, -4, 0, 0, 0, "");
                return;
            }
            this.m_GroupMembers.Add(PlayObject);
            PlayObject.JoinGroup(this);
            SendDefMessage(grobal2.SM_GROUPADDMEM_OK, 0, 0, 0, 0, "");
            SendGroupMembers();
            if (M2Share.g_FunctionNPC != null)
            {
                M2Share.g_FunctionNPC.GotoLable(this, "@GroupAddMember", false);
            }
        }

        private void ClientDelGroupMember(string sHumName)
        {
            var PlayObject = M2Share.UserEngine.GetPlayObject(sHumName);
            if (m_GroupOwner != this)
            {
                SendDefMessage(grobal2.SM_GROUPDELMEM_FAIL, -1, 0, 0, 0, "");
                return;
            }
            if (PlayObject == null)
            {
                SendDefMessage(grobal2.SM_GROUPDELMEM_FAIL, -2, 0, 0, 0, "");
                return;
            }
            if (!IsGroupMember(PlayObject))
            {
                SendDefMessage(grobal2.SM_GROUPDELMEM_FAIL, -3, 0, 0, 0, "");
                return;
            }
            DelMember(PlayObject);
            SendDefMessage(grobal2.SM_GROUPDELMEM_OK, 0, 0, 0, 0, sHumName);
            if (M2Share.g_FunctionNPC != null)
            {
                M2Share.g_FunctionNPC.GotoLable(this, "@GroupDelMember", false);
            }
        }

        private void ClientDealTry(string sHumName)
        {
            TPlayObject TargetPlayObject;
            if (M2Share.g_Config.boDisableDeal)
            {
                SendMsg(M2Share.g_ManageNPC, grobal2.RM_MENU_OK, 0, ObjectId, 0, 0, M2Share.g_sDisableDealItemsMsg);
                return;
            }
            if (m_boDealing)
            {
                return;
            }
            if (HUtil32.GetTickCount() - m_DealLastTick < M2Share.g_Config.dwTryDealTime)
            {
                SendMsg(M2Share.g_ManageNPC, grobal2.RM_MENU_OK, 0, ObjectId, 0, 0, M2Share.g_sPleaseTryDealLaterMsg);
                return;
            }
            if (!m_boCanDeal)
            {
                SendMsg(M2Share.g_ManageNPC, grobal2.RM_MENU_OK, 0, ObjectId, 0, 0, M2Share.g_sCanotTryDealMsg);
                return;
            }
            TargetPlayObject = (TPlayObject)GetPoseCreate();
            if (TargetPlayObject != null && TargetPlayObject != this)
            {
                if (TargetPlayObject.GetPoseCreate() == this && !TargetPlayObject.m_boDealing)
                {
                    if (TargetPlayObject.m_btRaceServer == grobal2.RC_PLAYOBJECT)
                    {
                        if (TargetPlayObject.m_boAllowDeal && TargetPlayObject.m_boCanDeal)
                        {
                            TargetPlayObject.SysMsg(m_sCharName + M2Share.g_sOpenedDealMsg, TMsgColor.c_Green, TMsgType.t_Hint);
                            SysMsg(TargetPlayObject.m_sCharName + M2Share.g_sOpenedDealMsg, TMsgColor.c_Green, TMsgType.t_Hint);
                            this.OpenDealDlg(TargetPlayObject);
                            TargetPlayObject.OpenDealDlg(this);
                        }
                        else
                        {
                            SysMsg(M2Share.g_sPoseDisableDealMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                        }
                    }
                }
                else
                {
                    SendDefMessage(grobal2.SM_DEALTRY_FAIL, 0, 0, 0, 0, "");
                }
            }
            else
            {
                SendDefMessage(grobal2.SM_DEALTRY_FAIL, 0, 0, 0, 0, "");
            }
        }

        private void ClientAddDealItem(int nItemIdx, string sItemName)
        {
            bool bo11;
            TUserItem UserItem;
            string sUserItemName;
            if (m_DealCreat == null || !m_boDealing)
            {
                return;
            }
            if (sItemName.IndexOf(' ') >= 0)
            {
                // 折分物品名称(信件物品的名称后面加了使用次数)
                HUtil32.GetValidStr3(sItemName, ref sItemName, new string[] { " " });
            }
            bo11 = false;
            if (!m_DealCreat.m_boDealOK)
            {
                for (var i = 0; i < m_ItemList.Count; i++)
                {
                    UserItem = m_ItemList[i];
                    if (UserItem.MakeIndex == nItemIdx)
                    {
                        sUserItemName = ItmUnit.GetItemName(UserItem);// 取自定义物品名称
                        if (sUserItemName.CompareTo(sItemName) == 0 && m_DealItemList.Count < 12)
                        {
                            m_DealItemList.Add(UserItem);
                            this.SendAddDealItem(UserItem);
                            m_ItemList.RemoveAt(i);
                            bo11 = true;
                            break;
                        }
                    }
                }
            }
            if (!bo11)
            {
                SendDefMessage(grobal2.SM_DEALADDITEM_FAIL, 0, 0, 0, 0, "");
            }
        }

        private void ClientDelDealItem(int nItemIdx, string sItemName)
        {
            TUserItem UserItem;
            string sUserItemName;
            if (M2Share.g_Config.boCanNotGetBackDeal)
            {
                SendMsg(M2Share.g_ManageNPC, grobal2.RM_MENU_OK, 0, ObjectId, 0, 0, M2Share.g_sDealItemsDenyGetBackMsg);
                SendDefMessage(grobal2.SM_DEALDELITEM_FAIL, 0, 0, 0, 0, "");
                return;
            }
            if (m_DealCreat == null || !m_boDealing)
            {
                return;
            }
            if (sItemName.IndexOf(' ') >= 0)
            {
                // 折分物品名称(信件物品的名称后面加了使用次数)
                HUtil32.GetValidStr3(sItemName, ref sItemName, new string[] { " " });
            }
            bool bo11 = false;
            if (!m_DealCreat.m_boDealOK)
            {
                for (var i = 0; i < m_DealItemList.Count; i++)
                {
                    UserItem = m_DealItemList[i];
                    if (UserItem.MakeIndex == nItemIdx)
                    {
                        // 取自定义物品名称
                        sUserItemName = ItmUnit.GetItemName(UserItem);
                        if (sUserItemName.CompareTo(sItemName) == 0)
                        {
                            m_ItemList.Add(UserItem);
                            this.SendDelDealItem(UserItem);
                            m_DealItemList.RemoveAt(i);
                            bo11 = true;
                            break;
                        }
                    }
                }
            }
            if (!bo11)
            {
                SendDefMessage(grobal2.SM_DEALDELITEM_FAIL, 0, 0, 0, 0, "");
            }
        }

        private void ClientCancelDeal()
        {
            DealCancel();
        }

        private void ClientChangeDealGold(int nGold)
        {
            bool bo09;
            if (m_nDealGolds > 0 && M2Share.g_Config.boCanNotGetBackDeal)// 禁止取回放入交易栏内的金币
            {
                SendMsg(M2Share.g_ManageNPC, grobal2.RM_MENU_OK, 0, ObjectId, 0, 0, M2Share.g_sDealItemsDenyGetBackMsg);
                SendDefMessage(grobal2.SM_DEALDELITEM_FAIL, 0, 0, 0, 0, "");
                return;
            }
            if (nGold < 0)
            {
                SendDefMessage(grobal2.SM_DEALCHGGOLD_FAIL, m_nDealGolds, HUtil32.LoWord(m_nGold), HUtil32.HiWord(m_nGold), 0, "");
                return;
            }
            bo09 = false;
            if (m_DealCreat != null && GetPoseCreate() == m_DealCreat)
            {
                if (!m_DealCreat.m_boDealOK)
                {
                    if (m_nGold + m_nDealGolds >= nGold)
                    {
                        m_nGold = m_nGold + m_nDealGolds - nGold;
                        m_nDealGolds = nGold;
                        SendDefMessage(grobal2.SM_DEALCHGGOLD_OK, m_nDealGolds, HUtil32.LoWord(m_nGold), HUtil32.HiWord(m_nGold), 0, "");
                        (m_DealCreat as TPlayObject).SendDefMessage(grobal2.SM_DEALREMOTECHGGOLD, m_nDealGolds, 0, 0, 0, "");
                        m_DealCreat.m_DealLastTick = HUtil32.GetTickCount();
                        bo09 = true;
                        m_DealLastTick = HUtil32.GetTickCount();
                    }
                }
            }
            if (!bo09)
            {
                SendDefMessage(grobal2.SM_DEALCHGGOLD_FAIL, m_nDealGolds, HUtil32.LoWord(m_nGold), HUtil32.HiWord(m_nGold), 0, "");
            }
        }

        private void ClientDealEnd()
        {
            bool bo11;
            TUserItem UserItem;
            TItem StdItem;
            TPlayObject PlayObject;
            m_boDealOK = true;
            if (m_DealCreat == null)
            {
                return;
            }
            if (HUtil32.GetTickCount() - m_DealLastTick < M2Share.g_Config.dwDealOKTime || HUtil32.GetTickCount() - m_DealCreat.m_DealLastTick
                < M2Share.g_Config.dwDealOKTime)
            {
                SysMsg(M2Share.g_sDealOKTooFast, TMsgColor.c_Red, TMsgType.t_Hint);
                DealCancel();
                return;
            }
            if (m_DealCreat.m_boDealOK)
            {
                bo11 = true;
                if (grobal2.MAXBAGITEM - m_ItemList.Count < m_DealCreat.m_DealItemList.Count)
                {
                    bo11 = false;
                    SysMsg(M2Share.g_sYourBagSizeTooSmall, TMsgColor.c_Red, TMsgType.t_Hint);
                }
                if (m_nGoldMax - m_nGold < m_DealCreat.m_nDealGolds)
                {
                    SysMsg(M2Share.g_sYourGoldLargeThenLimit, TMsgColor.c_Red, TMsgType.t_Hint);
                    bo11 = false;
                }
                if (grobal2.MAXBAGITEM - m_DealCreat.m_ItemList.Count < m_DealItemList.Count)
                {
                    SysMsg(M2Share.g_sDealHumanBagSizeTooSmall, TMsgColor.c_Red, TMsgType.t_Hint);
                    bo11 = false;
                }
                if (m_DealCreat.m_nGoldMax - m_DealCreat.m_nGold < m_nDealGolds)
                {
                    SysMsg(M2Share.g_sDealHumanGoldLargeThenLimit, TMsgColor.c_Red, TMsgType.t_Hint);
                    bo11 = false;
                }
                if (bo11)
                {
                    for (var i = 0; i < m_DealItemList.Count; i++)
                    {
                        UserItem = m_DealItemList[i];
                        m_DealCreat.AddItemToBag(UserItem);
                        (m_DealCreat as TPlayObject).SendAddItem(UserItem);
                        StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                        if (StdItem != null)
                        {
                            if (!M2Share.IsCheapStuff(StdItem.StdMode))
                            {
                                if (StdItem.NeedIdentify == 1)
                                {
                                    M2Share.AddGameDataLog('8' + "\t" + m_sMapName + "\t" + m_nCurrX.ToString() + "\t" + m_nCurrY.ToString() + "\t" + m_sCharName + "\t" + StdItem.Name + "\t" + UserItem.MakeIndex.ToString() + "\t" + '1' + "\t" + m_DealCreat.m_sCharName);
                                }
                            }
                        }
                    }
                    if (m_nDealGolds > 0)
                    {
                        m_DealCreat.m_nGold += m_nDealGolds;
                        m_DealCreat.GoldChanged();
                        if (M2Share.g_boGameLogGold)
                        {
                            M2Share.AddGameDataLog('8' + "\t" + m_sMapName + "\t" + m_nCurrX.ToString() + "\t" + m_nCurrY.ToString() + "\t" + m_sCharName + "\t" + grobal2.sSTRING_GOLDNAME + "\t" + m_nGold.ToString() + "\t" + '1' + "\t" + m_DealCreat.m_sCharName);
                        }
                    }
                    for (var i = 0; i < m_DealCreat.m_DealItemList.Count; i++)
                    {
                        UserItem = m_DealCreat.m_DealItemList[i];
                        AddItemToBag(UserItem);
                        this.SendAddItem(UserItem);
                        StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                        if (StdItem != null)
                        {
                            if (!M2Share.IsCheapStuff(StdItem.StdMode))
                            {
                                if (StdItem.NeedIdentify == 1)
                                {
                                    M2Share.AddGameDataLog('8' + "\t" + m_DealCreat.m_sMapName + "\t" + m_DealCreat.m_nCurrX.ToString() + "\t" + m_DealCreat.m_nCurrY.ToString() + "\t" + m_DealCreat.m_sCharName + "\t" + StdItem.Name + "\t" + UserItem.MakeIndex.ToString() + "\t" + '1' + "\t" + m_sCharName);
                                }
                            }
                        }
                    }
                    if (m_DealCreat.m_nDealGolds > 0)
                    {
                        m_nGold += m_DealCreat.m_nDealGolds;
                        GoldChanged();
                        if (M2Share.g_boGameLogGold)
                        {
                            M2Share.AddGameDataLog('8' + "\t" + m_DealCreat.m_sMapName + "\t" + m_DealCreat.m_nCurrX.ToString() + "\t" + m_DealCreat.m_nCurrY.ToString() + "\t" + m_DealCreat.m_sCharName + "\t" + grobal2.sSTRING_GOLDNAME + "\t" + m_DealCreat.m_nGold.ToString() + "\t" + '1' + "\t" + m_sCharName);
                        }
                    }
                    PlayObject = m_DealCreat as TPlayObject;
                    PlayObject.SendDefMessage(grobal2.SM_DEALSUCCESS, 0, 0, 0, 0, "");
                    PlayObject.SysMsg(M2Share.g_sDealSuccessMsg, TMsgColor.c_Green, TMsgType.t_Hint);
                    PlayObject.m_DealCreat = null;
                    PlayObject.m_boDealing = false;
                    PlayObject.m_DealItemList.Clear();
                    PlayObject.m_nDealGolds = 0;
                    PlayObject.m_boDealOK = false;
                    SendDefMessage(grobal2.SM_DEALSUCCESS, 0, 0, 0, 0, "");
                    SysMsg(M2Share.g_sDealSuccessMsg, TMsgColor.c_Green, TMsgType.t_Hint);
                    m_DealCreat = null;
                    m_boDealing = false;
                    m_DealItemList.Clear();
                    m_nDealGolds = 0;
                    m_boDealOK = false;
                }
                else
                {
                    DealCancel();
                }
            }
            else
            {
                SysMsg(M2Share.g_sYouDealOKMsg, TMsgColor.c_Green, TMsgType.t_Hint);
                m_DealCreat.SysMsg(M2Share.g_sPoseDealOKMsg, TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        private void ClientGetMinMap()
        {
            var nMinMap = m_PEnvir.nMinMap;
            if (nMinMap > 0)
            {
                SendDefMessage(grobal2.SM_READMINIMAP_OK, 0, (short)nMinMap, 0, 0, "");
            }
            else
            {
                SendDefMessage(grobal2.SM_READMINIMAP_FAIL, 0, 0, 0, 0, "");
            }
        }

        private void ClientMakeDrugItem(int NPC, string nItemName)
        {
            var Merchant = M2Share.UserEngine.FindMerchant<TMerchant>(NPC);
            if (Merchant == null || !Merchant.m_boMakeDrug)
            {
                return;
            }
            if (Merchant.m_PEnvir == m_PEnvir && Math.Abs(Merchant.m_nCurrX - m_nCurrX) < 15 && Math.Abs(Merchant.m_nCurrY - m_nCurrY) < 15)
            {
                Merchant.ClientMakeDrugItem(this, nItemName);
            }
        }

        private void ClientOpenGuildDlg()
        {
            string sC;
            if (m_MyGuild != null)
            {
                sC = m_MyGuild.sGuildName + '\r' + ' ' + '\r';
                if (m_nGuildRankNo == 1)
                {
                    sC = sC + '1' + '\r';
                }
                else
                {
                    sC = sC + '0' + '\r';
                }
                sC = sC + "<Notice>" + '\r';
                for (var I = 0; I < m_MyGuild.NoticeList.Count; I++)
                {
                    if (sC.Length > 5000)
                    {
                        break;
                    }
                    sC = sC + m_MyGuild.NoticeList[I] + '\r';
                }
                sC = sC + "<KillGuilds>" + '\r';
                for (var I = 0; I < m_MyGuild.GuildWarList.Count; I++)
                {
                    if (sC.Length > 5000)
                    {
                        break;
                    }
                    sC = sC + m_MyGuild.GuildWarList[I] + '\r';
                }
                sC = sC + "<AllyGuilds>" + '\r';
                for (var i = 0; i < m_MyGuild.GuildAllList.Count; i++)
                {
                    if (sC.Length > 5000)
                    {
                        break;
                    }
                    sC = sC + m_MyGuild.GuildAllList[i] + '\r';
                }
                m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_OPENGUILDDLG, 0, 0, 0, 1);
                SendSocket(m_DefMsg, EDcode.EncodeString(sC));
            }
            else
            {
                SendDefMessage(grobal2.SM_OPENGUILDDLG_FAIL, 0, 0, 0, 0, "");
            }
        }

        private void ClientGuildHome()
        {
            ClientOpenGuildDlg();
        }

        private void ClientGuildMemberList()
        {
            TGuildRank GuildRank;
            var sSendMsg = string.Empty;
            if (m_MyGuild == null)
            {
                return;
            }
            for (var i = 0; i < m_MyGuild.m_RankList.Count; i++)
            {
                GuildRank = m_MyGuild.m_RankList[i];
                sSendMsg = sSendMsg + '#' + GuildRank.nRankNo + "/*" + GuildRank.sRankName + '/';
                for (var j = 0; j < GuildRank.MemberList.Count; j++)
                {
                    if (sSendMsg.Length > 5000)
                    {
                        break;
                    }
                    sSendMsg = sSendMsg + GuildRank.MemberList[j] + '/';
                }
            }
            m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_SENDGUILDMEMBERLIST, 0, 0, 0, 1);
            SendSocket(m_DefMsg, EDcode.EncodeString(sSendMsg));
        }

        private void ClientGuildAddMember(string sHumName)
        {
            var nC = 1; // '你没有权利使用这个命令。'
            if (IsGuildMaster())
            {
                var PlayObject = M2Share.UserEngine.GetPlayObject(sHumName);
                if (PlayObject != null)
                {
                    if (PlayObject.GetPoseCreate() == this)
                    {
                        if (PlayObject.m_boAllowGuild)
                        {
                            if (!m_MyGuild.IsMember(sHumName))
                            {
                                if (PlayObject.m_MyGuild == null && m_MyGuild.m_RankList.Count < 400)
                                {
                                    m_MyGuild.AddMember(PlayObject);
                                    M2Share.UserEngine.SendServerGroupMsg(grobal2.SS_207, M2Share.nServerIndex,
                                        m_MyGuild.sGuildName);
                                    PlayObject.m_MyGuild = m_MyGuild;
                                    PlayObject.m_sGuildRankName =
                                        m_MyGuild.GetRankName(PlayObject, ref PlayObject.m_nGuildRankNo);
                                    PlayObject.RefShowName();
                                    PlayObject.SysMsg(
                                        "你已加入行会: " + m_MyGuild.sGuildName + " 当前封号为: " + PlayObject.m_sGuildRankName,
                                        TMsgColor.c_Green, TMsgType.t_Hint);
                                    nC = 0;
                                }
                                else
                                {
                                    nC = 4; // '对方已经加入其他行会。'
                                }
                            }
                            else
                            {
                                nC = 3; // '对方已经加入我们的行会。'
                            }
                        }
                        else
                        {
                            nC = 5; // '对方不允许加入行会。'
                            PlayObject.SysMsg("你拒绝加入行会。 [允许命令为 @" + M2Share.g_GameCommand.LETGUILD.sCmd + ']',
                                TMsgColor.c_Red, TMsgType.t_Hint);
                        }
                    }
                    else
                    {
                        nC = 2; // '想加入进来的成员应该来面对掌门人。'
                    }
                }
                else
                {
                    nC = 2;
                }
            }
            if (nC == 0)
            {
                SendDefMessage(grobal2.SM_GUILDADDMEMBER_OK, 0, 0, 0, 0, "");
            }
            else
            {
                SendDefMessage(grobal2.SM_GUILDADDMEMBER_FAIL, nC, 0, 0, 0, "");
            }
        }

        private void ClientGuildDelMember(string sHumName)
        {
            string s14;
            TPlayObject PlayObject;
            var nC = 1;
            if (IsGuildMaster())
            {
                if (m_MyGuild.IsMember(sHumName))
                {
                    if (m_sCharName != sHumName)
                    {
                        if (m_MyGuild.DelMember(sHumName))
                        {
                            PlayObject = M2Share.UserEngine.GetPlayObject(sHumName);
                            if (PlayObject != null)
                            {
                                PlayObject.m_MyGuild = null;
                                PlayObject.RefRankInfo(0, "");
                                PlayObject.RefShowName();
                            }
                            M2Share.UserEngine.SendServerGroupMsg(grobal2.SS_207, M2Share.nServerIndex, m_MyGuild.sGuildName);
                            nC = 0;
                        }
                        else
                        {
                            nC = 4;
                        }
                    }
                    else
                    {
                        nC = 3;
                        s14 = m_MyGuild.sGuildName;
                        if (m_MyGuild.CancelGuld(sHumName))
                        {
                            M2Share.GuildManager.DelGuild(s14);
                            M2Share.UserEngine.SendServerGroupMsg(grobal2.SS_206, M2Share.nServerIndex, s14);
                            m_MyGuild = null;
                            RefRankInfo(0, "");
                            RefShowName();
                            SysMsg("行会" + s14 + "已被取消！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                            nC = 0;
                        }
                    }
                }
                else
                {
                    nC = 2;
                }
            }
            if (nC == 0)
            {
                SendDefMessage(grobal2.SM_GUILDDELMEMBER_OK, 0, 0, 0, 0, "");
            }
            else
            {
                SendDefMessage(grobal2.SM_GUILDDELMEMBER_FAIL, nC, 0, 0, 0, "");
            }
        }

        private void ClientGuildUpdateNotice(string sNotict)
        {
            var sC = string.Empty;
            if (m_MyGuild == null || m_nGuildRankNo != 1)
            {
                return;
            }
            m_MyGuild.NoticeList.Clear();
            while (!string.IsNullOrEmpty(sNotict))
            {
                sNotict = HUtil32.GetValidStr3(sNotict, ref sC, new string[] { "\r" });
                m_MyGuild.NoticeList.Add(sC);
            }
            m_MyGuild.SaveGuildInfoFile();
            M2Share.UserEngine.SendServerGroupMsg(grobal2.SS_207, M2Share.nServerIndex, m_MyGuild.sGuildName);
            ClientOpenGuildDlg();
        }

        private void ClientGuildUpdateRankInfo(string sRankInfo)
        {
            if (m_MyGuild == null || m_nGuildRankNo != 1)
            {
                return;
            }
            var nC = m_MyGuild.UpdateRank(sRankInfo);
            if (nC == 0)
            {
                M2Share.UserEngine.SendServerGroupMsg(grobal2.SS_207, M2Share.nServerIndex, m_MyGuild.sGuildName);
                ClientGuildMemberList();
            }
            else
            {
                if (nC <= -2)
                {
                    SendDefMessage(grobal2.SM_GUILDRANKUPDATE_FAIL, nC, 0, 0, 0, "");
                }
            }
        }

        private void ClientGuildAlly()
        {
            const string sExceptionMsg = "[Exception] TPlayObject::ClientGuildAlly";
            try
            {
                var n8 = -1;
                TBaseObject BaseObjectC = GetPoseCreate();
                if (BaseObjectC != null && BaseObjectC.m_MyGuild != null && BaseObjectC.m_btRaceServer == grobal2.RC_PLAYOBJECT && BaseObjectC.GetPoseCreate() == this)
                {
                    if (BaseObjectC.m_MyGuild.m_boEnableAuthAlly)
                    {
                        if (BaseObjectC.IsGuildMaster() && IsGuildMaster())
                        {
                            if (m_MyGuild.IsNotWarGuild(BaseObjectC.m_MyGuild) && BaseObjectC.m_MyGuild.IsNotWarGuild(m_MyGuild))
                            {
                                m_MyGuild.AllyGuild(BaseObjectC.m_MyGuild);
                                BaseObjectC.m_MyGuild.AllyGuild(m_MyGuild);
                                m_MyGuild.SendGuildMsg(BaseObjectC.m_MyGuild.sGuildName + "行会已经和您的行会联盟成功。");
                                BaseObjectC.m_MyGuild.SendGuildMsg(m_MyGuild.sGuildName + "行会已经和您的行会联盟成功。");
                                m_MyGuild.RefMemberName();
                                BaseObjectC.m_MyGuild.RefMemberName();
                                M2Share.UserEngine.SendServerGroupMsg(grobal2.SS_207, M2Share.nServerIndex, m_MyGuild.sGuildName);
                                M2Share.UserEngine.SendServerGroupMsg(grobal2.SS_207, M2Share.nServerIndex, BaseObjectC.m_MyGuild.sGuildName);
                                n8 = 0;
                            }
                            else
                            {
                                n8 = -2;
                            }
                        }
                        else
                        {
                            n8 = -3;
                        }
                    }
                    else
                    {
                        n8 = -4;
                    }
                }
                if (n8 == 0)
                {
                    SendDefMessage(grobal2.SM_GUILDMAKEALLY_OK, 0, 0, 0, 0, "");
                }
                else
                {
                    SendDefMessage(grobal2.SM_GUILDMAKEALLY_FAIL, n8, 0, 0, 0, "");
                }
            }
            catch (Exception e)
            {
                M2Share.ErrorMessage(sExceptionMsg);
                M2Share.ErrorMessage(e.Message);
            }
        }

        private void ClientGuildBreakAlly(string sGuildName)
        {
            var n10 = -1;
            if (!IsGuildMaster())
            {
                return;
            }
            var guild = M2Share.GuildManager.FindGuild(sGuildName);
            if (guild != null)
            {
                if (m_MyGuild.IsAllyGuild(guild))
                {
                    m_MyGuild.DelAllyGuild(guild);
                    guild.DelAllyGuild(m_MyGuild);
                    m_MyGuild.SendGuildMsg(guild.sGuildName + " 行会与您的行会解除联盟成功！！！");
                    guild.SendGuildMsg(m_MyGuild.sGuildName + " 行会解除了与您行会的联盟！！！");
                    m_MyGuild.RefMemberName();
                    guild.RefMemberName();
                    M2Share.UserEngine.SendServerGroupMsg(grobal2.SS_207, M2Share.nServerIndex, m_MyGuild.sGuildName);
                    M2Share.UserEngine.SendServerGroupMsg(grobal2.SS_207, M2Share.nServerIndex, guild.sGuildName);
                    n10 = 0;
                }
                else
                {
                    n10 = -2;
                }
            }
            else
            {
                n10 = -3;
            }
            if (n10 == 0)
            {
                SendDefMessage(grobal2.SM_GUILDBREAKALLY_OK, 0, 0, 0, 0, "");
            }
            else
            {
                SendDefMessage(grobal2.SM_GUILDMAKEALLY_FAIL, 0, 0, 0, 0, "");
            }
        }

        public void RecalcAdjusBonus_AdjustAb(byte abil, short val, ref short lov, ref short hiv)
        {
            var lo = HUtil32.LoByte(abil);
            var hi = HUtil32.HiByte(abil);
            lov = 0;
            hiv = 0;
            for (var i = 1; i <= val; i++)
            {
                if (lo + 1 < hi)
                {
                    lo++;
                    lov++;
                }
                else
                {
                    hi++;
                    hiv++;
                }
            }
        }

        private void RecalcAdjusBonus()
        {
            TNakedAbility BonusTick;
            TNakedAbility NakedAbil;
            short adc;
            short amc;
            short asc;
            short aac;
            short amac;
            short ldc = 0;
            short lmc = 0;
            short lsc = 0;
            short lac = 0;
            short lmac = 0;
            short hdc = 0;
            short hmc = 0;
            short hsc = 0;
            short hac = 0;
            short hmac = 0;
            BonusTick = null;
            NakedAbil = null;
            switch (m_btJob)
            {
                case M2Share.jWarr:
                    BonusTick = M2Share.g_Config.BonusAbilofWarr;
                    NakedAbil = M2Share.g_Config.NakedAbilofWarr;
                    break;
                case M2Share.jWizard:
                    BonusTick = M2Share.g_Config.BonusAbilofWizard;
                    NakedAbil = M2Share.g_Config.NakedAbilofWizard;
                    break;
                case M2Share.jTaos:
                    BonusTick = M2Share.g_Config.BonusAbilofTaos;
                    NakedAbil = M2Share.g_Config.NakedAbilofTaos;
                    break;
            }
            adc = (short)(m_BonusAbil.DC / BonusTick.DC);
            amc = (short)(m_BonusAbil.MC / BonusTick.MC);
            asc = (short)(m_BonusAbil.SC / BonusTick.SC);
            aac = (short)(m_BonusAbil.AC / BonusTick.AC);
            amac = (short)(m_BonusAbil.MAC / BonusTick.MAC);
            RecalcAdjusBonus_AdjustAb((byte)NakedAbil.DC, adc, ref ldc, ref hdc);
            RecalcAdjusBonus_AdjustAb((byte)NakedAbil.MC, amc, ref lmc, ref hmc);
            RecalcAdjusBonus_AdjustAb((byte)NakedAbil.SC, asc, ref lsc, ref hsc);
            RecalcAdjusBonus_AdjustAb((byte)NakedAbil.AC, aac, ref lac, ref hac);
            RecalcAdjusBonus_AdjustAb((byte)NakedAbil.MAC, amac, ref lmac, ref hmac);
            m_WAbil.DC = HUtil32.MakeLong(HUtil32.LoWord(m_WAbil.DC) + ldc, HUtil32.HiWord(m_WAbil.DC) + hdc);
            m_WAbil.MC = HUtil32.MakeLong(HUtil32.LoWord(m_WAbil.MC) + lmc, HUtil32.HiWord(m_WAbil.MC) + hmc);
            m_WAbil.SC = HUtil32.MakeLong(HUtil32.LoWord(m_WAbil.SC) + lsc, HUtil32.HiWord(m_WAbil.SC) + hsc);
            m_WAbil.AC = HUtil32.MakeLong(HUtil32.LoWord(m_WAbil.AC) + lac, HUtil32.HiWord(m_WAbil.AC) + hac);
            m_WAbil.MAC = HUtil32.MakeLong(HUtil32.LoWord(m_WAbil.MAC) + lmac, HUtil32.HiWord(m_WAbil.MAC) + hmac);
            m_WAbil.MaxHP = (ushort)HUtil32._MIN(short.MaxValue, m_WAbil.MaxHP + m_BonusAbil.HP / BonusTick.HP);
            m_WAbil.MaxMP = (ushort)HUtil32._MIN(short.MaxValue, m_WAbil.MaxMP + m_BonusAbil.MP / BonusTick.MP);
            // m_btSpeedPoint:=m_btSpeedPoint + m_BonusAbil.Speed div BonusTick.Speed;
            // m_btHitPoint:=m_btHitPoint + m_BonusAbil.Hit div BonusTick.Hit;
        }

        private void ClientAdjustBonus(int nPoint, string sMsg)
        {
            var BonusAbil = new TNakedAbility();
            int nTotleUsePoint;
            //FillChar(BonusAbil, '\0');
            //EDcode.DecodeBuffer(sMsg, BonusAbil);
            nTotleUsePoint = BonusAbil.DC + BonusAbil.MC + BonusAbil.SC + BonusAbil.AC + BonusAbil.MAC + BonusAbil.HP + BonusAbil.MP + BonusAbil.Hit + BonusAbil.Speed + BonusAbil.X2;
            if (nPoint + nTotleUsePoint == m_nBonusPoint)
            {
                m_nBonusPoint = nPoint;
                m_BonusAbil.DC += BonusAbil.DC;
                m_BonusAbil.MC += BonusAbil.MC;
                m_BonusAbil.SC += BonusAbil.SC;
                m_BonusAbil.AC += BonusAbil.AC;
                m_BonusAbil.MAC += BonusAbil.MAC;
                m_BonusAbil.HP += BonusAbil.HP;
                m_BonusAbil.MP += BonusAbil.MP;
                m_BonusAbil.Hit += BonusAbil.Hit;
                m_BonusAbil.Speed += BonusAbil.Speed;
                m_BonusAbil.X2 += BonusAbil.X2;
                RecalcAbilitys();
                SendMsg(this, grobal2.RM_ABILITY, 0, 0, 0, 0, "");
                SendMsg(this, grobal2.RM_SUBABILITY, 0, 0, 0, 0, "");
            }
            else
            {
                SysMsg("非法数据调整！！！", TMsgColor.c_Red, TMsgType.t_Hint);
            }
        }

        public int GetMyStatus()
        {
            var result = m_nHungerStatus / 1000;
            if (result > 4)
            {
                result = 4;
            }
            return result;
        }

        private void SendAdjustBonus()
        {
            var sSendMsg = string.Empty;
            switch (m_btJob)
            {
                case M2Share.jWarr:
                    sSendMsg = EDcode.EncodeBuffer(M2Share.g_Config.BonusAbilofWarr) + '/' + EDcode.EncodeBuffer(m_BonusAbil) + '/' + EDcode.EncodeBuffer(M2Share.g_Config.NakedAbilofWarr);
                    break;
                case M2Share.jWizard:
                    sSendMsg = EDcode.EncodeBuffer(M2Share.g_Config.BonusAbilofWizard) + '/' + EDcode.EncodeBuffer(m_BonusAbil) + '/' + EDcode.EncodeBuffer(M2Share.g_Config.NakedAbilofWizard);
                    break;
                case M2Share.jTaos:
                    sSendMsg = EDcode.EncodeBuffer(M2Share.g_Config.BonusAbilofTaos) + '/' + EDcode.EncodeBuffer(m_BonusAbil) + '/' + EDcode.EncodeBuffer(M2Share.g_Config.NakedAbilofTaos);
                    break;
            }
            m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_ADJUST_BONUS, m_nBonusPoint, 0, 0, 0);
            SendSocket(m_DefMsg, sSendMsg);
        }

        private void ShowMapInfo(string sMap, string sX, string sY)
        {
            TEnvirnoment Map;
            TMapCellinfo MapCellInfo = null;
            var nX = (short)HUtil32.Str_ToInt(sX, 0);
            var nY = (short)HUtil32.Str_ToInt(sY, 0);
            if (sMap != "" && nX >= 0 && nY >= 0)
            {
                Map = M2Share.g_MapManager.FindMap(sMap);
                if (Map != null)
                {
                    if (Map.GetMapCellInfo(nX, nY, ref MapCellInfo))
                    {
                        SysMsg("标志: " + MapCellInfo.chFlag, TMsgColor.c_Green, TMsgType.t_Hint);
                        if (MapCellInfo.ObjList != null)
                        {
                            SysMsg("对象数: " + MapCellInfo.ObjList.Count, TMsgColor.c_Green, TMsgType.t_Hint);
                        }
                    }
                    else
                    {
                        SysMsg("取地图单元信息失败: " + sMap, TMsgColor.c_Red, TMsgType.t_Hint);
                    }
                }
            }
            else
            {
                SysMsg("请按正确格式输入: " + M2Share.g_GameCommand.MAPINFO.sCmd + " 地图号 X Y", TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        public void PKDie(TPlayObject PlayObject)
        {
            var nWinLevel = M2Share.g_Config.nKillHumanWinLevel;
            var nLostLevel = M2Share.g_Config.nKilledLostLevel;
            var nWinExp = M2Share.g_Config.nKillHumanWinExp;
            var nLostExp = M2Share.g_Config.nKillHumanLostExp;
            var boWinLEvel = M2Share.g_Config.boKillHumanWinLevel;
            var boLostLevel = M2Share.g_Config.boKilledLostLevel;
            var boWinExp = M2Share.g_Config.boKillHumanWinExp;
            var boLostExp = M2Share.g_Config.boKilledLostExp;
            if (m_PEnvir.Flag.boPKWINLEVEL)
            {
                boWinLEvel = true;
                nWinLevel = m_PEnvir.Flag.nPKWINLEVEL;
            }
            if (m_PEnvir.Flag.boPKLOSTLEVEL)
            {
                boLostLevel = true;
                nLostLevel = m_PEnvir.Flag.nPKLOSTLEVEL;
            }
            if (m_PEnvir.Flag.boPKWINEXP)
            {
                boWinExp = true;
                nWinExp = m_PEnvir.Flag.nPKWINEXP;
            }
            if (m_PEnvir.Flag.boPKLOSTEXP)
            {
                boLostExp = true;
                nLostExp = m_PEnvir.Flag.nPKLOSTEXP;
            }
            if (PlayObject.m_Abil.Level - m_Abil.Level > M2Share.g_Config.nHumanLevelDiffer)
            {
                if (!PlayObject.IsGoodKilling(this))
                {
                    PlayObject.IncPkPoint(M2Share.g_Config.nKillHumanAddPKPoint);
                    PlayObject.SysMsg(M2Share.g_sYouMurderedMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                    SysMsg(format(M2Share.g_sYouKilledByMsg, m_LastHiter.m_sCharName), TMsgColor.c_Red, TMsgType.t_Hint);
                    PlayObject.AddBodyLuck(-M2Share.g_Config.nKillHumanDecLuckPoint);
                    if (PKLevel() < 1)
                    {
                        if (M2Share.RandomNumber.Random(5) == 0)
                        {
                            PlayObject.MakeWeaponUnlock();
                        }
                    }
                    if (M2Share.g_FunctionNPC != null)
                    {
                        M2Share.g_FunctionNPC.GotoLable(PlayObject, "@OnMurder", false);
                        M2Share.g_FunctionNPC.GotoLable(this, "@Murdered", false);
                    }
                }
                else
                {
                    PlayObject.SysMsg(M2Share.g_sYouProtectedByLawOfDefense, TMsgColor.c_Green, TMsgType.t_Hint);
                }
                return;
            }
            if (boWinLEvel)
            {
                if (PlayObject.m_Abil.Level + nWinLevel <= M2Share.MAXUPLEVEL)
                {
                    PlayObject.m_Abil.Level += (ushort)nWinLevel;
                }
                else
                {
                    PlayObject.m_Abil.Level = M2Share.MAXUPLEVEL;
                }
                PlayObject.HasLevelUp(PlayObject.m_Abil.Level - nWinLevel);
                if (boLostLevel)
                {
                    if (PKLevel() >= 2)
                    {
                        if (m_Abil.Level >= nLostLevel * 2)
                        {
                            m_Abil.Level -= (ushort)(nLostLevel * 2);
                        }
                    }
                    else
                    {
                        if (m_Abil.Level >= nLostLevel)
                        {
                            m_Abil.Level -= (ushort)nLostLevel;
                        }
                    }
                }
            }
            if (boWinExp)
            {
                PlayObject.WinExp(nWinExp);
                if (boLostExp)
                {
                    if (m_Abil.Exp >= nLostExp)
                    {
                        if (m_Abil.Exp >= nLostExp)
                        {
                            m_Abil.Exp -= nLostExp;
                        }
                        else
                        {
                            m_Abil.Exp = 0;
                        }
                    }
                    else
                    {
                        if (m_Abil.Level >= 1)
                        {
                            m_Abil.Level -= 1;
                            m_Abil.Exp += GetLevelExp(m_Abil.Level);
                            if (m_Abil.Exp >= nLostExp)
                            {
                                m_Abil.Exp -= nLostExp;
                            }
                            else
                            {
                                m_Abil.Exp = 0;
                            }
                        }
                        else
                        {
                            m_Abil.Level = 0;
                            m_Abil.Exp = 0;
                        }
                    }
                }
            }
        }

        public bool CancelGroup()
        {
            var result = true;
            const string sCanceGrop = "你的小组被解散了.";
            if (m_GroupMembers.Count <= 1)
            {
                SendGroupText(sCanceGrop);
                m_GroupMembers.Clear();
                m_GroupOwner = null;
                result = false;
            }
            return result;
        }

        public void SendGroupMembers()
        {
            TPlayObject PlayObject;
            var sSendMsg = "";
            for (var i = 0; i < m_GroupMembers.Count; i++)
            {
                PlayObject = m_GroupMembers[i];
                sSendMsg = sSendMsg + PlayObject.m_sCharName + '/';
            }
            for (var i = 0; i < m_GroupMembers.Count; i++)
            {
                PlayObject = m_GroupMembers[i];
                PlayObject.SendDefMessage(grobal2.SM_GROUPMEMBERS, 0, 0, 0, 0, sSendMsg);
            }
        }

        protected ushort GetSpellPoint(TUserMagic UserMagic)
        {
            return (ushort)(HUtil32.Round(UserMagic.MagicInfo.wSpell / (UserMagic.MagicInfo.btTrainLv + 1) * (UserMagic.btLevel + 1)) + UserMagic.MagicInfo.btDefSpell);
        }

        public bool DoMotaebo_CanMotaebo(TBaseObject BaseObject, int nMagicLevel)
        {
            var result = true;
            if (m_Abil.Level > BaseObject.m_Abil.Level && !BaseObject.m_boStickMode)
            {
                var nC = m_Abil.Level - BaseObject.m_Abil.Level;
                if (M2Share.RandomNumber.Random(20) < nMagicLevel * 4 + 6 + nC)
                {
                    if (IsProperTarget(BaseObject))
                    {
                        result = true;
                    }
                }
            }
            return result;
        }

        protected bool DoMotaebo(byte nDir, int nMagicLevel)
        {
            int nDmg;
            TBaseObject BaseObject_30 = null;
            TBaseObject BaseObject_34 = null;
            short nX = 0;
            short nY = 0;
            var result = false;
            var bo35 = true;
            var n24 = nMagicLevel + 1;
            var n28 = n24;
            this.m_btDirection = nDir;
            var PoseCreate = GetPoseCreate();
            if (PoseCreate != null)
            {
                for (var i = 0; i <= HUtil32._MAX(2, nMagicLevel + 1); i++)
                {
                    PoseCreate = GetPoseCreate();
                    if (PoseCreate != null)
                    {
                        n28 = 0;
                        if (!DoMotaebo_CanMotaebo(PoseCreate, nMagicLevel))
                        {
                            break;
                        }
                        if (nMagicLevel >= 3)
                        {
                            if (m_PEnvir.GetNextPosition(m_nCurrX, m_nCurrY, m_btDirection, 2, ref nX, ref nY))
                            {
                                BaseObject_30 = (TBaseObject)m_PEnvir.GetMovingObject(nX, nY, true);
                                if (BaseObject_30 != null && DoMotaebo_CanMotaebo(BaseObject_30, nMagicLevel))
                                {
                                    BaseObject_30.CharPushed(m_btDirection, 1);
                                }
                            }
                        }
                        BaseObject_34 = PoseCreate;
                        if (PoseCreate.CharPushed(m_btDirection, 1) != 1)
                        {
                            break;
                        }
                        GetFrontPosition(ref nX, ref nY);
                        if (m_PEnvir.MoveToMovingObject(m_nCurrX, m_nCurrY, this, nX, nY, false) > 0)
                        {
                            m_nCurrX = nX;
                            m_nCurrY = nY;
                            SendRefMsg(grobal2.RM_RUSH, nDir, m_nCurrX, m_nCurrY, 0, "");
                            bo35 = false;
                            result = true;
                        }
                        n24 -= 1;
                    }
                }
            }
            else
            {
                bo35 = false;
                for (var i = 0; i <= HUtil32._MAX(2, nMagicLevel + 1); i++)
                {
                    GetFrontPosition(ref nX, ref nY);
                    if (m_PEnvir.MoveToMovingObject(m_nCurrX, m_nCurrY, this, nX, nY, false) > 0)
                    {
                        m_nCurrX = nX;
                        m_nCurrY = nY;
                        SendRefMsg(grobal2.RM_RUSH, nDir, m_nCurrX, m_nCurrY, 0, "");
                        n28 -= 1;
                    }
                    else
                    {
                        if (m_PEnvir.CanWalk(nX, nY, true))
                        {
                            n28 = 0;
                        }
                        else
                        {
                            bo35 = true;
                            break;
                        }
                    }
                }
            }
            if (BaseObject_34 != null)
            {
                if (n24 < 0)
                {
                    n24 = 0;
                }
                nDmg = M2Share.RandomNumber.Random((n24 + 1) * 10) + (n24 + 1) * 10;
                nDmg = BaseObject_34.GetHitStruckDamage(this, nDmg);
                BaseObject_34.StruckDamage(nDmg);
                BaseObject_34.SendRefMsg(grobal2.RM_STRUCK, (short)nDmg, BaseObject_34.m_WAbil.HP, BaseObject_34.m_WAbil.MaxHP, ObjectId, "");
                if (BaseObject_34.m_btRaceServer != grobal2.RC_PLAYOBJECT)
                {
                    BaseObject_34.SendMsg(BaseObject_34, grobal2.RM_STRUCK, (short)nDmg, BaseObject_34.m_WAbil.HP, BaseObject_34.m_WAbil.MaxHP, ObjectId, "");
                }
            }
            if (bo35)
            {
                GetFrontPosition(ref nX, ref nY);
                SendRefMsg(grobal2.RM_RUSHKUNG, m_btDirection, nX, nY, 0, "");
                SysMsg(M2Share.sMateDoTooweak, TMsgColor.c_Red, TMsgType.t_Hint);
            }
            if (n28 > 0)
            {
                if (n24 < 0)
                {
                    n24 = 0;
                }
                nDmg = M2Share.RandomNumber.Random(n24 * 10) + (n24 + 1) * 3;
                nDmg = GetHitStruckDamage(this, nDmg);
                StruckDamage(nDmg);
                SendRefMsg(grobal2.RM_STRUCK, (short)nDmg, m_WAbil.HP, m_WAbil.MaxHP, 0, "");
            }
            return result;
        }

        private bool DoSpell(TUserMagic UserMagic, short nTargetX, short nTargetY, TBaseObject BaseObject)
        {
            var result = false;
            ushort nSpellPoint;
            try
            {
                if (!M2Share.MagicManager.IsWarrSkill(UserMagic.wMagIdx))
                {
                    nSpellPoint = GetSpellPoint(UserMagic);
                    if (nSpellPoint > 0)
                    {
                        if (m_WAbil.MP < nSpellPoint)
                        {
                            return result;
                        }
                        DamageSpell(nSpellPoint);
                        HealthSpellChanged();
                    }
                    result = M2Share.MagicManager.DoSpell(this, UserMagic, nTargetX, nTargetY, BaseObject);
                }
            }
            catch (Exception e)
            {
                M2Share.ErrorMessage(format("[Exception] TPlayObject.DoSpell MagID:{0} X:{1} Y:{2}", UserMagic.wMagIdx, nTargetX, nTargetY));
                M2Share.ErrorMessage(e.Message);
            }
            return result;
        }

        private bool PileStones(int nX, int nY)
        {
            var result = false;
            var s1C = string.Empty;
            var mineEvent = (TStoneMineEvent)m_PEnvir.GetEvent(nX, nY);
            if (mineEvent != null && mineEvent.m_nEventType == grobal2.ET_MINE)
            {
                if (mineEvent.m_nMineCount > 0)
                {
                    mineEvent.m_nMineCount -= 1;
                    if (M2Share.RandomNumber.Random(M2Share.g_Config.nMakeMineHitRate) == 0)
                    {
                        var pileEvent = (TPileStones)m_PEnvir.GetEvent(m_nCurrX, m_nCurrY);
                        if (pileEvent == null)
                        {
                            pileEvent = new TPileStones(m_PEnvir, m_nCurrX, m_nCurrY, grobal2.ET_PILESTONES, 5 * 60 * 1000);
                            M2Share.EventManager.AddEvent(pileEvent);
                        }
                        else
                        {
                            if (pileEvent.m_nEventType == grobal2.ET_PILESTONES)
                            {
                                pileEvent.AddEventParam();
                            }
                        }
                        if (M2Share.RandomNumber.Random(M2Share.g_Config.nMakeMineRate) == 0)
                        {
                            if (m_PEnvir.Flag.boMINE)
                            {
                                MakeMine();
                            }
                            else if (m_PEnvir.Flag.boMINE2)
                            {
                                MakeMine2();
                            }
                        }
                        s1C = "1";
                        DoDamageWeapon(M2Share.RandomNumber.Random(15) + 5);
                        result = true;
                    }
                }
                else
                {
                    if (HUtil32.GetTickCount() - mineEvent.m_dwAddStoneMineTick > 10 * 60 * 1000)
                    {
                        mineEvent.AddStoneMine();
                    }
                }
            }
            SendRefMsg(grobal2.RM_HEAVYHIT, m_btDirection, m_nCurrX, m_nCurrY, 0, s1C);
            return result;
        }

        private void SendSaveItemList(int nBaseObject)
        {
            TItem Item;
            string sSendMsg;
            TClientItem ClientItem = null;
            TOClientItem OClientItem = null;
            TStdItem StdItem = null;
            TUserItem UserItem;
            if (m_nSoftVersionDateEx == 0)
            {
                sSendMsg = "";
                for (var i = 0; i < m_StorageItemList.Count; i++)
                {
                    UserItem = m_StorageItemList[i];
                    Item = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                    if (Item != null)
                    {
                        Item.GetStandardItem(ref StdItem);
                        Item.GetItemAddValue(UserItem, ref StdItem);
                        StdItem.Name = ItmUnit.GetItemName(UserItem);
                        M2Share.CopyStdItemToOStdItem(StdItem, OClientItem.S);
                        OClientItem.Dura = UserItem.Dura;
                        OClientItem.DuraMax = UserItem.DuraMax;
                        OClientItem.MakeIndex = UserItem.MakeIndex;
                        sSendMsg = sSendMsg + EDcode.EncodeBuffer(OClientItem) + '/';
                    }
                }
                m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_SAVEITEMLIST, nBaseObject, 0, 0, (short)m_StorageItemList.Count);
                SendSocket(m_DefMsg, sSendMsg);
            }
            else
            {
                sSendMsg = "";
                for (var i = 0; i < m_StorageItemList.Count; i++)
                {
                    UserItem = m_StorageItemList[i];
                    Item = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                    if (Item != null)
                    {
                        Item.GetStandardItem(ref ClientItem.S);
                        Item.GetItemAddValue(UserItem, ref ClientItem.S);
                        ClientItem.S.Name = ItmUnit.GetItemName(UserItem);
                        ClientItem.Dura = UserItem.Dura;
                        ClientItem.DuraMax = UserItem.DuraMax;
                        ClientItem.MakeIndex = UserItem.MakeIndex;
                        sSendMsg = sSendMsg + EDcode.EncodeBuffer(ClientItem) + '/';
                    }
                }
                m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_SAVEITEMLIST, nBaseObject, 0, 0, (short)m_StorageItemList.Count);
                SendSocket(m_DefMsg, sSendMsg);
            }
        }

        private void SendChangeGuildName()
        {
            if (m_MyGuild != null)
            {
                SendDefMessage(grobal2.SM_CHANGEGUILDNAME, 0, 0, 0, 0, m_MyGuild.sGuildName + '/' + m_sGuildRankName);
            }
            else
            {
                SendDefMessage(grobal2.SM_CHANGEGUILDNAME, 0, 0, 0, 0, "");
            }
        }

        private void SendDelItemList(IList<int> ItemList)
        {
            var s10 = string.Empty;
            for (var i = 0; i < ItemList.Count; i++)
            {
                s10 = s10 + ItemList[i] + '/' + ItemList[i] + '/';
            }
            m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_DELITEMS, 0, 0, 0, (short)ItemList.Count);
            SendSocket(m_DefMsg, EDcode.EncodeString(s10));
        }

        public void SendDelItems(TUserItem UserItem)
        {
            TItem StdItem = null;
            TStdItem StdItem80 = null;
            if (m_nSoftVersionDateEx == 0 && m_dwClientTick == 0)
            {
                StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                if (StdItem != null)
                {
                    TOClientItem OClientItem = new TOClientItem();
                    StdItem.GetStandardItem(ref StdItem80);
                    StdItem.GetItemAddValue(UserItem, ref StdItem80);
                    StdItem80.Name = ItmUnit.GetItemName(UserItem);
                    M2Share.CopyStdItemToOStdItem(StdItem80, OClientItem.S);
                    OClientItem.MakeIndex = UserItem.MakeIndex;
                    OClientItem.Dura = UserItem.Dura;
                    OClientItem.DuraMax = UserItem.DuraMax;
                    if (StdItem.StdMode == 50)
                    {
                        OClientItem.S.Name = OClientItem.S.Name + " #" + UserItem.Dura;
                    }
                    m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_DELITEM, ObjectId, 0, 0, 1);
                    SendSocket(m_DefMsg, EDcode.EncodeBuffer(OClientItem));
                }
            }
            else
            {
                StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                if (StdItem != null)
                {
                    TClientItem ClientItem = new TClientItem();
                    StdItem.GetStandardItem(ref ClientItem.S);
                    StdItem.GetItemAddValue(UserItem, ref ClientItem.S);
                    ClientItem.S.Name = ItmUnit.GetItemName(UserItem);
                    ClientItem.MakeIndex = UserItem.MakeIndex;
                    ClientItem.Dura = UserItem.Dura;
                    ClientItem.DuraMax = UserItem.DuraMax;
                    if (StdItem.StdMode == 50)
                    {
                        ClientItem.S.Name = ClientItem.S.Name + " #" + UserItem.Dura;
                    }
                    m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_DELITEM, ObjectId, 0, 0, 1);
                    SendSocket(m_DefMsg, EDcode.EncodeBuffer(ClientItem));
                }
            }
        }

        public void SendUpdateItem(TUserItem UserItem)
        {
            TItem StdItem;
            TStdItem StdItem80 = null;
            if (m_nSoftVersionDateEx == 0 && m_dwClientTick == 0)
            {
                StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                if (StdItem != null)
                {
                    TOClientItem OClientItem = new TOClientItem();
                    StdItem.GetStandardItem(ref StdItem80);
                    StdItem.GetItemAddValue(UserItem, ref StdItem80);
                    StdItem80.Name = ItmUnit.GetItemName(UserItem);
                    M2Share.CopyStdItemToOStdItem(StdItem80, OClientItem.S);
                    OClientItem.MakeIndex = UserItem.MakeIndex;
                    OClientItem.Dura = UserItem.Dura;
                    OClientItem.DuraMax = UserItem.DuraMax;
                    if (StdItem.StdMode == 50)
                    {
                        OClientItem.S.Name = OClientItem.S.Name + " #" + UserItem.Dura;
                    }
                    m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_UPDATEITEM, ObjectId, 0, 0, 1);
                    SendSocket(m_DefMsg, EDcode.EncodeBuffer(OClientItem));
                }
            }
            else
            {
                StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                if (StdItem != null)
                {
                    TClientItem ClientItem = new TClientItem();
                    StdItem.GetStandardItem(ref ClientItem.S);
                    StdItem.GetItemAddValue(UserItem, ref ClientItem.S);
                    ClientItem.S.Name = ItmUnit.GetItemName(UserItem);
                    ClientItem.MakeIndex = UserItem.MakeIndex;
                    ClientItem.Dura = UserItem.Dura;
                    ClientItem.DuraMax = UserItem.DuraMax;
                    if (StdItem.StdMode == 50)
                    {
                        ClientItem.S.Name = ClientItem.S.Name + " #" + UserItem.Dura;
                    }
                    m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_UPDATEITEM, ObjectId, 0, 0, 1);
                    SendSocket(m_DefMsg, EDcode.EncodeBuffer(ClientItem));
                }
            }
        }

        private bool CheckTakeOnItems(int nWhere, ref TStdItem StdItem)
        {
            var result = false;
            TUserCastle Castle;
            if (StdItem.StdMode == 10 && m_btGender != ObjBase.gMan)
            {
                SysMsg(M2Share.sWearNotOfWoMan, TMsgColor.c_Red, TMsgType.t_Hint);
                return false;
            }
            if (StdItem.StdMode == 11 && m_btGender != ObjBase.gWoMan)
            {
                SysMsg(M2Share.sWearNotOfMan, TMsgColor.c_Red, TMsgType.t_Hint);
                return false;
            }
            if (nWhere == 1 || nWhere == 2)
            {
                if (StdItem.Weight > m_WAbil.MaxHandWeight)
                {
                    SysMsg(M2Share.sHandWeightNot, TMsgColor.c_Red, TMsgType.t_Hint);
                    return false;
                }
            }
            else
            {
                if (StdItem.Weight + GetUserItemWeitht(nWhere) > m_WAbil.MaxWearWeight)
                {
                    SysMsg(M2Share.sWearWeightNot, TMsgColor.c_Red, TMsgType.t_Hint);
                    return false;
                }
            }
            Castle = M2Share.CastleManager.IsCastleMember(this);
            switch (StdItem.Need)
            {
                case 0:
                    if (m_Abil.Level >= StdItem.NeedLevel)
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(M2Share.g_sLevelNot, TMsgColor.c_Red, TMsgType.t_Hint);
                    }
                    break;
                case 1:
                    if (HUtil32.HiWord(m_WAbil.DC) >= StdItem.NeedLevel)
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(M2Share.g_sDCNot, TMsgColor.c_Red, TMsgType.t_Hint);
                    }
                    break;
                case 10:
                    if (m_btJob == HUtil32.LoWord(StdItem.NeedLevel) && m_Abil.Level >= HUtil32.HiWord(StdItem.NeedLevel))
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(M2Share.g_sJobOrLevelNot, TMsgColor.c_Red, TMsgType.t_Hint);
                    }
                    break;
                case 11:
                    if (m_btJob == HUtil32.LoWord(StdItem.NeedLevel) && HUtil32.HiWord(m_WAbil.DC) >= HUtil32.HiWord(StdItem.NeedLevel))
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(M2Share.g_sJobOrDCNot, TMsgColor.c_Red, TMsgType.t_Hint);
                    }
                    break;
                case 12:
                    if (m_btJob == HUtil32.LoWord(StdItem.NeedLevel) && HUtil32.HiWord(m_WAbil.MC) >= HUtil32.HiWord(StdItem.NeedLevel))
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(M2Share.g_sJobOrMCNot, TMsgColor.c_Red, TMsgType.t_Hint);
                    }
                    break;
                case 13:
                    if (m_btJob == HUtil32.LoWord(StdItem.NeedLevel) && HUtil32.HiWord(m_WAbil.SC) >= HUtil32.HiWord(StdItem.NeedLevel))
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(M2Share.g_sJobOrSCNot, TMsgColor.c_Red, TMsgType.t_Hint);
                    }
                    break;
                case 2:
                    if (HUtil32.HiWord(m_WAbil.MC) >= StdItem.NeedLevel)
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(M2Share.g_sMCNot, TMsgColor.c_Red, TMsgType.t_Hint);
                    }
                    break;
                case 3:
                    if (HUtil32.HiWord(m_WAbil.SC) >= StdItem.NeedLevel)
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(M2Share.g_sSCNot, TMsgColor.c_Red, TMsgType.t_Hint);
                    }
                    break;
                case 4:
                    if (m_btReLevel >= StdItem.NeedLevel)
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(M2Share.g_sReNewLevelNot, TMsgColor.c_Red, TMsgType.t_Hint);
                    }
                    break;
                case 40:
                    if (m_btReLevel >= HUtil32.LoWord(StdItem.NeedLevel))
                    {
                        if (m_Abil.Level >= HUtil32.HiWord(StdItem.NeedLevel))
                        {
                            result = true;
                        }
                        else
                        {
                            SysMsg(M2Share.g_sLevelNot, TMsgColor.c_Red, TMsgType.t_Hint);
                        }
                    }
                    else
                    {
                        SysMsg(M2Share.g_sReNewLevelNot, TMsgColor.c_Red, TMsgType.t_Hint);
                    }
                    break;
                case 41:
                    if (m_btReLevel >= HUtil32.LoWord(StdItem.NeedLevel))
                    {
                        if (HUtil32.HiWord(m_WAbil.DC) >= HUtil32.HiWord(StdItem.NeedLevel))
                        {
                            result = true;
                        }
                        else
                        {
                            SysMsg(M2Share.g_sDCNot, TMsgColor.c_Red, TMsgType.t_Hint);
                        }
                    }
                    else
                    {
                        SysMsg(M2Share.g_sReNewLevelNot, TMsgColor.c_Red, TMsgType.t_Hint);
                    }
                    break;
                case 42:
                    if (m_btReLevel >= HUtil32.LoWord(StdItem.NeedLevel))
                    {
                        if (HUtil32.HiWord(m_WAbil.MC) >= HUtil32.HiWord(StdItem.NeedLevel))
                        {
                            result = true;
                        }
                        else
                        {
                            SysMsg(M2Share.g_sMCNot, TMsgColor.c_Red, TMsgType.t_Hint);
                        }
                    }
                    else
                    {
                        SysMsg(M2Share.g_sReNewLevelNot, TMsgColor.c_Red, TMsgType.t_Hint);
                    }
                    break;
                case 43:
                    if (m_btReLevel >= HUtil32.LoWord(StdItem.NeedLevel))
                    {
                        if (HUtil32.HiWord(m_WAbil.SC) >= HUtil32.HiWord(StdItem.NeedLevel))
                        {
                            result = true;
                        }
                        else
                        {
                            SysMsg(M2Share.g_sSCNot, TMsgColor.c_Red, TMsgType.t_Hint);
                        }
                    }
                    else
                    {
                        SysMsg(M2Share.g_sReNewLevelNot, TMsgColor.c_Red, TMsgType.t_Hint);
                    }
                    break;
                case 44:
                    if (m_btReLevel >= HUtil32.LoWord(StdItem.NeedLevel))
                    {
                        if (m_btCreditPoint >= HUtil32.HiWord(StdItem.NeedLevel))
                        {
                            result = true;
                        }
                        else
                        {
                            SysMsg(M2Share.g_sCreditPointNot, TMsgColor.c_Red, TMsgType.t_Hint);
                        }
                    }
                    else
                    {
                        SysMsg(M2Share.g_sReNewLevelNot, TMsgColor.c_Red, TMsgType.t_Hint);
                    }
                    break;
                case 5:
                    if (m_btCreditPoint >= StdItem.NeedLevel)
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(M2Share.g_sCreditPointNot, TMsgColor.c_Red, TMsgType.t_Hint);
                    }
                    break;
                case 6:
                    if (m_MyGuild != null)
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(M2Share.g_sGuildNot, TMsgColor.c_Red, TMsgType.t_Hint);
                    }
                    break;
                case 60:
                    if (m_MyGuild != null && m_nGuildRankNo == 1)
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(M2Share.g_sGuildMasterNot, TMsgColor.c_Red, TMsgType.t_Hint);
                    }
                    break;
                case 7:
                    if (m_MyGuild != null && Castle != null)
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(M2Share.g_sSabukHumanNot, TMsgColor.c_Red, TMsgType.t_Hint);
                    }
                    break;
                case 70:
                    if (m_MyGuild != null && Castle != null && m_nGuildRankNo == 1)
                    {
                        if (m_Abil.Level >= StdItem.NeedLevel)
                        {
                            result = true;
                        }
                        else
                        {
                            SysMsg(M2Share.g_sLevelNot, TMsgColor.c_Red, TMsgType.t_Hint);
                        }
                    }
                    else
                    {
                        SysMsg(M2Share.g_sSabukMasterManNot, TMsgColor.c_Red, TMsgType.t_Hint);
                    }
                    break;
                case 8:
                    if (m_nMemberType != 0)
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(M2Share.g_sMemberNot, TMsgColor.c_Red, TMsgType.t_Hint);
                    }
                    break;
                case 81:
                    if (m_nMemberType == HUtil32.LoWord(StdItem.NeedLevel) && m_nMemberLevel >= HUtil32.HiWord(StdItem.NeedLevel))
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(M2Share.g_sMemberTypeNot, TMsgColor.c_Red, TMsgType.t_Hint);
                    }
                    break;
                case 82:
                    if (m_nMemberType >= HUtil32.LoWord(StdItem.NeedLevel) && m_nMemberLevel >= HUtil32.HiWord(StdItem.NeedLevel))
                    {
                        result = true;
                    }
                    else
                    {
                        SysMsg(M2Share.g_sMemberTypeNot, TMsgColor.c_Red, TMsgType.t_Hint);
                    }
                    break;
            }
            return result;
        }

        private int GetUserItemWeitht(int nWhere)
        {
            int result;
            var n14 = 0;
            TItem StdItem;
            for (var i = m_UseItems.GetLowerBound(0); i <= m_UseItems.GetUpperBound(0); i++)
            {
                if (nWhere == -1 || !(i == nWhere) && !(i == 1) && !(i == 2))
                {
                    if (m_UseItems[i] == null)
                    {
                        continue;
                    }
                    StdItem = M2Share.UserEngine.GetStdItem(m_UseItems[i].wIndex);
                    if (StdItem != null)
                    {
                        n14 += StdItem.Weight;
                    }
                }
            }
            result = n14;
            return result;
        }

        private bool EatItems(TItem StdItem, TUserItem Useritem)
        {
            var result = false;
            bool boNeedRecalc;
            int nOldStatus;
            if (m_PEnvir.Flag.boNODRUG)
            {
                SysMsg(M2Share.sCanotUseDrugOnThisMap, TMsgColor.c_Red, TMsgType.t_Hint);
                return result;
            }
            switch (StdItem.StdMode)
            {
                case 0:
                    switch (StdItem.Shape)
                    {
                        case 1:
                            IncHealthSpell(StdItem.AC, StdItem.MAC);
                            result = true;
                            break;
                        case 2:
                            m_boUserUnLockDurg = true;
                            result = true;
                            break;
                        case 3:
                            IncHealthSpell(HUtil32.Round(m_WAbil.MaxHP / 100 * StdItem.AC), HUtil32.Round(m_WAbil.MaxMP / 100 * StdItem.MAC));
                            result = true;
                            break;
                        default:
                            if (StdItem.AC > 0)
                            {
                                m_nIncHealth += StdItem.AC;
                            }
                            if (StdItem.MAC > 0)
                            {
                                m_nIncSpell += StdItem.MAC;
                            }
                            result = true;
                            break;
                    }
                    break;
                case 1:
                    nOldStatus = GetMyStatus();
                    m_nHungerStatus += StdItem.DuraMax / 10;
                    m_nHungerStatus = HUtil32._MIN(5000, m_nHungerStatus);
                    if (nOldStatus != GetMyStatus())
                    {
                        RefMyStatus();
                    }
                    result = true;
                    break;
                case 2:
                    result = true;
                    break;
                case 3:
                    switch (StdItem.Shape)
                    {
                        case 12:
                            boNeedRecalc = false;
                            if (StdItem.DC > 0)
                            {
                                m_wStatusArrValue[0] = StdItem.DC;
                                m_dwStatusArrTimeOutTick[0] = HUtil32.GetTickCount() + StdItem.MAC2 * 1000;
                                SysMsg("攻击力增加" + StdItem.MAC2 + "秒.", TMsgColor.c_Green, TMsgType.t_Hint);
                                boNeedRecalc = true;
                            }
                            if (StdItem.MC > 0)
                            {
                                m_wStatusArrValue[1] = StdItem.MC;
                                m_dwStatusArrTimeOutTick[1] = HUtil32.GetTickCount() + StdItem.MAC2 * 1000;
                                SysMsg("魔法力增加" + StdItem.MAC2 + "秒.", TMsgColor.c_Green, TMsgType.t_Hint);
                                boNeedRecalc = true;
                            }
                            if (StdItem.SC > 0)
                            {
                                m_wStatusArrValue[2] = StdItem.SC;
                                m_dwStatusArrTimeOutTick[2] = HUtil32.GetTickCount() + StdItem.MAC2 * 1000;
                                SysMsg("道术增加" + StdItem.MAC2 + "秒.", TMsgColor.c_Green, TMsgType.t_Hint);
                                boNeedRecalc = true;
                            }
                            if (StdItem.AC2 > 0)
                            {
                                m_wStatusArrValue[3] = StdItem.AC2;
                                m_dwStatusArrTimeOutTick[3] = HUtil32.GetTickCount() + StdItem.MAC2 * 1000;
                                SysMsg("攻击速度增加" + StdItem.MAC2 + "秒.", TMsgColor.c_Green, TMsgType.t_Hint);
                                boNeedRecalc = true;
                            }
                            if (StdItem.AC > 0)
                            {
                                m_wStatusArrValue[4] = StdItem.AC;
                                m_dwStatusArrTimeOutTick[4] = HUtil32.GetTickCount() + StdItem.MAC2 * 1000;
                                SysMsg("生命值增加" + StdItem.MAC2 + "秒.", TMsgColor.c_Green, TMsgType.t_Hint);
                                boNeedRecalc = true;
                            }
                            if (StdItem.MAC > 0)
                            {
                                m_wStatusArrValue[5] = StdItem.MAC;
                                m_dwStatusArrTimeOutTick[5] = HUtil32.GetTickCount() + StdItem.MAC2 * 1000;
                                SysMsg("魔法值增加" + StdItem.MAC2 + "秒.", TMsgColor.c_Green, TMsgType.t_Hint);
                                boNeedRecalc = true;
                            }
                            if (boNeedRecalc)
                            {
                                RecalcAbilitys();
                                SendMsg(this, grobal2.RM_ABILITY, 0, 0, 0, 0, "");
                                result = true;
                            }
                            break;
                        case 13:
                            GetExp(StdItem.DuraMax);
                            result = true;
                            break;
                        default:
                            result = EatUseItems(StdItem.Shape);
                            break;
                    }
                    break;
            }
            return result;
        }

        private bool ReadBook(TItem StdItem)
        {
            TUserMagic UserMagic;
            TPlayObject PlayObject;
            var result = false;
            var magic = M2Share.UserEngine.FindMagic(StdItem.Name);
            if (magic != null)
            {
                if (!IsTrainingSkill(magic.wMagicID))
                {
                    if (magic.btJob == 99 || magic.btJob == m_btJob)
                    {
                        if (m_Abil.Level >= magic.TrainLevel[0])
                        {
                            UserMagic = new TUserMagic
                            {
                                MagicInfo = magic,
                                wMagIdx = magic.wMagicID,
                                btKey = 0,
                                btLevel = 0,
                                nTranPoint = 0
                            };
                            m_MagicList.Add(UserMagic);
                            RecalcAbilitys();
                            if (m_btRaceServer == grobal2.RC_PLAYOBJECT)
                            {
                                PlayObject = this;
                                PlayObject.SendAddMagic(UserMagic);
                            }
                            result = true;
                        }
                    }
                }
            }
            return result;
        }

        public void SendAddMagic(TUserMagic UserMagic)
        {
            var clientMagic = new TClientMagic
            {
                Key = (char)UserMagic.btKey,
                Level = UserMagic.btLevel,
                CurTrain = UserMagic.nTranPoint,
                Def = UserMagic.MagicInfo
            };
            m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_ADDMAGIC, 0, 0, 0, 1);
            SendSocket(m_DefMsg, EDcode.EncodeBuffer(clientMagic));
        }

        internal void SendDelMagic(TUserMagic UserMagic)
        {
            m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_DELMAGIC, UserMagic.wMagIdx, 0, 0, 1);
            SendSocket(m_DefMsg);
        }

        /// <summary>
        /// 使用物品
        /// </summary>
        /// <param name="nShape"></param>
        /// <returns></returns>
        private bool EatUseItems(int nShape)
        {
            var result = false;
            switch (nShape)
            {
                case 1:
                    SendRefMsg(grobal2.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
                    BaseObjectMove(m_sHomeMap, 0, 0);
                    result = true;
                    break;
                case 2:
                    if (!m_PEnvir.Flag.boNORANDOMMOVE)
                    {
                        SendRefMsg(grobal2.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
                        BaseObjectMove(m_sMapName, 0, 0);
                        result = true;
                    }
                    break;
                case 3:
                    SendRefMsg(grobal2.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
                    if (PKLevel() < 2)
                    {
                        BaseObjectMove(m_sHomeMap, m_nHomeX, m_nHomeY);
                    }
                    else
                    {
                        BaseObjectMove(M2Share.g_Config.sRedHomeMap, M2Share.g_Config.nRedHomeX, M2Share.g_Config.nRedHomeY);
                    }
                    result = true;
                    break;
                case 4:
                    if (WeaptonMakeLuck())
                    {
                        result = true;
                    }
                    break;
                case 5:
                    if (m_MyGuild != null)
                    {
                        if (!m_boInFreePKArea)
                        {
                            TUserCastle Castle = M2Share.CastleManager.IsCastleMember(this);
                            if (Castle != null && Castle.IsMasterGuild(m_MyGuild))
                            {
                                BaseObjectMove(Castle.m_sHomeMap, Castle.GetHomeX(), Castle.GetHomeY());
                            }
                            else
                            {
                                SysMsg("无效", TMsgColor.c_Red, TMsgType.t_Hint);
                            }
                            result = true;
                        }
                        else
                        {
                            SysMsg("此处无法使用", TMsgColor.c_Red, TMsgType.t_Hint);
                        }
                    }
                    break;
                case 9:
                    if (RepairWeapon())
                    {
                        result = true;
                    }
                    break;
                case 10:
                    if (SuperRepairWeapon())
                    {
                        result = true;
                    }
                    break;
                case 11:
                    WinLottery();
                    result = true;
                    break;
            }
            return result;
        }

        protected void MoveToHome()
        {
            SendRefMsg(grobal2.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
            BaseObjectMove(m_sHomeMap, m_nHomeX, m_nHomeY);
        }

        private void BaseObjectMove(string sMap, short sX, short sY)
        {
            if (string.IsNullOrEmpty(sMap))
            {
                sMap = m_sMapName;
            }
            if (sX != 0 && sY != 0)
            {
                short nX = sX;
                short nY = sY;
                SpaceMove(sMap, nX, nY, 0);
            }
            else
            {
                MapRandomMove(sMap, 0);
            }
            var envir = m_PEnvir;
            if (envir != m_PEnvir && m_btRaceServer == grobal2.RC_PLAYOBJECT)
            {
                m_boTimeRecall = false;
            }
        }

        private void ChangeServerMakeSlave(TSlaveInfo slaveInfo)
        {
            int nSlavecount;
            if (m_btJob == M2Share.jTaos)
            {
                nSlavecount = 1;
            }
            else
            {
                nSlavecount = 5;
            }
            var BaseObject = MakeSlave(slaveInfo.sSlaveName, 3, slaveInfo.btSlaveLevel, nSlavecount,
                slaveInfo.dwRoyaltySec);
            if (BaseObject != null)
            {
                BaseObject.m_nKillMonCount = slaveInfo.nKillCount;
                BaseObject.m_btSlaveExpLevel = slaveInfo.btSlaveExpLevel;
                BaseObject.m_WAbil.HP = slaveInfo.nHP;
                BaseObject.m_WAbil.MP = slaveInfo.nMP;
                if (1500 - slaveInfo.btSlaveLevel * 200 < BaseObject.m_nWalkSpeed)
                {
                    BaseObject.m_nWalkSpeed = 1500 - slaveInfo.btSlaveLevel * 200;
                }
                if (2000 - slaveInfo.btSlaveLevel * 200 < BaseObject.m_nNextHitTime)
                {
                    BaseObject.m_nWalkSpeed = 2000 - slaveInfo.btSlaveLevel * 200;
                }
                RecalcAbilitys();
            }
        }

        private void SendDelDealItem(TUserItem UserItem)
        {
            TItem pStdItem;
            TStdItem StdItem = null;
            TClientItem ClientItem = null;
            TOClientItem OClientItem = null;
            SendDefMessage(grobal2.SM_DEALDELITEM_OK, 0, 0, 0, 0, "");
            if (m_DealCreat != null)
            {
                if ((m_DealCreat as TPlayObject).m_nSoftVersionDateEx == 0)
                {
                    pStdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                    if (pStdItem != null)
                    {
                        OClientItem=new TOClientItem();
                        pStdItem.GetStandardItem(ref StdItem);
                        pStdItem.GetItemAddValue(UserItem, ref StdItem);
                        StdItem.Name = ItmUnit.GetItemName(UserItem);
                        M2Share.CopyStdItemToOStdItem(StdItem, OClientItem.S);
                        OClientItem.MakeIndex = UserItem.MakeIndex;
                        OClientItem.Dura = UserItem.Dura;
                        OClientItem.DuraMax = UserItem.DuraMax;
                    }
                    m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_DEALREMOTEDELITEM, ObjectId, 0, 0, 1);
                    (m_DealCreat as TPlayObject).SendSocket(m_DefMsg, EDcode.EncodeBuffer(OClientItem));
                }
                else
                {
                    pStdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                    if (pStdItem != null)
                    {
                        ClientItem=new TClientItem();
                        pStdItem.GetStandardItem(ref ClientItem.S);
                        ClientItem.S.Name = ItmUnit.GetItemName(UserItem);
                        ClientItem.MakeIndex = UserItem.MakeIndex;
                        ClientItem.Dura = UserItem.Dura;
                        ClientItem.DuraMax = UserItem.DuraMax;
                    }
                    m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_DEALREMOTEDELITEM, ObjectId, 0, 0, 1);
                    (m_DealCreat as TPlayObject).SendSocket(m_DefMsg, EDcode.EncodeBuffer(ClientItem));
                }
                m_DealCreat.m_DealLastTick = HUtil32.GetTickCount();
                m_DealLastTick = HUtil32.GetTickCount();
            }
        }

        private void SendAddDealItem(TUserItem UserItem)
        {
            TItem StdItem;
            TStdItem StdItem80 = null;
            TClientItem ClientItem = null;
            TOClientItem OClientItem = null;
            SendDefMessage(grobal2.SM_DEALADDITEM_OK, 0, 0, 0, 0, "");
            if (m_DealCreat != null)
            {
                if ((m_DealCreat as TPlayObject).m_nSoftVersionDateEx == 0)
                {
                    StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                    if (StdItem != null)
                    {
                        OClientItem = new TOClientItem();
                        StdItem.GetStandardItem(ref StdItem80);
                        StdItem.GetItemAddValue(UserItem, ref StdItem80);
                        StdItem80.Name = ItmUnit.GetItemName(UserItem);
                        M2Share.CopyStdItemToOStdItem(StdItem80, OClientItem.S);
                        OClientItem.MakeIndex = UserItem.MakeIndex;
                        OClientItem.Dura = UserItem.Dura;
                        OClientItem.DuraMax = UserItem.DuraMax;
                        m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_DEALREMOTEADDITEM, ObjectId, 0, 0, 1);
                        (m_DealCreat as TPlayObject).SendSocket(m_DefMsg, EDcode.EncodeBuffer(OClientItem));
                        m_DealCreat.m_DealLastTick = HUtil32.GetTickCount();
                        m_DealLastTick = HUtil32.GetTickCount();
                    }
                }
                else
                {
                    StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                    if (StdItem != null)
                    {
                        ClientItem = new TClientItem();
                        StdItem.GetStandardItem(ref ClientItem.S);
                        StdItem.GetItemAddValue(UserItem, ref ClientItem.S);
                        ClientItem.S.Name = ItmUnit.GetItemName(UserItem);
                        ClientItem.MakeIndex = UserItem.MakeIndex;
                        ClientItem.Dura = UserItem.Dura;
                        ClientItem.DuraMax = UserItem.DuraMax;
                        m_DefMsg = grobal2.MakeDefaultMsg(grobal2.SM_DEALREMOTEADDITEM, ObjectId, 0, 0, 1);
                        (m_DealCreat as TPlayObject).SendSocket(m_DefMsg, EDcode.EncodeBuffer(ClientItem));
                        m_DealCreat.m_DealLastTick = HUtil32.GetTickCount();
                        m_DealLastTick = HUtil32.GetTickCount();
                    }
                }
            }
        }

        private void OpenDealDlg(TBaseObject BaseObject)
        {
            m_boDealing = true;
            m_DealCreat = BaseObject;
            GetBackDealItems();
            SendDefMessage(grobal2.SM_DEALMENU, 0, 0, 0, 0, m_DealCreat.m_sCharName);
            m_DealLastTick = HUtil32.GetTickCount();
        }

        private void JoinGroup(TPlayObject PlayObject)
        {
            m_GroupOwner = PlayObject;
            SendGroupText(format(M2Share.g_sJoinGroup, m_sCharName));
        }

        /// <summary>
        /// 随机矿石持久度
        /// </summary>
        /// <returns></returns>
        private ushort MakeMineRandomDrua()
        {
            var result = M2Share.RandomNumber.Random(M2Share.g_Config.nStoneGeneralDuraRate) + M2Share.g_Config.nStoneMinDura;
            if (M2Share.RandomNumber.Random(M2Share.g_Config.nStoneAddDuraRate) == 0)
            {
                result = result + M2Share.RandomNumber.Random(M2Share.g_Config.nStoneAddDuraMax);
            }
            return (ushort)result;
        }

        /// <summary>
        /// 制造矿石
        /// </summary>
        private void MakeMine()
        {
            TUserItem UserItem;
            int nRandom;
            if (m_ItemList.Count >= grobal2.MAXBAGITEM)
            {
                return;
            }
            nRandom = M2Share.RandomNumber.Random(M2Share.g_Config.nStoneTypeRate);
            if (nRandom >= M2Share.g_Config.nGoldStoneMin && nRandom <= M2Share.g_Config.nGoldStoneMax)
            {
                UserItem = new TUserItem();
                if (M2Share.UserEngine.CopyToUserItemFromName(M2Share.g_Config.sGoldStone, ref UserItem))
                {
                    UserItem.Dura = MakeMineRandomDrua();
                    m_ItemList.Add(UserItem);
                    WeightChanged();
                    SendAddItem(UserItem);
                }
                else
                {
                    Dispose(UserItem);
                }
                return;
            }
            if (nRandom >= M2Share.g_Config.nSilverStoneMin && nRandom <= M2Share.g_Config.nSilverStoneMax)
            {
                UserItem = new TUserItem();
                if (M2Share.UserEngine.CopyToUserItemFromName(M2Share.g_Config.sSilverStone, ref UserItem))
                {
                    UserItem.Dura = MakeMineRandomDrua();
                    m_ItemList.Add(UserItem);
                    WeightChanged();
                    SendAddItem(UserItem);
                }
                else
                {
                    Dispose(UserItem);
                }
                return;
            }
            if (nRandom >= M2Share.g_Config.nSteelStoneMin && nRandom <= M2Share.g_Config.nSteelStoneMax)
            {
                UserItem = new TUserItem();
                if (M2Share.UserEngine.CopyToUserItemFromName(M2Share.g_Config.sSteelStone, ref UserItem))
                {
                    UserItem.Dura = MakeMineRandomDrua();
                    m_ItemList.Add(UserItem);
                    WeightChanged();
                    SendAddItem(UserItem);
                }
                else
                {
                    Dispose(UserItem);
                }
                return;
            }
            if (nRandom >= M2Share.g_Config.nBlackStoneMin && nRandom <= M2Share.g_Config.nBlackStoneMax)
            {
                UserItem = new TUserItem();
                if (M2Share.UserEngine.CopyToUserItemFromName(M2Share.g_Config.sBlackStone, ref UserItem))
                {
                    UserItem.Dura = MakeMineRandomDrua();
                    m_ItemList.Add(UserItem);
                    WeightChanged();
                    SendAddItem(UserItem);
                }
                else
                {
                    Dispose(UserItem);
                }
                return;
            }
            UserItem = new TUserItem();
            if (M2Share.UserEngine.CopyToUserItemFromName(M2Share.g_Config.sCopperStone, ref UserItem))
            {
                UserItem.Dura = MakeMineRandomDrua();
                m_ItemList.Add(UserItem);
                WeightChanged();
                SendAddItem(UserItem);
            }
            else
            {
                Dispose(UserItem);
            }
        }

        /// <summary>
        /// 制造矿石
        /// </summary>
        private void MakeMine2()
        {
            if (m_ItemList.Count >= grobal2.MAXBAGITEM)
            {
                return;
            }
            var mineRate = M2Share.RandomNumber.Random(120);
            TUserItem mineItem = null;
            if (HUtil32.RangeInDefined(mineRate, 1, 2))
            {
                if (M2Share.UserEngine.CopyToUserItemFromName(M2Share.g_Config.sGemStone1, ref mineItem))
                {
                    mineItem.Dura = MakeMineRandomDrua();
                    m_ItemList.Add(mineItem);
                    WeightChanged();
                    SendAddItem(mineItem);
                }
                else
                {
                    Dispose(mineItem);
                }
            }
            else if (HUtil32.RangeInDefined(mineRate, 3, 20))
            {
                if (M2Share.UserEngine.CopyToUserItemFromName(M2Share.g_Config.sGemStone2, ref mineItem))
                {
                    mineItem.Dura = MakeMineRandomDrua();
                    m_ItemList.Add(mineItem);
                    WeightChanged();
                    SendAddItem(mineItem);
                }
                else
                {
                    Dispose(mineItem);
                }
            }
            else if (HUtil32.RangeInDefined(mineRate, 21, 45))
            {
                if (M2Share.UserEngine.CopyToUserItemFromName(M2Share.g_Config.sGemStone3, ref mineItem))
                {
                    mineItem.Dura = MakeMineRandomDrua();
                    m_ItemList.Add(mineItem);
                    WeightChanged();
                    SendAddItem(mineItem);
                }
                else
                {
                    Dispose(mineItem);
                }
            }
            else
            {
                if (M2Share.UserEngine.CopyToUserItemFromName(M2Share.g_Config.sGemStone4, ref mineItem))
                {
                    mineItem.Dura = MakeMineRandomDrua();
                    m_ItemList.Add(mineItem);
                    WeightChanged();
                    SendAddItem(mineItem);
                }
                else
                {
                    Dispose(mineItem);
                }
            }
        }

        public TUserItem QuestCheckItem(string sItemName, ref int nCount, ref int nParam, ref int nDura)
        {
            TUserItem UserItem;
            string s1C;
            TUserItem result = null;
            nParam = 0;
            nDura = 0;
            nCount = 0;
            for (var i = 0; i < m_ItemList.Count; i++)
            {
                UserItem = m_ItemList[i];
                s1C = M2Share.UserEngine.GetStdItemName(UserItem.wIndex);
                if (string.Compare(s1C, sItemName, StringComparison.Ordinal) == 0)
                {
                    if (UserItem.Dura > nDura)
                    {
                        nDura = UserItem.Dura;
                        result = UserItem;
                    }
                    nParam += UserItem.Dura;
                    if (result == null)
                    {
                        result = UserItem;
                    }
                    nCount++;
                }
            }
            return result;
        }

        public bool QuestTakeCheckItem(TUserItem CheckItem)
        {
            TUserItem UserItem;
            var result = false;
            for (var i = 0; i < m_ItemList.Count; i++)
            {
                UserItem = m_ItemList[i];
                if (UserItem == CheckItem)
                {
                    SendDelItems(UserItem);

                    Dispose(UserItem);
                    m_ItemList.RemoveAt(i);
                    result = true;
                    break;
                }
            }
            for (var i = m_UseItems.GetLowerBound(0); i <= m_UseItems.GetUpperBound(0); i++)
            {
                if (m_UseItems[i] == CheckItem)
                {
                    SendDelItems(m_UseItems[i]);
                    m_UseItems[i].wIndex = 0;
                    result = true;
                    break;
                }
            }
            return result;
        }

        private void ClientQueryRepairCost(int nParam1, int nInt, string sMsg)
        {
            TUserItem UserItem;
            TUserItem UserItemA = null;
            string sUserItemName;
            for (var i = 0; i < m_ItemList.Count; i++)
            {
                UserItem = m_ItemList[i];
                if (UserItem.MakeIndex == nInt)
                {
                    sUserItemName = ItmUnit.GetItemName(UserItem); // 取自定义物品名称
                    if (string.Compare(sUserItemName, sMsg, StringComparison.Ordinal) == 0)
                    {
                        UserItemA = UserItem;
                        break;
                    }
                }
            }
            if (UserItemA == null)
            {
                return;
            }
            var merchant = M2Share.UserEngine.FindMerchant<TMerchant>(nParam1);
            if (merchant != null && merchant.m_PEnvir == m_PEnvir && Math.Abs(merchant.m_nCurrX - m_nCurrX) < 15 && Math.Abs(merchant.m_nCurrY - m_nCurrY) < 15)
            {
                merchant.ClientQueryRepairCost(this, UserItemA);
            }
        }

        private void ClientRepairItem(int nParam1, int nInt, string sMsg)
        {
            TUserItem UserItem = null;
            string sUserItemName;
            for (var i = 0; i < m_ItemList.Count; i++)
            {
                UserItem = m_ItemList[i];
                sUserItemName = ItmUnit.GetItemName(UserItem);// 取自定义物品名称
                if (UserItem.MakeIndex == nInt && string.Compare(sUserItemName, sMsg, StringComparison.Ordinal) == 0)
                {
                    break;
                }
            }
            if (UserItem == null)
            {
                return;
            }
            TMerchant merchant = M2Share.UserEngine.FindMerchant<TMerchant>(nParam1);
            if (merchant != null && merchant.m_PEnvir == m_PEnvir && Math.Abs(merchant.m_nCurrX - m_nCurrX) < 15 && Math.Abs(merchant.m_nCurrY - m_nCurrY) < 15)
            {
                merchant.ClientRepairItem(this, UserItem);
            }
        }

        private void ClientStorageItem(int ObjectId, int nItemIdx, string sMsg)
        {
            TItem StdItem;
            var bo19 = false;
            TUserItem UserItem = null;
            string sUserItemName;
            if (sMsg.IndexOf(' ') >= 0)
            {
                HUtil32.GetValidStr3(sMsg, ref sMsg, new string[] { " " });
            }
            if (m_nPayMent == 1 && !M2Share.g_Config.boTryModeUseStorage)
            {
                SysMsg(M2Share.g_sTryModeCanotUseStorage, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            TMerchant merchant = M2Share.UserEngine.FindMerchant<TMerchant>(ObjectId);
            for (var i = 0; i < m_ItemList.Count; i++)
            {
                UserItem = m_ItemList[i];
                sUserItemName = ItmUnit.GetItemName(UserItem); // 取自定义物品名称
                if (UserItem.MakeIndex == nItemIdx && string.Compare(sUserItemName, sMsg, StringComparison.Ordinal) == 0)
                {
                    // 检查NPC是否允许存物品
                    if (merchant != null && merchant.m_boStorage && (merchant.m_PEnvir == m_PEnvir && Math.Abs(merchant.m_nCurrX - m_nCurrX) < 15 && Math.Abs(merchant.m_nCurrY - m_nCurrY) < 15 || merchant == M2Share.g_FunctionNPC))
                    {
                        if (m_StorageItemList.Count < 39)
                        {
                            m_StorageItemList.Add(UserItem);
                            m_ItemList.RemoveAt(i);
                            WeightChanged();
                            SendDefMessage(grobal2.SM_STORAGE_OK, 0, 0, 0, 0, "");
                            StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                            if (StdItem.NeedIdentify == 1)
                            {
                                // UserEngine.GetStdItemName(UserItem.wIndex) + #9 +
                                M2Share.AddGameDataLog('1' + "\t" + m_sMapName + "\t" + m_nCurrX.ToString() + "\t" + m_nCurrY.ToString() + "\t" + m_sCharName + "\t" + StdItem.Name + "\t" + UserItem.MakeIndex.ToString() + "\t" + '1' + "\t" + '0');
                            }
                        }
                        else
                        {
                            SendDefMessage(grobal2.SM_STORAGE_FULL, 0, 0, 0, 0, "");
                        }
                        bo19 = true;
                    }
                    break;
                }
            }
            if (!bo19)
            {
                SendDefMessage(grobal2.SM_STORAGE_FAIL, 0, 0, 0, 0, "");
            }
        }

        private void ClientTakeBackStorageItem(int NPC, int nItemIdx, string sMsg)
        {
            TItem StdItem;
            string sUserItemName;
            var bo19 = false;
            TUserItem UserItem = null;
            var merchant = M2Share.UserEngine.FindMerchant<TMerchant>(NPC);
            if (merchant == null)
            {
                return;
            }
            if (m_nPayMent == 1 && !M2Share.g_Config.boTryModeUseStorage)
            {
                // '试玩模式不可以使用仓库功能！！！'
                SysMsg(M2Share.g_sTryModeCanotUseStorage, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (!m_boCanGetBackItem)
            {
                SendMsg(merchant, grobal2.RM_MENU_OK, 0, ObjectId, 0, 0, M2Share.g_sStorageIsLockedMsg + "\\ \\" + "仓库开锁命令: @" + M2Share.g_GameCommand.UNLOCKSTORAGE.sCmd + '\\' + "仓库加锁命令: @" + M2Share.g_GameCommand.__LOCK.sCmd + '\\' + "设置密码命令: @" + M2Share.g_GameCommand.SETPASSWORD.sCmd + '\\' + "修改密码命令: @" + M2Share.g_GameCommand.CHGPASSWORD.sCmd);
                return;
            }
            for (var i = 0; i < m_StorageItemList.Count; i++)
            {
                UserItem = m_StorageItemList[i];
                sUserItemName = ItmUnit.GetItemName(UserItem); // 取自定义物品名称
                if (UserItem.MakeIndex == nItemIdx && string.Compare(sUserItemName, sMsg, StringComparison.Ordinal) == 0)
                {
                    if (IsAddWeightAvailable(M2Share.UserEngine.GetStdItemWeight(UserItem.wIndex)))
                    {
                        // 检查NPC是否允许取物品
                        if (merchant.m_boGetback && (merchant.m_PEnvir == m_PEnvir && Math.Abs(merchant.m_nCurrX - m_nCurrX) < 15 && Math.Abs(merchant.m_nCurrY - m_nCurrY) < 15 || merchant == M2Share.g_FunctionNPC))
                        {
                            if (AddItemToBag(UserItem))
                            {
                                SendAddItem(UserItem);
                                m_StorageItemList.RemoveAt(i);
                                SendDefMessage(grobal2.SM_TAKEBACKSTORAGEITEM_OK, nItemIdx, 0, 0, 0, "");
                                StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                                if (StdItem.NeedIdentify == 1)
                                {
                                    // UserEngine.GetStdItemName(UserItem.wIndex) + #9 +
                                    M2Share.AddGameDataLog('0' + "\t" + m_sMapName + "\t" + m_nCurrX.ToString() + "\t" + m_nCurrY.ToString() + "\t" + m_sCharName + "\t" + StdItem.Name + "\t" + UserItem.MakeIndex.ToString() + "\t" + '1' + "\t" + '0');
                                }
                            }
                            else
                            {
                                SendDefMessage(grobal2.SM_TAKEBACKSTORAGEITEM_FULLBAG, 0, 0, 0, 0, "");
                            }
                            bo19 = true;
                        }
                    }
                    else
                    {
                        // '无法携带更多的东西！！！'
                        SysMsg(M2Share.g_sCanotGetItems, TMsgColor.c_Red, TMsgType.t_Hint);
                    }
                    break;
                }
            }
            if (!bo19)
            {
                SendDefMessage(grobal2.SM_TAKEBACKSTORAGEITEM_FAIL, 0, 0, 0, 0, "");
            }
        }

        public void MakeSaveRcd(ref THumDataInfo HumanRcd)
        {
            THumInfoData HumData;
            TUserItem[] HumItems;
            TUserItem[] BagItems;
            TUserItem[] StorageItems;
            TMagicRcd[] HumMagic;
            TUserMagic UserMagic;
            HumData = HumanRcd.Data;
            HumData.sChrName = m_sCharName;
            HumData.sCurMap = m_sMapName;
            HumData.wCurX = m_nCurrX;
            HumData.wCurY = m_nCurrY;
            HumData.btDir = m_btDirection;
            HumData.btHair = m_btHair;
            HumData.btSex = m_btGender;
            HumData.btJob = m_btJob;
            HumData.nGold = m_nGold;
            HumData.Abil.Level = m_Abil.Level;
            HumData.Abil.HP = m_Abil.HP;
            HumData.Abil.MP = m_Abil.MP;
            HumData.Abil.MaxHP = m_Abil.MaxHP;
            HumData.Abil.MaxMP = m_Abil.MaxMP;
            HumData.Abil.Exp = m_Abil.Exp;
            HumData.Abil.MaxExp = m_Abil.MaxExp;
            HumData.Abil.Weight = m_Abil.Weight;
            HumData.Abil.MaxWeight = m_Abil.MaxWeight;
            HumData.Abil.WearWeight = m_Abil.WearWeight;
            HumData.Abil.MaxWearWeight = m_Abil.MaxWearWeight;
            HumData.Abil.HandWeight = m_Abil.HandWeight;
            HumData.Abil.MaxHandWeight = m_Abil.MaxHandWeight;
            // HumData.Abil:=m_Abil;
            HumData.Abil.HP = m_WAbil.HP;
            HumData.Abil.MP = m_WAbil.MP;
            HumData.wStatusTimeArr = m_wStatusTimeArr;
            HumData.sHomeMap = m_sHomeMap;
            HumData.wHomeX = m_nHomeX;
            HumData.wHomeY = m_nHomeY;
            HumData.nPKPoint = m_nPkPoint;
            HumData.BonusAbil = m_BonusAbil;// 08/09
            HumData.nBonusPoint = m_nBonusPoint;// 08/09
            HumData.sStoragePwd = m_sStoragePwd;
            HumData.btCreditPoint = m_btCreditPoint;
            HumData.btReLevel = m_btReLevel;
            HumData.sMasterName = m_sMasterName;
            HumData.boMaster = m_boMaster;
            HumData.sDearName = m_sDearName;
            HumData.nGameGold = m_nGameGold;
            HumData.nGamePoint = m_nGamePoint;
            if (m_boAllowGroup)
            {
                HumData.btAllowGroup = 1;
            }
            else
            {
                HumData.btAllowGroup = 0;
            }
            HumData.btF9 = btB2;
            HumData.btAttatckMode = m_btAttatckMode;
            HumData.btIncHealth = (byte)m_nIncHealth;
            HumData.btIncSpell = (byte)m_nIncSpell;
            HumData.btIncHealing = (byte)m_nIncHealing;
            HumData.btFightZoneDieCount = (byte)m_nFightZoneDieCount;
            HumData.sAccount = m_sUserID;
            HumData.btEE = (byte)nC4;
            HumData.boLockLogon = m_boLockLogon;
            HumData.wContribution = m_wContribution;
            HumData.btEF = btC8;
            HumData.nHungerStatus = m_nHungerStatus;
            HumData.boAllowGuildReCall = m_boAllowGuildReCall;
            HumData.wGroupRcallTime = m_wGroupRcallTime;
            HumData.dBodyLuck = m_dBodyLuck;
            HumData.boAllowGroupReCall = m_boAllowGroupReCall;
            HumData.QuestUnitOpen = m_QuestUnitOpen;
            HumData.QuestUnit = m_QuestUnit;
            HumData.QuestFlag = m_QuestFlag;
            HumItems = HumanRcd.Data.HumItems;
            if (HumItems == null)
            {
                HumItems = new TUserItem[13];
            }
            HumItems[grobal2.U_DRESS] = m_UseItems[grobal2.U_DRESS];
            HumItems[grobal2.U_WEAPON] = m_UseItems[grobal2.U_WEAPON];
            HumItems[grobal2.U_RIGHTHAND] = m_UseItems[grobal2.U_RIGHTHAND];
            HumItems[grobal2.U_HELMET] = m_UseItems[grobal2.U_NECKLACE];
            HumItems[grobal2.U_NECKLACE] = m_UseItems[grobal2.U_HELMET];
            HumItems[grobal2.U_ARMRINGL] = m_UseItems[grobal2.U_ARMRINGL];
            HumItems[grobal2.U_ARMRINGR] = m_UseItems[grobal2.U_ARMRINGR];
            HumItems[grobal2.U_RINGL] = m_UseItems[grobal2.U_RINGL];
            HumItems[grobal2.U_RINGR] = m_UseItems[grobal2.U_RINGR];
            HumItems[grobal2.U_BUJUK] = m_UseItems[grobal2.U_BUJUK];
            HumItems[grobal2.U_BELT] = m_UseItems[grobal2.U_BELT];
            HumItems[grobal2.U_BOOTS] = m_UseItems[grobal2.U_BOOTS];
            HumItems[grobal2.U_CHARM] = m_UseItems[grobal2.U_CHARM];
            BagItems = HumanRcd.Data.BagItems;
            if (BagItems == null)
            {
                BagItems = new TUserItem[43];
            }
            for (var i = 0; i < m_ItemList.Count; i++)
            {
                if (i >= grobal2.MAXBAGITEM)
                {
                    break;
                }
                BagItems[i] = m_ItemList[i];
            }
            HumMagic = HumanRcd.Data.Magic;
            if (HumMagic == null)
            {
                HumMagic = new TMagicRcd[20];
            }
            for (var i = 0; i < m_MagicList.Count; i++)
            {
                if (i >= grobal2.MAXMAGIC)
                {
                    break;
                }
                UserMagic = m_MagicList[i];
                if (HumMagic[i] == null)
                {
                    HumMagic[i] = new TMagicRcd();
                }
                HumMagic[i].wMagIdx = UserMagic.wMagIdx;
                HumMagic[i].btLevel = UserMagic.btLevel;
                HumMagic[i].btKey = UserMagic.btKey;
                HumMagic[i].nTranPoint = UserMagic.nTranPoint;
            }
            StorageItems = HumanRcd.Data.StorageItems;
            if (StorageItems == null)
            {
                StorageItems = new TUserItem[50];
            }
            for (var i = 0; i < this.m_StorageItemList.Count; i++)
            {
                if (i >= StorageItems.GetUpperBound(0))
                {
                    break;
                }
                StorageItems[i] = this.m_StorageItemList[i];
            }
        }

        public void RefRankInfo(int nRankNo, string sRankName)
        {
            m_nGuildRankNo = nRankNo;
            m_sGuildRankName = sRankName;
            SendMsg(this, grobal2.RM_CHANGEGUILDNAME, 0, 0, 0, 0, "");
        }

        private void GetOldAbil(ref TOAbility OAbility)
        {
            OAbility=new TOAbility();
            OAbility.Level = m_WAbil.Level;
            OAbility.AC = HUtil32.MakeWord(HUtil32._MIN(byte.MaxValue, HUtil32.LoWord(m_WAbil.AC)), HUtil32._MIN(byte.MaxValue, HUtil32.HiWord(m_WAbil.AC)));
            OAbility.MAC = HUtil32.MakeWord(HUtil32._MIN(byte.MaxValue, HUtil32.LoWord(m_WAbil.MAC)), HUtil32._MIN(byte.MaxValue, HUtil32.HiWord(m_WAbil.MAC)));
            OAbility.DC = HUtil32.MakeWord(HUtil32._MIN(byte.MaxValue, HUtil32.LoWord(m_WAbil.DC)), HUtil32._MIN(byte.MaxValue, HUtil32.HiWord(m_WAbil.DC)));
            OAbility.MC = HUtil32.MakeWord(HUtil32._MIN(byte.MaxValue, HUtil32.LoWord(m_WAbil.MC)), HUtil32._MIN(byte.MaxValue, HUtil32.HiWord(m_WAbil.MC)));
            OAbility.SC = HUtil32.MakeWord(HUtil32._MIN(byte.MaxValue, HUtil32.LoWord(m_WAbil.SC)), HUtil32._MIN(byte.MaxValue, HUtil32.HiWord(m_WAbil.SC)));
            OAbility.HP = m_WAbil.HP;
            OAbility.MP = m_WAbil.MP;
            OAbility.MaxHP = m_WAbil.MaxHP;
            OAbility.MaxMP = m_WAbil.MaxMP;
            OAbility.Exp = m_WAbil.Exp;
            OAbility.MaxExp = m_WAbil.MaxExp;
            OAbility.Weight = m_WAbil.Weight;
            OAbility.MaxWeight = m_WAbil.MaxWeight;
            OAbility.WearWeight = (byte)HUtil32._MIN(byte.MaxValue, m_WAbil.WearWeight);
            OAbility.MaxWearWeight = (byte)HUtil32._MIN(byte.MaxValue, m_WAbil.MaxWearWeight);
            OAbility.HandWeight = (byte)HUtil32._MIN(byte.MaxValue, m_WAbil.HandWeight);
            OAbility.MaxHandWeight = (byte)HUtil32._MIN(byte.MaxValue, m_WAbil.MaxHandWeight);
        }

        /// <summary>
        /// 攻击消息数量
        /// </summary>
        /// <returns></returns>
        private int GetHitMsgCount()
        {
            SendMessage SendMessage;
            var result = 0;
            try
            {
                HUtil32.EnterCriticalSection(M2Share.ProcessMsgCriticalSection);
                for (var i = 0; i < m_MsgList.Count; i++)
                {
                    SendMessage = m_MsgList[i];
                    if (SendMessage.wIdent == grobal2.CM_HIT || SendMessage.wIdent == grobal2.CM_HEAVYHIT || SendMessage.wIdent == grobal2.CM_BIGHIT || SendMessage.wIdent == grobal2.CM_POWERHIT 
                        || SendMessage.wIdent == grobal2.CM_LONGHIT || SendMessage.wIdent == grobal2.CM_WIDEHIT || SendMessage.wIdent == grobal2.CM_FIREHIT)
                    {
                        result++;
                    }
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(M2Share.ProcessMsgCriticalSection);
            }
            return result;
        }

        /// <summary>
        /// 魔法消息数量
        /// </summary>
        /// <returns></returns>
        private int GetSpellMsgCount()
        {
            SendMessage SendMessage;
            var result = 0;
            try
            {
                HUtil32.EnterCriticalSection(M2Share.ProcessMsgCriticalSection);
                for (var i = 0; i < m_MsgList.Count; i++)
                {
                    SendMessage = m_MsgList[i];
                    if (SendMessage.wIdent == grobal2.CM_SPELL)
                    {
                        result++;
                    }
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(M2Share.ProcessMsgCriticalSection);
            }
            return result;
        }

        /// <summary>
        /// 跑步消息数量
        /// </summary>
        /// <returns></returns>
        private int GetRunMsgCount()
        {
            SendMessage SendMessage;
            var result = 0;
            try
            {
                HUtil32.EnterCriticalSection(M2Share.ProcessMsgCriticalSection);
                for (var i = 0; i < m_MsgList.Count; i++)
                {
                    SendMessage = m_MsgList[i];
                    if (SendMessage.wIdent == grobal2.CM_RUN)
                    {
                        result++;
                    }
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(M2Share.ProcessMsgCriticalSection);
            }
            return result;
        }

        /// <summary>
        /// 走路消息数量
        /// </summary>
        /// <returns></returns>
        private int GetWalkMsgCount()
        {
            SendMessage SendMessage;
            var result = 0;
            try
            {
                HUtil32.EnterCriticalSection(M2Share.ProcessMsgCriticalSection);
                for (var i = 0; i < m_MsgList.Count; i++)
                {
                    SendMessage = m_MsgList[i];
                    if (SendMessage.wIdent == grobal2.CM_WALK)
                    {
                        result++;
                    }
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(M2Share.ProcessMsgCriticalSection);
            }
            return result;
        }

        private int GetTurnMsgCount()
        {
            SendMessage SendMessage;
            var result = 0;
            try
            {
                HUtil32.EnterCriticalSection(M2Share.ProcessMsgCriticalSection);
                for (var i = 0; i < m_MsgList.Count; i++)
                {
                    SendMessage = m_MsgList[i];
                    if (SendMessage.wIdent == grobal2.CM_TURN)
                    {
                        result++;
                    }
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(M2Share.ProcessMsgCriticalSection);
            }
            return result;
        }

        private int GetSiteDownMsgCount()
        {
            var result = 0;
            SendMessage SendMessage;
            HUtil32.EnterCriticalSection(M2Share.ProcessMsgCriticalSection);
            try
            {
                for (var i = 0; i < m_MsgList.Count; i++)
                {
                    SendMessage = m_MsgList[i];
                    if (SendMessage.wIdent == grobal2.CM_SITDOWN)
                    {
                        result++;
                    }
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(M2Share.ProcessMsgCriticalSection);
            }
            return result;
        }

        private bool CheckActionStatus(short wIdent, ref int dwDelayTime)
        {
            int dwCheckTime = 0;
            var result = false;
            dwDelayTime = 0;
            // 检查人物弯腰停留时间
            if (!M2Share.g_Config.boDisableStruck)
            {
                dwCheckTime = HUtil32.GetTickCount() - m_dwStruckTick;
                if (M2Share.g_Config.dwStruckTime > dwCheckTime)
                {
                    dwDelayTime = M2Share.g_Config.dwStruckTime - dwCheckTime;
                    m_btOldDir = m_btDirection;
                    return false;
                }
            }
            // 检查二个不同操作之间所需间隔时间
            dwCheckTime = HUtil32.GetTickCount() - m_dwActionTick;
            if (m_boTestSpeedMode)
            {
                SysMsg("间隔: " + dwCheckTime, TMsgColor.c_Blue, TMsgType.t_Notice);
            }
            if (m_wOldIdent == wIdent)
            {
                // 当二次操作一样时，则将 boFirst 设置为 真 ，退出由调用函数本身检查二个相同操作之间的间隔时间
                return true;
            }
            if (!M2Share.g_Config.boControlActionInterval)
            {
                return true;
            }
            int dwActionIntervalTime = m_dwActionIntervalTime;
            switch (wIdent)
            {
                case grobal2.CM_LONGHIT:
                    if (M2Share.g_Config.boControlRunLongHit && m_wOldIdent == grobal2.CM_RUN && m_btOldDir != m_btDirection)
                    {
                        dwActionIntervalTime = m_dwRunLongHitIntervalTime;// 跑位刺杀
                    }
                    break;
                case grobal2.CM_HIT:
                    if (M2Share.g_Config.boControlWalkHit && m_wOldIdent == grobal2.CM_WALK && m_btOldDir != m_btDirection)
                    {
                        dwActionIntervalTime = m_dwWalkHitIntervalTime; // 走位攻击
                    }
                    if (M2Share.g_Config.boControlRunHit && m_wOldIdent == grobal2.CM_RUN && m_btOldDir != m_btDirection)
                    {
                        dwActionIntervalTime = m_dwRunHitIntervalTime;// 跑位攻击
                    }
                    break;
                case grobal2.CM_RUN:
                    if (M2Share.g_Config.boControlRunLongHit && m_wOldIdent == grobal2.CM_LONGHIT && m_btOldDir != m_btDirection)
                    {
                        dwActionIntervalTime = m_dwRunLongHitIntervalTime;// 跑位刺杀
                    }
                    if (M2Share.g_Config.boControlRunHit && m_wOldIdent == grobal2.CM_HIT && m_btOldDir != m_btDirection)
                    {
                        dwActionIntervalTime = m_dwRunHitIntervalTime;// 跑位攻击
                    }
                    if (M2Share.g_Config.boControlRunMagic && m_wOldIdent == grobal2.CM_SPELL && m_btOldDir != m_btDirection)
                    {
                        dwActionIntervalTime = m_dwRunMagicIntervalTime;// 跑位魔法
                    }
                    break;
                case grobal2.CM_WALK:
                    if (M2Share.g_Config.boControlWalkHit && m_wOldIdent == grobal2.CM_HIT && m_btOldDir != m_btDirection)
                    {
                        dwActionIntervalTime = m_dwWalkHitIntervalTime;// 走位攻击
                    }
                    if (M2Share.g_Config.boControlRunLongHit && m_wOldIdent == grobal2.CM_LONGHIT && m_btOldDir != m_btDirection)
                    {
                        dwActionIntervalTime = m_dwRunLongHitIntervalTime;// 跑位刺杀
                    }
                    break;
                case grobal2.CM_SPELL: 
                    if (M2Share.g_Config.boControlRunMagic && m_wOldIdent == grobal2.CM_RUN && m_btOldDir != m_btDirection)
                    {
                        dwActionIntervalTime = m_dwRunMagicIntervalTime;// 跑位魔法
                    }
                    break;
            }
            // 将几个攻击操作合并成一个攻击操作代码
            if (wIdent == grobal2.CM_HIT || wIdent == grobal2.CM_HEAVYHIT || wIdent == grobal2.CM_BIGHIT || wIdent == grobal2.CM_POWERHIT || wIdent == grobal2.CM_WIDEHIT || wIdent == grobal2.CM_FIREHIT)
            {
                wIdent = grobal2.CM_HIT;
            }
            if (dwCheckTime >= dwActionIntervalTime)
            {
                m_dwActionTick = HUtil32.GetTickCount();
                result = true;
            }
            else
            {
                dwDelayTime = dwActionIntervalTime - dwCheckTime;
            }
            m_wOldIdent = wIdent;
            m_btOldDir = m_btDirection;
            return result;
        }

        public void SetScriptLabel(string sLabel)
        {
            m_CanJmpScriptLableList.Clear();
            m_CanJmpScriptLableList.Add(sLabel, sLabel);
        }

        /// <summary>
        /// 取得当前脚本可以跳转的标签
        /// </summary>
        /// <param name="sMsg"></param>
        public void GetScriptLabel(string sMsg)
        {
            var sText = string.Empty;
            m_CanJmpScriptLableList.Clear();
            while (true)
            {
                if (string.IsNullOrEmpty(sMsg))
                {
                    break;
                }
                sMsg = HUtil32.GetValidStr3(sMsg, ref sText, "\\");
                if (!string.IsNullOrEmpty(sText))
                {
                    var matchCollection = Regex.Matches(sText, "<?@(\\w+?>)", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(3));
                    foreach (Match match in matchCollection)
                    {
                        var line = match.Value.Remove(match.Value.Length - 1);
                        //if (line[0] != '<')
                        //{
                        //    line = "<" + HUtil32.GetValidStr3(line, ref sData, "<");
                        //}
                        //sText = HUtil32.ArrestStringEx(line, "<", ">", ref sCmdStr);
                        //var sLabel = HUtil32.GetValidStr3(sCmdStr, ref sCmdStr, "/");
                        if (!string.IsNullOrEmpty(line))
                        {
                            m_CanJmpScriptLableList.Add(line, line);
                        }
                    }
                }
            }
        }

        public bool LableIsCanJmp(string sLabel)
        {
            var result = false;
            if (string.Compare(sLabel, "@main", StringComparison.Ordinal) == 0)
            {
                result = true;
                return result;
            }
            if (m_CanJmpScriptLableList.ContainsKey(sLabel.ToLower()))
            {
                result = true;
                return result;
            }
            if (string.Compare(sLabel, m_sPlayDiceLabel, StringComparison.Ordinal) == 0)
            {
                m_sPlayDiceLabel = string.Empty;
                result = true;
                return result;
            }
            return result;
        }

        private bool CheckItemsNeed(TItem StdItem)
        {
            var result = true;
            var castle = M2Share.CastleManager.IsCastleMember(this);
            switch (StdItem.Need)
            {
                case 6:
                    if (m_MyGuild == null)
                    {
                        result = false;
                    }
                    break;
                case 60:
                    if (m_MyGuild == null || m_nGuildRankNo != 1)
                    {
                        result = false;
                    }
                    break;
                case 7:
                    if (castle == null)
                    {
                        result = false;
                    }
                    break;
                case 70:
                    if (castle == null || m_nGuildRankNo != 1)
                    {
                        result = false;
                    }
                    break;
                case 8:
                    if (m_nMemberType == 0)
                    {
                        result = false;
                    }
                    break;
                case 81:
                    if (m_nMemberType != HUtil32.LoWord(StdItem.NeedLevel) || m_nMemberLevel < HUtil32.HiWord(StdItem.NeedLevel))
                    {
                        result = false;
                    }
                    break;
                case 82:
                    if (m_nMemberType < HUtil32.LoWord(StdItem.NeedLevel) || m_nMemberLevel < HUtil32.HiWord(StdItem.NeedLevel))
                    {
                        result = false;
                    }
                    break;
            }
            return result;
        }

        private void CheckMarry()
        {
            bool boIsfound;
            string sUnMarryFileName;
            StringList LoadList;
            string sSayMsg;
            boIsfound = false;
            sUnMarryFileName = M2Share.g_Config.sEnvirDir + "UnMarry.txt";
            if (File.Exists(sUnMarryFileName))
            {
                LoadList = new StringList();
                LoadList.LoadFromFile(sUnMarryFileName);
                for (var i = 0; i < LoadList.Count; i++)
                {
                    if (LoadList[i].CompareTo(this.m_sCharName) == 0)
                    {
                        LoadList.RemoveAt(i);
                        boIsfound = true;
                        break;
                    }
                }
                LoadList.SaveToFile(sUnMarryFileName);
                LoadList.Dispose();
                LoadList = null;
            }
            if (boIsfound)
            {
                if (m_btGender == ObjBase.gMan)
                {
                    sSayMsg = M2Share.g_sfUnMarryManLoginMsg.Replace("%d", m_sDearName);
                    sSayMsg = sSayMsg.Replace("%s", m_sDearName);
                }
                else
                {
                    sSayMsg = M2Share.g_sfUnMarryWoManLoginMsg.Replace("%d", m_sCharName);
                    sSayMsg = sSayMsg.Replace("%s", m_sCharName);
                }
                SysMsg(sSayMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                m_sDearName = "";
                RefShowName();
            }
            m_DearHuman = M2Share.UserEngine.GetPlayObject(m_sDearName);
            if (m_DearHuman != null)
            {
                m_DearHuman.m_DearHuman = this;
                if (m_btGender == ObjBase.gMan)
                {
                    sSayMsg = M2Share.g_sManLoginDearOnlineSelfMsg.Replace("%d", m_sDearName);
                    sSayMsg = sSayMsg.Replace("%s", m_sCharName);
                    sSayMsg = sSayMsg.Replace("%m", m_DearHuman.m_PEnvir.sMapDesc);
                    sSayMsg = sSayMsg.Replace("%x", m_DearHuman.m_nCurrX.ToString());
                    sSayMsg = sSayMsg.Replace("%y", m_DearHuman.m_nCurrY.ToString());
                    SysMsg(sSayMsg, TMsgColor.c_Blue, TMsgType.t_Hint);
                    sSayMsg = M2Share.g_sManLoginDearOnlineDearMsg.Replace("%d", m_sDearName);
                    sSayMsg = sSayMsg.Replace("%s", m_sCharName);
                    sSayMsg = sSayMsg.Replace("%m", m_PEnvir.sMapDesc);
                    sSayMsg = sSayMsg.Replace("%x", m_nCurrX.ToString());
                    sSayMsg = sSayMsg.Replace("%y", m_nCurrY.ToString());
                    m_DearHuman.SysMsg(sSayMsg, TMsgColor.c_Blue, TMsgType.t_Hint);
                }
                else
                {
                    sSayMsg = M2Share.g_sWoManLoginDearOnlineSelfMsg.Replace("%d", m_sDearName);
                    sSayMsg = sSayMsg.Replace("%s", m_sCharName);
                    sSayMsg = sSayMsg.Replace("%m", m_DearHuman.m_PEnvir.sMapDesc);
                    sSayMsg = sSayMsg.Replace("%x", m_DearHuman.m_nCurrX.ToString());
                    sSayMsg = sSayMsg.Replace("%y", m_DearHuman.m_nCurrY.ToString());
                    SysMsg(sSayMsg, TMsgColor.c_Blue, TMsgType.t_Hint);
                    sSayMsg = M2Share.g_sWoManLoginDearOnlineDearMsg.Replace("%d", m_sDearName);
                    sSayMsg = sSayMsg.Replace("%s", m_sCharName);
                    sSayMsg = sSayMsg.Replace("%m", m_PEnvir.sMapDesc);
                    sSayMsg = sSayMsg.Replace("%x", m_nCurrX.ToString());
                    sSayMsg = sSayMsg.Replace("%y", m_nCurrY.ToString());
                    m_DearHuman.SysMsg(sSayMsg, TMsgColor.c_Blue, TMsgType.t_Hint);
                }
            }
            else
            {
                if (m_btGender == ObjBase.gMan)
                {
                    SysMsg(M2Share.g_sManLoginDearNotOnlineMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                }
                else
                {
                    SysMsg(M2Share.g_sWoManLoginDearNotOnlineMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                }
            }
        }

        private void CheckMaster()
        {
            bool boIsfound = false;
            string sSayMsg;
            TPlayObject Human;
            // 处理强行脱离师徒关系
            for (var i = 0; i < M2Share.g_UnForceMasterList.Count; i++)
            {
                if (String.Compare(M2Share.g_UnForceMasterList[i], this.m_sCharName, StringComparison.Ordinal) == 0)
                {
                    M2Share.g_UnForceMasterList.RemoveAt(i);
                    M2Share.SaveUnForceMasterList();
                    boIsfound = true;
                    break;
                }
            }
            if (boIsfound)
            {
                if (m_boMaster)
                {
                    sSayMsg = M2Share.g_sfUnMasterLoginMsg.Replace("%d", m_sMasterName);
                    sSayMsg = sSayMsg.Replace("%s", m_sMasterName);
                }
                else
                {
                    sSayMsg = M2Share.g_sfUnMasterListLoginMsg.Replace("%d", m_sMasterName);
                    sSayMsg = sSayMsg.Replace("%s", m_sMasterName);
                }
                SysMsg(sSayMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                m_sMasterName = "";
                RefShowName();
            }
            if (!string.IsNullOrEmpty(m_sMasterName) && !m_boMaster)
            {
                if (m_Abil.Level >= M2Share.g_Config.nMasterOKLevel)
                {
                    Human = M2Share.UserEngine.GetPlayObject(m_sMasterName);
                    if (Human != null && !Human.m_boDeath && !Human.m_boGhost)
                    {
                        sSayMsg = M2Share.g_sYourMasterListUnMasterOKMsg.Replace("%d", m_sCharName);
                        Human.SysMsg(sSayMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                        SysMsg(M2Share.g_sYouAreUnMasterOKMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                        // 如果大徒弟则将师父上的名字去掉
                        if (m_sCharName == Human.m_sMasterName)
                        {
                            Human.m_sMasterName = "";
                            Human.RefShowName();
                        }
                        for (var i = 0; i < Human.m_MasterList.Count; i++)
                        {
                            if (Human.m_MasterList[i] == this)
                            {
                                Human.m_MasterList.RemoveAt(i);
                                break;
                            }
                        }
                        m_sMasterName = "";
                        RefShowName();
                        if (Human.m_btCreditPoint + M2Share.g_Config.nMasterOKCreditPoint <= byte.MaxValue)
                        {
                            Human.m_btCreditPoint += (byte)M2Share.g_Config.nMasterOKCreditPoint;
                        }
                        Human.m_nBonusPoint += M2Share.g_Config.nMasterOKBonusPoint;
                        Human.SendMsg(Human, grobal2.RM_ADJUST_BONUS, 0, 0, 0, 0, "");
                    }
                    else
                    {
                        // 如果师父不在线则保存到记录表中
                        try
                        {
                            boIsfound = false;
                            for (var i = 0; i < M2Share.g_UnMasterList.Count; i++)
                            {
                                if (String.Compare(M2Share.g_UnMasterList[i], this.m_sCharName, StringComparison.Ordinal) == 0)
                                {
                                    boIsfound = true;
                                    break;
                                }
                            }
                            if (!boIsfound)
                            {
                                M2Share.g_UnMasterList.Add(m_sMasterName);
                            }
                        }
                        finally
                        {
                        }
                        if (!boIsfound)
                        {
                            M2Share.SaveUnMasterList();
                        }
                        SysMsg(M2Share.g_sYouAreUnMasterOKMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                        m_sMasterName = "";
                        RefShowName();
                    }
                }
            }
            // 处理出师记录
            boIsfound = false;
            try
            {
                for (var i = 0; i < M2Share.g_UnMasterList.Count; i++)
                {
                    if (String.Compare(M2Share.g_UnMasterList[i], this.m_sCharName, StringComparison.Ordinal) == 0)
                    {
                        M2Share.g_UnMasterList.RemoveAt(i);
                        M2Share.SaveUnMasterList();
                        boIsfound = true;
                        break;
                    }
                }
            }
            finally
            {
            }
            if (boIsfound && m_boMaster)
            {
                SysMsg(M2Share.g_sUnMasterLoginMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                m_sMasterName = "";
                RefShowName();
                if (m_btCreditPoint + M2Share.g_Config.nMasterOKCreditPoint <= byte.MaxValue)
                {
                    m_btCreditPoint += (byte)M2Share.g_Config.nMasterOKCreditPoint;
                }
                m_nBonusPoint += M2Share.g_Config.nMasterOKBonusPoint;
                SendMsg(this, grobal2.RM_ADJUST_BONUS, 0, 0, 0, 0, "");
            }
            if (string.IsNullOrEmpty(m_sMasterName))
            {
                return;
            }
            if (m_boMaster) // 师父上线通知
            {
                m_MasterHuman = M2Share.UserEngine.GetPlayObject(m_sMasterName);
                if (m_MasterHuman != null)
                {
                    m_MasterHuman.m_MasterHuman = this;
                    m_MasterList.Add(m_MasterHuman);
                    sSayMsg = M2Share.g_sMasterOnlineSelfMsg.Replace("%d", m_sMasterName);
                    sSayMsg = sSayMsg.Replace("%s", m_sCharName);
                    sSayMsg = sSayMsg.Replace("%m", m_MasterHuman.m_PEnvir.sMapDesc);
                    sSayMsg = sSayMsg.Replace("%x", m_MasterHuman.m_nCurrX.ToString());
                    sSayMsg = sSayMsg.Replace("%y", m_MasterHuman.m_nCurrY.ToString());
                    SysMsg(sSayMsg, TMsgColor.c_Blue, TMsgType.t_Hint);
                    sSayMsg = M2Share.g_sMasterOnlineMasterListMsg.Replace("%d", m_sMasterName);
                    sSayMsg = sSayMsg.Replace("%s", m_sCharName);
                    sSayMsg = sSayMsg.Replace("%m", m_PEnvir.sMapDesc);
                    sSayMsg = sSayMsg.Replace("%x", m_nCurrX.ToString());
                    sSayMsg = sSayMsg.Replace("%y", m_nCurrY.ToString());
                    m_MasterHuman.SysMsg(sSayMsg, TMsgColor.c_Blue, TMsgType.t_Hint);
                }
                else
                {
                    SysMsg(M2Share.g_sMasterNotOnlineMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                }
            }
            else
            {
                // 徒弟上线通知
                if (!string.IsNullOrEmpty(m_sMasterName))
                {
                    m_MasterHuman = M2Share.UserEngine.GetPlayObject(m_sMasterName);
                    if (m_MasterHuman != null)
                    {
                        if (m_MasterHuman.m_sMasterName == m_sCharName)
                        {
                            m_MasterHuman.m_MasterHuman = this;
                        }
                        m_MasterHuman.m_MasterList.Add(this);
                        sSayMsg = M2Share.g_sMasterListOnlineSelfMsg.Replace("%d", m_sMasterName);
                        sSayMsg = sSayMsg.Replace("%s", m_sCharName);
                        sSayMsg = sSayMsg.Replace("%m", m_MasterHuman.m_PEnvir.sMapDesc);
                        sSayMsg = sSayMsg.Replace("%x", m_MasterHuman.m_nCurrX.ToString());
                        sSayMsg = sSayMsg.Replace("%y", m_MasterHuman.m_nCurrY.ToString());
                        SysMsg(sSayMsg, TMsgColor.c_Blue, TMsgType.t_Hint);
                        sSayMsg = M2Share.g_sMasterListOnlineMasterMsg.Replace("%d", m_sMasterName);
                        sSayMsg = sSayMsg.Replace("%s", m_sCharName);
                        sSayMsg = sSayMsg.Replace("%m", m_PEnvir.sMapDesc);
                        sSayMsg = sSayMsg.Replace("%x", m_nCurrX.ToString());
                        sSayMsg = sSayMsg.Replace("%y", m_nCurrY.ToString());
                        m_MasterHuman.SysMsg(sSayMsg, TMsgColor.c_Blue, TMsgType.t_Hint);
                    }
                    else
                    {
                        SysMsg(M2Share.g_sMasterListNotOnlineMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                    }
                }
            }
        }

        public string GetMyInfo()
        {
            string result;
            string sMyInfo;
            sMyInfo = M2Share.g_sMyInfo;
            sMyInfo = sMyInfo.Replace("%name", m_sCharName);
            sMyInfo = sMyInfo.Replace("%map", m_PEnvir.sMapDesc);
            sMyInfo = sMyInfo.Replace("%x", m_nCurrX.ToString());
            sMyInfo = sMyInfo.Replace("%y", m_nCurrY.ToString());
            sMyInfo = sMyInfo.Replace("%level", m_Abil.Level.ToString());
            sMyInfo = sMyInfo.Replace("%gold", m_nGold.ToString());
            sMyInfo = sMyInfo.Replace("%pk", m_nPkPoint.ToString());
            sMyInfo = sMyInfo.Replace("%minhp", m_WAbil.HP.ToString());
            sMyInfo = sMyInfo.Replace("%maxhp", m_WAbil.MaxHP.ToString());
            sMyInfo = sMyInfo.Replace("%minmp", m_WAbil.MP.ToString());
            sMyInfo = sMyInfo.Replace("%maxmp", m_WAbil.MaxMP.ToString());
            sMyInfo = sMyInfo.Replace("%mindc", HUtil32.LoWord(m_WAbil.DC).ToString());
            sMyInfo = sMyInfo.Replace("%maxdc", HUtil32.HiWord(m_WAbil.DC).ToString());
            sMyInfo = sMyInfo.Replace("%minmc", HUtil32.LoWord(m_WAbil.MC).ToString());
            sMyInfo = sMyInfo.Replace("%maxmc", HUtil32.HiWord(m_WAbil.MC).ToString());
            sMyInfo = sMyInfo.Replace("%minsc", HUtil32.LoWord(m_WAbil.SC).ToString());
            sMyInfo = sMyInfo.Replace("%maxsc", HUtil32.HiWord(m_WAbil.SC).ToString());
            sMyInfo = sMyInfo.Replace("%logontime", m_dLogonTime.ToString());
            sMyInfo = sMyInfo.Replace("%logonint", ((HUtil32.GetTickCount() - m_dwLogonTick) / 60000).ToString());
            result = sMyInfo;
            return result;
        }

        private bool CheckItemBindUse(TUserItem UserItem)
        {
            bool result;
            TItemBind ItemBind;
            result = true;
            try
            {
                for (var i = 0; i < M2Share.g_ItemBindAccount.Count; i++)
                {
                    ItemBind = M2Share.g_ItemBindAccount[i];
                    if (ItemBind.nMakeIdex == UserItem.MakeIndex && ItemBind.nItemIdx == UserItem.wIndex)
                    {
                        result = false;
                        if (ItemBind.sBindName.CompareTo(m_sUserID) == 0)
                        {
                            result = true;
                        }
                        else
                        {
                            SysMsg(M2Share.g_sItemIsNotThisAccount, TMsgColor.c_Red, TMsgType.t_Hint);
                        }
                        return result;
                    }
                }
            }
            finally
            {
            }
            try
            {
                for (var i = 0; i < M2Share.g_ItemBindIPaddr.Count; i++)
                {
                    ItemBind = M2Share.g_ItemBindIPaddr[i];
                    if (ItemBind.nMakeIdex == UserItem.MakeIndex && ItemBind.nItemIdx == UserItem.wIndex)
                    {
                        result = false;
                        if (ItemBind.sBindName.CompareTo(m_sIPaddr) == 0)
                        {
                            result = true;
                        }
                        else
                        {
                            SysMsg(M2Share.g_sItemIsNotThisIPaddr, TMsgColor.c_Red, TMsgType.t_Hint);
                        }
                        return result;
                    }
                }
            }
            finally
            {
            }
            try
            {
                for (var i = 0; i < M2Share.g_ItemBindCharName.Count; i++)
                {
                    ItemBind = M2Share.g_ItemBindCharName[i];
                    if (ItemBind.nMakeIdex == UserItem.MakeIndex && ItemBind.nItemIdx == UserItem.wIndex)
                    {
                        result = false;
                        if (ItemBind.sBindName.CompareTo(m_sCharName) == 0)
                        {
                            result = true;
                        }
                        else
                        {
                            SysMsg(M2Share.g_sItemIsNotThisCharName, TMsgColor.c_Red, TMsgType.t_Hint);
                        }
                        return result;
                    }
                }
            }
            finally
            {
            }
            return result;
        }

        private void ProcessClientPassword(TProcessMessage ProcessMsg)
        {
            int nLen;
            string sData;
            if (ProcessMsg.wParam == 0)
            {
                ProcessUserLineMsg('@' + M2Share.g_GameCommand.UNLOCK.sCmd);
                return;
            }
            sData = ProcessMsg.sMsg;
            nLen = sData.Length;
            if (m_boSetStoragePwd)
            {
                m_boSetStoragePwd = false;
                if (nLen > 3 && nLen < 8)
                {
                    m_sTempPwd = sData;
                    m_boReConfigPwd = true;
                    SysMsg(M2Share.g_sReSetPasswordMsg, TMsgColor.c_Green, TMsgType.t_Hint);// '请重复输入一次仓库密码：'
                    SendMsg(this, grobal2.RM_PASSWORD, 0, 0, 0, 0, "");
                }
                else
                {
                    SysMsg(M2Share.g_sPasswordOverLongMsg, TMsgColor.c_Red, TMsgType.t_Hint);// '输入的密码长度不正确！！！，密码长度必须在 4 - 7 的范围内，请重新设置密码。'
                }
                return;
            }
            if (m_boReConfigPwd)
            {
                m_boReConfigPwd = false;
                if (String.Compare(m_sTempPwd, sData, StringComparison.Ordinal) == 0)
                {
                    m_sStoragePwd = sData;
                    m_boPasswordLocked = true;
                    m_sTempPwd = "";
                    SysMsg(M2Share.g_sReSetPasswordOKMsg, TMsgColor.c_Blue, TMsgType.t_Hint);// '密码设置成功！！，仓库已经自动上锁，请记好您的仓库密码，在取仓库时需要使用此密码开锁。'
                }
                else
                {
                    m_sTempPwd = "";
                    SysMsg(M2Share.g_sReSetPasswordNotMatchMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                }
                return;
            }
            if (m_boUnLockPwd || m_boUnLockStoragePwd)
            {
                if (String.Compare(m_sStoragePwd, sData, StringComparison.Ordinal) == 0)
                {
                    m_boPasswordLocked = false;
                    if (m_boUnLockPwd)
                    {
                        if (M2Share.g_Config.boLockDealAction)
                        {
                            m_boCanDeal = true;
                        }
                        if (M2Share.g_Config.boLockDropAction)
                        {
                            m_boCanDrop = true;
                        }
                        if (M2Share.g_Config.boLockWalkAction)
                        {
                            m_boCanWalk = true;
                        }
                        if (M2Share.g_Config.boLockRunAction)
                        {
                            m_boCanRun = true;
                        }
                        if (M2Share.g_Config.boLockHitAction)
                        {
                            m_boCanHit = true;
                        }
                        if (M2Share.g_Config.boLockSpellAction)
                        {
                            m_boCanSpell = true;
                        }
                        if (M2Share.g_Config.boLockSendMsgAction)
                        {
                            m_boCanSendMsg = true;
                        }
                        if (M2Share.g_Config.boLockUserItemAction)
                        {
                            m_boCanUseItem = true;
                        }
                        if (M2Share.g_Config.boLockInObModeAction)
                        {
                            m_boObMode = false;
                            m_boAdminMode = false;
                        }
                        m_boLockLogoned = true;
                        SysMsg(M2Share.g_sPasswordUnLockOKMsg, TMsgColor.c_Blue, TMsgType.t_Hint);
                    }
                    if (m_boUnLockStoragePwd)
                    {
                        if (M2Share.g_Config.boLockGetBackItemAction)
                        {
                            m_boCanGetBackItem = true;
                        }
                        SysMsg(M2Share.g_sStorageUnLockOKMsg, TMsgColor.c_Blue, TMsgType.t_Hint);
                    }
                }
                else
                {
                    m_btPwdFailCount++;
                    SysMsg(M2Share.g_sUnLockPasswordFailMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                    if (m_btPwdFailCount > 3)
                    {
                        SysMsg(M2Share.g_sStoragePasswordLockedMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                    }
                }
                m_boUnLockPwd = false;
                m_boUnLockStoragePwd = false;
                return;
            }
            if (m_boCheckOldPwd)
            {
                m_boCheckOldPwd = false;
                if (m_sStoragePwd == sData)
                {
                    SendMsg(this, grobal2.RM_PASSWORD, 0, 0, 0, 0, "");
                    SysMsg(M2Share.g_sSetPasswordMsg, TMsgColor.c_Green, TMsgType.t_Hint);
                    m_boSetStoragePwd = true;
                }
                else
                {
                    m_btPwdFailCount++;
                    SysMsg(M2Share.g_sOldPasswordIncorrectMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                    if (m_btPwdFailCount > 3)
                    {
                        SysMsg(M2Share.g_sStoragePasswordLockedMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                        m_boPasswordLocked = true;
                    }
                }
            }
        }

        public void RecallHuman(string sHumName)
        {
            short nX = 0;
            short nY = 0;
            short n18 = 0;
            short n1C = 0;
            var PlayObject = M2Share.UserEngine.GetPlayObject(sHumName);
            if (PlayObject != null)
            {
                if (GetFrontPosition(ref nX, ref nY))
                {
                    if (sub_4C5370(nX, nY, 3, ref n18, ref n1C))
                    {
                        PlayObject.SendRefMsg(grobal2.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
                        PlayObject.SpaceMove(m_sMapName, n18, n1C, 0);
                    }
                }
                else
                {
                    SysMsg("召唤失败！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                }
            }
            else
            {
                SysMsg(format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumName), TMsgColor.c_Red, TMsgType.t_Hint);
            }
        }

        public void ReQuestGuildWar(string sGuildName)
        {
            TGuild Guild;
            TWarGuild WarGuild;
            bool boReQuestOK;
            if (!IsGuildMaster())
            {
                SysMsg("只有行会掌门人才能申请！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (M2Share.nServerIndex != 0)
            {
                SysMsg("这个命令不能在本服务器上使用！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            Guild = M2Share.GuildManager.FindGuild(sGuildName);
            if (Guild == null)
            {
                SysMsg("行会不存在！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            boReQuestOK = false;
            WarGuild = m_MyGuild.AddWarGuild(Guild);
            if (WarGuild != null)
            {
                if (Guild.AddWarGuild(m_MyGuild) == null)
                {
                    WarGuild.dwWarTick = 0;
                }
                else
                {
                    boReQuestOK = true;
                }
            }
            if (boReQuestOK)
            {
                M2Share.UserEngine.SendServerGroupMsg(grobal2.SS_207, M2Share.nServerIndex, m_MyGuild.sGuildName);
                M2Share.UserEngine.SendServerGroupMsg(grobal2.SS_207, M2Share.nServerIndex, Guild.sGuildName);
            }
        }

        private bool CheckDenyLogon()
        {
            bool result;
            result = false;
            if (M2Share.GetDenyIPAddrList(m_sIPaddr))
            {
                SysMsg(M2Share.g_sYourIPaddrDenyLogon, TMsgColor.c_Red, TMsgType.t_Hint);
                result = true;
            }
            else if (M2Share.GetDenyAccountList(m_sUserID))
            {
                SysMsg(M2Share.g_sYourAccountDenyLogon, TMsgColor.c_Red, TMsgType.t_Hint);
                result = true;
            }
            else if (M2Share.GetDenyChrNameList(m_sCharName))
            {
                SysMsg(M2Share.g_sYourCharNameDenyLogon, TMsgColor.c_Red, TMsgType.t_Hint);
                result = true;
            }
            if (result)
            {
                m_boEmergencyClose = true;
            }
            return result;
        }
    }
}