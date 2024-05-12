namespace colab_api.Requests.user
{
    public class BookingUpsertRequest
    {
       
    
        public int userId { get; set; }
        public int spaceId { get; set; }
        public  TimeOnly startTime { get; set; }
        public TimeOnly endTime { get; set; }
        public int bookingStatus { get; set; }
        public DateOnly bookingDate { get; set; }
        public string note { get; set; }
    }
}
