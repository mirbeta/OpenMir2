using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SystemModule.Common
{
    /// <summary>
    /// Provides methods for reading and writing to an conf file.
    /// </summary>
    public abstract class IniFile
    {
        private string _fileName;
        private Dictionary<string, Dictionary<string, string>> iniCahce = new Dictionary<string, Dictionary<string, string>>();

        private bool largeCommentFlag = false;

        protected int ConfigCount => iniCahce.Count;

        protected IniFile()
        {

        }
        
        protected IniFile(string fileName)
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

        private bool GetBool(string section, string key, bool defValue)
        {
            if (iniCahce.ContainsKey(section))
            {
                Dictionary<string, string> hash = iniCahce[section];
                if (hash.ContainsKey(key))
                {
                    var str = hash[key].ToUpper();
                    if (string.IsNullOrEmpty(str))
                    {
                        return defValue;
                    }
                    return ((((str == "是") || (str == "YES")) || (str == "1")) || (str == "TRUE"));
                }
            }
            return defValue;
        }

        protected bool ReadBool(string section, string key, bool defValue)
        {
            return GetBool(section, key, defValue);
        }

        protected double ReadFloat(string section, string key, double defValue)
        {
            if (iniCahce.ContainsKey(section))
            {
                Dictionary<string, string> hash = iniCahce[section];
                if (hash.ContainsKey(key))
                {
                    var str = hash[key].ToUpper();
                    if (string.IsNullOrEmpty(str))
                    {
                        return defValue;
                    }
                    return double.Parse(str);
                }
            }
            return defValue;
        }

        protected DateTime ReadDateTime(string section, string key, DateTime defValue)
        {
            if (iniCahce.ContainsKey(section))
            {
                Dictionary<string, string> hash = iniCahce[section];
                if (hash.ContainsKey(key))
                {
                    var str = hash[key].ToUpper();
                    if (string.IsNullOrEmpty(str))
                    {
                        return defValue;
                    }
                    return DateTime.Parse(str);
                }
            }
            return defValue;
        }

        public int ReadInteger(string section, string key, int defValue)
        {
            return GetInt(section, key, defValue);
        }

        protected T Read<T>(string section, string key, object defValue)
        {
            if (iniCahce.ContainsKey(section))
            {
                Dictionary<string, string> hash = iniCahce[section];
                if (hash.ContainsKey(key))
                {
                    var str = hash[key];
                    if (string.IsNullOrEmpty(str))
                    {
                        return (T)Convert.ChangeType(defValue, typeof(T));
                    }
                    return (T)Convert.ChangeType(str, typeof(T));
                }
            }
            return (T)Convert.ChangeType(defValue, typeof(T));
        }

        protected int ReadInteger(string section, string key, byte defValue)
        {
            return GetInt(section, key, defValue);
        }

        protected int GetInt(string section, string key, int defValue)
        {
            if (iniCahce.ContainsKey(section))
            {
                Dictionary<string, string> hash = iniCahce[section];
                int ret;
                if (hash.ContainsKey(key))
                {
                    var str = hash[key];
                    if (string.IsNullOrEmpty(str))
                    {
                        return defValue;
                    }
                    if (int.TryParse(str, out ret))
                    {
                        return ret;
                    }
                }
            }
            return defValue;
        }

        private static string GetSecString(string str)
        {
            if (str[0] != '[')
                return string.Empty;
            var pos = str.IndexOf(']');
            return pos > 0 ? str.Substring(1, pos - 1).Trim() : string.Empty;
        }

        protected ICollection<string> GetSectionItemName(string sectName)
        {
            if (iniCahce.ContainsKey(sectName))
            {
                Dictionary<string, string> tbl = iniCahce[sectName];
                return tbl.Keys;
            }
            return new List<string>();
        }

        protected ICollection<string> GetAllValues(string sectName)
        {
            if (iniCahce.ContainsKey(sectName))
            {
                Dictionary<string, string> tbl = iniCahce[sectName];
                return tbl.Values;
            }
            return new List<String>();
        }

        public string ReadString(string section, string key, string defval)
        {
            var result = GetString(section, key);
            return string.IsNullOrEmpty(result) ? defval : result;
        }

        private string GetString(string section, string key)
        {
            if (iniCahce.ContainsKey(section))
            {
                var hash = iniCahce[section];
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
            var rd = new StreamReader(File.Open(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite), Encoding.UTF8);
            var isCurSecComment = false;
            Dictionary<string, string> curSec = null;
        Label_02A6:
            var str = rd.ReadLine();
            if (str != null)
            {
                str = str.Trim();
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
                        var sec = GetSecString(str);
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
                            if (iniCahce.ContainsKey(sec))
                            {
                                // Output.ShowMessageBox(sec + " 段重复, 请修改配置文件!");
                                goto Label_02AE;
                            }
                            curSec = new Dictionary<string, string>();
                            iniCahce.Add(sec, curSec);
                        }
                        else if (!isCurSecComment)
                        {
                            var index = str.IndexOf(";;", StringComparison.Ordinal);
                            if (index >= 0)
                            {
                                str = str[..index].Trim();
                            }
                            if (curSec == null)
                            {
                                curSec = new Dictionary<string, string>();
                                iniCahce.Add("", curSec);
                            }
                            var substr = SplitKeyVal(str);
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

        private string ReadLine(StreamReader rd)
        {
            var str = "";
            var s = rd.ReadLine();
            if (s == null)
            {
                return null;
            }
            s = s.Trim();
            if (!string.IsNullOrEmpty(s))
            {
                str += s;
            }
            return str;
        }

        protected void Save()
        {
            if (!File.Exists(FileName))
            {
                File.Create(FileName).Close();
            }
            var fi = new FileInfo(FileName);
            var isRealOnly = fi.IsReadOnly;
            if (!Directory.Exists(fi.Directory?.FullName))
            {
                Directory.CreateDirectory(fi.Directory.FullName);
            }
            if (isRealOnly && fi.Exists)
            {
                fi.IsReadOnly = false;
            }
            var sw = new StreamWriter(File.Open(FileName, FileMode.Truncate, FileAccess.Write, FileShare.ReadWrite), Encoding.UTF8);
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

        private string[] SplitKeyVal(string str)
        {
            var pos = -1;
            for (var i = 0; i < str.Length; i++)
            {
                if (str[i] == '=')
                {
                    pos = i;
                    break;
                }
            }
            if ((pos > 0) && (pos < str.Length))
            {
                return new string[] { str[..pos], str[(pos + 1)..] };
            }
            return null;
        }

        protected void WriteInt(string section, string key, int val)
        {
            WriteString(section, key, val.ToString());
        }

        protected void WriteBool(string section, string key, bool val)
        {
            if (!iniCahce.ContainsKey(section))
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
            if (!iniCahce.ContainsKey(section))
            {
                iniCahce.Add(section, new Dictionary<string, string>());
            }
            if (!string.IsNullOrEmpty(key))
            {
                Dictionary<string, string> secTbl = iniCahce[section];
                secTbl[key] = val.ToString();
            }
        }

        protected void WriteDateTime(string section, string key, DateTime val)
        {
            //Console.WriteLine("todo ini WriteDateTime");
        }

        protected void WriteString(string section, string key, object str)
        {
            if (str == null)
            {
                return;
            }
            if (!iniCahce.ContainsKey(section))
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
    }
}