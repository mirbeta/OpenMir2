using SystemModule;
using SystemModule.Actors;

namespace ScriptSystem
{
    public interface IScriptEngine
    {
        void GotoLable(IPlayerActor playerActor, int actorId, string sLabel, bool boExtJmp = false);

        void GotoLable(INormNpc normNpc, IPlayerActor playerActor, string sLabel, bool boExtJmp = false);
    }
}