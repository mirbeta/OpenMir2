using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace mSystemModule
{
 /// <summary>
    /// 时间戳ID
    /// </summary>
    public class TimestampID
    {
        private long _lastTimestamp;
        private long _sequence; //计数从零开始
        private readonly DateTime? _initialDateTime;
        private static TimestampID _timestampID;
        private const int MAX_END_NUMBER = 9999;

        private TimestampID(DateTime? initialDateTime)
        {
            _initialDateTime = initialDateTime;
        }

        /// <summary>
        /// 获取单个实例对象
        /// </summary>
        /// <param name="initialDateTime">最初时间，与当前时间做个相差取时间戳</param>
        /// <returns></returns>
        public static TimestampID GetInstance(DateTime? initialDateTime = null)
        {
            if (_timestampID == null) Interlocked.CompareExchange(ref _timestampID, new TimestampID(initialDateTime), null);
            return _timestampID;
        }

        /// <summary>
        /// 最初时间，作用时间戳的相差
        /// </summary>
        protected DateTime InitialDateTime
        {
            get
            {
                if (_initialDateTime == null || _initialDateTime.Value == DateTime.MinValue) return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                return _initialDateTime.Value;
            }
        }

        /// <summary>
        /// 获取时间戳ID
        /// </summary>
        /// <returns></returns>
        public string GetID()
        {
            long temp;
            var timestamp = GetUniqueTimeStamp(_lastTimestamp, out temp);
            return string.Format("{0}{Fill({1})}", timestamp, temp);
        }

        /// <summary>
        /// 获取一个时间戳字符串
        /// </summary>
        /// <returns></returns>
        public long GetUniqueTimeStamp(long lastTimeStamp, out long temp)
        {
            lock (this)
            {
                temp = 1;
                var timeStamp = GetTimestamp();
                if (timeStamp == _lastTimestamp)
                {
                    _sequence = _sequence + 1;
                    temp = _sequence;
                    if (temp >= MAX_END_NUMBER)
                    {
                        timeStamp = GetTimestamp();
                        _lastTimestamp = timeStamp;
                        temp = _sequence = 1;
                    }
                }
                else
                {
                    _sequence = 1;
                    _lastTimestamp = timeStamp;
                }
                return timeStamp;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private long GetTimestamp()
        {
            if (InitialDateTime >= DateTime.Now) throw new Exception("最初时间比当前时间还大，不合理");
            var ts = DateTime.UtcNow - InitialDateTime;
            return (long)ts.TotalMilliseconds;
        }
    }
}
