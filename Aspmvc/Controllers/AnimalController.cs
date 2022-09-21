using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Aspmvc.Models.WS;
using Aspmvc.Models;

namespace Aspmvc.Controllers
{
    public class AnimalController : BaseController
    {
        [HttpPost]
        public Reply Get([FromBody]SecurityViewModel model)
        {
            Reply oR = new Reply();
            oR.result = 0;

            if (!Verify(model.token)){
                oR.message = "No autorizado";
                return oR;
            }

            try
            {
                using (DB2Entities2 db = new DB2Entities2())
                {
                    List<ListAnimalsViewModel> lst = List(db);
                    oR.data = lst;
                    oR.result = 1;
                }
            }
            catch(Exception ex)
            {
                oR.message = "Ocurrion un error en el servidor";
            }

            return oR;
        }

        [HttpPost]
        public Reply Add([FromBody] AnimalViewModel model)
        {
            Reply oR = new Reply();
            oR.result = 0;

            if (!Verify(model.token))
            {
                oR.message = "No autorizado";
                return oR;
            }

            if (!Validate(model))
            {
                oR.message = error;
                return oR;
            }

            try
            {
                using (DB2Entities2 db = new DB2Entities2())
                {
                    Animal oAnimal = new Animal();
                    oAnimal.idState = 1;
                    oAnimal.name = model.Name;
                    oAnimal.patas = model.Patas;

                    db.Animals.Add(oAnimal);
                    db.SaveChanges();

                    List<ListAnimalsViewModel> lst = List(db);
                    oR.data = lst;
                    oR.result = 1;
                }
            }
            catch (Exception ex)
            {
                oR.message = "Ocurrio un error en el servidor";
            }

            return oR;
        }


        [HttpPost]
        public Reply Edit([FromBody] AnimalViewModel model)
        {
            Reply oR = new Reply();
            oR.result = 0;

            if (!Verify(model.token))
            {
                oR.message = "No autorizado";
                return oR;
            }

            if (!Validate(model))
            {
                oR.message = error;
                return oR;
            }

            try
            {
                using (DB2Entities2 db = new DB2Entities2())
                {
                    Animal oAnimal = db.Animals.Find(model.Id);
                    oAnimal.name = model.Name;
                    oAnimal.patas = model.Patas;

                    db.Entry(oAnimal).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    List<ListAnimalsViewModel> lst = List(db);
                    oR.data = lst;
                    oR.result = 1;
                }
            }
            catch (Exception ex)
            {
                oR.message = "Ocurrio un error en el servidor";
            }

            return oR;
        }


        [HttpPost]
        public Reply Delete([FromBody] AnimalViewModel model)
        {
            Reply oR = new Reply();
            oR.result = 0;

            if (!Verify(model.token))
            {
                oR.message = "No autorizado";
                return oR;
            }

         
            try
            {
                using (DB2Entities2 db = new DB2Entities2())
                {
                    Animal oAnimal = db.Animals.Find(model.Id);
                    oAnimal.idState = 2;

                    db.Entry(oAnimal).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    List<ListAnimalsViewModel> lst = List(db);
                    oR.data = lst;
                    oR.result = 1;
                }
            }
            catch (Exception ex)
            {
                oR.message = "Ocurrio un error en el servidor";
            }

            return oR;
        }

        #region HELPERS
        private bool Validate(AnimalViewModel model)
        {
            if (model.Name == "")
            {
                error = "Nombre es obligatorio";
                return false;
            }
            return true;
        }

        private List<ListAnimalsViewModel> List(DB2Entities2 db)
        {
            List<ListAnimalsViewModel> list =(from d in db.Animals
             where d.idState == 1
             select new ListAnimalsViewModel
             {
                 Name = d.name,
                 Patas = d.patas
             }).ToList();

            return list;
        }
        #endregion
    }
}
