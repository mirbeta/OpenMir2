using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemModule{
    public interface IMapSystem
    {
        public int GetMapOfServerIndex(string mapName)
        {
            return 0;
        }

        public IEnvirnoment FindMap(string mapName)
        {
            return null;
        }
    }
}
