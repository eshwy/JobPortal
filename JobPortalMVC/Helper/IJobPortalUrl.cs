using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace JopPortalMVC.Helper
{
    interface IJobPortalUrl
    {
        HttpClient initial();        
    }
}
