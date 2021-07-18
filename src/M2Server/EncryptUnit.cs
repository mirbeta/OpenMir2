
namespace M2Server
{
    public class EncryptUnit
    {
        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="str">密文</param>
        /// <param name="chinese">是否返回中文</param>
        /// <returns></returns>
        public static unsafe string DeCodeString(string str, bool chinese)
        {
            string result;
            var encBuf = new byte[grobal2.BUFFERSIZE];
            var bSrc = HUtil32.StringToByteAry(str);
            var nLen = EDcode.Decode6BitBuf(bSrc, encBuf, bSrc.Length, grobal2.BUFFERSIZE);
            fixed (byte* pb = encBuf)
            {
                if (chinese)
                {
                    result = HUtil32.SBytePtrToString((sbyte*)pb, nLen);
                }
                else
                {
                    result = HUtil32.SBytePtrToString((sbyte*)pb, 0, nLen);
                }
            }
            return result;
        }
    }
}

