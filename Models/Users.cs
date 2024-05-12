using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace colab_api.Models
{
    [Table("users")]
    public class Users
    {
        [Key] public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public DateOnly birthday { get; set; }
        public string phone_number { get; set; }
        public string username { get; set; }
        public string? password { get; set; }
        public string user_type { get; set;}
        public int active { get; set; }

    }
}
