using System;
namespace M2Server
{
    public class EListError: Exception
    {
        //@ Constructor auto-generated 
        public EListError(String message)
            :base(message)
        {
        }
        //@ Constructor auto-generated 
        public EListError(String message, Exception innerException)
            :base(message, innerException)
        {
        }
    } // end EListError

    public class CList
    {
        public int Capacity
        {
          get {
            return FCapacity;
          }
          set {
            SetCapacity(value);
          }
        }
        public int Count
        {
          get {
            return FCount;
          }
          set {
            SetCount(value);
          }
        }
        public object this[int Index]
        {
          get {
            return Get(Index);
          }
          set {
            Put(Index, value);
          }
        }
        public object[] List
        {
          get {
            return FList;
          }
        }
        private object[] FList;
        private int FCount = 0;
        private int FCapacity = 0;
        // TList
        //@ Destructor  Destroy()
        ~CList()
        {
            Clear();
        }
        public int Add(object Item)
        {
            int result;
            result = FCount;
            if (result == FCapacity)
            {
                Grow();
            }
            List[result] = Item;
            FCount ++;
            if (Item != null)
            {
                Notify(Item, TListNotification.lnAdded);
            }
            return result;
        }

        public virtual void Clear()
        {
            Count = 0;
            Capacity = 0;
        }

        public void Delete(int Index)
        {
            object Temp;
            if ((Index < 0) || (Index >= FCount))
            {
                
                Error(SListIndexError, Index);
            }
            Temp = this[Index];
            FCount -= 1;
            if (Index < FCount)
            {
                //@ Unsupported function or procedure: 'Move'
                Move(List[Index + 1], List[Index], (FCount - Index) * sizeof(object));
            }
            if (Temp != null)
            {
                Notify(Temp, TListNotification.lnDeleted);
            }
        }

        public object Error_ReturnAddr()
        {
            object result;
            // asm
            // MOV     EAX,[EBP+4]
            // end

            return result;
        }

        public virtual void Error(string Msg, int Data)
        {
           
            throw EListError.CreateFmt(Msg, new int[] {Data});
        }

        public virtual void Error(PResStringRec Msg, int Data)
        {
            
            CList.Error(LoadResString(Msg), Data);
        }

        public void Exchange(int Index1, int Index2)
        {
            object Item;
            if ((Index1 < 0) || (Index1 >= FCount))
            {
                
                Error(SListIndexError, Index1);
            }
            if ((Index2 < 0) || (Index2 >= FCount))
            {
                
                Error(SListIndexError, Index2);
            }
            Item = List[Index1];
            List[Index1] = List[Index2];
            List[Index2] = Item;
        }

        public CList Expand()
        {
            CList result;
            if (FCount == FCapacity)
            {
                Grow();
            }
            result = this;
            return result;
        }

        public object First()
        {
            object result;
            result = this[0];
            return result;
        }

        protected object Get(int Index)
        {
            object result;
            if ((Index < 0) || (Index >= FCount))
            {
                
                Error(SListIndexError, Index);
            }
            result = List[Index];
            return result;
        }

        public virtual void Grow()
        {
            int Delta;
            if (FCapacity > 64)
            {
                Delta = FCapacity / 4;
            }
            else if (FCapacity > 8)
            {
                Delta = 16;
            }
            else
            {
                Delta = 4;
            }
            Capacity = FCapacity + Delta;
        }

        public int IndexOf(object Item)
        {
            int result;
            result = 0;
            while ((result < FCount) && (List[result] != Item))
            {
                result ++;
            }
            if (result == FCount)
            {
                result =  -1;
            }
            return result;
        }

        public void Insert(int Index, object Item)
        {
            if ((Index < 0) || (Index > FCount))
            {
                
                Error(SListIndexError, Index);
            }
            if (FCount == FCapacity)
            {
                Grow();
            }
            if (Index < FCount)
            {
                //@ Unsupported function or procedure: 'Move'
                Move(List[Index], List[Index + 1], (FCount - Index) * sizeof(object));
            }
            List[Index] = Item;
            FCount ++;
            if (Item != null)
            {
                Notify(Item, TListNotification.lnAdded);
            }
        }

        public object Last()
        {
            object result;
            result = this[FCount - 1];
            return result;
        }

        public void Move(int CurIndex, int NewIndex)
        {
            object Item;
            if (CurIndex != NewIndex)
            {
                if ((NewIndex < 0) || (NewIndex >= FCount))
                {
                    
                    Error(SListIndexError, NewIndex);
                }
                Item = this[CurIndex];
                List[CurIndex] = null;
                Delete(CurIndex);
                Insert(NewIndex, null);
                List[NewIndex] = Item;
            }
        }

        protected void Put(int Index, object Item)
        {
            object Temp;
            if ((Index < 0) || (Index >= FCount))
            {
                
                Error(SListIndexError, Index);
            }
            if (Item != List[Index])
            {
                Temp = List[Index];
                List[Index] = Item;
                if (Temp != null)
                {
                    Notify(Temp, TListNotification.lnDeleted);
                }
                if (Item != null)
                {
                    Notify(Item, TListNotification.lnAdded);
                }
            }
        }

        public int Remove(object Item)
        {
            int result;
            result = IndexOf(Item);
            if (result >= 0)
            {
                Delete(result);
            }
            return result;
        }

        public void Pack()
        {
            int I;
            for (I = FCount - 1; I >= 0; I-- )
            {
                if (this[I] == null)
                {
                    Delete(I);
                }
            }
        }

        protected void SetCapacity(int NewCapacity)
        {
            if ((NewCapacity < FCount) || (NewCapacity > Units.JClasses.MaxListSize))
            {
                
                Error(SListCapacityError, NewCapacity);
            }
            if (NewCapacity != FCapacity)
            {
                //@ Unsupported function or procedure: 'ReallocMem'
                ReallocMem(FList, NewCapacity * sizeof(object));
                FCapacity = NewCapacity;
            }
        }

        protected void SetCount(int NewCount)
        {
            int I;
            if ((NewCount < 0) || (NewCount > Units.JClasses.MaxListSize))
            {
                
                Error(SListCountError, NewCount);
            }
            if (NewCount > FCapacity)
            {
                Capacity = NewCount;
            }
            if (NewCount > FCount)
            {
                //@ Unsupported function or procedure: 'FillChar'
                FillChar(List[FCount], (NewCount - FCount) * sizeof(object), 0);
            }
            else
            {
                for (I = FCount - 1; I >= NewCount; I-- )
                {
                    Delete(I);
                }
            }
            FCount = NewCount;
        }

        public void Sort(TListSortCompare Compare)
        {
            if ((FList != null) && (Count > 0))
            {
                Units.JClasses.QuickSort(FList, 0, Count - 1, Compare);
            }
        }

        public object Extract(object Item)
        {
            object result;
            int I;
            result = null;
            I = IndexOf(Item);
            if (I >= 0)
            {
                result = Item;
                List[I] = null;
                Delete(I);
                Notify(result, TListNotification.lnExtracted);
            }
            return result;
        }

        public virtual void Notify(object Ptr, TListNotification Action)
        {
        }

        public void Assign(CList ListA, TListAssignOp AOperator, CList ListB)
        {
            int I;
            CList LTemp;
            CList LSource;
            // ListB given?
            if (ListB != null)
            {
                LSource = ListB;
                Assign(ListA);
            }
            else
            {
                LSource = ListA;
            }
            switch(AOperator)
            {
                case TListAssignOp.laCopy:
                    // on with the show
                    // 12345, 346 = 346 : only those in the new list
                    Clear();
                    Capacity = LSource.Capacity;
                    for (I = 0; I < LSource.Count; I ++ )
                    {
                        Add(LSource[I]);
                    }
                    break;
                case TListAssignOp.laAnd:
                    // 12345, 346 = 34 : intersection of the two lists
                    for (I = Count - 1; I >= 0; I-- )
                    {
                        if (LSource.IndexOf(this[I]) ==  -1)
                        {
                            Delete(I);
                        }
                    }
                    break;
                case TListAssignOp.laOr:
                    // 12345, 346 = 123456 : union of the two lists
                    for (I = 0; I < LSource.Count; I ++ )
                    {
                        if (IndexOf(LSource[I]) ==  -1)
                        {
                            Add(LSource[I]);
                        }
                    }
                    break;
                case TListAssignOp.laXor:
                    // 12345, 346 = 1256 : only those not in both lists
                    LTemp = new CList();
                    // Temp holder of 4 byte values
                    try {
                        LTemp.Capacity = LSource.Count;
                        for (I = 0; I < LSource.Count; I ++ )
                        {
                            if (IndexOf(LSource[I]) ==  -1)
                            {
                                LTemp.Add(LSource[I]);
                            }
                        }
                        for (I = Count - 1; I >= 0; I-- )
                        {
                            if (LSource.IndexOf(this[I]) !=  -1)
                            {
                                Delete(I);
                            }
                        }
                        I = Count + LTemp.Count;
                        if (Capacity < I)
                        {
                            Capacity = I;
                        }
                        for (I = 0; I < LTemp.Count; I ++ )
                        {
                            Add(LTemp[I]);
                        }
                    } finally {
                       
                        LTemp.Free;
                    }
                    break;
                case TListAssignOp.laSrcUnique:
                    // 12345, 346 = 125 : only those unique to source
                    for (I = Count - 1; I >= 0; I-- )
                    {
                        if (LSource.IndexOf(this[I]) !=  -1)
                        {
                            Delete(I);
                        }
                    }
                    break;
                case TListAssignOp.laDestUnique:
                    // 12345, 346 = 6 : only those unique to dest
                    LTemp = new CList();
                    try {
                        LTemp.Capacity = LSource.Count;
                        for (I = LSource.Count - 1; I >= 0; I-- )
                        {
                            if (IndexOf(LSource[I]) ==  -1)
                            {
                                LTemp.Add(LSource[I]);
                            }
                        }
                        Assign(LTemp);
                    } finally {
                       
                        LTemp.Free;
                    }
                    break;
            }
        }

    } // end CList

    public delegate int TListSortCompare(object Item1, object Item2);
    public enum TListNotification
    {
        lnAdded,
        lnExtracted,
        lnDeleted
    } // end TListNotification

    public enum TListAssignOp
    {
        laCopy,
        laAnd,
        laOr,
        laXor,
        laSrcUnique,
        laDestUnique
    } // end TListAssignOp

}

namespace M2Server
{
    public class JClasses
    {
        public const int MaxListSize = Int32.MaxValue / 16;
        public static void QuickSort(object[] SortList, int L, int R, TListSortCompare SCompare)
        {
            int I;
            int J;
            object P;
            object T;
            do
            {
                I = L;
                J = R;
                P = SortList[(L + R) >> 1];
                do
                {
                    while (SCompare(SortList[I], P) < 0)
                    {
                        I ++;
                    }
                    while (SCompare(SortList[J], P) > 0)
                    {
                        J -= 1;
                    }
                    if (I <= J)
                    {
                        T = SortList[I];
                        SortList[I] = SortList[J];
                        SortList[J] = T;
                        I ++;
                        J -= 1;
                    }
                } while (!(I > J));
                if (L < J)
                {
                    QuickSort(SortList, L, J, SCompare);
                }
                L = I;
            } while (!(I >= R));
        }

    } // end JClasses

}

