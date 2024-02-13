using Spectre.Console;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DBSrv
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            PrintUsage();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            AppServer serviceRunner = new AppServer();
            await serviceRunner.StartAsync(CancellationToken.None);
        }

        private static void PrintUsage()
        {
            Table table = new Table()
            {
                Border = TableBorder.None,
                Expand = true,
            }.HideHeaders();
            table.AddColumn(new TableColumn("One"));

            FigletText header = new FigletText("OpenMir2")
            {
                Color = Color.Fuchsia
            };
            FigletText header2 = new FigletText("DB Server")
            {
                Color = Color.Aqua
            };

            StringBuilder sb = new StringBuilder();
            sb.Append("[bold fuchsia]/s[/] [aqua]查看[/] 网关状况\n");
            sb.Append("[bold fuchsia]/r[/] [aqua]重读[/] 配置文件\n");
            sb.Append("[bold fuchsia]/c[/] [aqua]清空[/] 清除屏幕\n");
            sb.Append("[bold fuchsia]/q[/] [aqua]退出[/] 退出程序\n");
            Markup markup = new Markup(sb.ToString());

            table.AddColumn(new TableColumn("Two"));

            Table rightTable = new Table()
                .HideHeaders()
                .Border(TableBorder.None)
                .AddColumn(new TableColumn("Content"));

            rightTable.AddRow(header)
                .AddRow(header2)
                .AddEmptyRow()
                .AddEmptyRow()
                .AddRow(markup);
            table.AddRow(rightTable);

            AnsiConsole.Write(table);
            AnsiConsole.WriteLine();
        }

    }
}