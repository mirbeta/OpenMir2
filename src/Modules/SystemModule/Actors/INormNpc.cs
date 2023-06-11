namespace SystemModule
{
    public interface INormNpc : IActor
    {
        bool IsHide { get; set; }

        string m_sPath { get; set; }

        int ProcessRefillIndex { get; set; }

        //IList<ScriptInfo> GetScriptList();

        void SendSayMsg(string sText);

        void UserSelect(IPlayerActor actor, string sText);

        void Click(IPlayerActor actor);

        void GotoLable(IPlayerActor PlayObject, string sLabel, bool boExtJmp);

        void GotoLable(IPlayerActor PlayObject, string sLabel, bool boExtJmp, string sMsg);
        
        void LoadNPCScript();

        void ClearScript();

        string GetLineVariableText(IPlayerActor PlayObject, string sMsg);
    }
}