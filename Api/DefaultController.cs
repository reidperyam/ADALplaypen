using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Microsoft.Owin.Security.ActiveDirectory;
using Microsoft.Owin.Security;

namespace Api
{
    public class DefaultController : ApiController
    {
        public DefaultController()
        { 
            
        }

        [Authorize]
        [Route("api/hi/")]
        public string GetAssemblies()
        {
            return "Hi";
        }
    }
}
