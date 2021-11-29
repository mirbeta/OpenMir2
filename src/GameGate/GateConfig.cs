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
        public bool m_fOverSpeedSendBack;
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
        /// 发言间隔
        /// </summary>
        public bool m_fChatInterval;
        public bool m_fChatCmdFilter;
        public bool m_fTurnInterval;
        public bool m_fMoveInterval;
        public bool m_fSpellInterval;
        public bool m_fAttackInterval;
        public bool m_fButchInterval;
        public bool m_fSitDownInterval;
        public bool m_fSpaceMoveNextPickupInterval;
        public bool m_fPickupInterval;
        public bool m_fEatInterval;
        /// <summary>
        /// 是否开启客户端机器码控制
        /// </summary>
        public bool m_fProcClientHWID;
        public int m_nChatInterval;
        public int m_nTurnInterval;
        public int m_nMoveInterval;
        public int m_nSpellNextInterval;
        public int m_nAttackInterval;
        public int m_nButchInterval;
        public int m_nSitDownInterval;
        public int m_nPickupInterval;
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
    }
}