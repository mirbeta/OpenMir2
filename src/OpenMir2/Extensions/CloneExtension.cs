namespace OpenMir2.Extensions
{
    public static class CloneExtension
    {
        /// <summary>
        /// 写入Ascii字符串
        /// </summary>
        /// <returns></returns>
        public static T Clone<T>(this object obj)
        {
           //IFormatter formatter = new BinaryFormatter();
            //using (MemoryStream stream = new MemoryStream())
            //{
                /*formatter.Serialize(stream, obj);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);*/
            //}
            return default(T);
        }
    }
}