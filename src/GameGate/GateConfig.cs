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
      public bool m_fChatFilter;
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
      public int m_nMoveNextSpellCompensate;
      public int m_nMoveNextAttackCompensate;
      public int m_nAttackNextMoveCompensate;
      public int m_nAttackNextSpellCompensate;
      public int m_nSpellNextMoveCompensate;
      public int m_nSpellNextAttackCompensate;
      public int m_nSpaceMoveNextPickupInterval;
      public int m_nPunishBaseInterval;
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

   public enum TPunishMethod
   {
      ptTurnPack,
      ptDropPack,
      ptNullPack,
      ptDelaySend
   }

   public enum TChatFilterMethod
   {
      ctReplaceAll,
      ctReplaceOne,
      ctDropconnect
   }

   public enum TOverSpeedMsgMethod
   {
      ptSysmsg,
      ptMenuOK
   }
}