using MakePlayer.Cliens;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using SystemModule;

namespace MakePlayer
{
    public struct RecvicePacket
    {
        public string SessionId;
        public byte[] ReviceBuffer;
    }

    public class ClientManager
    {
        private int g_dwProcessTimeMin = 0;
        private int g_dwProcessTimeMax = 0;
        private int g_nPosition = 0;
        private int dwRunTick = 0;
        private readonly ConcurrentDictionary<string, PlayClient> _clients;
        private readonly IList<PlayClient> _clientList;
        private readonly IList<string> _sayMsgList;
        private readonly Channel<RecvicePacket> _reviceQueue;
        private readonly CancellationTokenSource _cancellation;

        public ClientManager()
        {
            _clients = new ConcurrentDictionary<string, PlayClient>();
            _clientList = new List<PlayClient>();
            _sayMsgList = new List<string>();
            _reviceQueue = Channel.CreateUnbounded<RecvicePacket>();
            _cancellation = new CancellationTokenSource();
        }

        public void Start()
        {
            _sayMsgList.Add("压力测试工具");
            _sayMsgList.Add("我把自己吃这么圆,就是为了不让别人把我看扁了。");
            _sayMsgList.Add("终于知道吃奥利奥为什么要先“舔一舔”了,因为那样就没人抢了啊!");
            _sayMsgList.Add("你以为有钱人很快乐吗?错了,他们的快乐,你根本想象不到。");
            _sayMsgList.Add("既然不让谈恋爱,干脆也别发校服了,省得别人说是情侣装。");
            _sayMsgList.Add("多年前你一句珍重，我至今没瘦。");
            _sayMsgList.Add("人间有味是清欢，搞个对象行不行。");
            _sayMsgList.Add("不要在我的世界里玩火，否则你付不起这后果。");
            _sayMsgList.Add("钞票不是万能的，有的时候还需要信用卡");
            _sayMsgList.Add("真正的爱情需要等待，谁都可以说爱你，但不是人人都能等你");
            _sayMsgList.Add("做一个勇敢的人，学着去承受命运给你的每一个耳光");
            _sayMsgList.Add("我做过最勇敢的事，就是听你讲你们的故事");
            _sayMsgList.Add("向日葵自以为离不开太阳，但是它也离不开土壤");
            _sayMsgList.Add("请你骗人的时候用点技术，既然骗了，就不要让我知道真相");
            _sayMsgList.Add("别的女孩子性格好，长相好，脾气好，而我胃口好！");
            _sayMsgList.Add("心里藏着小星星，生活才能亮晶晶。");
            _sayMsgList.Add("我是小肥宅，逍遥又自在，白天不起床，晚上不睡觉。");
            _sayMsgList.Add("遗忘的痕迹，划过心底一道深深的伤痕。");
            _sayMsgList.Add("跟我混吧，以后有我一口饭吃，就有你一个碗刷。");
            _sayMsgList.Add("单身久了，连煮饺子看见两个粘在一起，我都要用铲子把它们分开");
            _sayMsgList.Add("那些说我不用减肥的人都是坏人。");
            _sayMsgList.Add("当别人说”你好娘“的时候，你可以说：你好儿子。");
            _sayMsgList.Add("生活处处是坎坷，笑到最后是大哥。");
            _sayMsgList.Add("幸亏长得丑，没经历过各位的爱恨情仇。");
            _sayMsgList.Add("减肥始终是人生第二大事，第一大事是吃好喝好。");
            _sayMsgList.Add("女生晒照的自我修养：自拍三千只取一张。");
            _sayMsgList.Add("你总说梦想遥不可及，可你却从不早睡，也不早起。");
            _sayMsgList.Add("娶一个像我这样的女人吧，虽然不倾城也不倾国，但足以让你倾家荡产。");
            _sayMsgList.Add("一个哥们结婚，给他红包。哥们客气的说不用我说：那哪行，一年就一次，一定得拿着。");
            _sayMsgList.Add("护士看到一病人在病房喝酒，就走过去小声地对他说：“小心肝！”病人微笑着说：“小宝贝。”");
            _sayMsgList.Add("有一次我向女同学借钱，本来想说的是“等我取了钱就还你”说成“等我有了钱就取你”。");
            _sayMsgList.Add("妹子：大哥什么叫禅？大哥：我这里有鸡腿你想吃吗？妹子：想。大哥：这就叫馋..");
            _sayMsgList.Add("我以后生个儿子名字要叫“好帅”，那别人看到我就会说“好帅的爸爸”。");
            _sayMsgList.Add("熊猫遇到从超市里怒气冲冲出来的袋鼠，问道：“怎么了？气成这样” 袋鼠喘着气说：“它们不许我进，非让我先存包”");
            _sayMsgList.Add("小时候，爸爸看我写作文。有个很简单的字写错了，爸爸笑着跟我妈说：“我发现你的儿子很笨。”我急了，大声跟我爸说：“你的儿子才笨！”");
            _sayMsgList.Add("老师：“你作为一名班长，看到有人在自习课上下象棋，怎么不制止？”班长：“因为观棋不语真君子！”");
            _sayMsgList.Add("“皇上，你准备把皇位传给谁？”“传。。。传。。。传太医。。。呃。。。”然后野心勃勃的太子将太医杀死，历史上第一次医患纠纷产生了。。。");
            _sayMsgList.Add("晚上十点我家熊孩子还在写作业，我：太晚了，明天再写吧！熊孩子：不行！明天耽误了女同学抄，她就不喜欢我了。。。");
            _sayMsgList.Add("有一个傻子，竟然要用苹果换我的中兴手机，我立刻答应了。然后，我把手机给了他，他从包里拿出一个又圆又大的红富士给了我！");
            _sayMsgList.Add("回到家看见大儿子正在打小儿子，我大声喝止，揪着大儿子的耳朵问道：你为什么打弟弟？大儿子不服气的说道：我帮你练小号呢。。。");
            _sayMsgList.Add("其实我挺喜欢数学的，它没有语文的迂回，没有英语的语法，没有历史政治的复杂和信息量，它有的只是不会做，不会做，和不会做。");
            _sayMsgList.Add("朋友来我家做客，看我金鱼养的非常好，问我秘诀，毕竟是我好朋友，我就把我养鱼的六字秘诀告诉他：多换水，勤换鱼!");
            _sayMsgList.Add("逛商场要走时门口保安喊我：“等一下，你衣服鼓鼓囊囊的装了什么？”我愤怒地掀起大衣吼：“是肉，是肉！我自己的。”");

            Task.Factory.StartNew(async () =>
            {
                while (await _reviceQueue.Reader.WaitToReadAsync(_cancellation.Token))
                {
                    if (_reviceQueue.Reader.TryRead(out var message))
                    {
                        if (_clients.ContainsKey(message.SessionId))
                        {
                            _clients[message.SessionId].ProcessPacket(message.ReviceBuffer);
                        }
                    }
                }
            }, _cancellation.Token);
        }

        public void Stop()
        {
            _cancellation.Cancel();
        }

        public void AddPacket(string socHandle, byte[] reviceBuff)
        {
            var clientPacket = new RecvicePacket();
            clientPacket.SessionId = socHandle;
            clientPacket.ReviceBuffer = reviceBuff;
            _reviceQueue.Writer.TryWrite(clientPacket);
        }

        public void AddClient(string sessionId, PlayClient objClient)
        {
            _clients.TryAdd(sessionId, objClient);
            _clientList.Add(objClient);
        }

        public void DelClient(PlayClient objClient)
        {
            //_Clients.Remove(objClient);
        }

        public void Run()
        {
            dwRunTick = HUtil32.GetTickCount();
            var boProcessLimit = false;
            for (var i = g_nPosition; i < _clients.Count; i++)
            {
                _clientList[i].Run();
                if (((HUtil32.GetTickCount() - dwRunTick) > 50))
                {
                    g_nPosition = i;
                    boProcessLimit = true;
                    break;
                }
                if (_clientList[i].m_boLogin && (HUtil32.GetTickCount() - _clientList[i].m_dwSayTick > 3000))
                {
                    _clientList[i].m_dwSayTick = HUtil32.GetTickCount();
                    _clientList[i].ClientLoginSay(_sayMsgList[RandomNumber.GetInstance().Random(_sayMsgList.Count)]);
                }
            }
            if (!boProcessLimit)
            {
                g_nPosition = 0;
            }
            g_dwProcessTimeMin = HUtil32.GetTickCount() - dwRunTick;
            if (g_dwProcessTimeMin > g_dwProcessTimeMax)
            {
                g_dwProcessTimeMax = g_dwProcessTimeMin;
            }
        }
    }
}