using EnterSentials.Framework.Properties;

namespace EnterSentials.Framework
{
    public static class ExceptionPolicy
    {
        public static readonly string Default = Settings.Default.DefaultExceptionPolicyName;
        public static readonly string ShieldFromDataAccessExceptions = Settings.Default.ShieldFromDataAccessExceptionsExceptionPolicyName;
        public static readonly string ShieldFromBusinessLogicExceptions = Settings.Default.ShieldFromBusinessLogicExceptionsExceptionPolicyName;
    }
}
