using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace colab_api.Models
{
    [Table("chat")]
    public class Chat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int chat_id { get; set; }
        [Column("user_id")]
        public int user_id { get; set; }
        [Column("details")]
        public string details { get; set;}
       
    }
}
