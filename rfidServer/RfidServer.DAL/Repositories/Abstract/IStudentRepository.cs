using System.Collections.Generic;
using System.Threading.Tasks;
using RfidServer.DAL.Entity;

namespace RfidServer.DAL.Repositories.Abstract
{
    public interface IStudentRepository
    {
	    Task InsertStudent(Student student);
	    Task AddStudentToVariant(int studentId, int variantId);
		Task<List<Student>> GetAllStudents();
		Task<Student> GetStudentById(int id);
		Task<List<Student>> GetStudentsInVariant(int id);
		Task UpdateStudent(int id, Variant variant);
		Task RegisterStudent(int wisId);
		Task DeleteStudent(int wisId);
	    Task SaveChanges();
	    bool StudentExists(int wisId);
    }
}