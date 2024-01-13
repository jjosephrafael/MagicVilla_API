using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository
{
    // CTRL . to add using statement and CTRL . again to implement the interface
    public class VillaRepository : Repository<Villa>, IVillaRepository
    {
        // Add ApplicationDbContext using dependency injection
        private readonly ApplicationDbContext _db;

        // pass db to the base class
        public VillaRepository(ApplicationDbContext db) : base(db) 
        {
            _db = db;
        }

        public async Task<Villa> UpdateAsync(Villa entity)
        {
            entity.UpdatedDate = DateTime.Now;
            _db.Villas.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
