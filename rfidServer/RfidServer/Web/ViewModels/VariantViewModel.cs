using System.Collections.Generic;
using RfidServer.DAL.Entity;
using RfidServer.WisAPI;
using RfidServer.WisAPI.Dto;

namespace RfidServer.Web.ViewModels
{
	public class VariantViewModel
	{
		public int Id { get; set; }
		public int WisId { get; set; }
		public int WisItemId { get; set; }
		public string Title { get; set; }
		public int Points { get; set; }
		public int Limit { get; set; }
		public int Reg { get; set; }
		public CourseViewModel Course { get; set; }

		public static int? ActiveVariantId { get; set; }

		public static VariantViewModel CreateVariantVm(Variant variant)
		{
			return new VariantViewModel
			{
				Id = variant.Id,
				WisId = variant.WisId,
				WisItemId = variant.WisItemId,
				Points = variant.Points,
				Title = variant.Title,
				Limit = variant.Limit,
				Course = new CourseViewModel
				{
					WisId = variant.WisId,
					Abbrv = variant.CourseAbbrv,
					Year = variant.Year,
					Sem = variant.Sem
				}
			};
		}

		public static VariantViewModel CreateVariantVm(VariantDto variantDto, int itemId, int points, CourseViewModel courseVm)
		{
			return new VariantViewModel
			{
				WisId = variantDto.Id,
				WisItemId = itemId,
				Title = variantDto.Title,
				Points = points,
				Limit = variantDto.Limit,
				Reg = variantDto.Reg,
				Course = courseVm
			};
		}

		public static Variant CreateVariant(VariantViewModel variantVm)
		{
			return new Variant()
			{
				WisId = variantVm.WisId,
				WisItemId = variantVm.WisItemId,
				Title = variantVm.Title,
				Points = variantVm.Points,
				Limit = variantVm.Limit,
				WisCourseId = variantVm.Course.WisId,
				CourseAbbrv = variantVm.Course.Abbrv,
				Year = variantVm.Course.Year,
				Sem = variantVm.Course.Sem
			};
		}

		public static RegistrationDto CreateRegistrationDto(Variant variant)
		{
			return new RegistrationDto
			{
				WisVariantId = variant.WisId,
				WisItemId = variant.WisItemId,
				WisCourseId = variant.WisCourseId,
				CourseAbbrv = variant.CourseAbbrv,
				Year = variant.Year,
				Sem = variant.Sem,
				Points = variant.Points
			};
		}
	}
}
