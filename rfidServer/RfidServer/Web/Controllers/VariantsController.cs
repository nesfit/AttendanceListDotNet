using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RfidServer.DAL;
using RfidServer.DAL.Repositories;
using RfidServer.DAL.Repositories.Abstract;
using RfidServer.Web.ViewModels;
using RfidServer.WisAPI;

namespace RfidServer.Web.Controllers
{
    public class VariantsController : BaseController
    {
	    private readonly IVariantRepository _variantRepository;
 
		public VariantsController(RegistrationDbContext context)
		{
			_variantRepository = new VariantRepository(context);
		}

        // GET: Variants
        public async Task<IActionResult> Index()
        {
	        var variants = await _variantRepository.GetAllVariants();
			var variantsVm = variants.Select(VariantViewModel.CreateVariantVm);
			ViewBag.activeVariantId = VariantViewModel.ActiveVariantId;
			ViewBag.autoRegister = WisClient.AutoRegister;
			return View(variantsVm);
        }

        // Post: Variants
		[HttpPost, ActionName("Index")]
        public async Task<IActionResult> IndexPost(int? activeVariantId)
        {
	        var variants = await _variantRepository.GetAllVariants();
	        var variantsVm = variants.Select(VariantViewModel.CreateVariantVm);
	        if (activeVariantId != null)
	        {
		        var activeVariant = await _variantRepository.GetVariantById(activeVariantId.Value);
		        if (activeVariant == null)
		        {
			        return NotFound();
		        }
		        VariantViewModel.ActiveVariantId = activeVariantId;
		        WisClient.ActiveCourseId = activeVariant.WisCourseId;
	        }
			ViewBag.activeVariantId = VariantViewModel.ActiveVariantId;
	        ViewBag.autoRegister = WisClient.AutoRegister;
	        return View(variantsVm);
        }

		// GET: Variants/Details/id
		public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var variant = await _variantRepository
	            .GetVariantById(id.Value);

			if (variant == null)
            {
                return NotFound();
            }

			var variantVm = VariantViewModel.CreateVariantVm(variant);
			return View(variantVm);
        }

        // GET: Variants/FindCourse
        public async Task<IActionResult> FindCourse(string searchString)
        {
			if (!string.IsNullOrEmpty(searchString))
	        {
				var courseDto = (await WisClient.GetCoursesAsync(searchString))?.FirstOrDefault();
		        if (courseDto != null)
		        {
			        var items = (await WisClient.GetItemsAsync(courseDto.Id));
			        if (items != null)
			        {
				        var courseVm = CourseViewModel.CreateCourseVm(courseDto);
						courseVm.Items = items.Select(ItemViewModel.CreateItemVm);
				        return View(courseVm);
			        }
		        }
	        }
	        return View();
        }

		// GET: Variants/Create
		public async Task<IActionResult> Create(CourseViewModel courseVm, int? itemId, int? points)
		{
			if (!ModelState.IsValid || courseVm == null || itemId == null || points == null)
			{
				return BadRequest();
			}

			var variantsDto = await WisClient.GetVariantsAsync(courseVm.WisId, itemId.Value);
			if (variantsDto == null)
			{
				return NotFound();
			}
			var variantsVm = variantsDto.Select(v => VariantViewModel.CreateVariantVm(v, itemId.Value, points.Value, courseVm));
			return View(variantsVm);
		}

		// POST: Variants/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost, ActionName("Create")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateConfirmed(VariantViewModel variantVm)
		{
			if (ModelState.IsValid)
			{
				if (!_variantRepository.VariantExists(variantVm.WisId))
				{
					try
					{
						var variant = VariantViewModel.CreateVariant(variantVm);
						if (variant != null)
						{
							await _variantRepository.InsertVariant(variant);
							await _variantRepository.SaveChanges();
							VariantViewModel.ActiveVariantId = variant.Id;
						}
					}
					catch (DbUpdateConcurrencyException) { }
				}
				return RedirectToAction(nameof(Index));
			}
			return View();
		}

		// GET: Variants/Delete/id
		public async Task<IActionResult> Delete(int? id)
        {
			if (id == null)
			{
                return BadRequest();
            }

			var variant = await _variantRepository
				.GetVariantById(id.GetValueOrDefault());
			if (variant == null)
            {
                return NotFound();
            }

			var variantVm = VariantViewModel.CreateVariantVm(variant);
			return View(variantVm);
        }

        // POST: Variants/Delete/id
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
	        if (id == null)
	        {
		        return BadRequest();
	        }

			await _variantRepository.DeleteVariant(id.Value);
	        await _variantRepository.SaveChanges();
	        if (id.Value == VariantViewModel.ActiveVariantId)
	        {
		        VariantViewModel.ActiveVariantId = null;
			}
			return RedirectToAction(nameof(Index));
        }

        [Route("autoregister")]
        public OkResult ToggleAutoRegister(int toggle)
        {
	        WisClient.AutoRegister = toggle == 1;
	        return Ok();
        }
    }
}
