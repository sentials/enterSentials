using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;
using EnterSentials.Framework;
using EnterSentials.Framework.Services.WCF.Properties;

namespace EnterSentials.Framework.Services.WCF
{
    public class ExceptionManagerBasedErrorHandler : IErrorHandler
    {
        private const string DefaultWcfExceptionShieldingExceptionPolicyName = "WCF Exception Shielding";

        private static readonly string ExceptionPolicyName = ExceptionPolicy.ShieldFromBusinessLogicExceptions;

        private static readonly bool ExceptionPolicyNameIsDefaultWcfExceptionShieldingExceptionPolicyName = 
            string.Equals(ExceptionPolicyName,  DefaultWcfExceptionShieldingExceptionPolicyName, StringComparison.InvariantCultureIgnoreCase);
        

        private readonly IExceptionManager exceptionManager = null;
        private readonly ILog log = null;
        

        protected string GetFormattedExceptionMessageFor(Exception exception, Guid handlingInstanceId)
        { return string.Format(Resources.ClientUnhandledExceptionMessageFormat, exception.GetHandlingInstanceId(handlingInstanceId)); }


        protected Guid LogServerException(Exception exception)
        {
            var handlingInstanceId = exception.GetHandlingInstanceId(Guid.Empty);
            log.ExceptionByPolicy(exception.ToString(), handlingInstanceId); 
            return handlingInstanceId;
        }


        private Message GetFaultFor(Exception exception, Guid handlingInstanceId, MessageVersion version)
        {
            var reason = GetFormattedExceptionMessageFor(exception, handlingInstanceId);

            var fault = Message.CreateMessage(version, "", reason, new DataContractJsonSerializer(typeof(string)));
            
            fault.Properties.Add(WebBodyFormatMessageProperty.Name, new WebBodyFormatMessageProperty(WebContentFormat.Json));

            fault.Properties.Add(
                HttpResponseMessageProperty.Name, 
                new HttpResponseMessageProperty
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    StatusDescription = reason
                }
            );

            return fault;
        }


        protected virtual object GetFaultContractFromFaultContractWrappingException(Exception exception)
        { return null; }

        protected virtual bool IsFaultContractWrappingException(Exception exception)
        { return false; }


        private Message GetFaultForFaultContractWrappingException(
            Exception faultContractWrapper,
            MessageVersion version
        )
        {
            var fault = (Message)null;

            try
            {
                var faultContract = GetFaultContractFromFaultContractWrappingException(faultContractWrapper);
                fault = Message.CreateMessage(version, "", faultContract, new DataContractJsonSerializer(faultContract.GetType()));

                fault.Properties.Add(WebBodyFormatMessageProperty.Name, new WebBodyFormatMessageProperty(WebContentFormat.Json));

                fault.Properties.Add(
                    HttpResponseMessageProperty.Name, 
                    new HttpResponseMessageProperty
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        StatusDescription = faultContractWrapper.Message
                    }
                );
            }
            catch (Exception unhandledException)
            {
                // There was an error during MessageFault build process, so treat it as an Unhandled Exception
                // log the exception and send an unhandled server exception
                var handlingInstanceId = LogServerException(unhandledException);
                fault = GetFaultFor(unhandledException, handlingInstanceId, version);
            }

            return fault;
        }
        

        private void ProcessUnhandledException(Exception originalException, Exception unhandledException, ref Message fault, MessageVersion version)
        {
            if (!unhandledException.IsFaultException())
            {
                var handlingInstanceId = unhandledException.GetHandlingInstanceId(Guid.Empty);
                
                if (handlingInstanceId.Equals(Guid.Empty))
                    handlingInstanceId = LogServerException(unhandledException);

                fault = GetFaultFor(unhandledException, handlingInstanceId, version);
            }
        }


        private void ProcessUnhandledException(Exception originalException, ref Message fault, MessageVersion version)
        { ProcessUnhandledException(originalException, originalException, ref fault, version); }


        public void ProvideFault(Exception exception, MessageVersion version, ref Message fault)
        {
            var originalException = exception;

            try
            {
                var exceptionToThrow = (Exception) null;
                var shouldThrowException = exceptionManager.HandleException(exception, ExceptionPolicyName, out exceptionToThrow);
                if (exceptionToThrow != null)
                    exception = exceptionToThrow;

                if (!ExceptionPolicyNameIsDefaultWcfExceptionShieldingExceptionPolicyName)
                    shouldThrowException = exceptionManager.HandleException(exception, DefaultWcfExceptionShieldingExceptionPolicyName);

                ProcessUnhandledException(exception, ref fault, version);
            }
            catch (FaultException)
            { }
            catch (Exception unhandledException)
            {
                if (IsFaultContractWrappingException(unhandledException))
                    fault = GetFaultForFaultContractWrappingException(unhandledException, version);
                else
                    ProcessUnhandledException(originalException, unhandledException, ref fault, version);
            }
        }


        public bool HandleError(Exception error)
        { return true; }


        public ExceptionManagerBasedErrorHandler(IExceptionManager exceptionManager, ILog log)
        {
            Guard.AgainstNull(exceptionManager, "exceptionManager");
            Guard.AgainstNull(log, "log");
            this.exceptionManager = exceptionManager;
            this.log = log;
        }
    }
}
