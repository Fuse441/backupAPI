namespace colab_api.Requests.user
{
    public class UserUpsertRequest
    {
        public string username { get; set; }
        public string? password { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public DateOnly birthDay { get; set; }
        public string phoneNumber { get; set; }
        public string? userType { get; set; }
        public int active { get; set; }
    }
}
