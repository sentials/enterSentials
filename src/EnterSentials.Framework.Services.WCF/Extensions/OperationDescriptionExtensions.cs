using System;
using System.Linq;
using System.ServiceModel.Description;

namespace EnterSentials.Framework.Services.WCF
{
    public static class OperationDescriptionExtensions
    {
        internal static SerializationParameterKeyedCollection GetParameters(this OperationDescription operation)
        {
            Guard.AgainstNull(operation, "operation");
            var parameters = new SerializationParameterKeyedCollection();

            if ((operation.Messages != null)
                && (operation.Messages.Any())
                && (operation.Messages[0].Body != null)
                && (operation.Messages[0].Body.Parts != null))
            {
                var i = 0;
                foreach (var part in operation.Messages[0].Body.Parts)
                    parameters.Add(new SerializationParameter(i++, part.Type, part.Name));
            }

            return parameters;
        }


        internal static Type GetReturnType(this OperationDescription operation)
        {
            Guard.AgainstNull(operation, "operation");
            return (operation.Messages != null)
                && (operation.Messages.Count() > 1)
                && (operation.Messages[1].Body != null)
                && (operation.Messages[1].Body.ReturnValue != null)
                ? operation.Messages[1].Body.ReturnValue.Type
                : null;
        }


        internal static string GetResponseAction(this OperationDescription operation)
        {
            Guard.AgainstNull(operation, "operation");
            return (operation.Messages != null) && (operation.Messages.Count() > 1)
                ? operation.Messages[1].Action
                : null;
        }


        internal static string GetRequestAction(this OperationDescription operation)
        {
            Guard.AgainstNull(operation, "operation");
            return (operation.Messages != null) && (operation.Messages.Any())
                ? operation.Messages[0].Action
                : null;
        }
    }
}