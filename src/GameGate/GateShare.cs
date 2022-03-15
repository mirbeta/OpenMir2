using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using SystemModule;
using SystemModule.Common;

namespace GameGate
{
    public class GateShare
    {
        public static bool ShowLog = true;
        /// <summary>
        ///  网关游戏服务器之间检测超时时间长度
        /// </summary>
        public static long dwCheckServerTimeOutTime = 3 * 60 * 1000;
        public static IList<string> AbuseList = null;
        public static string sReplaceWord = "*";
        public static long dwCheckRecviceTick = 0;
        public static long dwCheckServerTick = 0;
        public static long dwCheckServerTimeMin = 0;
        public static long dwCheckServerTimeMax = 0;
        /// <summary>
        /// 禁止连接IP列表
        /// </summary>
        public static StringList BlockIPList = null;
        /// <summary>
        /// 临时禁止连接IP列表
        /// </summary>
        public static IList<string> TempBlockIPList = null;
        public static int nMaxConnOfIPaddr = 50;
        /// <summary>
        /// 会话超时时间
        /// </summary>
        public static long dwSessionTimeOutTime = 15 * 24 * 60 * 60 * 1000;
        public static ConcurrentDictionary<string, byte> g_ChatCmdFilterList;
        public static Dictionary<string, ClientSession> PunishList;
        public static HWIDFilter HWFilter;

        public static void Initialization()
        {
            AbuseList = new List<string>();
            BlockIPList = new StringList();
            TempBlockIPList = new List<string>();
            PunishList = new Dictionary<string, ClientSession>();
            g_ChatCmdFilterList = new ConcurrentDictionary<string, byte>(StringComparer.OrdinalIgnoreCase);
        }
    }

    public class HardwareHeader : Packets
    {
        public uint dwMagicCode;
        public byte[] xMd5Digest;

        public HardwareHeader(byte[] buffer) : base(buffer)
        {

        }

        protected override void ReadPacket(BinaryReader reader)
        {
            dwMagicCode = reader.ReadUInt32();
            xMd5Digest = reader.ReadBytes(16);
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}