using SystemModule;

namespace ScriptSystem
{
    public interface IScriptEngine
    {
        void GotoLable(IPlayerActor playerActor, int npcId, string sLabel, bool boExtJmp = false);

        void GotoLableGiveItem(IPlayerActor playerActor, string sItemName, int nItemCount);
    }
}