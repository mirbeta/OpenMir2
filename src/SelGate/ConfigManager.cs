using System;
using System.IO;
using SystemModule.Common;

namespace SelGate
{
    public struct TGameGateList
    {
        public string sServerAdress;
        public int nServerPort;
        public int nGatePort;
    }

    public class ConfigManager : IniFile
    {
        public string m_szTitle = String.Empty;
        public int m_nShowLogLevel = 0;
        public int m_nGateCount = 0;
        public TGameGateList[] m_xGameGateList;
        public bool m_fCheckNewIDOfIP = false;
        public bool m_fCheckNullSession = false;
        public bool m_fOverSpeedSendBack = false;
        public bool m_fDefenceCCPacket = false;
        public bool m_fKickOverSpeed = false;
        public bool m_fKickOverPacketSize = false;
        // 新加角色过滤功能 2018-09-06
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
        public TBlockIPMethod m_tBlockIPMethod;

        public ConfigManager(string szFileName) : base(szFileName)
        {
            m_szTitle = "角色网关";
            m_nShowLogLevel = 3;
            m_nGateCount = 1;
            for (var i = m_xGameGateList.GetLowerBound(0); i <= m_xGameGateList.GetUpperBound(0); i++)
            {
                m_xGameGateList[i].sServerAdress = "127.0.0.1";
                m_xGameGateList[i].nServerPort = 5100;
                m_xGameGateList[i].nGatePort = 7100 + i - 1;
            }
            m_fCheckNewIDOfIP = true;
            m_fCheckNullSession = true;
            m_fOverSpeedSendBack = false;
            m_fDefenceCCPacket = false;
            m_fKickOverSpeed = false;
            m_fKickOverPacketSize = true;
            // 新加角色过滤功能 2018-09-06
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
        }

        public string ReadString(string Section, string Ident, string __Default)
        {
            string result;
            string szLoadStr;
            result = __Default;
            szLoadStr = base.ReadString(Section, Ident, "");
            if (szLoadStr == "")
            {
                WriteString(Section, Ident, __Default);
            }
            else
            {
                result = szLoadStr;
            }
            return result;
        }

        public int ReadInteger(string Section, string Ident, int __Default)
        {
            int result;
            int szLoadInt;
            result = __Default;
            szLoadInt = ReadInteger(Section, Ident, -1);
            if (szLoadInt < 0)
            {
                WriteInteger(Section, Ident, __Default);
            }
            else
            {
                result = szLoadInt;
            }
            return result;
        }

        public bool ReadBool(string Section, string Ident, bool __Default)
        {
            bool result;
            int szLoadInt;
            result = __Default;
            szLoadInt = ReadInteger(Section, Ident, -1);
            if (szLoadInt < 0)
            {
                WriteBool(Section, Ident, __Default);
            }
            else
            {
                result = szLoadInt != 0;
            }
            return result;
        }

        public double ReadFloat(string Section, string Ident, double __Default)
        {
            double result;
            double szLoadDW;
            result = __Default;
            if (ReadFloat(Section, Ident, 0) < 0.10)
            {
                WriteInteger(Section, Ident, __Default);
            }
            else
            {
                result = ReadFloat(Section, Ident, __Default);
            }
            return result;
        }

        public void LoadConfig()
        {
            int i;
            m_szTitle = ReadString("Strings", "Title", m_szTitle);
            m_nShowLogLevel = ReadInteger("Integer", "ShowLogLevel", m_nShowLogLevel);
            m_nClientTimeOutTime = ReadInteger("Integer", "ClientTimeOutTime", m_nClientTimeOutTime);
            if (m_nClientTimeOutTime < 10 * 1000)
            {
                m_nClientTimeOutTime = 10 * 1000;
                WriteInteger("Integer", "ClientTimeOutTime", m_nClientTimeOutTime);
            }
            m_nMaxConnectOfIP = ReadInteger("Integer", "MaxConnectOfIP", m_nMaxConnectOfIP);
            m_nCheckNewIDOfIP = ReadInteger("Integer", "CheckNewIDOfIP", m_nCheckNewIDOfIP);
            m_nClientTimeOutTime = ReadInteger("Integer", "ClientTimeOutTime", m_nClientTimeOutTime);
            m_nNomClientPacketSize = ReadInteger("Integer", "NomClientPacketSize", m_nNomClientPacketSize);
            m_nMaxClientPacketCount = ReadInteger("Integer", "MaxClientPacketCount", m_nMaxClientPacketCount);
            m_fCheckNewIDOfIP = ReadBool("Switch", "CheckNewIDOfIP", m_fCheckNewIDOfIP);
            m_fCheckNullSession = ReadBool("Switch", "CheckNullSession", m_fCheckNullSession);
            m_fOverSpeedSendBack = ReadBool("Switch", "OverSpeedSendBack", m_fOverSpeedSendBack);
            m_fDefenceCCPacket = ReadBool("Switch", "DefenceCCPacket", m_fDefenceCCPacket);
            m_fKickOverSpeed = ReadBool("Switch", "KickOverSpeed", m_fKickOverSpeed);
            m_fKickOverPacketSize = ReadBool("Switch", "KickOverPacketSize", m_fKickOverPacketSize);
            // 新加角色过滤功能 2018-09-06
            m_fAllowGetBackChr = ReadBool("Switch", "AllowGetBackChr", m_fAllowGetBackChr);
            m_fAllowDeleteChr = ReadBool("Switch", "AllowDeleteChr", m_fAllowDeleteChr);
            m_fNewChrNameFilter = ReadBool("Switch", "NewChrNameFilter", m_fNewChrNameFilter);
            m_fDenyNullChar = ReadBool("Switch", "DenyNullChar", m_fDenyNullChar);
            m_fDenyAnsiChar = ReadBool("Switch", "DenyAnsiChar", m_fDenyAnsiChar);
            m_fDenySpecChar = ReadBool("Switch", "DenySpecChar", m_fDenySpecChar);
            m_fDenyHellenicChars = ReadBool("Switch", "DenyHellenicChars", m_fDenyHellenicChars);
            m_fDenyRussiaChar = ReadBool("Switch", "DenyRussiaChar", m_fDenyRussiaChar);
            m_fDenySpecNO1 = ReadBool("Switch", "DenySpecNO1", m_fDenySpecNO1);
            m_fDenySpecNO2 = ReadBool("Switch", "DenySpecNO2", m_fDenySpecNO2);
            m_fDenySpecNO3 = ReadBool("Switch", "DenySpecNO3", m_fDenySpecNO3);
            m_fDenySpecNO4 = ReadBool("Switch", "DenySpecNO4", m_fDenySpecNO4);
            m_fDenySBCChar = ReadBool("Switch", "DenySBCChar", m_fDenySBCChar);
            m_fDenykanjiChar = ReadBool("Switch", "DenykanjiChar", m_fDenykanjiChar);
            m_fDenyTabsChar = ReadBool("Switch", "DenyTabsChar", m_fDenyTabsChar);
            m_tBlockIPMethod = ((TBlockIPMethod)(ReadInteger("Method", "BlockIPMethod", (int)m_tBlockIPMethod)));
            m_nGateCount = ReadInteger("GameGate", "Count", m_nGateCount);
            for (i = 1; i <= m_nGateCount; i++)
            {
                m_xGameGateList[i].sServerAdress = ReadString("GameGate", "ServerAddr" + (i).ToString(), m_xGameGateList[i].sServerAdress);
                m_xGameGateList[i].nServerPort = ReadInteger("GameGate", "ServerPort" + (i).ToString(), m_xGameGateList[i].nServerPort);
                m_xGameGateList[i].nGatePort = ReadInteger("GameGate", "GatePort" + (i).ToString(), m_xGameGateList[i].nGatePort);
            }
        }

        public void SaveConfig(int nType)
        {
            int i;
            switch (nType)
            {
                case 0:
                    WriteString("Strings", "Title", m_szTitle);
                    WriteInteger("Integer", "ShowLogLevel", m_nShowLogLevel);
                    WriteInteger("GameGate", "Count", m_nGateCount);
                    for (i = 1; i <= m_nGateCount; i++)
                    {
                        WriteString("GameGate", "ServerAddr" + (i).ToString(), m_xGameGateList[i].sServerAdress);
                        WriteInteger("GameGate", "ServerPort" + (i).ToString(), m_xGameGateList[i].nServerPort);
                        WriteInteger("GameGate", "GatePort" + (i).ToString(), m_xGameGateList[i].nGatePort);
                    }
                    break;
                case 1:
                    WriteInteger("Integer", "MaxConnectOfIP", m_nMaxConnectOfIP);
                    WriteInteger("Integer", "CheckNewIDOfIP", m_nCheckNewIDOfIP);
                    WriteInteger("Integer", "ClientTimeOutTime", m_nClientTimeOutTime);
                    WriteInteger("Integer", "NomClientPacketSize", m_nNomClientPacketSize);
                    WriteInteger("Integer", "MaxClientPacketCount", m_nMaxClientPacketCount);
                    WriteBool("Switch", "CheckNewIDOfIP", m_fCheckNewIDOfIP);
                    WriteBool("Switch", "CheckNullSession", m_fCheckNullSession);
                    WriteBool("Switch", "OverSpeedSendBack", m_fOverSpeedSendBack);
                    WriteBool("Switch", "DefenceCCPacket", m_fDefenceCCPacket);
                    WriteBool("Switch", "KickOverSpeed", m_fKickOverSpeed);
                    WriteBool("Switch", "KickOverPacketSize", m_fKickOverPacketSize);
                    // 新加角色过滤功能 2018-09-06
                    WriteBool("Switch", "AllowGetBackChr", m_fAllowGetBackChr);
                    WriteBool("Switch", "AllowDeleteChr", m_fAllowDeleteChr);
                    WriteBool("Switch", "NewChrNameFilter", m_fNewChrNameFilter);
                    WriteBool("Switch", "DenyNullChar", m_fDenyNullChar);
                    WriteBool("Switch", "DenyAnsiChar", m_fDenyAnsiChar);
                    WriteBool("Switch", "DenySpecChar", m_fDenySpecChar);
                    WriteBool("Switch", "DenyHellenicChars", m_fDenyHellenicChars);
                    WriteBool("Switch", "DenyRussiaChar", m_fDenyRussiaChar);
                    WriteBool("Switch", "DenySpecNO1", m_fDenySpecNO1);
                    WriteBool("Switch", "DenySpecNO2", m_fDenySpecNO2);
                    WriteBool("Switch", "DenySpecNO3", m_fDenySpecNO3);
                    WriteBool("Switch", "DenySpecNO4", m_fDenySpecNO4);
                    WriteBool("Switch", "DenySBCChar", m_fDenySBCChar);
                    WriteBool("Switch", "DenykanjiChar", m_fDenykanjiChar);
                    WriteBool("Switch", "DenyTabsChar", m_fDenyTabsChar);
                    WriteInteger("Method", "BlockIPMethod", (int)m_tBlockIPMethod);
                    break;
                case 2:
                    break;
            }
        }

    }
}