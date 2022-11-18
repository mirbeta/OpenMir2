using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace SystemModule.Extensions
{
    public static class CloneExtension
    {
        /// <summary>
        /// 写入Ascii字符串
        /// </summary>
        /// <returns></returns>
        public static T Clone<T>(this object obj)
        {
            IFormatter formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                /*formatter.Serialize(stream, obj);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);*/
            }
            return default(T);
        }
    }
}