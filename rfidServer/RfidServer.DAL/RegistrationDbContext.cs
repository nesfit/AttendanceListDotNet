using Microsoft.EntityFrameworkCore;
using RfidServer.DAL.Entity;

namespace RfidServer.DAL
{
	public class RegistrationDbContext : DbContext
	{
		public RegistrationDbContext(DbContextOptions<RegistrationDbContext> options)
			: base(options)
		{
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Student>()
				.HasOne(s => s.RegisteredVariant)
				.WithMany(v => v.Students)
				.HasForeignKey(s => s.VariantId);
		}
		// Entities
		public DbSet<Student> Students { get; set; }
		public DbSet<Variant> Variants { get; set; }
	}
}
