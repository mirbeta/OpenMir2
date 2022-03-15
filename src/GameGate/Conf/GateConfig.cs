using System;
using System.IO;

namespace GameGate
{
    /// <summary>
    /// 网关配置类
    /// </summary>
    public class GateConfig
    {
        public bool m_fAddLog;
        /// <summary>
        /// 日志等级
        /// </summary>
        public int ShowLogLevel;
        /// <summary>
        /// 是否显示Debug日志
        /// </summary>
        public bool ShowDebugLog;
        /// <summary>
        /// 网关数
        /// </summary>
        public int GateCount;
        /// <summary>
        /// 检查空会话
        /// </summary>
        public bool CheckNullSession;
        /// <summary>
        /// 加速方式
        /// </summary>
        public bool m_fOverSpeedSendBack;
        /// <summary>
        /// 开启防御CC攻击
        /// </summary>
        public bool m_fDefenceCCPacket;
        public bool m_fKickOverSpeed;
        public bool m_fDenyPresend;
        public bool m_fItemSpeedCompensate;
        public bool m_fDoMotaeboSpeedCheck;
        public bool m_fKickOverPacketSize;
        public int m_nMaxConnectOfIP;
        public int m_nMaxClientCount;
        public int m_nClientTimeOutTime;
        public int m_nNomClientPacketSize;
        public int m_nMaxClientPacketSize;
        public int m_nMaxClientPacketCount;
        public string m_szCMDSpaceMove;
        public string m_szOverClientCntMsg;
        public string m_szHWIDBlockedMsg;
        public string m_szChatFilterReplace;
        public string m_szOverSpeedSendBack;
        public string m_szPacketDecryptFailed;
        public string m_szBlockHWIDFileName;
        /// <summary>
        /// 聊天过滤
        /// </summary>
        public bool IsChatFilter;
        /// <summary>
        /// 是否开启发言间隔控制
        /// </summary>
        public bool IsChatInterval;
        /// <summary>
        /// 是否开启发言命令过滤
        /// </summary>
        public bool IsChatCmdFilter;
        /// <summary>
        /// 是否开启转向间隔
        /// </summary>
        public bool IsTurnInterval;
        /// <summary>
        /// 是否开启移动间隔
        /// </summary>
        public bool IsMoveInterval;
        /// <summary>
        /// 是否开启施法间隔
        /// </summary>
        public bool IsSpellInterval;
        /// <summary>
        /// 是否开启攻击间隔
        /// </summary>
        public bool IsAttackInterval;
        /// <summary>
        /// 是否开启挖取间隔
        /// </summary>
        public bool IsButchInterval;
        /// <summary>
        /// 是否开启蹲下间隔
        /// </summary>
        public bool IsSitDownInterval;
        /// <summary>
        /// 使用传送命令之后,多久才能拾取物品  
        /// </summary>
        public bool IsSpaceMoveNextPickupInterval;
        /// <summary>
        /// 是否开启拾取间隔
        /// </summary>
        public bool IsPickupInterval;
        /// <summary>
        /// 是否开启吃东西间隔
        /// </summary>
        public bool IsEatInterval;
        /// <summary>
        /// 是否开启客户端机器码控制
        /// </summary>
        public bool IsProcClientHardwareID;
        /// <summary>
        /// 客户端机器码加密Key
        /// 开启机器码控制后客户端也需修改同步Key,否则将无法进入游戏
        /// </summary>
        public string ProClientHardwareKey;
        /// <summary>
        /// 聊天间隔时间
        /// </summary>
        public int ChatInterval;
        /// <summary>
        /// 转身间隔时间
        /// </summary>
        public int TurnInterval;
        /// <summary>
        /// 移动间隔时间
        /// </summary>
        public int MoveInterval;
        /// <summary>
        /// 施法间隔时间
        /// </summary>
        public int SpellNextInterval;
        /// <summary>
        /// 攻击间隔时间
        /// </summary>
        public int AttackInterval;
        /// <summary>
        /// 挖取间隔时间
        /// </summary>
        public int ButchInterval;
        /// <summary>
        /// 蹲下间隔时间
        /// </summary>
        public int SitDownInterval;
        /// <summary>
        /// 拾取间隔时间
        /// </summary>
        public int PickupInterval;
        /// <summary>
        /// 使用物品间隔时间
        /// </summary>
        public int EatInterval;
        /// <summary>
        /// 移动后对攻击动作的补偿时间，数值越小，封加速越严厉，默认280。
        /// </summary>
        public int MoveNextSpellCompensate;
        /// <summary>
        /// 移动后对魔法动作的补偿时间，数值越小，封加速越严厉，默认80。
        /// </summary>
        public int MoveNextAttackCompensate;
        /// <summary>
        /// 攻击后对移动，魔法等其他动作的补偿时间，数值越小，封加速越严厉，默认200。
        /// </summary>
        public int AttackNextMoveCompensate;
        /// <summary>
        /// 攻击后对移动，魔法等其他动作的补偿时间，数值越小，封加速越严厉，默认200。
        /// </summary>
        public int AttackNextSpellCompensate;
        /// <summary>
        /// 魔法后对移动，攻击等其他动作的补偿时间，数值越小，封加速越严厉，默认200。
        /// </summary>
        public int SpellNextMoveCompensate;
        /// <summary>
        /// 魔法后对移动，攻击等其他动作的补偿时间，数值越小，封加速越严厉，默认200。
        /// </summary>
        public int SpellNextAttackCompensate;
        /// <summary>
        /// 使用Move命令之后拾取物品间隔
        /// </summary>
        public int SpaceMoveNextPickupInterval;
        /// <summary>
        /// 对加速惩罚的基数，数据越大，封加速越严厉，默认0，建议调节到20~120。
        /// </summary>
        public int PunishBaseInterval;
        /// <summary>
        /// 对超速惩罚的倍数，数值越大，封加速越严厉，默认1.00。
        /// 超速越多，游戏越卡
        /// </summary>
        public double PunishIntervalRate;
        public int PunishMoveInterval;
        public int PunishSpellInterval;
        public int PunishAttackInterval;
        public int MaxItemSpeed;
        public int m_nMaxItemSpeedRate;
        /// <summary>
        /// 客户端显示物品方式
        /// </summary>
        public bool ClientShowHintNewType;
        public bool OpenClientSpeedRate;
        public bool SyncClientSpeed;
        public int ClientMoveSpeedRate;
        public int ClientSpellSpeedRate;
        public int ClientAttackSpeedRate;
        public TPunishMethod OverSpeedPunishMethod;
        public TBlockIPMethod BlockIPMethod;
        public TChatFilterMethod ChatFilterMethod;
        public TOverSpeedMsgMethod SpeedHackWarnMethod;

        public GateConfig()
        {
            CheckNullSession = true;
            m_fOverSpeedSendBack = false;
            m_fDefenceCCPacket = false;
            m_fKickOverSpeed = false;
            m_fDenyPresend = false;
            m_fItemSpeedCompensate = false;
            m_fDoMotaeboSpeedCheck = true;
            m_fKickOverPacketSize = true;
            BlockIPMethod = TBlockIPMethod.mDisconnect;
            m_nNomClientPacketSize = 400;
            m_nMaxClientPacketSize = 10240;
            m_nMaxConnectOfIP = 50;
            m_nMaxClientCount = 50;
            m_nClientTimeOutTime = 15 * 1000;
            m_nMaxClientPacketCount = 15;
            m_szOverSpeedSendBack = "[提示]：请爱护游戏环境，关闭加速外挂重新登陆！";
            m_szCMDSpaceMove = "Move";
            m_szPacketDecryptFailed = "[警告]：游戏连接被断开，请重新登陆！原因：使用非法外挂，客户端不配套，开启的客户端数量过多。";
            m_szOverClientCntMsg = "开启游戏过多，链接被断开！";
            m_szHWIDBlockedMsg = "机器码已被封，链接被断开！";
            m_szChatFilterReplace = "说话内容被屏蔽";
            m_szBlockHWIDFileName = Path.Combine(AppContext.BaseDirectory, "BlockHWID.txt");
            IsChatCmdFilter = false;
            IsChatFilter = true;
            IsChatInterval = true;
            IsTurnInterval = true;
            IsMoveInterval = true;
            IsSpellInterval = true;
            IsAttackInterval = true;
            IsButchInterval = true;
            IsSitDownInterval = true;
            IsSpaceMoveNextPickupInterval = true;
            IsPickupInterval = true;
            IsEatInterval = true;
            IsProcClientHardwareID = false;
            ProClientHardwareKey = "openmir2";
            ChatInterval = 800;
            TurnInterval = 350;
            MoveInterval = 570;
            AttackInterval = 900;
            ButchInterval = 450;
            SitDownInterval = 450;
            PickupInterval = 330;
            EatInterval = 330;
            MoveNextSpellCompensate = 100;
            MoveNextAttackCompensate = 250;
            AttackNextMoveCompensate = 200;
            AttackNextSpellCompensate = 200;
            SpellNextMoveCompensate = 200;
            SpellNextAttackCompensate = 200;
            SpaceMoveNextPickupInterval = 600;
            PunishBaseInterval = 20;
            PunishIntervalRate = 1.00;
            OverSpeedPunishMethod = TPunishMethod.DelaySend;
            PunishMoveInterval = 150;
            PunishSpellInterval = 150;
            PunishAttackInterval = 150;
            ChatFilterMethod = TChatFilterMethod.ctReplaceAll;
            SpeedHackWarnMethod = TOverSpeedMsgMethod.ptSysmsg;
            MaxItemSpeed = 6;
            m_nMaxItemSpeedRate = 60;
            ClientShowHintNewType = true;
            OpenClientSpeedRate = false;
            SyncClientSpeed = false;
            ClientMoveSpeedRate = 0;
            ClientSpellSpeedRate = 0;
            ClientAttackSpeedRate = 0;
            ShowDebugLog = false;
        }
    }
}