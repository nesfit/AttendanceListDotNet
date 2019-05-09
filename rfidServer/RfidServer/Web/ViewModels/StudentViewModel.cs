using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RfidServer.DAL.Entity;
using RfidServer.WisAPI.Dto;

namespace RfidServer.Web.ViewModels
{
	public class StudentViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Login { get; set; }
		public string Email { get; set; }
		public int Points { get; set; }
		public string Date { get; set; }
		public string Who { get; set; }
		public string RegType { get; set; }
		public string Update { get; set; }
		public string RegTime { get; set; }
		public bool Registered { get; set; }
		public string RegisteredVariant { get; set; }

		public static StudentViewModel CreateStudentVm(Student student)
		{
			return new StudentViewModel
			{
				Id = student.Id,
				Name = student.Name,
				Email = student.Email,
				Login = student.Login,
				Points = student.Points,
				Date = student.Date?.ToString(CultureInfo.CurrentCulture),
				Who = student.Who,
				RegType = student.RegType,
				RegTime = student.RegTime?.ToString(CultureInfo.CurrentCulture),
				Update = student.Update?.ToString(CultureInfo.CurrentCulture),
				Registered = student.Registered,
				RegisteredVariant = student.RegisteredVariant?.Title
			};
		}

		public static Student CreateStudent(StudentDto studentDto)
		{
			return new Student()
			{
				WisId = studentDto.Id,
				WisPersonId = studentDto.Person_Id,
				Name = studentDto.Name,
				Email = studentDto.Email,
				Points = studentDto.Points,
				Date = studentDto.Date,
				Who = studentDto.Who,
				RegType = studentDto.RegType,
				RegTime = studentDto.RegTime,
				Update = studentDto.Update,
				VariantId = studentDto.VariantId
			};
		}

		public static Student CreateStudent(StudentCourseDto studentCourseDto)
		{
			return new Student()
			{
				WisId = studentCourseDto.Id,
				WisPersonId = studentCourseDto.Person_Id,
				Name = studentCourseDto.Name,
				Email = studentCourseDto.Email,
				Login = studentCourseDto.Login,
				Update = studentCourseDto.Update
			};
		}

		public static StudentCourseDto CreateStudentDto(Student student)
		{
			return new StudentCourseDto
			{
				Id = student.WisId,
				Person_Id = student.WisPersonId,
				Name = student.Name,
				Email = student.Email,
				Login = student.Login,
				Points = student.Points,
				Update = student.Update?.ToString(CultureInfo.CurrentCulture)
			};
		}

		public static StudentPostDto CreateStudentPostDto(Student student)
		{
			return new StudentPostDto
			{
				id = student.WisId,
				person_id = student.WisPersonId,
				name = student.Name,
				email = student.Email,
				login = student.Login
			};
		}

		public static Student CreateStudent(StudentRfidDto studentRfidDto)
		{
			return new Student()
			{
				Name = studentRfidDto.Name,
				Login = studentRfidDto.Login
			};
		}

		public static string ToCsv(IEnumerable<Student> students, string separator)
		{
			string header = ";Jméno;Login;Body;Celk;Datum;Kdo;";

			StringBuilder csvData = new StringBuilder();
			csvData.AppendLine(header);

			foreach (var student in students)
				csvData.AppendLine(ToCsvFields(separator, student));

			byte[] csvBytes = Encoding.Default.GetBytes(csvData.ToString());
			return Encoding.UTF8.GetString(csvBytes);
		}

		public static string ToCsvFields(string separator, Student student)
		{
			StringBuilder line = new StringBuilder();
			line.Append(student.Id + separator);
			line.Append(student.Name + separator);
			line.Append(student.Login + separator);
			line.Append(student.Points + separator);
			line.Append(student.Points + separator);
			line.Append(student.Date + separator);
			line.Append(student.Who + separator);

			return line.ToString();
		}

		public static async Task<IEnumerable<Student>> SelectStudentsByNameOrLogin(IEnumerable<Student> students, string searchString)
		{
			return await Task.Run(() =>
				students
					.Where(s =>
					{
						bool nameCheck = s.Name?.ToLower().Contains(searchString.ToLower()) ?? false;
						bool loginCheck = s.Login?.ToLower().Contains(searchString.ToLower()) ?? false;
						return nameCheck || loginCheck;
					})
			);
		}

		public static async Task<IEnumerable<Student>> SortStudents(IEnumerable<Student> students, string sortOrder)
		{
			return await Task.Run(() =>
			{
				switch (sortOrder)
				{
					case "name_desc":
						students = students.OrderByDescending(s => s.Name);
						break;
					case "Login":
						students = students.OrderBy(s => s.Login);
						break;
					case "login_desc":
						students = students.OrderByDescending(s => s.Login);
						break;
					case "Variant":
						students = students.OrderBy(s => s.RegisteredVariant.Title);
						break;
					case "variant_desc":
						students = students.OrderByDescending(s => s.RegisteredVariant.Title);
						break;
					default:
						students = students.OrderBy(s => s.Name);
						break;
				}
				return students;
			});
		}
	}
}
