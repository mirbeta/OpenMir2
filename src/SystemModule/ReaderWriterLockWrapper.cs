using System;
using System.Threading;

namespace SystemModule;

public class ReaderWriterLockWrapper
{
    private readonly ReaderWriterLockSlim lck;
    private readonly ReaderWrapper reader;
    private readonly WriterWrapper writer;

    public ReaderWriterLockWrapper()
    {
        lck = new ReaderWriterLockSlim();
        reader = new ReaderWrapper(lck);
        writer = new WriterWrapper(lck);
    }

    public IDisposable EnterReadLock()
    {
        lck.EnterReadLock();
        return reader;
    }

    public IDisposable EnterWriteLock()
    {
        lck.EnterWriteLock();
        return writer;
    }

    private struct ReaderWrapper : IDisposable
    {
        private readonly ReaderWriterLockSlim lck;

        public ReaderWrapper(ReaderWriterLockSlim lck)
        {
            this.lck = lck;
        }

        public void Dispose()
        {
            lck.ExitReadLock();
        }
    }

    private struct WriterWrapper : IDisposable
    {
        private readonly ReaderWriterLockSlim lck;

        public WriterWrapper(ReaderWriterLockSlim lck)
        {
            this.lck = lck;
        }

        public void Dispose()
        {
            lck.ExitWriteLock();
        }
    }
}