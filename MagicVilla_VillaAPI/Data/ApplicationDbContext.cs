using MagicVilla_VillaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Data
{
    // ApplicationDbContext inherits DbContext
    // Create the Connection String in appsettings.json
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Villa> Villas { get; set; }
    }
}


// Script to add migration to SQL table, make sure to change the Default Project before doing migration
// add-migration AddVillaTable
// Script to update the database after migratiom
// update-database