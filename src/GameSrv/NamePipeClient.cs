using System.IO.Pipes;
using System.Security.Principal;

namespace GameSrv {
    public class NamePipeClient {
        private readonly NamedPipeClientStream pipeClient;
        private readonly StreamWriter streamWriter;
        private readonly StreamReader streamReader;
        private readonly byte[] receiveBuff = new byte[1024];

        public NamePipeClient() {
            pipeClient = new NamedPipeClientStream("localhost", "mirmap.pipe", PipeDirection.InOut, PipeOptions.None, TokenImpersonationLevel.None);
            streamWriter = new StreamWriter(pipeClient);
            streamReader = new StreamReader(pipeClient);
        }

        public void Connect() {
            pipeClient.Connect(5000);
        }

        public void Close() {
            pipeClient.Close();
        }

        public void SendPipeMessage(byte[] data) {
            string clientSendMsg = $"123,Client now is {DateTime.Now:yyyyMMddHHmmssffff}";
            char[] sendBytes = clientSendMsg.ToCharArray();
            streamWriter.WriteLine(clientSendMsg);
            streamWriter.Flush();
            ReceivePipeMessage();
        }

        private void ReceivePipeMessage() {
            string s = streamReader.ReadLine();
            //pipeClient.Read(receiveBuff, 0, receiveBuff.Length);
            //Console.WriteLine($"{DateTime.Now}:{Encoding.UTF8.GetString(receiveBuff).Trim(new char[] { '\0' })}");
            Console.WriteLine(s);
        }
    }
}