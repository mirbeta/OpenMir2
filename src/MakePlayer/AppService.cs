using MakePlayer.Cliens;
using MakePlayer.Option;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SystemModule;
using System;

namespace MakePlayer
{
    public class AppService : BackgroundService
    {
        private readonly MakePlayOptions _options;
        private readonly CancellationTokenSource _cancellation = new CancellationTokenSource();
        private int _loginIndex;
        private int _loginTimeTick;

        public AppService(IOptions<MakePlayOptions> options)
        {
            this._options = options.Value;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            PlayHelper.LoadSayListFile();
            _options.ChrCount = HUtil32._MIN(_options.ChrCount, _options.TotalChrCount);
            _loginTimeTick = HUtil32.GetTickCount() - 1000 * _options.ChrCount;
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
                if (_options.TotalChrCount > 0)
                {
                    if ((HUtil32.GetTickCount() - _loginTimeTick) > 1000 * _options.ChrCount)
                    {
                        _loginTimeTick = HUtil32.GetTickCount();
                        if (_options.TotalChrCount >= _options.ChrCount)
                        {
                            _options.TotalChrCount -= _options.ChrCount;
                        }
                        else
                        {
                            _options.TotalChrCount = 0;
                        }
                        for (var i = 0; i < _options.ChrCount; i++)
                        {
                            var playClient = new PlayClient(_options);
                            playClient.SessionId = Guid.NewGuid().ToString("N");
                            playClient.CreateAccount = _options.NewAccount;
                            var loginAccount = string.Concat(_options.LoginAccount, _loginIndex);
                            if (loginAccount.Length > 10)
                            {
                                loginAccount = string.Concat(loginAccount.AsSpan(2, 8), RandomNumber.GetInstance().GenerateRandomNumber(3));
                            }
                            var loginPasswd = loginAccount;
                            playClient.ChrName = loginAccount;
                            playClient.ServerName = _options.ServerName;
                            playClient.RunTick = HUtil32.GetTickCount() + (i + 1) * 3000;
                            playClient.LoginId = loginAccount;
                            playClient.LoginPasswd = loginPasswd;
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