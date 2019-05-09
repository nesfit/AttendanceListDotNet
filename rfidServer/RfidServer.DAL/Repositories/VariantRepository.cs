using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RfidServer.DAL.Entity;
using RfidServer.DAL.Repositories.Abstract;

namespace RfidServer.DAL.Repositories
{
	public class VariantRepository : IVariantRepository
	{
		private readonly RegistrationDbContext _context;

		public VariantRepository(RegistrationDbContext context)
		{
			_context = context;
		}

		public async Task InsertVariant(Variant variant)
		{
			await _context.Variants.AddAsync(variant);
		}

		public async Task<List<Variant>> GetAllVariants()
		{
			var variants = await _context.Variants.ToListAsync();
			return variants;
		}

		public async Task<Variant> GetVariantById(int id)
		{
			var variant = await _context.Variants
				.FirstOrDefaultAsync(v => v.Id == id);
			return variant;
		}

		public async Task<List<Variant>> GetVariantsByName(string partialName)
		{
			var variants = await _context.Variants
				.Where(v => v.Title.ToLower().Contains(partialName.ToLower()))
				.ToListAsync();
			return variants;
		}

		public async Task DeleteVariant(int id)
		{
			var variant = await _context.Variants.FindAsync(id);
			if (variant != null)
			{
				_context.Variants.Remove(variant);
			}
		}

		public async Task SaveChanges()
		{
			await _context.SaveChangesAsync();
		}

		public bool VariantExists(int id)
		{
			return _context.Variants.Any(v => v.WisId == id);
		}
	}
}
