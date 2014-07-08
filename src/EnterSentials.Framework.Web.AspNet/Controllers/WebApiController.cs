using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace EnterSentials.Framework.Web.AspNet
{
    public abstract class WebApiController : ApiController
    {
        protected IUnitOfWorkFactory UnitOfWork
        { get; private set; }
        
        
        public WebApiController(IUnitOfWorkFactory uowFactory)
        {
            Guard.AgainstNull(uowFactory, "uowFactory");
            UnitOfWork = uowFactory;
        }
    }
}
