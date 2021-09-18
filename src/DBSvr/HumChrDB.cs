using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemModule.Common;

namespace DBSvr
{
    public class HumChrDB
    {

        public bool Open()
        {
            return true;
        }

        public int ChrCountOfAccount(string sAccount)
        {
            return 0;
        }

        public int FindByAccount(string sAccount, ref IList<TQuickID> ChrList)
        {
            return 0;
        }

        public bool Add(ref THumInfo humRecord)
        {
            return true;
        }

        public int Index(string sChrName)
        {
            return 0;
        }

        public void Get(int n08, ref THumInfo HumDBRecord)
        {

        }

        public bool Update(int n08, ref THumInfo HumDBRecord)
        {
            return true;
        }
    }
}
