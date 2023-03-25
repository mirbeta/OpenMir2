using MQTTnet;
using MQTTnet.Client;
using NLog;

namespace GameSrv.Services
{
    /// <summary>
    /// 公共聊天频道服务类
    /// 简单的设计，后续需要根据聊天频道架构进行修改
    /// </summary>
    public class ChatChannelService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly MqttFactory mqttFactory;
        private readonly IMqttClient chatClient;

        public ChatChannelService()
        {
            mqttFactory = new MqttFactory();
            chatClient = mqttFactory.CreateMqttClient();
            chatClient.DisconnectedAsync += ClientDisconnectedAsync;
            chatClient.ConnectedAsync += ClientConnectedAsync;
        }

        private Task ClientConnectedAsync(MqttClientConnectedEventArgs arg)
        {
            logger.Info("链接世界聊天频道服务器成功...");
            return Task.CompletedTask;
        }

        public bool IsEnableChatServer
        {
            get { return M2Share.Config.EnableChatServer; }
        }

        public async Task Start()
        {
            if (!IsEnableChatServer)
            {
                return;
            }
            logger.Info("开始链接世界聊天频道...");
            var chatClientOptions = new MqttClientOptionsBuilder().WithTcpServer(M2Share.Config.ChatSrvAddr, M2Share.Config.ChatSrvPort).Build();
            try
            {
                using var timeoutToken = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                var response = await chatClient.ConnectAsync(chatClientOptions, CancellationToken.None);
                if (response.ResultCode == MqttClientConnectResultCode.Success)
                {
                    logger.Info("链接世界聊天频道成功...");
                }
                else
                {
                    logger.Info("链接世界聊天频道失败...");
                }
            }
            catch (OperationCanceledException)
            {
                logger.Warn("链接世界聊天频道超时,请确认配置是否正确.");
                return;
            }
            logger.Info("链接世界聊天频道初始化完成...");
        }

        private async Task ClientDisconnectedAsync(MqttClientDisconnectedEventArgs arg)
        {
            if (arg.ClientWasConnected)
            {
                await chatClient.ConnectAsync(chatClient.Options);
            }
            logger.Info("与世界聊天频道失去链接...");
        }

        public async Task Stop()
        {
            logger.Info("断开世界聊天频道...");
            await chatClient.DisconnectAsync(new MqttClientDisconnectOptionsBuilder().WithReason(MqttClientDisconnectReason.NormalDisconnection).Build());
        }

        public async Task Ping()
        {
            if (!IsEnableChatServer)
            {
                return;
            }
            if (!await chatClient.TryPingAsync())
            {
                using (var timeout = new CancellationTokenSource(5000))
                {
                    await chatClient.ConnectAsync(chatClient.Options, timeout.Token);
                }
                logger.Info("与世界聊天频道失去链接...");
            }
        }

        /// <summary>
        /// 发送公共频道消息（世界频道）
        /// 所有玩家可见
        /// </summary>
        /// <param name="sendMsg"></param>
        public void SendPubChannelMessage(string sendMsg)
        {
            //todo 需要对消息加密处理
            chatClient.PublishStringAsync("mir/chat", sendMsg);
        }
    }
}