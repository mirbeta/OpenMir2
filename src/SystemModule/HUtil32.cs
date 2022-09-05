using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using SystemModule.Packet.ClientPackets;

namespace SystemModule
{
    public class HUtil32
    {
        public const string Backslash = "/";
        public static readonly string[] Separator =  { " ", ",", "\t" };
        public static readonly TUserItem DelfautItem = new TUserItem();
        public static readonly TMagicRcd DetailtMagicRcd = new TMagicRcd();

        /// <summary>
        /// 根据GUID获取唯一数字序列
        /// </summary>
        public static int Sequence()
        {
            var bytes = Guid.NewGuid().ToByteArray();
            var sequence = BitConverter.ToInt32(bytes, 0);
            while (sequence < 0)
            {
                bytes = Guid.NewGuid().ToByteArray();
                sequence = BitConverter.ToInt32(bytes, 0);
                if (sequence > 0) break;
            }
            return sequence;
        }

        public static int GetTickCount()
        {
            return Environment.TickCount;
        }

        public static ushort MakeLong(int lowPart, int highPart)
        {
            return (ushort)(lowPart | highPart << 16);
        }

        public static int MakeLong(double lowPart, double highPart)
        {
            return (int)lowPart | ((int)highPart << 16);
        }
        
        public static ushort MakeLong(ushort lowPart, int highPart)
        {
            return (ushort)(lowPart | highPart << 16);
        }

        public static int MakeLong(short lowPart, int highPart)
        {
            return (ushort)lowPart | ((short)highPart << 16);
        }

        public static int MakeLong(short lowPart, short highPart)
        {
            return (ushort)lowPart | (highPart << 16);
        }

        public static int MakeLong(short lowPart, ushort highPart)
        {
            return (ushort)lowPart | ((short)highPart << 16);
        }

        public static ushort MakeWord(int bLow, int bHigh)
        {
            return (ushort)(bLow | (bHigh << 8));
        }

        public static ushort HiWord(ushort dword)
        {
            return (ushort)(dword >> 16);
        }
        
        public static ushort HiWord(int dword)
        {
            return (ushort)(dword >> 16);
        }

        public static ushort LoWord(int dword)
        {
            return (ushort)dword;
        }

        public static byte HiByte(short w)
        {
            return (byte)(w >> 8);
        }

        public static byte HiByte(int w)
        {
            return (byte)(w >> 8);
        }

        public static byte LoByte(short w)
        {
            return (byte)w;
        }

        public static byte LoByte(int w)
        {
            return (byte)w;
        }

        public static bool IsVarNumber(string str)
        {
            return (CompareLStr(str, "HUMAN", 5)) || (CompareLStr(str, "GUILD", 5)) || (CompareLStr(str, "GLOBAL", 6));
        }

        public static int Round(object r)
        {
            return (int)Math.Round(Convert.ToDouble(r) + 0.5, 1, MidpointRounding.AwayFromZero);
        }

        public static int Round(ushort r)
        {
            return (int)Math.Round(Convert.ToDouble(r) + 0.5, 1, MidpointRounding.AwayFromZero);
        }
        
        public static int Round(int r)
        {
            return (int)Math.Round(Convert.ToDouble(r) + 0.5, 1, MidpointRounding.AwayFromZero);
        }
        
        public static int Round(double r)
        {
            return (int)Math.Round(Convert.ToDouble(r) + 0.5, 1, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// 判断数值是否在范围之内
        /// </summary>
        /// <returns></returns>
        public static bool RangeInDefined(byte values, int min, int max)
        {
            return Math.Max(min, values) == Math.Min(values, max);
        }
        
        /// <summary>
        /// 判断数值是否在范围之内
        /// </summary>
        /// <returns></returns>
        public static bool RangeInDefined(int values, int min, int max)
        {
            return Math.Max(min, values) == Math.Min(values, max);
        }

        /// <summary>
        /// 判断数值是否在范围之内
        /// </summary>
        /// <returns></returns>
        public static bool RangeInDefined(long values, int min, int max)
        {
            return Math.Max(min, values) == Math.Min(values, max);
        }

        public static void EnterCriticalSections(object obj)
        {
            Monitor.Enter(obj);
        }

        public static void LeaveCriticalSections(object obj)
        {
            Monitor.Exit(obj);
        }

        public static void EnterCriticalSection(object obj)
        {
            Monitor.Enter(obj);
        }

        public static void LeaveCriticalSection(object obj)
        {
            Monitor.Exit(obj);
        }

        public static string GetString(byte[] bytes, int index, int count)
        {
            return Encoding.GetEncoding("gb2312").GetString(bytes, index, count);
        }

        public static DateTime DoubleToDateTime(double xd)
        {
            return (new DateTime(1899, 12, 30)).AddDays(xd);
        }

        public static double DateTimeToDouble(DateTime dt)
        {
            TimeSpan ts = dt - new DateTime(1899, 12, 30);
            return ts.TotalDays;
        }

        public static string StrPas(byte[] buff)
        {
            var nLen = buff.Length;
            var ret = new string('\0', nLen);
            var sb = new StringBuilder(ret);
            for (var i = 0; i < nLen; i++)
            {
                sb[i] = (char)buff[i];
            }
            return sb.ToString();
        }

        /// <summary>
        /// 字符串转丹字节
        /// 思路：对于含有高字节不为0的，说明字符串包含汉字，用Encoding.Default.GetBytes
        /// 这样会导致服务端string结构发生变化，但是不影响网络传输的数据
        /// 对于高字节为0的，仅处理低字节
        /// retby 为 null 表示仅计算长度并返回
        /// </summary>
        /// <param name="str"></param>
        /// <param name="retby"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        private static unsafe int StringToBytePtr(string str, byte* retby, int startIndex)
        {
            var bDecode = false;
            if (string.IsNullOrEmpty(str)) return 0;
            for (var i = 0; i < str.Length; i++)
                if (str[i] >> 8 != 0)
                {
                    bDecode = true;
                    break;
                }

            var nLen = 0;
            if (bDecode)
                nLen = Encoding.GetEncoding("gb2312").GetByteCount(str);
            else
                nLen = str.Length;
            if (retby == null)
                return nLen;

            if (bDecode)
            {
                var by = Encoding.GetEncoding("gb2312").GetBytes(str);
                var pb = retby + startIndex;
                for (var i = 0; i < by.Length; i++)
                    *pb++ = by[i];
            }
            else
            {
                var pb = retby + startIndex;
                for (var i = 0; i < str.Length; i++) *pb++ = (byte)str[i];
            }

            return nLen;
        }

        protected static string CaptureString(string source, ref string rdstr)
        {
            string result;
            int st;
            int et;
            int c;
            int len;
            int i;
            if (source == "")
            {
                rdstr = "";
                result = "";
                return result;
            }
            c = 1;
            len = source.Length;
            while (source[c] == ' ')
                if (c < len)
                    c++;
                else
                    break;
            if (source[c] == '\"' && c < len)
            {
                st = c + 1;
                et = len;
                for (i = c + 1; i <= len; i++)
                    if (source[i] == '\"')
                    {
                        et = i - 1;
                        break;
                    }
            }
            else
            {
                st = c;
                et = len;
                for (i = c; i <= len; i++)
                    if (source[i] == ' ')
                    {
                        et = i - 1;
                        break;
                    }
            }

            rdstr = source.Substring(st - 1, et - st + 1);
            if (len >= et + 2)
                result = source.Substring(et + 2 - 1, len - (et + 1));
            else
                result = "";
            return result;
        }

        public static int Str_ToInt(string str, int def)
        {
            var result = def;
            if (int.TryParse(str, out result))
            {
                return result;
            }
            return result;
        }

        public static DateTime Str_ToDate(string str)
        {
            DateTime result;
            if (str.Trim() == "")
                result = DateTime.Today;
            else
                result = Convert.ToDateTime(str);
            return result;
        }

        public static DateTime Str_ToTime(string str)
        {
            DateTime result;
            if (str.Trim() == "")
                result = DateTime.Now;
            else
                result = Convert.ToDateTime(str);
            return result;
        }

        public static string GetValidStr3(string str, ref string dest, char divider)
        {
            var ary = str.Split('/'); //返回不包含空的值
            dest = ary.Length > 0 ? ary[0] : ""; //目标置为第一个
            return ary.Length > 1 ? ary[1] : ""; //返回第二个
        }

        public static string GetValidStr3(string str, ref string dest, char[] dividerAry)
        {
            var div = new char[dividerAry.Length];
            for (var i = 0; i < dividerAry.Length; i++) div[i] = dividerAry[i];
            var ary = str.Split(div, 2, StringSplitOptions.RemoveEmptyEntries); //返回不包含空的值
            dest = ary.Length > 0 ? ary[0] : ""; //目标置为第一个
            return ary.Length > 1 ? ary[1] : ""; //返回第二个
        }

        public static string GetValidStr3(string str, ref string dest, string[] dividerAry)
        {
            var div = new char[dividerAry.Length];
            for (var i = 0; i < dividerAry.Length; i++) div[i] = dividerAry[i][0];
            var ary = str.Split(div, 2, StringSplitOptions.RemoveEmptyEntries); //返回不包含空的值
            dest = ary.Length > 0 ? ary[0] : "";
            return ary.Length > 1 ? ary[1] : "";
        }

        public static string GetValidStr3(string str, ref int dest, string[] dividerAry)
        {
            var div = new char[dividerAry.Length];
            for (var i = 0; i < dividerAry.Length; i++) div[i] = dividerAry[i][0];
            var ary = str.Split(div, 2, StringSplitOptions.RemoveEmptyEntries); //返回不包含空的值
            if (ary.Length <= 0) return ary.Length > 1 ? ary[1] : "";
            if (!int.TryParse(ary[0], out dest))
            {
                dest = -1;
            }
            return ary.Length > 1 ? ary[1] : "";
        }

        public static string GetValidStr3(string str, ref string dest, string dividerAry)
        {
            var div = new char[dividerAry.Length];
            for (var i = 0; i < dividerAry.Length; i++) div[i] = dividerAry[i];
            var ary = str.Split(div, 2, StringSplitOptions.RemoveEmptyEntries); //返回不包含空的值
            dest = ary.Length > 0 ? ary[0] : "";
            return ary.Length > 1 ? ary[1] : "";
        }

        public static string GetValidStrCap(string str, ref string dest, string[] divider)
        {
            string result;
            str = str.TrimStart();
            if (str != "")
            {
                if (str[0] == '\"')
                    result = CaptureString(str, ref dest);
                else
                    result = GetValidStr3(str, ref dest, divider);
            }
            else
            {
                result = "";
                dest = "";
            }
            return result;
        }

        public static bool IsStringNumber(string str)
        {
            var result = true;
            for (var i = 0; i < str.Length - 1; i++)
            {
                if ((byte)str[i] < (byte)'0' || (byte)str[i] > (byte)'9')
                {
                    result = false;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// 截取字符串 例 ArrestStringEx('[1234]','[',']',str)    str=1234
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="searchAfter">需要匹配的符号</param>
        /// <param name="arrestBefore">需要匹配的符号</param>
        /// <param name="arrestStr">截取之后的结果</param>
        /// <returns></returns>
        public static string ArrestStringEx(string source, string searchAfter, string arrestBefore, ref string arrestStr)
        {
            if (string.IsNullOrEmpty(source))
            {
                return string.Empty;
            }
            var sourceSpan = source.AsSpan();
            var spanLen = sourceSpan.Length;
            var result = string.Empty;
            var findData = false;
            try
            {
                if (spanLen >= 2)
                {
                    if (source.StartsWith(searchAfter))
                    {
                        sourceSpan = sourceSpan[1..spanLen];
                        findData = true;
                    }
                    else
                    {
                        var n = sourceSpan.IndexOf(searchAfter, StringComparison.Ordinal);
                        if (n > 0)
                        {
                            sourceSpan = sourceSpan.Slice(n + 1, spanLen - n - 1);
                            findData = true;
                        }
                    }
                }
                if (findData)
                {
                    var n = sourceSpan.IndexOf(arrestBefore, StringComparison.Ordinal) + 1;
                    if (n > 0)
                    {
                        arrestStr = sourceSpan[..(n - 1)].ToString();
                        result = sourceSpan[(arrestStr.Length + 1)..].ToString();
                    }
                    else
                    {
                        result = searchAfter + sourceSpan.ToString();
                    }
                }
                else
                {
                    for (var i = 0; i < spanLen; i++)
                    {
                        if (sourceSpan[i - 1].ToString() == searchAfter)
                        {
                            result = sourceSpan.Slice(i - 1, spanLen - i + 1).ToString();
                            break;
                        }
                    }
                }
            }
            catch
            {
                arrestStr = string.Empty;
                result = string.Empty;
            }
            return result;
        }

        public static bool CompareLStr(string src, string targ, int compn)
        {
            if (compn <= 0) return false;
            if (src.Length < compn) return false;
            if (targ.Length < compn) return false;
            for (var i = 0; i < compn; i++)
            {
                if (char.ToUpper(src[i]) == char.ToUpper(targ[i])) continue;
                return false;
            }
            return true;
        }

        private static bool IsEnglish(char ch)
        {
            return ch >= 'A' && ch <= 'Z' || ch >= 'a' && ch <= 'z';
        }

        public static bool IsEngNumeric(char ch)
        {
            return IsEnglish(ch) || ch >= '0' && ch <= '9'; ;
        }

        public static bool IsEnglishStr(string sEngStr)
        {
            var result = false;
            for (var i = 0; i < sEngStr.Length; i++)
            {
                result = IsEnglish(sEngStr[i]);
                if (result) break;
            }
            return result;
        }

        public static string ReplaceChar(string src, char srcchr, char repchr)
        {
            if (src != "")
            {
                int len = src.Length;
                var sb = new StringBuilder();
                for (var i = 0; i < len; i++)
                    if (src[i] == srcchr)
                        sb.Append(repchr);
            }
            return src;
        }

        public static int TagCount(string source, char tag)
        {
            var tcount = 0;
            for (var i = 0; i < source.Length; i++)
                if (source[i] == tag)
                    tcount++;
            return tcount;
        }

        public static string BoolToStr(bool boo)
        {
            return boo ? "TRUE" : "FALSE";
        }

        public static int _MIN(int n1, int n2)
        {
            return n1 < n2 ? n1 : n2;
        }

        public static int _MAX(int n1, int n2)
        {
            return n1 > n2 ? n1 : n2;
        }

        public static string BoolToCStr(bool b)
        {
            return b ? "是" : "否"; ;
        }

        public static string BoolToIntStr(bool b)
        {
            return b ? "1" : "0";
        }

        public static byte[] GetBytes(string str)
        {
            return Encoding.GetEncoding("gb2312").GetBytes(str);
        }

        public static byte[] GetBytes(int str)
        {
            return Encoding.GetEncoding("gb2312").GetBytes(str.ToString());
        }

        public static int GetByteCount(char strSrc)
        {
            return Encoding.GetEncoding("gb2312").GetByteCount(strSrc.ToString());
        }

        public static int GetDayCount(DateTime maxDate, DateTime minDate)
        {
            if (maxDate < minDate) return 0;
            int yearMax = maxDate.Year;
            int monthMax = maxDate.Month;
            int dayMax = maxDate.Day;
            int yearMin = minDate.Year;
            int monthMin = minDate.Month;
            int dayMin = minDate.Day;
            yearMax -= yearMin;
            yearMin = 0;
            return yearMax * 12 * 30 + monthMax * 30 + dayMax - (yearMin * 12 * 30 + monthMin * 30 + dayMin);
        }

        /// <summary>
        /// SByte转string
        /// </summary>
        /// <param name="by"></param>
        /// <param name="startIndex"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static unsafe string SBytePtrToString(sbyte* by, int startIndex, int len)
        {
            try
            {
                return BytePtrToString((byte*)by, startIndex, len);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static unsafe string BytePtrToString(byte* by, int startIndex, int len)
        {
            var ret = new string('\0', len);
            var sb = new StringBuilder(ret);
            by += startIndex;
            for (var i = 0; i < len; i++) sb[i] = (char)*@by++;
            return sb.ToString();
        }

        /// <summary>
        /// 字符串转Byte字节数组
        /// </summary>
        /// <returns></returns>
        public static byte[] StringToByte(string str)
        {
            return Encoding.GetEncoding("GB2312").GetBytes(str);
        }
        
        /// <summary>
        /// 字符串转Byte字节数组
        /// </summary>
        /// <returns></returns>
        public static unsafe byte[] StringToByteAry(string str, out int strLength)
        {
            strLength = StringToBytePtr(str, null, 0);
            var ret = new byte[strLength + 1];
            fixed (byte* pb = ret)
            {
                StringToBytePtr(str, pb, 1);
            }
            return ret;
        }

        public static bool CompareBackLStr(string src, string targ, int compn)
        {
            var result = false;
            if (compn <= 0)
            {
                return result;
            }
            if (src.Length < compn)
            {
                return result;
            }
            if (targ.Length < compn)
            {
                return result;
            }
            var slen = src.Length;
            var tLen = targ.Length;
            result = true;
            for (var i = 0; i < compn; i++)
            {
                if (char.ToUpper(src[slen - (i + 1)]) != char.ToUpper(targ[tLen - (i + 1)]))
                {
                    result = false;
                    break;
                }
            }
            return result;
        }

        public static long IpToInt(string ip)
        {
            if (string.IsNullOrEmpty(ip))
            {
                return -1;
            }
            char[] separator = new[] { '.' };
            string[] items = ip.Split(separator);
            return long.Parse(items[0]) << 24
                   | long.Parse(items[1]) << 16
                   | long.Parse(items[2]) << 8
                   | long.Parse(items[3]);
        }

    }
}