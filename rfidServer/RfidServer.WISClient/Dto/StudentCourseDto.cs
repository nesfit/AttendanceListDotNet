namespace RfidServer.WisAPI.Dto
{
    public class StudentCourseDto
    {
        public int Id { get; set; }
        public int Person_Id { get; set; }
        public string Name { get; set; }
		public string Email { get; set; }
		public string Login { get; set; }
		public string Status { get; set; }
		public bool Passed { get; set; }
		public string Type { get; set; }
		public string ExamGrade { get; set; }
        public int ExamCount { get; set; }
        public int Points { get; set; }
		public string Update { get; set; }
    }
}