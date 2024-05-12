using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace colab_api.Responses.user
{

    public class UsersResponse
    {
        public int id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public DateOnly birthDay { get; set; }
        public string phoneNumber { get; set; }
        public string username { get; set; }

        public string userType { get; set; }
        public bool active { get; set; }

    }
}
