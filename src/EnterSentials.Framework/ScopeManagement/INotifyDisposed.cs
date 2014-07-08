using System;

namespace EnterSentials.Framework
{
    // Borrowed or adapted from: http://justmikesmith.blogspot.com/2011/04/how-to-properly-implement-idisposable.html
    public interface INotifyDisposed
    {
        event EventHandler Disposed;
    }
}