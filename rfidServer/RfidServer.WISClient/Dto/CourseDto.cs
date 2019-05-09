namespace RfidServer.WisAPI.Dto
{
    public class CourseDto
    {
        public int Id { get; set; }
        public string Abbrv { get; set; }
        public int Year { get; set; }
        public string Sem { get; set; }
        public string Lang { get; set; }
        public int Credits { get; set; }
        public string Completion { get; set; }
        public int Capacity { get; set; }
        public int Students { get; set; }
		public string Title { get; set; }
    }
}
