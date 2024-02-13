using System.Threading;

namespace OpenMir2
{
    /// <summary>
    ///  A monitor for the networking I/O. From COPS V6 Enhanced Edition.
    /// </summary>
    public sealed class NetworkMonitor
    {
        /// <summary>
        /// The title of the console.
        /// </summary>
        private const string FORMAT_S = "(↑{0:F2} kbps [{2}], ↓{1:F2} kbps [{3}])";

        private long m_TotalRecvBytes = 0;
        /// <summary>
        /// The number of bytes received by the server.
        /// </summary>
        private int m_RecvBytes = 0;

        private long m_TotalSentBytes = 0;
        /// <summary>
        /// The number of bytes sent by the server.
        /// </summary>
        private int m_SentBytes = 0;

        private int m_SentPackets = 0;
        private int m_TotalSentPackets = 0;
        private int m_RecvPackets = 0;
        private int m_TotalRecvPackets = 0;

        /// <summary>
        /// Called by the timer.
        /// </summary>
        public string UpdateStatsAsync(int interval)
        {
            double download = m_RecvBytes / (double)interval * 8.0 / 1024.0;
            double upload = m_SentBytes / (double)interval * 8.0 / 1024.0;
            int sent = m_SentPackets;
            int recv = m_RecvPackets;

            m_RecvBytes = 0;
            m_SentBytes = 0;
            m_RecvPackets = 0;
            m_SentPackets = 0;

            return string.Format(FORMAT_S, upload, download, sent, recv);
        }

        public string ShowSendStats()
        {
            string str = HUtil32.FormatBytesValue(BytesSent);
            m_SentBytes = 0;
            m_SentPackets = 0;
            return str;
        }

        public string ShowReceive()
        {
            string str = HUtil32.FormatBytesValue(BytesRecv);
            m_RecvBytes = 0;
            m_RecvPackets = 0;
            return str;
        }

        public int PacketsSent => m_SentPackets;
        public int PacketsRecv => m_RecvPackets;
        public int BytesSent => m_SentBytes;
        public int BytesRecv => m_RecvBytes;
        public long TotalPacketsSent => m_TotalSentPackets;
        public long TotalPacketsRecv => m_TotalRecvPackets;
        public long TotalBytesSent => m_TotalSentBytes;
        public long TotalBytesRecv => m_TotalRecvBytes;

        /// <summary>
        /// Signal to the monitor that aLength bytes were sent.
        /// </summary>
        /// <param name="aLength">The number of bytes sent.</param>
        public void Send(int aLength)
        {
            Interlocked.Increment(ref m_SentPackets);
            Interlocked.Increment(ref m_TotalSentPackets);
            Interlocked.Add(ref m_SentBytes, aLength);
            Interlocked.Add(ref m_TotalSentBytes, aLength);
        }

        /// <summary>
        /// Signal to the monitor that aLength bytes were received.
        /// </summary>
        /// <param name="aLength">The number of bytes received.</param>
        public void Receive(int aLength)
        {
            Interlocked.Increment(ref m_RecvPackets);
            Interlocked.Increment(ref m_TotalRecvPackets);
            Interlocked.Add(ref m_RecvBytes, aLength);
            Interlocked.Add(ref m_TotalRecvBytes, aLength);
        }
    }
}