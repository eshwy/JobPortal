using JopPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JopPortal.Repository
{
    public interface IJWTManagerRepository
    {
        public string Authenticate(LoginTbl users);

    }
}
