using System;

namespace EnterSentials.Framework
{
    public abstract class ExceptionManagerBase : IExceptionManager
    {
        public abstract bool HandleException(Exception exceptionToHandle, string policyName, out Exception exceptionToThrow);


        public virtual bool HandleException(Exception exceptionToHandle, string policyName)
        {
            var ignored = (Exception)null;
            return HandleException(exceptionToHandle, policyName, out ignored);
        }


        public virtual void UsePolicyToProcessAction(string policyName, Action action, Action finallyAction)
        {
            if (action != null)
            {
                try
                { action(); }
                catch (Exception ex)
                {
                    var exToThrow = (Exception)null;
                    if (HandleException(ex, policyName, out exToThrow))
                    {
                        if (exToThrow == null)
                            throw;
                        else
                            throw exToThrow;
                    }
                }
                finally
                {
                    if (finallyAction != null)
                        UsePolicyToProcessAction(ExceptionPolicy.Default, finallyAction);
                }
            }
        }


        public virtual void UsePolicyToProcessAction(string policyName, Action action)
        { UsePolicyToProcessAction(policyName, action, null); }
    }
}
