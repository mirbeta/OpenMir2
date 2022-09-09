using DBSvr.Storage.Model;
using System;
using System.Collections.Generic;

namespace DBSvr.Storage
{
    public class QuickIdList
    {
        private readonly Dictionary<string, IList<QuickId>> m_List = new Dictionary<string, IList<QuickId>>();
        private readonly IList<QuickId> _quickList = new List<QuickId>();

        public void Clear()
        {
            m_List.Clear();
            _quickList.Clear();
        }

        public void AddRecord(string sAccount, string sChrName, int nIndex, int nSelIndex)
        {
            IList<QuickId> ChrList;
            var QuickID = new QuickId();
            QuickID.sAccount = sAccount;
            QuickID.sChrName = sChrName;
            QuickID.nIndex = nIndex;
            QuickID.nSelectID = nSelIndex;
            if (_quickList.Count == 0)
            {
                ChrList = new List<QuickId>();
                ChrList.Add(QuickID);
                m_List.Add(sAccount, ChrList);
                _quickList.Add(QuickID);
            }
            else if (!m_List.ContainsKey(sAccount))
            {
                ChrList = new List<QuickId>();
                ChrList.Add(QuickID);
                m_List.Add(sAccount, ChrList);
                _quickList.Add(QuickID);
            }
            else
            {
                int nMed;
                if (m_List.Count == 1)
                {
                    nMed = string.Compare(sAccount, _quickList[0].sAccount, StringComparison.OrdinalIgnoreCase);
                    if (nMed > 0)
                    {
                        ChrList = new List<QuickId>();
                        ChrList.Add(QuickID);
                        m_List.Add(sAccount, ChrList);
                        _quickList.Add(QuickID);
                    }
                    else
                    {
                        if (nMed < 0)
                        {
                            ChrList = new List<QuickId>();
                            ChrList.Add(QuickID);
                            m_List[sAccount].Add(QuickID);
                            _quickList.Add(QuickID);
                            //m_List.Add(sAccount, ChrList);
                        }
                        else
                        {
                            ChrList = m_List[_quickList[0].sAccount];
                            ChrList.Add(QuickID);
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
                            var n20 = string.Compare(sAccount, _quickList[nHigh].sAccount, StringComparison.OrdinalIgnoreCase);
                            if (n20 > 0)
                            {
                                ChrList = new List<QuickId>();
                                ChrList.Add(QuickID);
                                //this.InsertObject(nHigh + 1, sAccount, ChrList);
                                m_List[sAccount].Add(QuickID);
                                _quickList.Add(QuickID);
                                break;
                            }
                            else
                            {
                                if (String.Compare(sAccount, _quickList[nHigh].sAccount, StringComparison.OrdinalIgnoreCase) == 0)
                                {
                                    ChrList = m_List[_quickList[nHigh].sAccount] as List<QuickId>;
                                    ChrList.Add(QuickID);
                                    _quickList.Add(QuickID);
                                    break;
                                }
                                else
                                {
                                    n20 = string.Compare(sAccount, _quickList[nLow].sAccount, StringComparison.OrdinalIgnoreCase);
                                    if (n20 > 0)
                                    {
                                        ChrList = new List<QuickId>();
                                        ChrList.Add(QuickID);
                                        //this.InsertObject(nLow + 1, sAccount, ChrList);
                                        m_List[sAccount].Add(QuickID);
                                        _quickList.Add(QuickID);
                                        break;
                                    }
                                    else
                                    {
                                        if (n20 < 0)
                                        {
                                            ChrList = new List<QuickId>();
                                            ChrList.Add(QuickID);
                                            //this.InsertObject(nLow, sAccount, ChrList);
                                            m_List[sAccount].Add(QuickID);
                                            _quickList.Add(QuickID);
                                            break;
                                        }
                                        else
                                        {
                                            ChrList = m_List[_quickList[n20].sAccount] as List<QuickId>;
                                            ChrList.Add(QuickID);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            var n1C = string.Compare(sAccount, _quickList[nMed].sAccount, StringComparison.OrdinalIgnoreCase);
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
                            ChrList = m_List[_quickList[nMed].sAccount];
                            ChrList.Add(QuickID);
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
            IList<QuickId> ChrList = m_List[sChrName];
            for (var i = 0; i < ChrList.Count; i++)
            {
                var QuickID = ChrList[i];
                if (QuickID.sChrName == sChrName)
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

        public int GetChrList(string sAccount, ref IList<QuickId> ChrNameList)
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
                if (string.Compare(sAccount, _quickList[0].sAccount, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    ChrNameList = m_List[_quickList[0].sAccount];
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
                        if (string.Compare(sAccount, _quickList[nHigh].sAccount, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            n24 = nHigh;
                        }
                        if (string.Compare(sAccount, _quickList[nLow].sAccount, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            n24 = nLow;
                        }
                        break;
                    }
                    else
                    {
                        n20 = string.Compare(sAccount, _quickList[nMed].sAccount, StringComparison.OrdinalIgnoreCase);
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
                    ChrNameList = m_List[_quickList[n24].sAccount];
                }
                result = n24;
            }
            return result;
        }
    }
}