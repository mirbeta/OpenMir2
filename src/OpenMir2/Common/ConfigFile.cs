using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OpenMir2.Common
{
    /// <summary>
    /// Provides methods for reading and writing to an conf file.
    /// </summary>
    public abstract class ConfigFile
    {
        private string _fileName;
        private readonly Dictionary<string, Dictionary<string, string>> iniCahce = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);

        private bool largeCommentFlag = false;

        protected int ConfigCount => iniCahce.Count;

        protected ConfigFile()
        {

        }

        protected ConfigFile(string fileName)
        {
            FileName = fileName;
        }

        protected void Clear()
        {
            iniCahce.Clear();
        }

        protected bool ContainSectionName(string secName)
        {
            return GetAllSectionName().Contains(secName);
        }

        private ICollection<string> GetAllSectionName()
        {
            return iniCahce.Keys;
        }

        protected byte ReadWriteByte(string section, string key, byte defValue)
        {
            if (CheckSectionExists(section) && CheckSectionNodeExists(section, key))
            {
                return Read<byte>(section, key, defValue);
            }
            WriteInteger(section, key, defValue);
            return defValue;
        }

        public int ReadWriteInteger(string section, string key, int defValue)
        {
            if (CheckSectionExists(section) && CheckSectionNodeExists(section, key))
            {
                return ReadInteger(section, key, defValue);
            }
            WriteInteger(section, key, defValue);
            return defValue;
        }

        protected short ReadWriteInt16(string section, string key, short defValue)
        {
            if (CheckSectionExists(section) && CheckSectionNodeExists(section, key))
            {
                return Read<short>(section, key, defValue);
            }
            WriteInteger(section, key, defValue);
            return defValue;
        }

        protected ushort ReadWriteUInt16(string section, string key, ushort defValue)
        {
            if (CheckSectionExists(section) && CheckSectionNodeExists(section, key))
            {
                return Read<ushort>(section, key, defValue);
            }
            WriteInteger(section, key, defValue);
            return defValue;
        }

        public bool ReadWriteBool(string section, string key, bool defValue)
        {
            if (CheckSectionExists(section) && CheckSectionNodeExists(section, key))
            {
                return ReadBool(section, key, defValue);
            }
            WriteBool(section, key, defValue);
            return defValue;
        }

        protected double ReadWriteFloat(string section, string key, double defValue)
        {
            if (CheckSectionExists(section) && CheckSectionNodeExists(section, key))
            {
                return ReadFloat(section, key, defValue);
            }
            WriteFloat(section, key, defValue);
            return defValue;
        }

        public string ReadWriteString(string section, string key, string defValue)
        {
            if (CheckSectionExists(section) && CheckSectionNodeExists(section, key))
            {
                return ReadString(section, key, defValue);
            }
            WriteString(section, key, defValue);
            return defValue;
        }

        protected DateTime ReadWriteDate(string section, string key, DateTime defValue)
        {
            if (CheckSectionExists(section) && CheckSectionNodeExists(section, key))
            {
                return ReadDateTime(section, key, defValue);
            }
            WriteDateTime(section, key, defValue);
            return defValue;
        }

        private T Read<T>(string section, string key, object defValue)
        {
            Dictionary<string, string> hash = iniCahce[section];
            if (hash.ContainsKey(key))
            {
                string str = hash[key];
                if (string.IsNullOrEmpty(str))
                {
                    return (T)Convert.ChangeType(defValue, typeof(T));
                }
                return (T)Convert.ChangeType(str, typeof(T));
            }
            return (T)Convert.ChangeType(defValue, typeof(T));
        }

        private double ReadFloat(string section, string key, double defValue)
        {
            Dictionary<string, string> hash = iniCahce[section];
            if (hash.ContainsKey(key))
            {
                string str = hash[key];
                if (string.IsNullOrEmpty(str))
                {
                    return defValue;
                }
                return double.Parse(str);
            }
            return defValue;
        }

        private DateTime ReadDateTime(string section, string key, DateTime defValue)
        {
            Dictionary<string, string> hash = iniCahce[section];
            if (hash.ContainsKey(key))
            {
                string str = hash[key];
                if (string.IsNullOrEmpty(str))
                {
                    return defValue;
                }
                return DateTime.Parse(str);
            }
            return defValue;
        }

        private int ReadInteger(string section, string key, int defValue)
        {
            Dictionary<string, string> hash = iniCahce[section];
            if (hash.ContainsKey(key))
            {
                string str = hash[key];
                if (string.IsNullOrEmpty(str))
                {
                    return defValue;
                }
                if (int.TryParse(str, out int ret))
                {
                    return ret;
                }
            }
            return defValue;
        }

        private bool ReadBool(string section, string key, bool defValue)
        {
            if (CheckSectionExists(section))
            {
                Dictionary<string, string> hash = iniCahce[section];
                if (hash.ContainsKey(key))
                {
                    string str = hash[key].ToUpper();
                    if (string.IsNullOrEmpty(str))
                    {
                        return defValue;
                    }
                    return (str == "是") || (str == "YES") || (str == "1") || (str == "TRUE");
                }
            }
            return defValue;
        }

        private static string GetSecString(string str)
        {
            if (str[0] != '[')
            {
                return string.Empty;
            }

            int pos = str.IndexOf(']');
            return pos > 0 ? str[1..pos].Trim() : string.Empty;
        }

        protected ICollection<string> GetSectionItemName(string sectName)
        {
            if (CheckSectionExists(sectName))
            {
                Dictionary<string, string> tbl = iniCahce[sectName];
                return tbl.Keys;
            }
            return new List<string>();
        }

        protected ICollection<string> GetAllValues(string sectName)
        {
            if (CheckSectionExists(sectName))
            {
                Dictionary<string, string> tbl = iniCahce[sectName];
                return tbl.Values;
            }
            return new List<String>();
        }

        private string ReadString(string section, string key, string defval)
        {
            string result = GetString(section, key);
            return string.IsNullOrEmpty(result) ? defval : result;
        }

        private string GetString(string section, string key)
        {
            if (CheckSectionExists(section))
            {
                Dictionary<string, string> hash = iniCahce[section];
                if (hash.ContainsKey(key))
                {
                    return hash[key];
                }
            }
            return "";
        }

        protected void Load()
        {
            if (!File.Exists(FileName))
            {
                File.Create(FileName).Close();
                return;
            }
            StreamReader rd = new StreamReader(File.Open(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite), Encoding.GetEncoding("GB2312"));
            bool isCurSecComment = false;
            Dictionary<string, string> curSec = null;
        Label_02A6:
            string str = rd.ReadLine();
            if (str != null)
            {
                str = str.Trim();
                if (str.StartsWith(";"))
                {
                    goto Label_02A6;
                }
                if (!string.IsNullOrEmpty(str))
                {
                    if (str.StartsWith("/*"))
                    {
                        largeCommentFlag = true;
                    }
                    else if (largeCommentFlag)
                    {
                        if (str.StartsWith("*/"))
                        {
                            largeCommentFlag = false;
                        }
                    }
                    else if ((str.Length < 2) || ((str[0] != ';') || (str[1] != ';')))
                    {
                        string sec = GetSecString(str);
                        if (!string.IsNullOrEmpty(sec))
                        {
                            if (sec.Length >= 2)
                            {
                                if ((sec[0] == ';') && (sec[1] == ';'))
                                {
                                    isCurSecComment = true;
                                    goto Label_02A6;
                                }
                                isCurSecComment = false;
                            }
                            if (CheckSectionExists(sec))
                            {
                                // Output.ShowMessageBox(sec + " 段重复, 请修改配置文件!");
                                goto Label_02AE;
                            }
                            curSec = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                            iniCahce.Add(sec, curSec);
                        }
                        else if (!isCurSecComment)
                        {
                            int index = str.IndexOf(";;", StringComparison.OrdinalIgnoreCase);
                            if (index >= 0)
                            {
                                str = str[..index].Trim();
                            }
                            if (curSec == null)
                            {
                                curSec = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                                iniCahce.Add("", curSec);
                            }
                            string[] substr = SplitKeyVal(str);
                            if ((substr != null) && (substr.Length >= 2))
                            {
                                substr[0] = substr[0].Trim();
                                substr[1] = substr[1].Trim();
                                if (!curSec.ContainsKey(substr[0]))
                                {
                                    curSec.Add(substr[0], substr[1]);
                                }
                            }
                        }
                    }
                }
                goto Label_02A6;
            }
        Label_02AE:
            rd.Close();
            rd.Dispose();
        }

        protected void ReLoad()
        {
            iniCahce.Clear();
            Load();
        }

        protected void Save()
        {
            if (!File.Exists(FileName))
            {
                File.Create(FileName).Close();
            }
            FileInfo fi = new FileInfo(FileName);
            bool isRealOnly = fi.IsReadOnly;
            if (!Directory.Exists(fi.Directory?.FullName))
            {
                Directory.CreateDirectory(fi.Directory.FullName);
            }
            if (isRealOnly && fi.Exists)
            {
                fi.IsReadOnly = false;
            }
            StreamWriter sw = new StreamWriter(File.Open(FileName, FileMode.Truncate, FileAccess.Write, FileShare.ReadWrite), Encoding.GetEncoding("GB2312"));
            foreach (KeyValuePair<string, Dictionary<string, string>> pair in iniCahce)
            {
                sw.WriteLine("[" + pair.Key + "]");
                Dictionary<string, string> tbl = pair.Value;
                foreach (KeyValuePair<string, string> pair2 in tbl)
                {
                    sw.WriteLine(pair2.Key + "=" + pair2.Value);
                }
                sw.WriteLine("");
            }
            sw.Close();
            sw.Dispose();
            if (isRealOnly)
            {
                fi.IsReadOnly = true;
            }
        }

        private static string[] SplitKeyVal(string str)
        {
            int pos = -1;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == '=')
                {
                    pos = i;
                    break;
                }
            }
            if ((pos > 0) && (pos < str.Length))
            {
                return new[] { str[..pos], str[(pos + 1)..] };
            }
            return null;
        }

        protected void WriteInt(string section, string key, int val)
        {
            WriteString(section, key, val.ToString());
        }

        protected void WriteBool(string section, string key, bool val)
        {
            if (!CheckSectionExists(section))
            {
                iniCahce.Add(section, new Dictionary<string, string>());
            }
            if (!string.IsNullOrEmpty(key))
            {
                Dictionary<string, string> secTbl = iniCahce[section];
                secTbl[key] = (val ? 1 : 0).ToString();
            }
        }

        protected void WriteInteger(string section, string key, object val)
        {
            if (val == null)
            {
                return;
            }
            if (!CheckSectionExists(section))
            {
                iniCahce.Add(section, new Dictionary<string, string>());
            }
            if (!string.IsNullOrEmpty(key))
            {
                Dictionary<string, string> secTbl = iniCahce[section];
                secTbl[key] = val.ToString();
            }
        }

        private void WriteFloat(string section, string key, double val)
        {
            if (!CheckSectionExists(section))
            {
                iniCahce.Add(section, new Dictionary<string, string>());
            }
            if (!string.IsNullOrEmpty(key))
            {
                Dictionary<string, string> secTbl = iniCahce[section];
                secTbl[key] = val.ToString("f2");
            }
        }

        protected void WriteDateTime(string section, string key, DateTime val)
        {
            if (!CheckSectionExists(section))
            {
                iniCahce.Add(section, new Dictionary<string, string>());
            }
            if (!string.IsNullOrEmpty(key))
            {
                Dictionary<string, string> secTbl = iniCahce[section];
                secTbl[key] = val.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        protected void WriteString(string section, string key, object str)
        {
            if (str == null)
            {
                return;
            }
            if (!CheckSectionExists(section))
            {
                iniCahce.Add(section, new Dictionary<string, string>());
            }
            if (!string.IsNullOrEmpty(key))
            {
                Dictionary<string, string> secTbl = iniCahce[section];
                secTbl[key] = str.ToString();
            }
        }

        private string FileName
        {
            get
            {
                return _fileName;
            }
            set
            {
                _fileName = value;
            }
        }

        private bool CheckSectionExists(string sectionName)
        {
            return iniCahce.ContainsKey(sectionName);
        }

        private bool CheckSectionNodeExists(string sectionName, string nodeName)
        {
            return iniCahce[sectionName].ContainsKey(nodeName);
        }
    }
}