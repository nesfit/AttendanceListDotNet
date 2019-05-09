namespace RfidServer.WisAPI.Dto
{
    public class WisResponse<T>
    {
        public readonly string Status;
        public readonly int Count;
        public T data { get; set; }
    }
}
