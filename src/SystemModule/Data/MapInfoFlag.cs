namespace SystemModule.Data;

public record struct MapInfoFlag
{
    public bool SafeArea;
    public int RequestLevel;
    public int NeedSetonFlag;
    public int NeedOnOff;
    public bool Music;
    public int MusicId;
    public bool boDarkness;
    public bool DayLight;
    /// <summary>
    /// 行会战争地图
    /// </summary>
    public bool FightZone;
    /// <summary>
    /// 行会战争地图
    /// </summary>
    public bool Fight3Zone;
    public bool boQUIZ;
    public bool boNORECONNECT;
    public string sNoReConnectMap;
    public bool boEXPRATE;
    public int ExpRate;
    public bool boPKWINLEVEL;
    public int nPKWINLEVEL;
    public bool boPKWINEXP;
    public int nPKWINEXP;
    public bool boPKLOSTLEVEL;
    public int nPKLOSTLEVEL;
    public bool boPKLOSTEXP;
    public int nPKLOSTEXP;
    public bool boDECHP;
    public int nDECHPPOINT;
    public int nDECHPTIME;
    public bool boINCHP;
    public int nINCHPPOINT;
    public int nINCHPTIME;
    public bool boDECGAMEGOLD;
    public int nDECGAMEGOLD;
    public int nDECGAMEGOLDTIME;
    public bool boDECGAMEPOINT;
    public int nDECGAMEPOINT;
    public int nDECGAMEPOINTTIME;
    public bool boINCGAMEGOLD;
    public int nINCGAMEGOLD;
    public int nINCGAMEGOLDTIME;
    public bool boINCGAMEPOINT;
    public int nINCGAMEPOINT;
    public int nINCGAMEPOINTTIME;
    public bool RunHuman;
    public bool RunMon;
    public bool boNEEDHOLE;
    public bool NoReCall;
    public bool NoGuildReCall;
    public bool boNODEARRECALL;
    public bool MasterReCall;
    public bool boNORANDOMMOVE;
    public bool boNODRUG;
    public bool Mine;
    public bool boMINE2;
    public bool boNOPOSITIONMOVE;
    public bool NoDropItem;
    public bool NoThrowItem;
    public bool NoHorse;
    public bool boNOCHAT;
    public bool boKILLFUNC;
    public int nKILLFUNCNO;
    public bool boNOHUMNOMON;
}