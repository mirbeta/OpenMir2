using System;
using System.Collections;
namespace M2Server
{
    public struct TQuickID
    {
        public string[] sAccount;
        // 0x00
        public string[] sChrName;
        // 0x15
        public int nIndex;
        // 0x34
        public int nSelectID;
    } // end TQuickID

    public class TQuickList: ArrayList
    {
        public bool boCaseSensitive
        {
          get {
            return GetCaseSensitive();
          }
          set {
            SetCaseSensitive(value);
          }
        }
        private TRTLCriticalSection CriticalSection = null;

        public int GetIndex(string sName)
        {
            int result;
            // 0x0045B498
            int nLow;
            int nHigh;
            int nMed;
            int nCompareVal;
            string s;
            result =  -1;
            if (this.Count != 0)
            {
               
                if (this.Sorted)
                {
                    if (this.Count == 1)
                    {
                        if (sName.CompareTo(this[0]) == 0)
                        {
                            result = 0;
                        }
                    // - > 0x0045B71D
                    }
                    else
                    {
                        // 0x0045B51E
                        nLow = 0;
                        nHigh = this.Count - 1;
                        nMed = (nHigh - nLow) / 2 + nLow;
                        while (true)
                        {
                            if ((nHigh - nLow) == 1)
                            {
                                if (sName.CompareTo(this[nHigh]) == 0)
                                {
                                    result = nHigh;
                                }
                                if (sName.CompareTo(this[nLow]) == 0)
                                {
                                    result = nLow;
                                }
                                break;
                            // - > 0x0045B71D
                            }
                            else
                            {
                                // 0x0045B59A
                                nCompareVal = sName.CompareTo(this[nMed]);
                                if (nCompareVal > 0)
                                {
                                    nLow = nMed;
                                    nMed = (nHigh - nLow) / 2 + nLow;
                                    continue;
                                }
                                // 0x0045B5DA
                                if (nCompareVal < 0)
                                {
                                    nHigh = nMed;
                                    nMed = (nHigh - nLow) / 2 + nLow;
                                    continue;
                                }
                                result = nMed;
                                break;
                            }
                        // 0x0045B609
                        }
                    }
                }
                else
                {
                    // 0x0045B609
                    if (this.Count == 1)
                    {
                        if ((sName).ToLower().CompareTo((this[0]).ToLower()) == 0)
                        {
                            result = 0;
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
                                if ((sName).ToLower().CompareTo((this[nHigh]).ToLower()) == 0)
                                {
                                    result = nHigh;
                                }
                                if ((sName).ToLower().CompareTo((this[nLow]).ToLower()) == 0)
                                {
                                    result = nLow;
                                }
                                break;
                            }
                            else
                            {
                                // 0x0045B6B3
                                nCompareVal = (sName).ToLower().CompareTo((this[nMed]).ToLower());
                                if (nCompareVal > 0)
                                {
                                    nLow = nMed;
                                    nMed = (nHigh - nLow) / 2 + nLow;
                                    continue;
                                }
                                if (nCompareVal < 0)
                                {
                                    nHigh = nMed;
                                    nMed = (nHigh - nLow) / 2 + nLow;
                                    continue;
                                }
                                result = nMed;
                                break;
                            }
                        }
                    }
                }
            // 0x0045B71D
            }
            return result;
        }

        public void SortString(int nMin, int nMax)
        {
            // 0x0045AF78
            int ntMin;
            int ntMax;
            string s18;
            if (this.Count > 0)
            {
                while (true)
                {
                    ntMin = nMin;
                    ntMax = nMax;
                    s18 = this[(nMin + nMax) >> 1];
                    while (true)
                    {
                        while (((this[ntMin]).ToLower().CompareTo((s18).ToLower()) < 0))
                        {
                            ntMin ++;
                        }
                        while (((this[ntMax]).ToLower().CompareTo((s18).ToLower()) > 0))
                        {
                            ntMax -= 1;
                        }
                        if (ntMin <= ntMax)
                        {
                           
                            this.Exchange(ntMin, ntMax);
                            ntMin ++;
                            ntMax -= 1;
                        }
                        if (ntMin > ntMax)
                        {
                            break;
                        }
                    }
                    if (nMin < ntMax)
                    {
                        SortString(nMin, ntMax);
                    }
                    nMin = ntMin;
                    if (ntMin >= nMax)
                    {
                        break;
                    }
                }
            }
        }

        public bool AddRecord(string sName, int nIndex)
        {
            bool result;
            // 0x0045B0D0
            int nLow;
            int nHigh;
            int nMed;
            int nCompareVal;
            result = true;
            if (this.Count == 0)
            {
                this.Add(sName, ((nIndex) as Object));
            }
            else
            {
                // 0x0045B133
               
                if (this.Sorted)
                {
                    if (this.Count == 1)
                    {
                        nMed = sName.CompareTo(this[0]);
                        if (nMed > 0)
                        {
                            this.Add(sName, ((nIndex) as Object));
                        }
                        else
                        {
                            if (nMed < 0)
                            {
                               
                                this.InsertObject(0, sName, ((nIndex) as Object));
                            }
                        }
                    }
                    else
                    {
                        // 0x0045B19F
                        nLow = 0;
                        nHigh = this.Count - 1;
                        nMed = (nHigh - nLow) / 2 + nLow;
                        while (true)
                        {
                            if ((nHigh - nLow) == 1)
                            {
                                nMed = sName.CompareTo(this[nHigh]);
                                if (nMed > 0)
                                {
                                   
                                    this.InsertObject(nHigh + 1, sName, ((nIndex) as Object));
                                    break;
                                }
                                else
                                {
                                    nMed = sName.CompareTo(this[nLow]);
                                    if (nMed > 0)
                                    {
                                       
                                        this.InsertObject(nLow + 1, sName, ((nIndex) as Object));
                                        break;
                                    }
                                    else
                                    {
                                        if (nMed < 0)
                                        {
                                            this.InsertObject(nLow, sName, ((nIndex) as Object));
                                            break;
                                        }
                                        else
                                        {
                                            result = false;
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                // 0x0045B26A
                                nCompareVal = sName.CompareTo(this[nMed]);
                                if (nCompareVal > 0)
                                {
                                    nLow = nMed;
                                    nMed = (nHigh - nLow) / 2 + nLow;
                                    continue;
                                }
                                if (nCompareVal < 0)
                                {
                                    nHigh = nMed;
                                    nMed = (nHigh - nLow) / 2 + nLow;
                                    continue;
                                }
                                result = false;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    if (this.Count == 1)
                    {
                        nMed = (sName).ToLower().CompareTo((this[0]).ToLower());
                        if (nMed > 0)
                        {
                            this.Add(sName, ((nIndex) as Object));
                        }
                        else
                        {
                            if (nMed < 0)
                            {
                               
                                this.InsertObject(0, sName, ((nIndex) as Object));
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
                                nMed = (sName).ToLower().CompareTo((this[nHigh]).ToLower());
                                if (nMed > 0)
                                {
                                   
                                    this.InsertObject(nHigh + 1, sName, ((nIndex) as Object));
                                    break;
                                }
                                else
                                {
                                    nMed = (sName).ToLower().CompareTo((this[nLow]).ToLower());
                                    if (nMed > 0)
                                    {
                                       
                                        this.InsertObject(nLow + 1, sName, ((nIndex) as Object));
                                        break;
                                    }
                                    else
                                    {
                                        if (nMed < 0)
                                        {
                                           
                                            this.InsertObject(nLow, sName, ((nIndex) as Object));
                                            break;
                                        }
                                        else
                                        {
                                            result = false;
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                // 0x0045B26A
                                nCompareVal = (sName).ToLower().CompareTo((this[nMed]).ToLower());
                                if (nCompareVal > 0)
                                {
                                    nLow = nMed;
                                    nMed = (nHigh - nLow) / 2 + nLow;
                                    continue;
                                }
                                if (nCompareVal < 0)
                                {
                                    nHigh = nMed;
                                    nMed = (nHigh - nLow) / 2 + nLow;
                                    continue;
                                }
                                result = false;
                                break;
                            }
                        }
                    }
                }
            }
            return result;
        }

        private bool GetCaseSensitive()
        {
            bool result;
           
            result = this.CaseSensitive;
            return result;
        }

        private void SetCaseSensitive(bool Value)
        {
           
            this.CaseSensitive = Value;
        }

        public void __Lock()
        {
            //@ Unsupported function or procedure: 'EnterCriticalSection'
            EnterCriticalSection(CriticalSection);
        }

        public void UnLock()
        {
            //@ Unsupported function or procedure: 'LeaveCriticalSection'
            LeaveCriticalSection(CriticalSection);
        }

        //Constructor  Create()
        public TQuickList() : base()
        {
            //@ Unsupported function or procedure: 'InitializeCriticalSection'
            InitializeCriticalSection(CriticalSection);
        }
        //@ Destructor  Destroy()
        ~TQuickList()
        {
            //@ Unsupported function or procedure: 'DeleteCriticalSection'
            DeleteCriticalSection(CriticalSection);
            // base.Destroy();
        }
    } // end TQuickList

    public class TQuickIDList: ArrayList
    {
        //@ Constructor auto-generated 
        public TQuickIDList(Int32 capacity)
            :base(capacity)
        {
        }
        //@ Constructor auto-generated 
        public TQuickIDList(ICollection c)
            :base(c)
        {
        }
        // TQuickIDList
        public void AddRecord(string sAccount, string sChrName, int nIndex, int nSelIndex)
        {
            // 0x0045B750
            TQuickID QuickID;
            ArrayList ChrList;
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
                ChrList = new ArrayList();
                ChrList.Add(QuickID);
                this.Add(sAccount, ChrList);
            }
            else
            {
                // 0x0045B839
                if (this.Count == 1)
                {
                    nMed = sAccount.CompareTo(this[0]);
                    if (nMed > 0)
                    {
                        ChrList = new ArrayList();
                        ChrList.Add(QuickID);
                        this.Add(sAccount, ChrList);
                    }
                    else
                    {
                        // 0x0045B89C
                        if (nMed < 0)
                        {
                            ChrList = new ArrayList();
                            ChrList.Add(QuickID);
                           
                            this.InsertObject(0, sAccount, ChrList);
                        }
                        else
                        {
                           
                            ChrList = ((this.Values[0]) as ArrayList);
                            ChrList.Add(QuickID);
                        }
                    }
                }
                else
                {
                    // 0x0045B8EF
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
                                ChrList = new ArrayList();
                                ChrList.Add(QuickID);
                               
                                this.InsertObject(nHigh + 1, sAccount, ChrList);
                                break;
                            }
                            else
                            {
                                if (sAccount.CompareTo(this[nHigh]) == 0)
                                {
                                   
                                    ChrList = ((this.Values[nHigh]) as ArrayList);
                                    ChrList.Add(QuickID);
                                    break;
                                }
                                else
                                {
                                    // 0x0045B9BB
                                    n20 = sAccount.CompareTo(this[nLow]);
                                    if (n20 > 0)
                                    {
                                        ChrList = new ArrayList();
                                        ChrList.Add(QuickID);
                                       
                                        this.InsertObject(nLow + 1, sAccount, ChrList);
                                        break;
                                    }
                                    else
                                    {
                                        if (n20 < 0)
                                        {
                                            ChrList = new ArrayList();
                                            ChrList.Add(QuickID);
                                           
                                            this.InsertObject(nLow, sAccount, ChrList);
                                            break;
                                        }
                                        else
                                        {
                                           
                                            ChrList = ((this.Values[n20]) as ArrayList);
                                            ChrList.Add(QuickID);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            // 0x0045BA6A
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
                           
                            ChrList = ((this.Values[nMed]) as ArrayList);
                            ChrList.Add(QuickID);
                            break;
                        }
                    }
                }
            }
        }

        public void DelRecord(int nIndex, string sChrName)
        {
            // 0x0045BCEC
            TQuickID QuickID;
            ArrayList ChrList;
            int i;
            if ((this.Count - 1) < nIndex)
            {
                return;
            }
           
            ChrList = ((this.Values[nIndex]) as ArrayList);
            for (i = 0; i < ChrList.Count; i ++ )
            {
                QuickID = ChrList[i];
                if (QuickID.sChrName == sChrName)
                {
                    //@ Unsupported function or procedure: 'Dispose'
                    Dispose(QuickID);
                    ChrList.RemoveAt(i);
                    break;
                }
            }
            if (ChrList.Count <= 0)
            {
               
                ChrList.Free;
                this.Remove(nIndex);
            }
        }

        public int GetChrList(string sAccount, ref ArrayList ChrNameList)
        {
            int result;
            // 0x0045BB28
            int nHigh;
            int nLow;
            int nMed;
            int n20;
            int n24;
            result =  -1;
            if (this.Count == 0)
            {
                return result;
            }
            if (this.Count == 1)
            {
                if (sAccount.CompareTo(this[0]) == 0)
                {
                   
                    ChrNameList = ((this.Values[0]) as ArrayList);
                    result = 0;
                }
            }
            else
            {
                // 0x0045BBB7
                nLow = 0;
                nHigh = this.Count - 1;
                nMed = (nHigh - nLow) / 2 + nLow;
                n24 =  -1;
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
                if (n24 !=  -1)
                {
                   
                    ChrNameList = ((this.Values[n24]) as ArrayList);
                }
                result = n24;
            }
            return result;
        }

    } // end TQuickIDList

}

