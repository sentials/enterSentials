using EnterSentials.Framework.Properties;
using System;

namespace EnterSentials.Framework
{
    public static class Components
    {
        private static readonly string ComponentsProviderAssemblyQualifiedTypeName = Settings.Default.ComponentsProviderAssemblyQualifiedTypeName;

        private static readonly object instanceProviderLock = new object();
        private static readonly object instanceLock = new object();

        private static Func<IComponents> instanceProvider = null;
        private static IComponents instance = null;
        private static bool instanceProviderHasBeenSet = false;


        public static Func<IComponents> InstanceProvider
        {
            get
            {
                if (instanceProvider == null)
                {
                    lock (instanceProviderLock)
                    {
                        if (instanceProvider == null)
                            instanceProvider = GetSettingsBasedComponentsInstance;
                    }
                }

                return instanceProvider;
            }

            set 
            {
                lock (instanceProviderLock)
                { 
                    instanceProvider = value;
                    instanceProviderHasBeenSet = true;
                }
            }
        }


        public static IComponents Instance
        { 
            get 
            {
                if (instance == null)
                {
                    lock (instanceLock)
                    {
                        if (instance == null)
                            instance = InstanceProvider();
                    }
                }

                return instance;
            }
        }


        public static void SetInstanceProviderIfNotAlreadySet(Func<IComponents> instanceProvider)
        {
            if (!instanceProviderHasBeenSet)
            {
                lock (instanceProviderLock)
                {
                    if (!instanceProviderHasBeenSet)
                        InstanceProvider = instanceProvider;
                }
            }
        }


        public static IComponents GetSettingsBasedComponentsInstance()
        {
            Guard.Against(string.IsNullOrEmpty(ComponentsProviderAssemblyQualifiedTypeName), Resources.ComponentsProviderSettingMissing);
            return (IComponents)Activator.CreateInstance(Type.GetType(ComponentsProviderAssemblyQualifiedTypeName, true));
        }
    }
}