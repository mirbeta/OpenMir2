namespace SystemModule
{
    public interface INormNpc : IActor
    {
        bool IsHide { get; set; }

        string Path { get; }

        int ProcessRefillIndex { get; set; }

        void AddScript(ScriptInfo scriptInfo);

        IList<ScriptInfo> ScriptList { get; }

        void Initialize();

        void Run();

        void SendSayMsg(string sText);

        void UserSelect(IPlayerActor actor, string sText);

        void Click(IPlayerActor actor);

        void GotoLable(IPlayerActor PlayObject, string sLabel, bool boExtJmp = false);

        void LoadNPCScript();

        void ClearScript();

        string GetLineVariableText(IPlayerActor PlayObject, string sMsg);
    }
}