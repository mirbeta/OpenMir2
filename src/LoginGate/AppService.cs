public class AppService : BackgroundService
{
    private readonly ConfigManager _configManager;
    private readonly ServerManager _serverManager;
    private readonly ClientManager _clientManager;

    public AppService(ConfigManager configManager, ServerManager serverManager, ClientManager clientManager)
    {
        _configManager = configManager;
        _serverManager = serverManager;
        _clientManager = clientManager;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.Register(() => LogService.Debug("LoginGate is stopping."));
        _serverManager.Start();
        _clientManager.Start();
        _serverManager.ProcessLoginMessage(stoppingToken);
        _clientManager.ProcessSendMessage(stoppingToken);
        return Task.CompletedTask;
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        GateShare.Initialization();
        _configManager.LoadConfig();
        Initialization();
        LogService.Info("服务已启动成功...", 2);
        LogService.Info("欢迎使用翎风系列游戏软件...", 0);
        LogService.Info("网站:http://www.gameofmir.com", 0);
        LogService.Info("论坛:http://bbs.gameofmir.com", 0);
        base.StartAsync(cancellationToken);
        return Task.CompletedTask;
    }

    private void Initialization()
    {
        _serverManager.Initialization();
        _clientManager.Initialization();
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        LogService.Debug("LoginGate is stopping.");
        LogService.Info("正在停止服务...", 2);
        _serverManager.Stop();
        _clientManager.Stop();
        LogService.Info("服务停止成功...", 2);
        return base.StopAsync(cancellationToken);
    }
}