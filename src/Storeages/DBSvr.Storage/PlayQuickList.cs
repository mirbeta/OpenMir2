using DBSvr.Storage.Model;
using System;
using System.Collections.Generic;

namespace DBSvr.Storage
{
    public class PlayQuickList
    {
        private readonly Dictionary<string, IList<PlayQuick>> m_List = new Dictionary<string, IList<PlayQuick>>();
        private readonly IList<PlayQuick> quickList;

        public PlayQuickList()
        {
            quickList = new List<PlayQuick>();
        }

        public void Clear()
        {
            m_List.Clear();
            quickList.Clear();
        }

        public void AddRecord(string sAccount, string sChrName, int nIndex, int nSelIndex)
        {
            IList<PlayQuick> ChrList;
            var playQuick = new PlayQuick();
            playQuick.Account = sAccount;
            playQuick.ChrName = sChrName;
            playQuick.Index = nIndex;
            playQuick.SelectID = nSelIndex;
            if (quickList.Count == 0)
            {
                ChrList = new List<PlayQuick>();
                ChrList.Add(playQuick);
                m_List.Add(sAccount, ChrList);
                quickList.Add(playQuick);
            }
            else if (!m_List.ContainsKey(sAccount))
            {
                ChrList = new List<PlayQuick>();
                ChrList.Add(playQuick);
                m_List.Add(sAccount, ChrList);
                quickList.Add(playQuick);
            }
            else
            {
                int nMed;
                if (m_List.Count == 1)
                {
                    nMed = string.Compare(sAccount, quickList[0].Account, StringComparison.OrdinalIgnoreCase);
                    if (nMed > 0)
                    {
                        ChrList = new List<PlayQuick>();
                        ChrList.Add(playQuick);
                        m_List.Add(sAccount, ChrList);
                        quickList.Add(playQuick);
                    }
                    else
                    {
                        if (nMed < 0)
                        {
                            ChrList = new List<PlayQuick>();
                            ChrList.Add(playQuick);
                            m_List[sAccount].Add(playQuick);
                            quickList.Add(playQuick);
                            //m_List.Add(sAccount, ChrList);
                        }
                        else
                        {
                            ChrList = m_List[quickList[0].Account];
                            ChrList.Add(playQuick);
                        }
                    }
                }
                else
                {
                    var nLow = 0;
                    var nHigh = m_List.Count - 1;
                    nMed = (nHigh - nLow) / 2 + nLow;
                    while (true)
                    {
                        if ((nHigh - nLow) == 1)
                        {
                            var n20 = string.Compare(sAccount, quickList[nHigh].Account, StringComparison.OrdinalIgnoreCase);
                            if (n20 > 0)
                            {
                                ChrList = new List<PlayQuick>();
                                ChrList.Add(playQuick);
                                //this.InsertObject(nHigh + 1, sAccount, ChrList);
                                m_List[sAccount].Add(playQuick);
                                quickList.Add(playQuick);
                                break;
                            }
                            else
                            {
                                if (string.Compare(sAccount, quickList[nHigh].Account, StringComparison.OrdinalIgnoreCase) == 0)
                                {
                                    ChrList = m_List[quickList[nHigh].Account];
                                    ChrList.Add(playQuick);
                                    quickList.Add(playQuick);
                                    break;
                                }
                                else
                                {
                                    n20 = string.Compare(sAccount, quickList[nLow].Account, StringComparison.OrdinalIgnoreCase);
                                    if (n20 > 0)
                                    {
                                        ChrList = new List<PlayQuick>();
                                        ChrList.Add(playQuick);
                                        //this.InsertObject(nLow + 1, sAccount, ChrList);
                                        m_List[sAccount].Add(playQuick);
                                        quickList.Add(playQuick);
                                        break;
                                    }
                                    else
                                    {
                                        if (n20 < 0)
                                        {
                                            ChrList = new List<PlayQuick>();
                                            ChrList.Add(playQuick);
                                            //this.InsertObject(nLow, sAccount, ChrList);
                                            m_List[sAccount].Add(playQuick);
                                            quickList.Add(playQuick);
                                            break;
                                        }
                                        else
                                        {
                                            ChrList = m_List[quickList[n20].Account];
                                            ChrList.Add(playQuick);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            var n1C = string.Compare(sAccount, quickList[nMed].Account, StringComparison.OrdinalIgnoreCase);
                            if (n1C > 0)
                            {
                                nLow = nMed;
                                nMed = (nHigh - nLow) / 2 + nLow;
                                continue;
                            }
                            if (n1C < 0)
                            {
                                nHigh = nMed;
                                nMed = (nHigh - nLow) / 2 + nLow;
                                continue;
                            }
                            ChrList = m_List[quickList[nMed].Account];
                            ChrList.Add(playQuick);
                            break;
                        }
                    }
                }
            }
        }

        public void DelRecord(int nIndex, string sChrName)
        {
            if ((m_List.Count - 1) < nIndex)
            {
                return;
            }
            var ChrList = m_List[sChrName];
            for (var i = 0; i < ChrList.Count; i++)
            {
                var QuickID = ChrList[i];
                if (QuickID.ChrName == sChrName)
                {
                    QuickID = null;
                    ChrList.RemoveAt(i);
                    break;
                }
            }
            if (ChrList.Count <= 0)
            {
                m_List.Remove(sChrName);
            }
        }

        public int GetChrList(string sAccount, ref IList<PlayQuick> ChrNameList)
        {
            int nHigh;
            int nLow;
            int nMed;
            int n20;
            int n24;
            var result = -1;
            if (m_List.Count == 0)
            {
                return result;
            }
            if (m_List.ContainsKey(sAccount))
            {
                ChrNameList = m_List[sAccount];
                result = ChrNameList.Count;
                return result;
            }
            if (m_List.Count == 1)
            {
                if (string.Compare(sAccount, quickList[0].Account, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    ChrNameList = m_List[quickList[0].Account];
                    result = 0;
                }
            }
            else
            {
                nLow = 0;
                nHigh = m_List.Count - 1;
                nMed = (nHigh - nLow) / 2 + nLow;
                n24 = -1;
                while (true)
                {
                    if ((nHigh - nLow) == 1)
                    {
                        if (string.Compare(sAccount, quickList[nHigh].Account, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            n24 = nHigh;
                        }
                        if (string.Compare(sAccount, quickList[nLow].Account, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            n24 = nLow;
                        }
                        break;
                    }
                    else
                    {
                        n20 = string.Compare(sAccount, quickList[nMed].Account, StringComparison.OrdinalIgnoreCase);
                        if (n20 > 0)
                        {
                            nLow = nMed;
                            nMed = (nHigh - nLow) / 2 + nLow;
                            continue;
                        }
                        if (n20 < 0)
                        {
                            nHigh = nMed;
                            nMed = (nHigh - nLow) / 2 + nLow;
                            continue;
                        }
                        n24 = nMed;
                        break;
                    }
                }
                if (n24 != -1)
                {
                    ChrNameList = m_List[quickList[n24].Account];
                }
                result = n24;
            }
            return result;
        }
    }
}