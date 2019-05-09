using System.Collections.Generic;
using System.Threading.Tasks;
using RfidServer.DAL.Entity;

namespace RfidServer.DAL.Repositories.Abstract
{
	public interface IVariantRepository
	{
		Task InsertVariant(Variant variant);
		Task<List<Variant>> GetAllVariants();
		Task<Variant> GetVariantById(int id);
		Task<List<Variant>> GetVariantsByName(string partialName);
		Task DeleteVariant(int id);
		Task SaveChanges();
		bool VariantExists(int id);

	}
}
