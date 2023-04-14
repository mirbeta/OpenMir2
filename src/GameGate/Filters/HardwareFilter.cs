using GameGate.Conf;
using System.Collections.Generic;
using System.IO;
using SystemModule;

namespace GameGate.Filters
{
    public class HardwareFilter
    {
        private readonly IList<HardwareCnt> m_xCurList = null;
        private readonly IList<HardwareCnt> m_xDenyList = null;
        private ConfigManager _configManager => ConfigManager.Instance;

        public HardwareFilter()
        {
            m_xCurList = new List<HardwareCnt>();
            m_xDenyList = new List<HardwareCnt>();
        }

        public int AddDeny(byte[] HWID)
        {
            HardwareCnt pHWIDCnt;
            int result = -1;
            for (var i = 0; i < m_xDenyList.Count; i++)
            {
                pHWIDCnt = m_xDenyList[i];
                if (MD5.MD5Match(pHWIDCnt.HWID, HWID))
                {
                    result = i;
                    return result;
                }
            }
            pHWIDCnt = new HardwareCnt();
            pHWIDCnt.HWID = HWID;
            pHWIDCnt.Count = 0;
            m_xDenyList.Add(pHWIDCnt);
            return 1;
        }

        public int DelDeny(byte[] HWID)
        {
            HardwareCnt pHWIDCnt;
            int result = -1;
            for (var i = 0; i < m_xDenyList.Count; i++)
            {
                pHWIDCnt = m_xDenyList[i];
                if (MD5.MD5Match(pHWIDCnt.HWID, HWID))
                {
                    pHWIDCnt = null;
                    m_xDenyList.RemoveAt(i);
                    result = i;
                    break;
                }
            }
            return result;
        }

        public void ClearDeny()
        {
            m_xDenyList.Clear();
        }

        public void LoadDenyList()
        {
            var ls = new SystemModule.Common.StringList();
            if (!File.Exists(_configManager.GateConfig.BlockHWIDFileName))
            {
                ls.SaveToFile(_configManager.GateConfig.BlockHWIDFileName);
            }
            ls.LoadFromFile(_configManager.GateConfig.BlockHWIDFileName);
            for (var i = 0; i < ls.Count; i++)
            {
                if (string.IsNullOrEmpty(ls[i]) || ls[i][0] == ';' || ls[i].Length != 32)
                {
                    continue;
                }
                AddDeny(MD5.MD5UnPrInt(ls[i]));
            }
        }

        public void SaveDenyList()
        {
            //ArrayList ls;
            //THWIDCnt pHWIDCnt;
            //ls = new ArrayList();
            //for (var i = 0; i < m_xDenyList.Count; i++)
            //{
            //    pHWIDCnt = ((m_xDenyList[i]) as THWIDCnt);
            //    ls.Add(MD5.Units.MD5.MD5Print(pHWIDCnt.HWID));
            //}
            //ls.SaveToFile(ConfigManager.Units.ConfigManager.g_pConfig.m_szBlockHWIDFileName);
            //ls.Free;
        }

        public bool IsFilter(byte[] HWID)
        {
            HardwareCnt pHWIDCnt;
            bool result = false;
            for (var i = 0; i < m_xDenyList.Count; i++)
            {
                pHWIDCnt = m_xDenyList[i];
                if (MD5.MD5Match(pHWIDCnt.HWID, HWID))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        public bool IsFilter(byte[] HWID, ref bool fOverClientCount)
        {
            HardwareCnt pHWIDCnt;
            bool result = false;
            var fMatch = false;
            for (var i = 0; i < m_xCurList.Count; i++)
            {
                pHWIDCnt = m_xCurList[i];
                if (MD5.MD5Match(pHWIDCnt.HWID, HWID))
                {
                    if (pHWIDCnt.Count + 1 > _configManager.GateConfig.MaxClientCount)
                    {
                        result = true;
                        fOverClientCount = true;
                    }
                    else
                    {
                        pHWIDCnt.Count++;
                    }
                    fMatch = true;
                    break;
                }
            }
            if (!fMatch)
            {
                pHWIDCnt = new HardwareCnt();
                pHWIDCnt.HWID = HWID;
                pHWIDCnt.Count = 1;
                m_xCurList.Add(pHWIDCnt);
            }
            if (!result)
            {
                for (var i = 0; i < m_xDenyList.Count; i++)
                {
                    pHWIDCnt = m_xDenyList[i];
                    if (MD5.MD5Match(pHWIDCnt.HWID, HWID))
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }

        public int GetItemCount(byte[] HWID)
        {
            HardwareCnt pHWIDCnt;
            int result = 0;
            for (var i = 0; i < m_xCurList.Count; i++)
            {
                pHWIDCnt = m_xCurList[i];
                if (MD5.MD5Match(pHWIDCnt.HWID, HWID))
                {
                    result = pHWIDCnt.Count;
                    break;
                }
            }
            return result;
        }

        public void DecHWIDCount(byte[] HWID)
        {
            HardwareCnt pHWIDCnt;
            for (var i = 0; i < m_xCurList.Count; i++)
            {
                pHWIDCnt = m_xCurList[i];
                if (MD5.MD5Match(pHWIDCnt.HWID, HWID))
                {
                    if (pHWIDCnt.Count > 0)
                    {
                        pHWIDCnt.Count -= 1;
                    }
                    if (pHWIDCnt.Count == 0)
                    {
                        pHWIDCnt = null;
                        m_xCurList.RemoveAt(i);
                    }
                    break;
                }
            }
        }

        public void ClearHWIDCount()
        {
            m_xCurList.Clear();
        }
    }
}
