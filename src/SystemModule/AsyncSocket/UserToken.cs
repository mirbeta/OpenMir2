using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace SystemModule.AsyncSocket
{
    public class UserToken
    {
        private Socket connectSocket;
        private TaskCompletionSource<SocketResult> completionSource;
        private readonly Timer timer = null;
        
        public UserToken()
        {
            completionSource = new TaskCompletionSource<SocketResult>();
            timer = new Timer(Timer_Elapsed, null, Timeout.Infinite, Timeout.Infinite);
        }

        private void Timer_Elapsed(object state)
        {
#if DEBUG
            Console.WriteLine("Timer_Elapsed fired");
#endif
            SetCanceled();
        }

        public string Id { get; set; }

        public SocketAsyncEventArgs ReceiveArgs
        {
            get;
            set;
        }

        public Socket ConnectSocket
        {
            get
            {
                return this.connectSocket;
            }
            set
            {
                this.connectSocket = value;
                this.ReceiveArgs.AcceptSocket = this.connectSocket;
            }
        }
        
        public Task<SocketResult> CompletionSource
        {
            get
            {
                ResetTask();
                if (TimeOut > 0)
                {
                    timer.Change(TimeOut, Timeout.Infinite);
                }
#if DEBUG
                Console.WriteLine($"return a UserToken: {Id}, TaskID: {completionSource.Task.Id}");
#endif
                return completionSource.Task;
            }
        }

        public int TimeOut { get; set; } = -1;
        
        public void SetResult(SocketResult result)
        {
            if (completionSource.Task.IsCompleted)
                return;
            completionSource.SetResult(result);
            timer.Change(Timeout.Infinite, Timeout.Infinite);
#if DEBUG
            Console.WriteLine($"UserToken SetResult: {Id} TaskID: {completionSource.Task.Id}");
#endif
        }
        
        public void SetException(Exception exception)
        {
            if (completionSource.Task.IsCompleted)
                return;
            completionSource.SetResult(SocketResult.NotSocket);
            timer.Change(Timeout.Infinite, Timeout.Infinite);
#if DEBUG
            Console.WriteLine($"UserToken SetException: {Id} TaskID: {completionSource.Task.Id}");
#endif
        }
        
        public void SetCanceled()
        {
            if (completionSource.Task.IsCompleted)
                return;
            completionSource.SetResult(SocketResult.NotSocket);
            timer.Change(Timeout.Infinite, Timeout.Infinite);
#if DEBUG
            Console.WriteLine($"UserToken SetCanceled: {Id} TaskID: {completionSource.Task.Id}");
#endif
        }
        
        private void ResetTask()
        {
            TaskCompletionSource.Reset(ref completionSource);
        }
    }
}