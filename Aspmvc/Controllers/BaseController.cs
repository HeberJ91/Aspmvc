using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Aspmvc.Models;

namespace Aspmvc.Controllers
{
    public class BaseController : ApiController
    {
        public string error = "";
        public bool Verify(string token)
        {
            using (DB2Entities2 db = new DB2Entities2())
            {
                if (db.Users.Where( d=> d.token == token && d.idEstatus == 1).Count()> 0)
                    return true;
            }

            return false;
        }

    }
}
