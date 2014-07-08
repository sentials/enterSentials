using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace EnterSentials.Framework.Web.AspNet
{
    public static class ApiControllerExtensions
    {
        public static IEnumerable<string> GetErrorMessagesFromModelState(this ApiController controller)
        { return controller.ModelState.SelectMany(x => x.Value.Errors.Select(error => error.ErrorMessage)); }
    }
}