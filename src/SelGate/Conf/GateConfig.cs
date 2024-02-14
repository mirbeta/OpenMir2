
namespace SelGate.Conf
{
    /// <summary>
    /// 网关配置类
    /// </summary>
    public class GateConfig
    {
        public int ShowLogLevel = 0;
        public bool ShowDebug = false;
        public int GateCount = 0;
        public bool CheckNewIDOfIP = false;
        public bool CheckNullSession = false;
        public bool OverSpeedSendBack = false;
        public bool DefenceCCPacket = false;
        public bool KickOverSpeed = false;
        /// <summary>
        /// 新加角色过滤功能
        /// </summary>
        public bool KickOverPacketSize = false;
        /// <summary>
        /// 允许找回人物角色
        /// </summary>
        public bool AllowGetBackChr = false;
        /// <summary>
        /// 允许删除人物角色
        /// </summary>
        public bool AllowDeleteChr = false;
        /// <summary>
        /// 新注册人物角色字符过滤
        /// </summary>
        public bool NewChrNameFilter = false;
        /// <summary>
        /// 禁止使用空格字符
        /// </summary>
        public bool DenyNullChar = false;
        /// <summary>
        /// 禁止使用英文和数字
        /// </summary>
        public bool DenyAnsiChar = false;
        /// <summary>
        /// 禁止使用特殊字符
        /// </summary>
        public bool DenySpecChar = false;
        /// <summary>
        /// 禁止使用希腊字符
        /// </summary>
        public bool DenyHellenicChars = false;
        /// <summary>
        /// 禁止使用俄罗斯字符
        /// </summary>
        public bool DenyRussiaChar = false;
        /// <summary>
        /// 禁止使用以下数字⒈⒉⒊⒋⒌⒍⒎⒏⒐⒑⒒⒓⒔⒕⒖⒗⒘⒙⒚⒛
        /// </summary>
        public bool DenySpecNO1 = false;
        /// <summary>
        /// 禁止使用以下数字⑴⑵⑶⑷⑸⑹⑺⑻⑼⑽⑾⑿⒀⒁⒂⒃⒄⒅⒆⒇
        /// </summary>
        public bool DenySpecNO2 = false;
        /// <summary>
        /// 禁止使用以下数字①②③④⑤⑥⑦⑧⑨⑩
        /// </summary>
        public bool DenySpecNO3 = false;
        /// <summary>
        /// 禁止使用以下数字㈠㈡㈢㈣㈤㈥㈦㈧㈨㈩
        /// </summary>
        public bool DenySpecNO4 = false;
        /// <summary>
        /// 禁止使用全角字符
        /// </summary>
        public bool DenySBCChar = false;
        /// <summary>
        /// 禁止使用假日文
        /// </summary>
        public bool DenykanjiChar = false;
        /// <summary>
        /// 禁止使用制表符
        /// </summary>
        public bool DenyTabsChar = false;
        public int m_nCheckNewIDOfIP = 0;
        public int m_nMaxConnectOfIP = 0;
        public int m_nClientTimeOutTime = 0;
        public int m_nNomClientPacketSize = 0;
        public int m_nMaxClientPacketCount = 0;

        public GateConfig()
        {
            ShowLogLevel = 3;
            GateCount = 1;
            CheckNewIDOfIP = true;
            CheckNullSession = true;
            OverSpeedSendBack = false;
            DefenceCCPacket = false;
            KickOverSpeed = false;
            KickOverPacketSize = true;
            // 新加角色过滤功能 
            AllowGetBackChr = true;
            AllowDeleteChr = true;
            NewChrNameFilter = true;
            DenyNullChar = true;
            DenyAnsiChar = false;
            DenySpecChar = false;
            DenyHellenicChars = true;
            DenyRussiaChar = true;
            DenySpecNO1 = true;
            DenySpecNO2 = true;
            DenySpecNO3 = false;
            DenySpecNO4 = true;
            DenySBCChar = true;
            DenykanjiChar = true;
            DenyTabsChar = false;
            m_nNomClientPacketSize = 400;
            m_nMaxConnectOfIP = 20;
            m_nCheckNewIDOfIP = 5;
            m_nClientTimeOutTime = 60 * 1000;
            m_nMaxClientPacketCount = 2;
            ShowDebug = false;
        }
    }
}