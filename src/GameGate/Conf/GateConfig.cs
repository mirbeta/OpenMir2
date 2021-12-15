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
        public int m_nShowLogLevel;
        public int m_nGateCount;
        public bool m_fCheckNullSession;
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
        public bool m_fChatFilter;
        /// <summary>
        /// 是否开启发言间隔控制
        /// </summary>
        public bool m_fChatInterval;
        /// <summary>
        /// 是否开启发言命令过滤
        /// </summary>
        public bool m_fChatCmdFilter;
        /// <summary>
        /// 是否开启转向间隔
        /// </summary>
        public bool m_fTurnInterval;
        /// <summary>
        /// 是否开启移动间隔
        /// </summary>
        public bool m_fMoveInterval;
        /// <summary>
        /// 是否开启施法间隔
        /// </summary>
        public bool m_fSpellInterval;
        /// <summary>
        /// 是否开启攻击间隔
        /// </summary>
        public bool m_fAttackInterval;
        /// <summary>
        /// 是否开启挖取间隔
        /// </summary>
        public bool m_fButchInterval;
        /// <summary>
        /// 是否开启蹲下间隔
        /// </summary>
        public bool m_fSitDownInterval;
        /// <summary>
        /// 使用传送命令之后,多久才能拾取物品  
        /// </summary>
        public bool m_fSpaceMoveNextPickupInterval;
        /// <summary>
        /// 是否开启拾取间隔
        /// </summary>
        public bool m_fPickupInterval;
        /// <summary>
        /// 是否开启吃东西间隔
        /// </summary>
        public bool m_fEatInterval;
        /// <summary>
        /// 是否开启客户端机器码控制
        /// </summary>
        public bool m_fProcClientHWID;
        /// <summary>
        /// 聊天间隔时间
        /// </summary>
        public int m_nChatInterval;
        /// <summary>
        /// 转身间隔时间
        /// </summary>
        public int m_nTurnInterval;
        /// <summary>
        /// 移动间隔时间
        /// </summary>
        public int m_nMoveInterval;
        /// <summary>
        /// 施法间隔时间
        /// </summary>
        public int m_nSpellNextInterval;
        /// <summary>
        /// 攻击间隔时间
        /// </summary>
        public int m_nAttackInterval;
        /// <summary>
        /// 挖取间隔时间
        /// </summary>
        public int m_nButchInterval;
        /// <summary>
        /// 蹲下间隔时间
        /// </summary>
        public int m_nSitDownInterval;
        /// <summary>
        /// 拾取间隔时间
        /// </summary>
        public int m_nPickupInterval;
        /// <summary>
        /// 使用物品间隔时间
        /// </summary>
        public int m_nEatInterval;
        /// <summary>
        /// 移动后对攻击动作的补偿时间，数值越小，封加速越严厉，默认280。
        /// </summary>
        public int m_nMoveNextSpellCompensate;
        /// <summary>
        /// 移动后对魔法动作的补偿时间，数值越小，封加速越严厉，默认80。
        /// </summary>
        public int m_nMoveNextAttackCompensate;
        /// <summary>
        /// 攻击后对移动，魔法等其他动作的补偿时间，数值越小，封加速越严厉，默认200。
        /// </summary>
        public int m_nAttackNextMoveCompensate;
        /// <summary>
        /// 攻击后对移动，魔法等其他动作的补偿时间，数值越小，封加速越严厉，默认200。
        /// </summary>
        public int m_nAttackNextSpellCompensate;
        /// <summary>
        /// 魔法后对移动，攻击等其他动作的补偿时间，数值越小，封加速越严厉，默认200。
        /// </summary>
        public int m_nSpellNextMoveCompensate;
        /// <summary>
        /// 魔法后对移动，攻击等其他动作的补偿时间，数值越小，封加速越严厉，默认200。
        /// </summary>
        public int m_nSpellNextAttackCompensate;
        public int m_nSpaceMoveNextPickupInterval;
        /// <summary>
        /// 对加速惩罚的基数，数据越大，封加速越严厉，默认0，建议调节到20~120。
        /// </summary>
        public int m_nPunishBaseInterval;
        /// <summary>
        /// 对超速惩罚的倍数，数值越大，封加速越严厉，默认1.00。
        /// 超速越多，游戏越卡
        /// </summary>
        public double m_nPunishIntervalRate;
        public int m_nPunishMoveInterval;
        public int m_nPunishSpellInterval;
        public int m_nPunishAttackInterval;
        public int m_nMaxItemSpeed;
        public int m_nMaxItemSpeedRate;
        public bool m_fClientShowHintNewType;
        public bool m_fOpenClientSpeedRate;
        public bool m_fSyncClientSpeed;
        public int m_nClientMoveSpeedRate;
        public int m_nClientSpellSpeedRate;
        public int m_nClientAttackSpeedRate;
        public TPunishMethod m_tOverSpeedPunishMethod;
        public TBlockIPMethod m_tBlockIPMethod;
        public TChatFilterMethod m_tChatFilterMethod;
        public TOverSpeedMsgMethod m_tSpeedHackWarnMethod;

        public GateConfig()
        {
            m_fCheckNullSession = true;
            m_fOverSpeedSendBack = false;
            m_fDefenceCCPacket = false;
            m_fKickOverSpeed = false;
            m_fDenyPresend = false;
            m_fItemSpeedCompensate = false;
            m_fDoMotaeboSpeedCheck = true;
            m_fKickOverPacketSize = true;
            m_tBlockIPMethod = TBlockIPMethod.mDisconnect;
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
            m_fChatCmdFilter = false;
            m_fChatFilter = true;
            m_fChatInterval = true;
            m_fTurnInterval = true;
            m_fMoveInterval = true;
            m_fSpellInterval = true;
            m_fAttackInterval = true;
            m_fButchInterval = true;
            m_fSitDownInterval = true;
            m_fSpaceMoveNextPickupInterval = true;
            m_fPickupInterval = true;
            m_fEatInterval = true;
            m_fProcClientHWID = false;
            m_nChatInterval = 800;
            m_nTurnInterval = 350;
            m_nMoveInterval = 570;
            m_nAttackInterval = 900;
            m_nButchInterval = 450;
            m_nSitDownInterval = 450;
            m_nPickupInterval = 330;
            m_nEatInterval = 330;
            m_nMoveNextSpellCompensate = 100;
            m_nMoveNextAttackCompensate = 250;
            m_nAttackNextMoveCompensate = 200;
            m_nAttackNextSpellCompensate = 200;
            m_nSpellNextMoveCompensate = 200;
            m_nSpellNextAttackCompensate = 200;
            m_nSpaceMoveNextPickupInterval = 600;
            m_nPunishBaseInterval = 20;
            m_nPunishIntervalRate = 1.00;
            m_tOverSpeedPunishMethod = TPunishMethod.DelaySend;
            m_nPunishMoveInterval = 150;
            m_nPunishSpellInterval = 150;
            m_nPunishAttackInterval = 150;
            m_tChatFilterMethod = TChatFilterMethod.ctReplaceAll;
            m_tSpeedHackWarnMethod = TOverSpeedMsgMethod.ptSysmsg;
            m_nMaxItemSpeed = 6;
            m_nMaxItemSpeedRate = 60;
            m_fClientShowHintNewType = true;
            m_fOpenClientSpeedRate = false;
            m_fSyncClientSpeed = false;
            m_nClientMoveSpeedRate = 0;
            m_nClientSpellSpeedRate = 0;
            m_nClientAttackSpeedRate = 0;
        }
    }
}