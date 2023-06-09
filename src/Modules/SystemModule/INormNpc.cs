using SystemModule.Data;

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

        void Click(IActor actor);

        void GotoLable(IPlayerActor PlayObject, string sLabel, bool boExtJmp);

        void GotoLable(IPlayerActor PlayObject, string sLabel, bool boExtJmp, string sMsg);

        void OnEnvirnomentChanged();

        void LoadNpcData();
    }
}