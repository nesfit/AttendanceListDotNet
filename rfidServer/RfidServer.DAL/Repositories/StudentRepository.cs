using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RfidServer.DAL.Entity;
using RfidServer.DAL.Repositories.Abstract;

namespace RfidServer.DAL.Repositories
{
	public class StudentRepository : IStudentRepository
	{
		private readonly RegistrationDbContext _context;

		public StudentRepository(RegistrationDbContext context)
		{
			_context = context;
		}

		public async Task InsertStudent(Student student)
		{
			await _context.Students.AddAsync(student);
		}

		public async Task AddStudentToVariant(int studentId, int variantId)
		{
			var student = await _context.Students.FindAsync(studentId);
			var variant = await _context.Variants
				.FirstOrDefaultAsync(v => v.Id == variantId);
			if (student != null && variant != null)
			{
				student.VariantId = variantId;
				student.RegisteredVariant = variant;
			}
		}

		public async Task<List<Student>> GetAllStudents()
		{
			var students = await _context.Students
				.Include(s => s.RegisteredVariant)
				.ToListAsync();
			return students;
		}

		public async Task<Student> GetStudentById(int id)
		{
			var student = await _context.Students
				.Include(s => s.RegisteredVariant)
				.FirstOrDefaultAsync(s => s.Id == id);
			return student;
		}

		public async Task<List<Student>> GetStudentsInVariant(int id)
		{
			var students = await Task.Run(() => 
				_context.Students
					.Include(s => s.RegisteredVariant)
					.Where(v => v.VariantId == id)
					.ToListAsync()
			);
			return students;
		}

		public async Task UpdateStudent(int id, Variant variant)
		{
			var student = await _context.Students.FindAsync(id);
			if (student != null)
			{
				student.VariantId = variant.Id;
				student.RegisteredVariant = variant;
				student.Registered = false;
			}
		}

		public async Task RegisterStudent(int wisId)
		{
			var student = await _context.Students.FirstOrDefaultAsync(s => s.WisId == wisId);
			if (student != null)
			{
				student.Registered = true;
			}
		}

		public async Task DeleteStudent(int id)
		{
			var student = await _context.Students.FindAsync(id);
			if (student != null)
			{
				_context.Students.Remove(student);
			}
		}

		public async Task SaveChanges()
		{
			await _context.SaveChangesAsync();
		}

		public bool StudentExists(int wisId)
		{
			return _context.Students.Any(s => s.WisId == wisId);
		}
	}
}