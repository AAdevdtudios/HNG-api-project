using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HNG_api_project.Models;

namespace HNG_api_project.Data
{
    public class HNG_api_projectContext : DbContext
    {
        public HNG_api_projectContext (DbContextOptions<HNG_api_projectContext> options)
            : base(options)
        {
        }

        public DbSet<HNG_api_project.Models.Bio> Bio { get; set; } = default!;
    }
}
