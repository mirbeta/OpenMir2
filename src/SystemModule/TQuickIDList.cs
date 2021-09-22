using System.Collections;
using System.Collections.Generic;

namespace SystemModule
{
    public class TQuickIDList : ArrayList
    {
        private readonly Dictionary<string, IList<TQuickID>> m_List = new Dictionary<string, IList<TQuickID>>();

        public void AddRecord(string sAccount, string sChrName, int nIndex, int nSelIndex)
        {
            TQuickID QuickID;
            IList<TQuickID> ChrList;
            int nLow;
            int nHigh;
            int nMed;
            int n1C;
            int n20;
            QuickID = new TQuickID();
            QuickID.sAccount = sAccount;
            QuickID.sChrName = sChrName;
            QuickID.nIndex = nIndex;
            QuickID.nSelectID = nSelIndex;
            if (this.Count == 0)
            {
                ChrList = new List<TQuickID>();
                ChrList.Add(QuickID);
                m_List.Add(sAccount, ChrList);
            }
            else
            {
                if (this.Count == 1)
                {
                    nMed = sAccount.CompareTo(this[0]);
                    if (nMed > 0)
                    {
                        ChrList = new List<TQuickID>();
                        ChrList.Add(QuickID);
                        m_List.Add(sAccount, ChrList);
                    }
                    else
                    {
                        if (nMed < 0)
                        {
                            ChrList = new List<TQuickID>();
                            ChrList.Add(QuickID);
                            m_List[sAccount].Add(QuickID);
                            //m_List.Add(sAccount, ChrList);
                        }
                        else
                        {
                            ChrList = this[0] as List<TQuickID>;
                            ChrList.Add(QuickID);
                        }
                    }
                }
                else
                {
                    nLow = 0;
                    nHigh = this.Count - 1;
                    nMed = (nHigh - nLow) / 2 + nLow;
                    while (true)
                    {
                        if ((nHigh - nLow) == 1)
                        {
                            n20 = sAccount.CompareTo(this[nHigh]);
                            if (n20 > 0)
                            {
                                ChrList = new List<TQuickID>();
                                ChrList.Add(QuickID);
                                //this.InsertObject(nHigh + 1, sAccount, ChrList);
                                m_List[sAccount].Add(QuickID);
                                break;
                            }
                            else
                            {
                                if (sAccount.CompareTo(this[nHigh]) == 0)
                                {
                                    ChrList = this[nHigh] as List<TQuickID>;
                                    ChrList.Add(QuickID);
                                    break;
                                }
                                else
                                {
                                    n20 = sAccount.CompareTo(this[nLow]);
                                    if (n20 > 0)
                                    {
                                        ChrList = new List<TQuickID>();
                                        ChrList.Add(QuickID);
                                        //this.InsertObject(nLow + 1, sAccount, ChrList);
                                        m_List[sAccount].Add(QuickID);
                                        break;
                                    }
                                    else
                                    {
                                        if (n20 < 0)
                                        {
                                            ChrList = new List<TQuickID>();
                                            ChrList.Add(QuickID);
                                            //this.InsertObject(nLow, sAccount, ChrList);
                                            m_List[sAccount].Add(QuickID);
                                            break;
                                        }
                                        else
                                        {
                                            ChrList = this[n20] as List<TQuickID>;
                                            ChrList.Add(QuickID);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            n1C = sAccount.CompareTo(this[nMed]);
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
                            ChrList = this[nMed] as List<TQuickID>;
                            ChrList.Add(QuickID);
                            break;
                        }
                    }
                }
            }
        }

        public void DelRecord(int nIndex, string sChrName)
        {
            TQuickID QuickID;
            IList<TQuickID> ChrList;
            if ((this.Count - 1) < nIndex)
            {
                return;
            }
            ChrList = this[nIndex] as List<TQuickID>;
            for (var i = 0; i < ChrList.Count; i++)
            {
                QuickID = ChrList[i];
                if (QuickID.sChrName == sChrName)
                {
                    QuickID = null;
                    ChrList.RemoveAt(i);
                    break;
                }
            }
            if (ChrList.Count <= 0)
            {
                //ChrList.Free;
                this.Remove(nIndex);
            }
        }

        public int GetChrList(string sAccount, ref IList<TQuickID> ChrNameList)
        {
            int result;
            int nHigh;
            int nLow;
            int nMed;
            int n20;
            int n24;
            result = -1;
            if (this.Count == 0)
            {
                return result;
            }
            if (this.Count == 1)
            {
                if (sAccount.CompareTo(this[0]) == 0)
                {
                    ChrNameList = this[0] as List<TQuickID>;
                    result = 0;
                }
            }
            else
            {
                nLow = 0;
                nHigh = this.Count - 1;
                nMed = (nHigh - nLow) / 2 + nLow;
                n24 = -1;
                while (true)
                {
                    if ((nHigh - nLow) == 1)
                    {
                        if (sAccount.CompareTo(this[nHigh]) == 0)
                        {
                            n24 = nHigh;
                        }
                        if (sAccount.CompareTo(this[nLow]) == 0)
                        {
                            n24 = nLow;
                        }
                        break;
                    }
                    else
                    {
                        n20 = sAccount.CompareTo(this[nMed]);
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
                    ChrNameList = this[n24] as List<TQuickID>;
                }
                result = n24;
            }
            return result;
        }
    }

    public class TQuickID
    {
        public int nSelectID;
        public string sAccount;
        public int nIndex;
        public string sChrName;

        public TQuickID() { }

        public TQuickID(string account, int index)
        {
            sAccount = account;
            nIndex = index;
        }
    }
}