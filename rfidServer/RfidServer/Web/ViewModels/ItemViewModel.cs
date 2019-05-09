using System.Collections.Generic;
using RfidServer.WisAPI.Dto;

namespace RfidServer.Web.ViewModels
{
	public class ItemViewModel
	{
		public int WisId { get; set; }
		public string @Class { get; set; }
		public string Title { get; set; }
		public int Points { get; set; }
		public int Limit { get; set; }
		public int Reg { get; set; }
		public CourseViewModel Course { get; set; }

		public static ItemViewModel CreateItemVm(ItemDto itemDto)
		{
			return new ItemViewModel
			{
				WisId = itemDto.Id,
				Class = itemDto.Class,
				Title = itemDto.Title,
				Points = itemDto.Max
			};
		}
	}
}
