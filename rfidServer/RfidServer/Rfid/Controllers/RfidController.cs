using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RfidServer.DAL;
using RfidServer.DAL.Entity;
using RfidServer.DAL.Repositories;
using RfidServer.DAL.Repositories.Abstract;
using RfidServer.Web.ViewModels;
using RfidServer.WisAPI;
using RfidServer.WisAPI.Dto;

namespace RfidServer.Rfid.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RfidController : Controller
    {
	    private readonly IStudentRepository _studentRepository;
	    private readonly IVariantRepository _variantRepository;

		public RfidController(RegistrationDbContext context)
		{
			_studentRepository = new StudentRepository(context);
			_variantRepository= new VariantRepository(context);
		}

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<string>> CreateStudent([FromForm] string uid)
        {
			if (uid != null)
			{
				var auth = WisClient.AuthBase64;
				if (string.IsNullOrEmpty(auth))
				{
					return NotFound();
				}
				Debug.WriteLine("Incoming ID: " + uid);
				var studentRfidDto = await WisClient.GetStudentByRfidUid(uid);
				int? activeVariantId = VariantViewModel.ActiveVariantId;
				if (activeVariantId == null)
				{
					return ("Variant!");
				}
				int variantId = activeVariantId.Value;
				var variant = await _variantRepository.GetVariantById(variantId);
				var studentsDto = await WisClient.GetStudents(variant.WisCourseId);
				var studentDto = studentsDto?.Single(s => s.Login == studentRfidDto.Login);
				if (studentDto == null)
				{
					return ("Not found");
				}
				if (_studentRepository.StudentExists(studentDto.Id))
				{
					return ("Registered");
				}
				try
				{
					var student = StudentViewModel.CreateStudent(studentDto);
					var studentPostDto = StudentViewModel.CreateStudentPostDto(student);
					await _studentRepository.InsertStudent(student);
					await _studentRepository.AddStudentToVariant(student.Id, variantId);
					await _studentRepository.SaveChanges();

					if (WisClient.AutoRegister)
					{
						var registerStudentsDto = new List<StudentPostDto> { studentPostDto };
						var registrationDto = VariantViewModel.CreateRegistrationDto(variant);
						var registeredIds = await WisClient.RegisterStudents(registerStudentsDto, registrationDto);
						foreach (var registeredId in registeredIds)
						{
							await _studentRepository.RegisterStudent(registeredId);
							await _studentRepository.SaveChanges();
						}
					}
				}
				catch (DbUpdateConcurrencyException) { }

				return studentRfidDto.Login;
			}

	        return NotFound();
        }
    }
}