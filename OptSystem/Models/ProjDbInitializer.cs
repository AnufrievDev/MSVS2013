using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace OptSystem.Models
{
    public class ProjDbInitializer : DropCreateDatabaseAlways<ProjContext>
    {
        protected override void Seed(ProjContext db)
        {
            db.Params.Add(new Param { Name = "R_R1 ", Change = true, Value = 220 });
            db.Params.Add(new Param { Name = "C_C16", Change = false, Value = 180 });
            db.Params.Add(new Param { Name = "R_R13 ", Change = false, Value = 150 });
            db.Params.Add(new Param { Name = "R_R15 ", Change = true, Value = 250 });
            db.Params.Add(new Param { Name = "C_C18 ", Change = true, Value = 15 });
 
            base.Seed(db);
        }
    }
}



