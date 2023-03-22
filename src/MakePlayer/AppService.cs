using MakePlayer.Cliens;
using MakePlayer.Option;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SystemModule;

namespace MakePlayer
{
    public class AppService : BackgroundService
    {
        private readonly MakePlayOptions options;
        private readonly CancellationTokenSource _cancellation = new CancellationTokenSource();
        private int _loginIndex;
        private int _loginTimeTick;

        public AppService(IOptions<MakePlayOptions> options)
        {
            this.options = options.Value;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            PlayHelper.LoadSayListFile();
            options.ChrCount = HUtil32._MIN(options.ChrCount, options.TotalChrCount);
            _loginTimeTick = HUtil32.GetTickCount() - 1000 * options.ChrCount;
            ClientManager.Start();
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            ClientManager.Stop();
            _cancellation.Cancel();
            return Task.CompletedTask;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!_cancellation.IsCancellationRequested)
            {
                if (options.TotalChrCount > 0)
                {
                    if ((HUtil32.GetTickCount() - _loginTimeTick) > 1000 * options.ChrCount)
                    {
                        _loginTimeTick = HUtil32.GetTickCount();
                        if (options.TotalChrCount >= options.ChrCount)
                        {
                            options.TotalChrCount -= options.ChrCount;
                        }
                        else
                        {
                            options.TotalChrCount = 0;
                        }
                        for (var i = 0; i < options.ChrCount; i++)
                        {
                            var playClient = new PlayClient(options);
                            playClient.SessionId = Guid.NewGuid().ToString("N");
                            playClient.CreateAccount = options.NewAccount;
                            var LoginAccount = string.Concat(options.LoginAccount, _loginIndex);
                            if (LoginAccount.Length > 10)
                            {
                                LoginAccount = LoginAccount.Substring(1, 8);
                            }
                            var LoginPasswd = LoginAccount;
                            playClient.ChrName = LoginAccount;
                            playClient.ServerName = options.ServerName;
                            playClient.RunTick = HUtil32.GetTickCount() + (i + 1) * 3000;
                            playClient.SetLoginInfo(LoginAccount, LoginPasswd);
                            ClientManager.AddClient(playClient.SessionId, playClient);
                            _loginIndex++;
                        }
                    }
                }
                ClientManager.Run();
                Thread.Sleep(TimeSpan.FromMilliseconds(50));
            }
            return Task.CompletedTask;
        }
    }
}