using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace OptSystem.Models
{
    public class ProjContext : DbContext
    {
        public DbSet<Param> Params { get; set; }
        public DbSet<Simul> Simuls { get; set; }
    }
}



