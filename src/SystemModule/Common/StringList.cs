using System;
using System.IO;
using System.Text;

namespace SystemModule.Common
{
    public sealed class StringList : IDisposable
    {
        private readonly int m_Capacity;
        private string[] _mStrings;
        private int _mSize;

        /// <summary>
        /// 数据个数属性
        /// </summary>
        public int Count
        {
            get
            {
                return _mSize;
            }
        }

        /// <summary>
        /// 缓存大小属性
        /// </summary>
        public int Capacity
        {
            get
            {
                return m_Capacity;
            }
            set
            {
                if (_mStrings == null)
                {
                    return;
                }

                if (value == _mStrings.Length) return;
                if (value < this._mSize)
                {
                    throw new ArgumentOutOfRangeException();
                }

                if (value > 0)
                {
                    var objArray1 = new string[value];
                    if (this._mSize > 0)
                    {
                        Array.Copy(this._mStrings, 0, objArray1, 0, this._mSize);
                    }
                    this._mStrings = objArray1;
                }
                else
                {
                    this._mStrings = new string[0x10];
                }
            }
        }

        public string Text
        {
            get
            {
                return this.ToString();
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public StringList() : this(10)
        {
            
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public StringList(int capacity)
        {
            m_Capacity = capacity;
            _mStrings = new string[capacity];
            _mSize = 0;
        }

        /// <summary>
        /// 读取某行内容
        /// </summary>
        /// <param name="index"></param>
        public string this[int index]
        {
            get
            {
                if ((index < 0) || (index >= _mSize))
                {
                    throw new ArgumentOutOfRangeException();
                }
                return this._mStrings[index];
            }
            set
            {
                if ((index < 0) || (index >= _mSize))
                {
                    throw new ArgumentOutOfRangeException();
                }
                this._mStrings[index] = value;
            }
        }

        /// <summary>
        /// 调整缓存大小
        /// </summary>
        private void EnsureCapacity(int min)
        {
            if (this._mStrings.Length >= min) return;
            var num1 = (this._mStrings.Length == 0) ? 0x10 : (this._mStrings.Length * 2);
            if (num1 < min)
            {
                num1 = min;
            }
            this.Capacity = num1;
        }

        public void Add(string value)
        {
            if (this.Count == _mStrings.Length)
            {
                EnsureCapacity(this.Count + 1);
            }
            _mStrings[this.Count] = value;
            _mSize++;
        }

        /// <summary>
        /// 追加一行
        /// </summary>
        public int AppendText(string value)
        {
            if (this.Count == _mStrings.Length)
            {
                EnsureCapacity(this.Count + 1);
            }

            _mStrings[this.Count] = value;
            _mSize++;

            return _mSize;
        }

        /// <summary>
        /// 插入一行
        /// </summary>
        /// <param name="index"></param>
        public int InsertText(int index, string value)
        {
            if (index < 0)
            {
                index = 0;
            }

            if (this.Count == _mStrings.Length)
            {
                EnsureCapacity(this.Count + 1);
            }

            if (index < this.Count)
            {
                Array.Copy(this._mStrings, index, this._mStrings, index + 1, this._mSize - index);
            }

            _mStrings[index] = value;
            _mSize++;

            return _mSize;
        }

        /// <summary>
        /// 查找数据的位置
        /// </summary>
        public int IndexOf(string value)
        {
            return Array.IndexOf(this._mStrings, value, 0, this._mSize);
        }

        /// <summary>
        /// 删除一行
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            if ((index < 0) || (index >= this._mSize))
            {
                throw new ArgumentOutOfRangeException();
            }
            this._mSize--;
            if (index < this._mSize)
            {
                Array.Copy(this._mStrings, index + 1, this._mStrings, index, this._mSize - index);
            }
            this._mStrings[this._mSize] = null;
        }

        /// <summary>
        /// 转换为字符串。
        /// </summary>
        public override string ToString()
        {
            var s = new StringBuilder(this.Count);
            for (var i = 0; i < this.Count; i++)
            {
                s.AppendLine(_mStrings[i]);
            }
            return s.ToString();
        }
        
        /// <summary>
        /// 转换为字符串。
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public string ToString(int startIndex, int count)
        {
            if (startIndex < 0)
            {
                startIndex = 0;
            }
            else if (startIndex >= this.Count)
            {
                return "";
            }

            if (count <= 0)
            {
                return "";
            }

            if (count + startIndex > this.Count)
            {
                count = this.Count - startIndex;
            }

            var s = new StringBuilder(this.Count);

            for (var i = startIndex; i < count; i++)
            {
                s.Append(_mStrings[i] + "\r\n");
            }

            return s.ToString();
        }

        /// <summary>
        /// 清除内容
        /// </summary>
        public void Clear()
        {
            this._mSize = 0;
        }

        /// <summary>
        /// 保存为一个文件
        /// </summary>
        /// <param name="fileName"></param>
        public void SaveToFile(string fileName)
        {
            var fs = GetEncoding(fileName, Encoding.GetEncoding("gb2312"));
            var sw2 = new StreamWriter(fileName, false, fs);
            for (var i = 0; i < this.Count; i++)
            {
                sw2.WriteLine(_mStrings[i]);
            }
            sw2.Close();
            sw2.Dispose();
        }

        /// <summary>
        /// 读入一个文本文件
        /// </summary>
        /// <param name="fileName"></param>
        public void LoadFromFile(string fileName)
        {
            if (!File.Exists(fileName))
            {
                return;
            }
            this.Clear();
            var fs = GetEncoding(fileName, Encoding.GetEncoding("gb2312"));
            var sr2 = new StreamReader(fileName, fs);
            while (sr2.Peek() >= 0)
            {
                this.AppendText(sr2.ReadLine());
            }
            sr2.Close();
            sr2.Dispose();
        }

        private static Encoding GetEncoding(string fileName, Encoding defaultEncoding)
        {
            var fs = new FileStream(fileName, FileMode.Open);
            var targetEncoding = GetEncoding(fs, defaultEncoding);
            fs.Close();
            fs.Dispose();
            return targetEncoding;
        }

        private static Encoding GetEncoding(Stream stream, Encoding defaultEncoding)
        {
            var targetEncoding = defaultEncoding;
            if (stream != null && stream.Length >= 2)
            {
                //保存文件流的前4个字节   
                byte byte1 = 0;
                byte byte2 = 0;
                byte byte3 = 0;
                byte byte4 = 0;
                //保存当前Seek位置   
                var origPos = stream.Seek(0, SeekOrigin.Begin);
                stream.Seek(0, SeekOrigin.Begin);
                var nByte = stream.ReadByte();
                byte1 = Convert.ToByte(nByte);
                byte2 = Convert.ToByte(stream.ReadByte());
                if (stream.Length >= 3)
                {
                    byte3 = Convert.ToByte(stream.ReadByte());
                }
                if (stream.Length >= 4)
                {
                    byte4 = Convert.ToByte(stream.ReadByte());
                }
                //根据文件流的前4个字节判断Encoding   
                //Unicode {0xFF, 0xFE};   
                //BE-Unicode {0xFE, 0xFF};   
                //UTF8 = {0xEF, 0xBB, 0xBF};   
                if (byte1 == 0xFE && byte2 == 0xFF)//UnicodeBe   
                {
                    targetEncoding = Encoding.BigEndianUnicode;
                }
                if (byte1 == 0xFF && byte2 == 0xFE && byte3 != 0xFF)//Unicode   
                {
                    targetEncoding = Encoding.Unicode;
                }
                if (byte1 == 0xEF && byte2 == 0xBB && byte3 == 0xBF)//UTF8   
                {
                    targetEncoding = Encoding.UTF8;
                }
                //恢复Seek位置         
                stream.Seek(origPos, SeekOrigin.Begin);
            }
            return targetEncoding;
        }

        public void Dispose()
        {
            _mStrings = null;
            _mSize = 0;
        }
    }
}