using System.Data;

namespace colab_api.Responses
{
    public class userloginResponse
    {
        public UserProfile userProfile { get; set; }
        public int active { get; set; }
        public string jwt { get; set; }
        public string userType { get; set; }
    }

    public class UserProfile
    {
        public int id { get; set; }
        public string username { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public DateOnly birthDay { get; set; }
        public string phoneNumber { get; set; }
        public string userType {get; set;}

    }
}
