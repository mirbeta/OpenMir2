using ProtoBuf;
using ProtoBuf.Meta;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SystemModule
{
    public class ProtoBufDecoder
    {
        public static byte[] Serialize<T>(T model)
        {
            try
            {
                RuntimeTypeModel.Default.SkipZeroLengthPackedArrays = true;
                using var ms = new MemoryStream();
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, model);
                //Serializer.Serialize(ms, model);
                byte[] result = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(result, 0, result.Length);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.StackTrace);
            }
        }

        public static T DeSerialize<T>(byte[] msg)
        {
            try
            {
                using var ms = new MemoryStream(msg);
                ms.Position = 0;
                BinaryFormatter formatter = new BinaryFormatter();
                return (T)formatter.Deserialize(ms);
                //return Serializer.Deserialize<T>(ms);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.StackTrace);
            }
        }
    }
}