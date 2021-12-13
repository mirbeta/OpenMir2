using System;
using System.Text;
using SystemModule;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            Console.WriteLine("Hello World!");

            var asdasd = MD5.MD5String("06AY4AKF29D4VCT53BCQ17.1.0 (51516)Parallels Virtual PlatformNone");

            var a = MD5.MD5UnPrInt("7DFF4F2459B3ADEC762EEDBD3D4DCA2FDE206010E8");

        }
    }
}
