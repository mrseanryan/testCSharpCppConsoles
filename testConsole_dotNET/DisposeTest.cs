using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace testConsole_dotNET
{
    class DisposeTestBase : IDisposable
    {
        bool isDisposed = false;

        #region Dispose
        //if we had unmanaged resources, we would have a finalizer ~() here

        public virtual void Dispose()
        {
            Dispose(true);

            //if we had unmanaged resources, we would call GC.SuppressFinalize()
        }

        private void Dispose(bool isDisposingOfManagedResources)
        {
            if (!isDisposed)
            {
                isDisposed = true;

                if (isDisposingOfManagedResources)
                {
                    DisposeOfManagedResources();
                }
                //if we had unmanaged resources, we would dispose of them here.
            }
        }

        private void DisposeOfManagedResources()
        {
        }
        #endregion
    }

    class DisposeTestDerived : DisposeTestBase
    {

        bool isDisposed = false;

        #region Dispose
        //if we had unmanaged resources, we would have a finalizer ~() here

        public override void Dispose()
        {
            Dispose(true);

            //if we had unmanaged resources, we would call GC.SuppressFinalize()

            base.Dispose(); //note: this calls the base method, even though it is virtual !
            /*
             * note: a better approach, when posible, is to call the base.Dispose(isDisposingOfManagedResources).
             * this is to efficiently handle repeated calls to Dispose().
             * 
             * ref: http://www.codeproject.com/Articles/15360/Implementing-IDisposable-and-the-Dispose-Pattern-P
             * 
             * ref: http://msdn.microsoft.com/en-us/library/system.idisposable.aspx
             * 
             * */
        }

        private void Dispose(bool isDisposingOfManagedResources)
        {
            if (!isDisposed)
            {
                isDisposed = true;

                if (isDisposingOfManagedResources)
                {
                    DisposeOfManagedResources();
                }
                //if we had unmanaged resources, we would dispose of them here.
            }
        }

        private void DisposeOfManagedResources()
        {
        }
        #endregion
    }
}
