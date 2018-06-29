using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace appp
{
    public class Ini
    {
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        public static string Filename = "C:\\config.txt";//配置文件路径

        /// <summary>
        /// 写入
        /// </summary>
        /// <param name="Section">配置项目</param>
        /// <param name="Key">配置关键字</param>
        /// <param name="Data">参数内容</param>
        public static void Write(string Section, string Key, string Data)
        {
            try
            {
                WritePrivateProfileString(Section, Key, Data, Filename);
            }
            catch
            {
                FileStream fs = new FileStream(Filename, FileMode.Create);
                fs.Close();
                WritePrivateProfileString(Section, Key, Data, Filename);
            }
        }

        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="Section"></param>
        public static void delete(string Section)
        {
            Write(Section, null, null);
        }



        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="Section">配置项目</param>
        /// <param name="Key">配置关键字</param>
        /// <returns>返回读取内容</returns>
        public static string Read(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "null", temp, 255, Filename);
            return temp.ToString();
        }
    }
}
