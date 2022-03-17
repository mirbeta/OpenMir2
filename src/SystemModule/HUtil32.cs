using System;
using System.Text;
using System.Threading;

namespace SystemModule
{
    public class HUtil32
    {
        public const string Backslash = "/";
        
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

        public static int MakeLong(int lowPart, int highPart)
        {
            return lowPart | (short) highPart << 16;
        }

        public static int MakeLong(double lowPart, double highPart)
        {
            return (int) lowPart | ((int) highPart << 16);
        }

        public static int MakeLong(ushort lowPart, int highPart)
        {
            return lowPart | (short) highPart << 16;
        }

        public static int MakeLong(short lowPart, int highPart)
        {
            return (ushort) lowPart | ((short) highPart << 16);
        }

        public static int MakeLong(short lowPart, short highPart)
        {
            return (ushort) lowPart | (highPart << 16);
        }

        public static int MakeLong(short lowPart, ushort highPart)
        {
            return (ushort) lowPart | ((short) highPart << 16);
        }

        //public static ushort MakeWord(byte bLow, byte bHigh)
        //{
        //    return (ushort)(bLow | (bHigh << 8));
        //}

        public static ushort MakeWord(int bLow, int bHigh)
        {
            return (ushort) (bLow | (bHigh << 8));
        }

        public static ushort HiWord(int dword)
        {
            return (ushort) (dword >> 16);
        }

        public static ushort LoWord(int dword)
        {
            return (ushort) dword;
        }

        public static byte HiByte(short W)
        {
            return (byte) (W >> 8);
        }

        public static byte HiByte(int W)
        {
            return (byte) (W >> 8);
        }

        public static byte LoByte(short W)
        {
            return (byte) W;
        }

        public static byte LoByte(int W)
        {
            return (byte) W;
        }

        public static bool IsVarNumber(string Str)
        {
            return (CompareLStr(Str, "HUMAN", 5)) || (CompareLStr(Str, "GUILD", 5)) || (CompareLStr(Str, "GLOBAL", 6));
        }

        public static int Round(object r)
        {
            return (int) Math.Round(Convert.ToDouble(r) + 0.5, 1, MidpointRounding.AwayFromZero);
        }

        public static int Round(double r)
        {
            return (int)Math.Round(Convert.ToDouble(r) + 0.5, 1, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// 判断数值是否在范围之内
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static bool RangeInDefined(int values, int min, int max)
        {
            return Math.Max(min, values) == Math.Min(values, max);
        }

        /// <summary>
        /// 判断数值是否在范围之内
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static bool RangeInDefined(long values, int min, int max)
        {
            return Math.Max(min, values) == Math.Min(values, max);
        }

        public static void EnterCriticalSection(object obj)
        {
           // Monitor.Enter(obj);
        }

        public static void LeaveCriticalSection(object obj)
        {
          //  Monitor.Exit(obj);
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
        /// <param name="StartIndex"></param>
        /// <returns></returns>
        private static unsafe int StringToBytePtr(string str, byte* retby, int StartIndex)
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
                var pb = retby + StartIndex;
                for (var i = 0; i < by.Length; i++)
                    *pb++ = by[i];
            }
            else
            {
                var pb = retby + StartIndex;
                for (var i = 0; i < str.Length; i++) *pb++ = (byte) str[i];
            }

            return nLen;
        }

        public static string CaptureString(string source, ref string rdstr)
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

        public static int Str_ToInt(string Str, int def)
        {
            var result = def;
            if (int.TryParse(Str, out result))
            {
                return result;
            }
            return result;
        }

        public static DateTime Str_ToDate(string Str)
        {
            DateTime result;
            if (Str.Trim() == "")
                result = DateTime.Today;
            else
                result = Convert.ToDateTime(Str);
            return result;
        }

        public static DateTime Str_ToTime(string Str)
        {
            DateTime result;
            if (Str.Trim() == "")
                result = DateTime.Now;
            else
                result = Convert.ToDateTime(Str);
            return result;
        }

        public static string GetValidStr3(string Str, ref string Dest, char Divider)
        {
            var Ary = Str.Split('/'); //返回不包含空的值
            if (Ary.Length > 0)
                Dest = Ary[0]; //目标置为第一个
            else
                Dest = "";
            if (Ary.Length > 1)
                return Ary[1]; //返回第二个
            else
                return "";
        }

        public static string GetValidStr3(string Str, ref string Dest, char[] DividerAry)
        {
            var Div = new char[DividerAry.Length];
            int i;
            for (i = 0; i < DividerAry.Length; i++) Div[i] = DividerAry[i];
            var Ary = Str.Split(Div, 2, StringSplitOptions.RemoveEmptyEntries); //返回不包含空的值
            if (Ary.Length > 0)
                Dest = Ary[0]; //目标置为第一个
            else
                Dest = "";
            if (Ary.Length > 1)
                return Ary[1]; //返回第二个
            else
                return "";
        }

        public static string GetValidStr3(string Str, ref string Dest, string[] DividerAry)
        {
            var Div = new char[DividerAry.Length];
            for (var i = 0; i < DividerAry.Length; i++) Div[i] = DividerAry[i][0];
            var Ary = Str.Split(Div, 2, StringSplitOptions.RemoveEmptyEntries); //返回不包含空的值
            Dest = Ary.Length > 0 ? Ary[0] : "";
            return Ary.Length > 1 ? Ary[1] : "";
        }
        
        public static string GetValidStr3(string Str, ref int Dest, string[] DividerAry)
        {
            var Div = new char[DividerAry.Length];
            for (var i = 0; i < DividerAry.Length; i++) Div[i] = DividerAry[i][0];
            var Ary = Str.Split(Div, 2, StringSplitOptions.RemoveEmptyEntries); //返回不包含空的值
            if (Ary.Length > 0)
            {
                if (!int.TryParse(Ary[0], out Dest))
                {
                    Dest = -1;
                }
            }
            return Ary.Length > 1 ? Ary[1] : "";
        }
        
        public static string GetValidStr3(string Str, ref string Dest, string DividerAry)
        {
            var div = new char[DividerAry.Length];
            for (var i = 0; i < DividerAry.Length; i++) div[i] = DividerAry[i];
            var Ary = Str.Split(div, 2, StringSplitOptions.RemoveEmptyEntries); //返回不包含空的值
            Dest = Ary.Length > 0 ? Ary[0] : "";
            return Ary.Length > 1 ? Ary[1] : "";
        }

        public static string GetValidStrCap(string Str, ref string Dest, string[] Divider)
        {
            string result;
            Str = Str.TrimStart();
            if (Str != "")
            {
                if (Str[0] == '\"')
                    result = CaptureString(Str, ref Dest);
                else
                    result = GetValidStr3(Str, ref Dest, Divider);
            }
            else
            {
                result = "";
                Dest = "";
            }

            return result;
        }

        public static bool IsStringNumber(string str)
        {
            var result = true;
            for (var i = 0; i <= str.Length - 1; i++)
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
        /// <param name="Source">源字符串</param>
        /// <param name="SearchAfter">需要匹配的符号</param>
        /// <param name="ArrestBefore">需要匹配的符号</param>
        /// <param name="ArrestStr">截取之后的结果</param>
        /// <returns></returns>
        public static string ArrestStringEx(string Source, string SearchAfter, string ArrestBefore, ref string ArrestStr)
        {
            if (string.IsNullOrEmpty(Source))
            {
                return string.Empty;
            }
            var result = string.Empty;
            bool GoodData = false;
            ArrestStr = string.Empty;
            try
            {
                int srclen = Source.Length;
                if (srclen >= 2)
                {
                    if (Source[0].ToString() == SearchAfter)
                    {
                        Source = Source.Substring(1, srclen - 1);
                        srclen = Source.Length;
                        GoodData = true;
                    }
                    else
                    {
                        var n = Source.IndexOf(SearchAfter, StringComparison.Ordinal) + 1;
                        if (n > 0)
                        {
                            Source = Source.Substring(n, srclen - n);
                            srclen = Source.Length;
                            GoodData = true;
                        }
                    }
                }
                if (GoodData)
                {
                    var n = Source.IndexOf(ArrestBefore, StringComparison.Ordinal) + 1;
                    if (n > 0)
                    {
                        ArrestStr = Source.Substring(0, n - 1);
                        result = Source.Substring(n, srclen - n);
                    }
                    else
                    {
                        result = SearchAfter + Source;
                    }
                }
                else
                {
                    for (var i = 0; i <= srclen; i++)
                    {
                        if (Source[i - 1].ToString() == SearchAfter)
                        {
                            result = Source.Substring(i - 1, srclen - i + 1);
                            break;
                        }
                    }
                }
            }
            catch
            {
                ArrestStr = string.Empty;
                result = string.Empty;
            }
            return result;
        }

        public static string ArrestStringEx(string Source, char SearchAfter, char ArrestBefore, ref string ArrestStr)
        {
            var result = string.Empty;
            int srclen;
            bool GoodData;
            int n;
            ArrestStr = string.Empty;
            if (Source == "")
            {
                result = "";
                return result;
            }

            try
            {
                srclen = Source.Length;
                GoodData = false;
                if (srclen >= 2)
                {
                    if (Source[0].ToString() == SearchAfter.ToString())
                    {
                        Source = Source.Substring(1, srclen - 1);
                        srclen = Source.Length;
                        GoodData = true;
                    }
                    else
                    {
                        n = Source.IndexOf(SearchAfter) + 1;
                        if (n > 0)
                        {
                            Source = Source.Substring(n, srclen - n);
                            srclen = Source.Length;
                            GoodData = true;
                        }
                    }
                }

                if (GoodData)
                {
                    n = Source.IndexOf(ArrestBefore) + 1;
                    if (n > 0)
                    {
                        ArrestStr = Source.Substring(0, n - 1);
                        result = Source.Substring(n, srclen - n);
                    }
                    else
                    {
                        result = SearchAfter + Source;
                    }
                }
                else
                {
                    for (var i = 0; i <= srclen; i++)
                        if (Source[i - 1].ToString() == SearchAfter.ToString())
                        {
                            result = Source.Substring(i - 1, srclen - i + 1);
                            break;
                        }
                }
            }
            catch
            {
                ArrestStr = "";
                result = "";
            }
            return result;
        }

        public static bool CompareLStr(string src, string targ, int compn)
        {
            var result = false;
            if (compn <= 0) return result;
            if (src.Length < compn) return result;
            if (targ.Length < compn) return result;
            result = true;
            for (var i = 0; i <= compn - 1; i++)
            {
                if (char.ToUpper(src[i]) == char.ToUpper(targ[i])) continue;
                result = false;
                break;
            }
            return result;
        }

        private static bool IsEnglish(char Ch)
        {
            return Ch >= 'A' && Ch <= 'Z' || Ch >= 'a' && Ch <= 'z';
        }

        public static bool IsEngNumeric(char Ch)
        {
            return IsEnglish(Ch) || Ch >= '0' && Ch <= '9';;
        }

        public static bool IsEnglishStr(string sEngStr)
        {
            var result = false;
            for (var i = 0; i <= sEngStr.Length; i++)
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
            for (var i = 0; i <= source.Length - 1; i++)
                if (source[i] == tag)
                    tcount++;
            return tcount;
        }

        public static string BoolToStr(bool boo)
        {
            string result;
            if (boo)
                result = "TRUE";
            else
                result = "FALSE";
            return result;
        }

        public static int _MIN(int n1, int n2)
        {
            int result;
            if (n1 < n2)
                result = n1;
            else
                result = n2;
            return result;
        }

        public static int _MAX(int n1, int n2)
        {
            int result;
            if (n1 > n2)
                result = n1;
            else
                result = n2;
            return result;
        }

        public static string BoolToCStr(bool b)
        {
            string result;
            if (b)
                result = "是";
            else
                result = "否";
            return result;
        }

        public static string BoolToIntStr(bool b)
        {
            string result;
            if (b)
                result = "1";
            else
                result = "0";
            return result;
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

        public static int GetDayCount(DateTime MaxDate, DateTime MinDate)
        {
            if (MaxDate < MinDate) return 0;
            int YearMax = MaxDate.Year;
            int MonthMax = MaxDate.Month;
            int DayMax = MaxDate.Day;
            int YearMin = MinDate.Year;
            int MonthMin = MinDate.Month;
            int DayMin = MinDate.Day;
            YearMax -= YearMin;
            YearMin = 0;
            return YearMax * 12 * 30 + MonthMax * 30 + DayMax - (YearMin * 12 * 30 + MonthMin * 30 + DayMin);
        }

        /// <summary>
        /// SByte转string
        /// </summary>
        /// <param name="by"></param>
        /// <param name="StartIndex"></param>
        /// <param name="Len"></param>
        /// <returns></returns>
        public static unsafe string SBytePtrToString(sbyte* by, int StartIndex, int Len)
        {
            try
            {
                return BytePtrToString((byte*) by, StartIndex, Len);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static unsafe string BytePtrToString(byte* by, int StartIndex, int Len)
        {
            var ret = new string('\0', Len);
            var sb = new StringBuilder(ret);

            by += StartIndex;
            for (var i = 0; i < Len; i++) sb[i] = (char) *@by++;

            return sb.ToString();
        }

        /// <summary>
        /// 字符串转Byte字节数组
        /// </summary>
        /// <param name="str"></param>
        /// <param name="strLength"></param>
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

        public static bool CompareBackLStr(string Src, string targ, int compn)
        {
            var result = false;
            if (compn <= 0)
            {
                return result;
            }
            if (Src.Length < compn)
            {
                return result;
            }
            if (targ.Length < compn)
            {
                return result;
            }
            var slen = Src.Length;
            var tLen = targ.Length;
            result = true;
            for (var i = 0; i < compn; i++)
            {
                if (char.ToUpper(Src[slen - (i + 1)]) != char.ToUpper(targ[tLen - (i + 1)]))
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