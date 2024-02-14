using System;
using System.IO;
using System.Text;

namespace OpenMir2.Common
{
    public sealed class StringList : IDisposable
    {
        private readonly int _capacity;
        private string[] _strings;
        private int _size;

        /// <summary>
        /// 数据个数属性
        /// </summary>
        public int Count => _size;

        /// <summary>
        /// 缓存大小属性
        /// </summary>
        public int Capacity
        {
            get => _capacity;
            set
            {
                if (_strings == null)
                {
                    return;
                }

                if (value == _strings.Length)
                {
                    return;
                }

                if (value < this._size)
                {
                    throw new ArgumentOutOfRangeException();
                }

                if (value > 0)
                {
                    string[] objArray1 = new string[value];
                    if (this._size > 0)
                    {
                        Array.Copy(this._strings, 0, objArray1, 0, this._size);
                    }
                    this._strings = objArray1;
                }
                else
                {
                    this._strings = new string[0x10];
                }
            }
        }

        public string Text => this.ToString();

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
            _capacity = capacity;
            _strings = new string[capacity];
            _size = 0;
        }

        /// <summary>
        /// 读取某行内容
        /// </summary>
        /// <param name="index"></param>
        public string this[int index]
        {
            get
            {
                if ((index < 0) || (index >= _size))
                {
                    throw new ArgumentOutOfRangeException();
                }
                return this._strings[index];
            }
            set
            {
                if ((index < 0) || (index >= _size))
                {
                    throw new ArgumentOutOfRangeException();
                }
                this._strings[index] = value;
            }
        }

        /// <summary>
        /// 调整缓存大小
        /// </summary>
        private void EnsureCapacity(int min)
        {
            if (this._strings.Length >= min)
            {
                return;
            }

            int num1 = (this._strings.Length == 0) ? 0x10 : (this._strings.Length * 2);
            if (num1 < min)
            {
                num1 = min;
            }
            this.Capacity = num1;
        }

        public void Add(string value)
        {
            if (this.Count == _strings.Length)
            {
                EnsureCapacity(this.Count + 1);
            }
            _strings[this.Count] = value;
            _size++;
        }

        /// <summary>
        /// 追加一行
        /// </summary>
        public int AppendText(string value)
        {
            if (this.Count == _strings.Length)
            {
                EnsureCapacity(this.Count + 1);
            }

            _strings[this.Count] = value;
            _size++;

            return _size;
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

            if (this.Count == _strings.Length)
            {
                EnsureCapacity(this.Count + 1);
            }

            if (index < this.Count)
            {
                Array.Copy(this._strings, index, this._strings, index + 1, this._size - index);
            }

            _strings[index] = value;
            _size++;

            return _size;
        }

        /// <summary>
        /// 查找数据的位置
        /// </summary>
        public int IndexOf(string value)
        {
            return Array.IndexOf(this._strings, value, 0, this._size);
        }

        /// <summary>
        /// 删除一行
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            if ((index < 0) || (index >= this._size))
            {
                throw new ArgumentOutOfRangeException();
            }
            this._size--;
            if (index < this._size)
            {
                Array.Copy(this._strings, index + 1, this._strings, index, this._size - index);
            }
            this._strings[this._size] = null;
        }

        /// <summary>
        /// 转换为字符串。
        /// </summary>
        public override string ToString()
        {
            StringBuilder s = new StringBuilder(this.Count);
            for (int i = 0; i < this.Count; i++)
            {
                s.AppendLine(_strings[i]);
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

            StringBuilder s = new StringBuilder(this.Count);

            for (int i = startIndex; i < count; i++)
            {
                s.Append(_strings[i] + "\r\n");
            }

            return s.ToString();
        }

        /// <summary>
        /// 清除内容
        /// </summary>
        public void Clear()
        {
            this._size = 0;
        }

        /// <summary>
        /// 保存为一个文件
        /// </summary>
        /// <param name="fileName"></param>
        public void SaveToFile(string fileName)
        {
            Encoding fs = GetEncoding(fileName, Encoding.GetEncoding("gb2312"));
            StreamWriter sw2 = new StreamWriter(fileName, false, fs);
            for (int i = 0; i < this.Count; i++)
            {
                sw2.WriteLine(_strings[i]);
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
            Encoding fs = GetEncoding(fileName, Encoding.GetEncoding("gb2312"));
            StreamReader sr2 = new StreamReader(fileName, fs);
            while (sr2.Peek() >= 0)
            {
                this.AppendText(sr2.ReadLine());
            }
            sr2.Close();
            sr2.Dispose();
        }

        private static Encoding GetEncoding(string fileName, Encoding defaultEncoding)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open);
            Encoding targetEncoding = GetEncoding(fs, defaultEncoding);
            fs.Close();
            fs.Dispose();
            return targetEncoding;
        }

        private static Encoding GetEncoding(Stream stream, Encoding defaultEncoding)
        {
            Encoding targetEncoding = defaultEncoding;
            if (stream != null && stream.Length >= 2)
            {
                //保存文件流的前4个字节   
                byte byte1 = 0;
                byte byte2 = 0;
                byte byte3 = 0;
                byte byte4 = 0;
                //保存当前Seek位置   
                long origPos = stream.Seek(0, SeekOrigin.Begin);
                stream.Seek(0, SeekOrigin.Begin);
                int nByte = stream.ReadByte();
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
            _strings = null;
            _size = 0;
        }
    }
}