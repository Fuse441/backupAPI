using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace colab_api.Models
{
    [Table("space")]
    public class Space
    {
        [Key] 
        public int space_id { get; set; }
        [Column("space_name")]
        public string space_name { get; set; }
        [Column("des")]
        public string des { get; set;}
        [Column("space_type")]
        public string space_type { get; set; }
        [Column("status")]
        public int status { get; set;}
        [Column("space_details")]
        public string space_details { get; set; }
        [Column("partner_id")]
        public int partner_id { get; set;}
        [Column("image")]
        public string image { get; set; }
    }
}
