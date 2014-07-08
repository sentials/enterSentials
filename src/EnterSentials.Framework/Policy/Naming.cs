namespace EnterSentials.Framework
{
    public static class Naming
    {
        public const string ProductIdentifier = "EnterSentials";

        public static readonly string ProductAssemblyNamePrefix = ProductIdentifier + ".";
        public static readonly string ProductAssemblySearchPattern = string.Format("{0}*.dll", Naming.ProductAssemblyNamePrefix); 
        
        public const string TestsSignifier = "Tests";
        public const string TestsAssemblyNameDelimiter = "." + TestsSignifier;
        public const string TestAssemblyNameSuffix = Naming.TestsAssemblyNameDelimiter;
    }
}