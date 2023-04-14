
namespace SelGate.Conf
{
    /// <summary>
    /// 网关配置类
    /// </summary>
    public class GateConfig
    {
        public int ShowLogLevel = 0;
        public bool ShowDebug = false;
        public int m_nGateCount = 0;
        public bool m_fCheckNewIDOfIP = false;
        public bool m_fCheckNullSession = false;
        public bool m_fOverSpeedSendBack = false;
        public bool m_fDefenceCCPacket = false;
        public bool m_fKickOverSpeed = false;
        public bool m_fKickOverPacketSize = false;
        // 新加角色过滤功能 
        public bool m_fAllowGetBackChr = false;
        // 允许找回人物角色
        public bool m_fAllowDeleteChr = false;
        // 允许删除人物角色
        public bool m_fNewChrNameFilter = false;
        // 新注册人物角色字符过滤
        public bool m_fDenyNullChar = false;
        // 禁止使用空格字符
        public bool m_fDenyAnsiChar = false;
        // 禁止使用英文和数字
        public bool m_fDenySpecChar = false;
        // 禁止使用特殊字符
        public bool m_fDenyHellenicChars = false;
        // 禁止使用希腊字符
        public bool m_fDenyRussiaChar = false;
        // 禁止使用俄罗斯字符
        public bool m_fDenySpecNO1 = false;
        // 禁止使用以下数字⒈⒉⒊⒋⒌⒍⒎⒏⒐⒑⒒⒓⒔⒕⒖⒗⒘⒙⒚⒛
        public bool m_fDenySpecNO2 = false;
        // 禁止使用以下数字⑴⑵⑶⑷⑸⑹⑺⑻⑼⑽⑾⑿⒀⒁⒂⒃⒄⒅⒆⒇
        public bool m_fDenySpecNO3 = false;
        // 禁止使用以下数字①②③④⑤⑥⑦⑧⑨⑩
        public bool m_fDenySpecNO4 = false;
        // 禁止使用以下数字㈠㈡㈢㈣㈤㈥㈦㈧㈨㈩
        public bool m_fDenySBCChar = false;
        // 禁止使用全角字符
        public bool m_fDenykanjiChar = false;
        // 禁止使用假日文
        public bool m_fDenyTabsChar = false;
        // 禁止使用制表符
        public int m_nCheckNewIDOfIP = 0;
        public int m_nMaxConnectOfIP = 0;
        public int m_nClientTimeOutTime = 0;
        public int m_nNomClientPacketSize = 0;
        public int m_nMaxClientPacketCount = 0;

        public GateConfig()
        {
            ShowLogLevel = 3;
            m_nGateCount = 1;
            m_fCheckNewIDOfIP = true;
            m_fCheckNullSession = true;
            m_fOverSpeedSendBack = false;
            m_fDefenceCCPacket = false;
            m_fKickOverSpeed = false;
            m_fKickOverPacketSize = true;
            // 新加角色过滤功能 
            m_fAllowGetBackChr = true;
            m_fAllowDeleteChr = true;
            m_fNewChrNameFilter = true;
            m_fDenyNullChar = true;
            m_fDenyAnsiChar = false;
            m_fDenySpecChar = false;
            m_fDenyHellenicChars = true;
            m_fDenyRussiaChar = true;
            m_fDenySpecNO1 = true;
            m_fDenySpecNO2 = true;
            m_fDenySpecNO3 = false;
            m_fDenySpecNO4 = true;
            m_fDenySBCChar = true;
            m_fDenykanjiChar = true;
            m_fDenyTabsChar = false;
            m_nNomClientPacketSize = 400;
            m_nMaxConnectOfIP = 20;
            m_nCheckNewIDOfIP = 5;
            m_nClientTimeOutTime = 60 * 1000;
            m_nMaxClientPacketCount = 2;
            ShowDebug = false;
        }
    }
}