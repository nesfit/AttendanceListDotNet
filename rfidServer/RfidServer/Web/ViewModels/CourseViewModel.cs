using System.Collections.Generic;
using RfidServer.WisAPI.Dto;

namespace RfidServer.Web.ViewModels
{
	public class CourseViewModel
	{
		public int WisId { get; set; }
		public string Abbrv { get; set; }
		public int Year { get; set; }
		public string Sem { get; set; }
		public string Lang { get; set; }
		public int Credits { get; set; }
		public string Completion { get; set; }
		public int Capacity { get; set; }
		public int StudentsCount { get; set; }
		public string Title { get; set; }
		public IEnumerable<ItemViewModel> Items { get; set; }

		public static CourseViewModel CreateCourseVm(CourseDto courseDto)
		{
			return new CourseViewModel
			{
				WisId = courseDto.Id,
				Abbrv = courseDto.Abbrv,
				Year = courseDto.Year,
				Sem = courseDto.Sem,
				Lang = courseDto.Lang,
				Credits = courseDto.Credits,
				Completion = courseDto.Completion,
				Capacity = courseDto.Capacity,
				StudentsCount = courseDto.Students,
				Title = courseDto.Title
			};
		}
	}
}
