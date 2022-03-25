using Spectre.Console.Cli;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace GameGate.Commands
{



    public sealed class ShowStatusCommand : Command<ShowStatusCommand.ShowStatusCommandSetting>
    {
        public sealed class ShowStatusCommandSetting : CommandSettings
        {
            [DefaultValue("Run")]
            [CommandOption("-c|--configuration <CONFIGURATION>")]
            public string Test { get; set; }
        }

        public override int Execute([NotNull] CommandContext context, [NotNull] ShowStatusCommandSetting settings)
        {
            System.Console.WriteLine("1");
            return 1;
        }
    }
}