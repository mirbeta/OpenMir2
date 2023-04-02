using System;
using System.Collections.Generic;
using System.Linq;

namespace SystemModule.CoreSocket;

/// <summary>
/// 控制台行为
/// </summary>
[IntelligentCoder.AsyncMethodPoster(Flags = IntelligentCoder.MemberFlags.Public)]
public partial class ConsoleAction
{
    private readonly string helpOrder;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="helpOrder">帮助信息指令，如："h|help|?"</param>
    public ConsoleAction(string helpOrder = "h|help|?")
    {
        this.helpOrder = helpOrder;

        Add(helpOrder, "帮助信息", ShowAll);

        string title = $@"

  _______                   _       _____               _          _
 |__   __|                 | |     / ____|             | |        | |
    | |  ___   _   _   ___ | |__  | (___    ___    ___ | | __ ___ | |_
    | | / _ \ | | | | / __|| '_ \  \___ \  / _ \  / __|| |/ // _ \| __|
    | || (_) || |_| || (__ | | | | ____) || (_) || (__ |   <|  __/| |_
    |_| \___/  \__,_| \___||_| |_||_____/  \___/  \___||_|\_\\___| \__|

 -------------------------------------------------------------------
     Author     :   若汝棋茗
     Version    :   {System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}
     Gitee      :   https://gitee.com/rrqm_home
     Github     :   https://github.com/rrqm
     API        :   https://www.yuque.com/rrqm/touchsocket/index
 -------------------------------------------------------------------
";
        Console.WriteLine(title);
    }

    /// <summary>
    /// 显示所有注册指令
    /// </summary>
    public void ShowAll()
    {
        int max = actions.Values.Max(a => a.FullOrder.Length) + 8;

        List<string> s = new List<string>();
        foreach (KeyValuePair<string, VAction> item in actions)
        {
            if (!s.Contains(item.Value.FullOrder.ToLower()))
            {
                s.Add(item.Value.FullOrder.ToLower());
                Console.Write($"[{item.Value.FullOrder}]");
                for (int i = 0; i < max - item.Value.FullOrder.Length; i++)
                {
                    Console.Write("-");
                }
                Console.WriteLine(item.Value.Description);
            }
        }
    }

    /// <summary>
    /// 帮助信息指令
    /// </summary>
    public string HelpOrder => helpOrder;

    private readonly Dictionary<string, VAction> actions = new Dictionary<string, VAction>();

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="order">指令，多个指令用“|”分割</param>
    /// <param name="description">描述</param>
    /// <param name="action"></param>
    public void Add(string order, string description, Action action)
    {
        string[] orders = order.ToLower().Split('|');
        foreach (string item in orders)
        {
            actions.Add(item, new VAction(description, order, action));
        }
    }

    /// <summary>
    /// 执行异常
    /// </summary>
    public event Action<Exception> OnException;

    /// <summary>
    /// 执行，返回值仅表示是否有这个指令，异常获取请使用<see cref="OnException"/>
    /// </summary>
    /// <param name="order"></param>
    /// <returns></returns>
    public bool Run(string order)
    {
        if (actions.TryGetValue(order.ToLower(), out VAction vAction))
        {
            try
            {
                vAction.Action.Invoke();
            }
            catch (Exception ex)
            {
                OnException?.Invoke(ex);
            }
            return true;
        }
        else
        {
            return false;
        }
    }
}

internal struct VAction
{
    private readonly Action action;

    public Action Action => action;

    private readonly string fullOrder;

    public string FullOrder => fullOrder;

    private readonly string description;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="action"></param>
    /// <param name="description"></param>
    /// <param name="fullOrder"></param>
    public VAction(string description, string fullOrder, Action action)
    {
        this.fullOrder = fullOrder;
        this.action = action ?? throw new ArgumentNullException(nameof(action));
        this.description = description ?? throw new ArgumentNullException(nameof(description));
    }

    public string Description => description;
}