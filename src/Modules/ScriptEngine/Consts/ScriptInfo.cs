using System.Collections;

namespace ScriptSystem
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ScriptDefName : Attribute
    {
        public string CommandName { get; private set; }

        public ScriptDefName(string codeName)
        {
            CommandName = codeName;
        }
    }
    
    public class DefineInfo
    {
        public string Name;
        public string Text;
    }

    public class TQDDinfo
    {
        public int n00;
        public string s04;
        public ArrayList sList;
    }
}