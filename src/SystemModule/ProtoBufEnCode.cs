using ProtoBuf;
using System;
using System.IO;

namespace SystemModule
{
    public class ProtoBufDecoder
    {
        public static byte[] Serialize<T>(T model)
        {
            try
            {
                using var ms = new MemoryStream();
                Serializer.Serialize(ms, model);
                byte[] result = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(result, 0, result.Length);
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static T DeSerialize<T>(byte[] msg)
        {
            try
            {
                using var ms = new MemoryStream(msg);
                ms.Position = 0;
                return Serializer.Deserialize<T>(ms);
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }
    }
}