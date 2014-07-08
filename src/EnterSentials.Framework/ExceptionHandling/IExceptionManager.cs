using System;

namespace EnterSentials.Framework
{
    // Borrowed or adapted from: http://justmikesmith.blogspot.com/2011/05/building-framework-for-cross-cutting.html
    public interface IExceptionManager
    {
        /// <summary>
        /// For advanced scenarios (suggested you use "UsePolicyToProcessAction" over this method): Handles the 
        /// specified <see cref="Exception"/> object according to the rules configured for <paramref name="policyName"/>. 
        /// </summary>a
        /// <param name="exceptionToHandle">An <see cref="Exception"/> object.</param>
        /// <param name="policyName">The name of the policy to handle.</param>
        /// <param name="exceptionToThrow">The new <see cref="Exception"/> to throw, if any.</param>
        /// <remarks>
        /// If a rethrow is recommended and <paramref name="exceptionToThrow"/> is <see langword="null"/>,
        /// then the original exception <paramref name="exceptionToHandle"/> should be rethrown; otherwise,
        /// the exception returned in <paramref name="exceptionToThrow"/> should be thrown.
        /// </remarks>
        /// <returns>
        /// Whether or not a rethrow is recommended. 
        /// </returns>
        /// <example>
        /// The following code shows the usage of the 
        /// exception handling framework.
        /// <code>
        /// try
        ///	{
        ///		Foo();
        ///	}
        ///	catch (Exception e)
        ///	{
        ///	    Exception exceptionToThrow;
        ///		if (exceptionManager.HandleException(e, "policy", out exceptionToThrow))
        ///		{
        ///		  if(exceptionToThrow == null)
        ///		    throw;
        ///		  else
        ///		    throw exceptionToThrow;
        ///		}
        ///	}
        /// </code>
        /// </example>
        /// <seealso cref="ExceptionManager.HandleException(Exception, string)"/>
        bool HandleException(Exception exceptionToHandle, string policyName, out Exception exceptionToThrow);

        /// <summary>
        /// For advanced scenarios (suggested you use "UsePolicyToProcessAction" over this method): Handles the 
        /// specified <see cref="Exception"/> object according to the rules configured for <paramref name="policyName"/>. 
        /// </summary>
        /// <param name="exceptionToHandle">An <see cref="Exception"/> object.</param>
        /// <param name="policyName">The name of the policy to handle.</param>        
        /// <returns>
        /// Whether or not a rethrow is recommended.
        /// </returns>
        /// <example>
        /// The following code shows the usage of the 
        /// exception handling framework.
        /// <code>
        /// try
        ///	{
        ///		Foo();
        ///	}
        ///	catch (Exception e)
        ///	{
        ///		if (exceptionManager.HandleException(e, "policy")) throw;
        ///	}
        /// </code>
        /// </example>
        /// <seealso cref="ExceptionManager.UsePolicyToProcessAction"/>
        bool HandleException(Exception exceptionToHandle, string policyName);

        /// <summary>
        /// Excecutes the supplied delegate <paramref name="action"/> and handles 
        /// any thrown exception according to the rules configured for <paramref name="policyName"/>.
        /// </summary>
        /// <param name="policyName">The name of the policy to handle.</param>
        /// <param name="action">The delegate to execute.</param>
        /// <param name="finallyAction">A delegate to execute as if in a "finally" block.</param>        
        /// <example>
        /// The following code shows one usage of this method.
        /// <code>
        ///		exceptionManager.UsePolicyToProcessAction("policy", () => { Foo(); }, () => { CleanUpResources(); });
        /// </code>
        /// </example>
        /// <seealso cref="ExceptionManager.HandleException(Exception, string)"/>
        void UsePolicyToProcessAction(string policyName, Action action, Action finallyAction);

        /// <summary>
        /// Excecutes the supplied delegate <paramref name="action"/> and handles 
        /// any thrown exception according to the rules configured for <paramref name="policyName"/>.
        /// </summary>
        /// <param name="policyName">The name of the policy to handle.</param>
        /// <param name="action">The delegate to execute.</param>        
        /// <example>
        /// The following code shows one usage of this method.
        /// <code>
        ///		exceptionManager.UsePolicyToProcessAction("policy", () => { Foo(); });
        /// </code>
        /// </example>
        /// <seealso cref="ExceptionManager.HandleException(Exception, string)"/>
        void UsePolicyToProcessAction(string policyName, Action action);
    }
}
