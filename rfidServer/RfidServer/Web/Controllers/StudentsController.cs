using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RfidServer.DAL;
using RfidServer.DAL.Entity;
using RfidServer.DAL.Repositories;
using RfidServer.DAL.Repositories.Abstract;
using RfidServer.Web.ViewModels;
using RfidServer.WisAPI;

namespace RfidServer.Web.Controllers
{
    public class StudentsController : BaseController
    {
	    private readonly IStudentRepository _studentRepository;
        private readonly IVariantRepository _variantRepository;

		public StudentsController(RegistrationDbContext context)
        {
            _studentRepository = new StudentRepository(context);
            _variantRepository = new VariantRepository(context);
        }

        // GET: Students
        public async Task<IActionResult> Index(string sortOrder, string searchString, int? variantId)
        {
			IEnumerable<Student> students;
	        if (variantId != null)
	        {
				students = await _studentRepository.GetStudentsInVariant(variantId.Value);			
	        }
	        else
	        {
		        students = await _studentRepository.GetAllStudents();
			}
	        if (!string.IsNullOrEmpty(searchString))
	        {
		        students = await StudentViewModel.SelectStudentsByNameOrLogin(students, searchString);
	        }
			if (!string.IsNullOrEmpty(sortOrder))
			{
				students = await StudentViewModel.SortStudents(students, sortOrder);
			}
			var studentsVm = students.Select(StudentViewModel.CreateStudentVm);

			ViewBag.VariantId = variantId;
			ViewBag.NameSort = sortOrder == "Name"
				? "name_desc" : "Name";
			ViewBag.LoginSort = sortOrder == "Login"
				? "login_desc" : "Login";
			ViewBag.VariantSort = sortOrder == "Variant"
				? "variant_desc" : "Variant";
			ViewData["Variants"] = new SelectList(await _variantRepository.GetAllVariants(), "Id", "Title");
			return View(studentsVm);
		}

        // GET: Students/Details/id
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
	            return BadRequest();
            }

            var student = await _studentRepository
	            .GetStudentById(id.Value);
            if (student == null)
            {
                return NotFound();
            }

            var studentVm = StudentViewModel.CreateStudentVm(student);
            return View(studentVm);
        }

        // GET: Students/Edit/
        public async Task<IActionResult> Edit(IEnumerable<int> sIds)
        {
			if (!sIds.Any())
			{
				return BadRequest();
			}

			ViewData["Variants"] = new SelectList(await _variantRepository.GetAllVariants(), "Id", "Title");
			return View(sIds);
        }

        // POST: Students/Edit/
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirmed(int? variantId, IList<int> sIds)
        {
	        if (!sIds.Any())
	        {
		        return BadRequest();
	        }

			if (ModelState.IsValid)
			{
				var variant = (variantId == null) 
					? null : (await _variantRepository.GetVariantById(variantId.Value));
				await Task.WhenAll(sIds.Select(id =>
					_studentRepository.UpdateStudent(id, variant)
				));
				await _studentRepository.SaveChanges();		
			}
			return RedirectToAction(nameof(Index));
		}

		// GET: Students/Register/
        public async Task<IActionResult> Register(int? variantId, IList<int> sIds)
        {
	        if (variantId == null || !sIds.Any())
	        {
		        return BadRequest();
	        }

			var variant = await _variantRepository.GetVariantById(variantId.Value);
			if (variant == null)
			{
				return NotFound();
			}
			ViewBag.Variant = new System.Dynamic.ExpandoObject();
			ViewBag.Variant.Id = variant.Id;
			ViewBag.Variant.Title = variant.Title;
			ViewBag.Variant.CourseAbbrv = variant.CourseAbbrv;
			ViewBag.Variant.Year = variant.Year;
			ViewBag.Variant.Sem = variant.Sem;
			return View(sIds);
		}

        // POST: Students/Register
        [HttpPost, ActionName("Register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterConfirmed(int? variantId, IList<int> sIds)
        {
	        if (variantId == null || !sIds.Any())
	        {
		        return BadRequest();
	        }

			if (ModelState.IsValid)
	        {
				var variant = (await _variantRepository.GetVariantById(variantId.Value));
				if (variant == null)
				{
					return NotFound();
				}
		        var students = await Task.WhenAll(sIds.Select(id =>
			        _studentRepository.GetStudentById(id)
		        ));
		        if (students != null)
		        {
			        var studentsPostDto = students.Select(StudentViewModel.CreateStudentPostDto).ToList();
					var registrationDto = VariantViewModel.CreateRegistrationDto(variant);
					var registeredIds = await WisClient.RegisterStudents(studentsPostDto, registrationDto);
					foreach (var registeredId in registeredIds)
					{
						await _studentRepository.RegisterStudent(registeredId);
						await _studentRepository.SaveChanges();
					}
		        }
	        }
			return RedirectToAction(nameof(Index));
		}

		// GET: Students/Delete/
		public IActionResult Delete(IEnumerable<int> sIds)
        {
			if (!sIds.Any())
			{
				return BadRequest();
			}
			return View(sIds);
        }

        // POST: Students/Delete/
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(IEnumerable<int> sIds)
        {
	        if (!sIds.Any())
	        {
		        return BadRequest();
	        }
			await Task.WhenAll(sIds.Select(id =>
				_studentRepository.DeleteStudent(id)
	        ));
            await _studentRepository.SaveChanges();
			return RedirectToAction(nameof(Index));
        }

        public async Task<ActionResult> DownloadStudentsCsv(int? variantId)
        {
	        IEnumerable<Student> students;
	        if (variantId != null)
	        {
		        students = await _studentRepository.GetStudentsInVariant(variantId.Value);
		        if (students != null)
		        {
			        string csvData = StudentViewModel.ToCsv(students, ";");

					Response.Clear();
					Response.Headers.Add("Content-Disposition", "attachment; filename=students.csv");
					Response.ContentType = "text/csv; charset=utf-8";

					await Response.WriteAsync(csvData);
				}
	        }
	        return Content(string.Empty);
        }
	}
}
