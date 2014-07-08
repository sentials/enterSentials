using System;
using System.Web.Http.ModelBinding;

namespace EnterSentials.Framework.Web.AspNet
{
    public static class ModelStateDictionaryExtensions
    {
        public static void AddModelError(this ModelStateDictionary modelStateDictionary, string errorMessage)
        { modelStateDictionary.AddModelError("", errorMessage); }

        public static void AddModelError(this ModelStateDictionary modelStateDictionary, Exception exception)
        { modelStateDictionary.AddModelError("", exception); }
    }
}