
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace JopPortalMVC.Helper
{
    public class JobPortalUrl : IJobPortalUrl
    {
        //IConfiguration configuration;

        //public JobPortalUrl(IConfiguration configuration)
        //{
        //    this.configuration = configuration;
        //}
        public HttpClient initial()
        {
            var client = new HttpClient();
            client.BaseAddress=new Uri("https://localhost:44352/");        
            
            
            return client;
        }
        
    }
}
