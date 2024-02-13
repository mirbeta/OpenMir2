using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DBSrv
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            AppServer serviceRunner = new AppServer();
            await serviceRunner.StartAsync(CancellationToken.None);
        }
    }
}