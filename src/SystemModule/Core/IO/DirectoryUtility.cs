using System.IO;
using System.Linq;

namespace SystemModule.Core.IO
{
    /// <summary>
    /// DirectoryUtility
    /// </summary>
    [IntelligentCoder.AsyncMethodPoster(Flags = IntelligentCoder.MemberFlags.Public)]
    public static partial class DirectoryUtility
    {
        /// <summary>
        /// 复制文件夹及文件
        /// </summary>
        /// <param name="sourceFolder">原文件路径</param>
        /// <param name="destFolder">目标文件路径</param>
        /// <returns></returns>
        public static void CopyDirectory(string sourceFolder, string destFolder)
        {
            //如果目标路径不存在,则创建目标路径
            if (!Directory.Exists(destFolder))
            {
                Directory.CreateDirectory(destFolder);
            }
            //得到原文件根目录下的所有文件
            string[] files = Directory.GetFiles(sourceFolder);
            foreach (string file in files)
            {
                string name = Path.GetFileName(file);
                string dest = Path.Combine(destFolder, name);
                File.Copy(file, dest);//复制文件
            }
            //得到原文件根目录下的所有文件夹
            string[] folders = Directory.GetDirectories(sourceFolder);
            foreach (string folder in folders)
            {
                string name = Path.GetFileName(folder);
                string dest = Path.Combine(destFolder, name);
                CopyDirectory(folder, dest);//构建目标路径,递归复制文件
            }
        }

        /// <summary>
        /// 获取文件夹下的一级文件夹目录名称，不含子文件夹。
        /// </summary>
        /// <param name="sourceFolder"></param>
        public static string[] GetDirectories(string sourceFolder)
        {
            return Directory.GetDirectories(sourceFolder)
                .Select(s => Path.GetFileName(s))
                .ToArray();
        }
    }
}