using System;
using System.Collections;
namespace M2Server
{
    public class TGList: ArrayList
    {
        public TGList() : base()
        {
          //  InitializeCriticalSection(CriticalSection);
        }

        ~TGList()
        {
           // DeleteCriticalSection(CriticalSection);
        }

        public void __Lock()
        {
            
          //  EnterCriticalSection(CriticalSection);
        }

        public void UnLock()
        {
            
      //      LeaveCriticalSection(CriticalSection);
        }

    } // end TGList

    public class TGStringList: ArrayList
    {
        public TGStringList() : base()
        {
            
           // InitializeCriticalSection(CriticalSection);
        }
        //@ Destructor  Destroy()
        ~TGStringList()
        {
            
         //   DeleteCriticalSection(CriticalSection);
            // base.Destroy();
        }
        public void __Lock()
        {
            
        //    EnterCriticalSection(CriticalSection);
        }

        public void UnLock()
        {
         //   LeaveCriticalSection(CriticalSection);
        }

    }
}

