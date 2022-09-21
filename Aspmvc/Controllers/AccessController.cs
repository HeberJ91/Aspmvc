using Aspmvc.Models.WS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Aspmvc.Models;
namespace Aspmvc.Controllers
{
    public class AccessController : ApiController
    {
        [HttpGet]
        public Reply HelloWorld()
        {
            Reply oR = new Reply();
            oR.result = 1;
            oR.message = "Hi World";

            return oR;
        }

        [HttpPost]
        public Reply Login([FromBody] AccessViewModel model)
        {
            Reply oR = new Reply();
            oR.result = 0;
            try
            {
                using (DB2Entities2 db= new DB2Entities2())
                {
                    var lst = db.Users.Where(d => d.email == model.email && d.password == model.password && d.idEstatus == 1);

                    if (lst.Count() > 0)
                    {
                        oR.result = 1;
                        oR.data = Guid.NewGuid().ToString();

                        User oUser = lst.First();
                        oUser.token = oR.data.ToString();
                        db.Entry(oUser).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

                    }

                }
            }
            catch(Exception ex)
            {
              
                oR.message = "Ocurrion un error";
            }

            return oR;
        }

    }
}
