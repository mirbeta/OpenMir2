using System;
using System.Data;

namespace SystemModule.Extensions
{
    public static class DataReaderExtension
    {
        private static int GetOrdinal(IDataReader dr, string name)
        {
            return dr.GetOrdinal(name);
        }

        public static string GetString(this IDataReader dr, string name)
        {
            var idx = GetOrdinal(dr, name);
            if (idx > -1)
            {
                return Convert.ToString(dr[idx]);
            }
            return string.Empty;
        }

        public static DateTime GetDateTime(this IDataReader dr, string name)
        {
            var idx = GetOrdinal(dr, name);
            if (idx > -1)
            {
                return Convert.ToDateTime(dr[idx]);
            }
            return DateTime.MinValue;
        }

        public static int GetInt32(this IDataReader dr, string name)
        {
            var idx = GetOrdinal(dr, name);
            if (idx > -1)
            {
                if (dr.IsDBNull(idx))
                {
                    return -1;
                }
                return Convert.ToInt32(dr[idx]);
            }
            return -1;
        }
        
        public static long GetInt64(this IDataReader dr, string name)
        {
            var idx = GetOrdinal(dr, name);
            if (idx > -1)
            {
                if (dr.IsDBNull(idx))
                {
                    return -1;
                }
                return Convert.ToInt64(dr[idx]);
            }
            return -1;
        }

        public static byte GetByte(this IDataReader dr, string name)
        {
            var idx = GetOrdinal(dr, name);
            if (idx > -1 && !(dr.IsDBNull(idx)))
            {
                byte result = 0;
                byte.TryParse(Convert.ToString(dr[idx]), out result);
                if (result > byte.MaxValue)
                {
                    return byte.MaxValue;
                }
                return result;
                //return Convert.ToByte(dr[idx]);
            }
            return 0;
        }

        public static sbyte GetSByte(this IDataReader dr, string name)
        {
            var idx = GetOrdinal(dr, name);
            if (idx > -1)
            {
                return Convert.ToSByte(dr[idx]);
            }
            return -1;
        }

        public static ushort GetUInt16(this IDataReader dr, string name)
        {
            var idx = GetOrdinal(dr, name);
            if (idx > -1)
            {
                ushort result = 0;
                ushort.TryParse(Convert.ToString(dr[idx]), out result);
                if (result > ushort.MaxValue)
                {
                    return ushort.MaxValue;
                }
                return result;
            }
            return 0;
        }

        public static short GetInt16(this IDataReader dr, string name)
        {
            var idx = GetOrdinal(dr, name);
            if (idx > -1 && !(dr.IsDBNull(idx)))
            {
                return Convert.ToInt16(dr[idx]);
            }
            return -1;
        }

        public static double GetDouble(this IDataReader dr, string name)
        {
            var idx = GetOrdinal(dr, name);
            if (idx > -1 && !(dr.IsDBNull(idx)))
            {
                return Convert.ToDouble(dr[idx]);
            }
            return -1;
        }

        public static UInt32 GetUInt32(this IDataReader dr, string name)
        {
            var idx = GetOrdinal(dr, name);
            if (idx > -1)
            {
                return Convert.ToUInt32(dr[idx]);
            }
            return 0;
        }
    }
}
