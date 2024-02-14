public class AppService : IHostedLifecycleService
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

    public Task StartingAsync(CancellationToken cancellationToken)
    {
        GateShare.Initialization();
        _configManager.LoadConfig();
        _serverManager.Initialization();
        _clientManager.Initialization();
        return Task.CompletedTask;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _serverManager.ProcessLoginMessage(cancellationToken);
        _clientManager.ProcessSendMessage(cancellationToken);
        return Task.CompletedTask;
    }

    public Task StartedAsync(CancellationToken cancellationToken)
    {
        _serverManager.Start();
        _clientManager.Start();
        LogService.Info("服务已启动成功...");
        LogService.Info("欢迎使用翎风系列游戏软件...");
        LogService.Info("网站:http://www.gameofmir.com");
        LogService.Info("论坛:http://bbs.gameofmir.com");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        LogService.Info("正在停止服务...");
        _serverManager.Stop();
        _clientManager.Stop();
        LogService.Info("服务停止成功...");
        return Task.CompletedTask;
    }

    public Task StoppingAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StoppedAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}