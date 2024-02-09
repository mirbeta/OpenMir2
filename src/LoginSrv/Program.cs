using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LoginSrv
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var serviceRunner = new AppServer();
            await serviceRunner.StartAsync(CancellationToken.None);
        }
    }
}