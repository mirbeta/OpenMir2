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

        public int Add(string value)
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
        /// <param name="encoding"></param>
        public void SaveToFile(string fileName, Encoding encoding)
        {
            var sw2 = new StreamWriter(fileName, false, encoding);
            for (var i = 0; i < this.Count; i++)
            {
                sw2.WriteLine(_mStrings[i]);
            }
            sw2.Close();
            sw2.Dispose();
        }

        public void SaveToFile(string fileName)
        {
            var sw2 = new StreamWriter(fileName, false, Encoding.GetEncoding("gb2312"));
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
            var sr2 = new StreamReader(fileName, Encoding.GetEncoding("gb2312"));
            while (sr2.Peek() >= 0)
            {
                this.AppendText(sr2.ReadLine());
            }
            sr2.Close();
            sr2.Dispose();
        }

        public void LoadFromFile(string fileName, bool isAdd)
        {
            this.Clear();
            var sr2 = new StreamReader(fileName, Encoding.GetEncoding("gb2312"));
            while (sr2.Peek() >= 0)
            {
                this.AppendText(sr2.ReadLine());
            }
            sr2.Close();
            sr2.Dispose();
        }

        public void LoadFromFile(string fileName, Encoding encoding)
        {
            this.Clear();
            var sr2 = new StreamReader(fileName, encoding);
            while (sr2.Peek() >= 0)
            {
                this.AppendText(sr2.ReadLine());
            }
            sr2.Close();
            sr2.Dispose();
        }
        
        public void Dispose()
        {
            _mStrings = null;
            _mSize = 0;
        }
    }
}