namespace RfidServer.WisAPI.Dto
{
    public class StudentDto
    {
        public int Id { get; set; }
        public int Person_Id { get; set; }
        public string Name { get; set; }
		public string Email { get; set; }
        public int Points { get; set; }
        public string Date { get; set; }
        public string Who { get; set; }
        public string RegType { get; set; }
		public string RegTime { get; set; }
		public string Update { get; set; }
		public int VariantId { get; set; }
    }
}
