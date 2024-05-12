using colab_api.Responses.user;

namespace colab_api.Responses.space
{
    public class BookingResponse : UsersResponse
    {
        public int bookingId { get; set; }
        public int userId {  get; set; }
        public int spaceId { get; set; }
        public TimeOnly bookingStartTime { get; set; }
        public TimeOnly bookingEndTime { get; set; }
        public DateOnly bookingDate { get; set; }
        public int bookingStatus { get; set; }
        public string additionalNote { get; set; }

        public string spaceName { get; set; }
        public string spaceDescription { get; set; }
        public string spaceType { get; set; }
        public string spaceDetails { get; set; }
    
    }
    public class ChartResponse
    {
        public int spaceId { get; set; }
        public string spaceName { get; set; }
        public int count { get; set; }
    }

}
