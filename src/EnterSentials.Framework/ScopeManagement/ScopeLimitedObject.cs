using System;

namespace EnterSentials.Framework
{
    // Borrowed or adapted from: http://justmikesmith.blogspot.com/2011/04/how-to-properly-implement-idisposable.html
    public abstract class ScopeLimitedObject : IDisposable, INotifyDisposed
    {
        public event EventHandler Disposed = null;


        // We provide this version of the property name as it is most common in legacy .NET and applications.
        public bool Disposing
        { get { return IsDisposing; } }

        protected bool IsDisposing
        { get; private set; }

        public bool IsDisposed
        { get; protected set; }


        private void RaiseDisposed()
        { Publish.Event(Disposed, this, null); }


        /// <summary>
        /// This is the dispose routine you want to normally handle.
        /// </summary>
        protected virtual void OnDisposeExplicit()
        { }

        /// <summary>
        /// This is the dispose routine that *typically* originates from garbage collection and where
        /// dispose was NEVER called explicitly.
        /// </summary>
        protected virtual void OnDisposeImplicit()
        { }

        /// <summary>
        /// This is a dispose routine that is always called, during both implicit and explicit calls to 
        /// Dispose.  WARNING: this method will likely (though not guaranteed) be executed twice, 
        /// so be careful of the side effects in your own overriding method.  You never need to provide
        /// an implementation of this method if you adequately provide implementations for both
        /// "OnDisposeExplicit" and "OnDisposeImplicit".
        /// </summary>
        protected virtual void OnDisposeRegardless()
        { }


        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design", 
            "CA1063:ImplementIDisposableCorrectly",
            Justification="Disposable is implemented correctly and simplified by this component."
        )]
        protected void Dispose(bool isDisposing)
        {
            if (!(IsDisposed || IsDisposing))
            {
                IsDisposing = true;

                if (isDisposing)
                {
                    IsDisposed = true;
                    OnDisposeExplicit();
                }
                else
                    OnDisposeImplicit();

                RaiseDisposed();
            }

            OnDisposeRegardless();
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        ~ScopeLimitedObject()
        { Dispose(false); }
    }
}
