using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskN1.Models;

namespace TaskN1.Data
{
    public class MvcPersonContext: DbContext

    {

        public MvcPersonContext(DbContextOptions<MvcPersonContext>options)
            :base(options)
        {

        }
        public DbSet<Person> Person { get; set; }

    }
}
