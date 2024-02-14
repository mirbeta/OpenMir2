using SystemModule.Actors;

namespace ScriptSystem
{
    public interface IScriptParsers
    {
        void LoadScript(INormNpc NPC, string sPatch, string scriptName);

        void LoadScriptFile(INormNpc NPC, string sPatch, string sScritpName, bool boFlag);
    }
}