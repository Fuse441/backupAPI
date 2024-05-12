using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace colab_api.Models
{
    [Table("booking")]
    public class Booking
    {
        [Key] 
        public int booking_id { get; set; }
        [Column("user_id")]
        public int user_id { get; set; }
        [Column("space_id")]
        public int space_id { get; set;}
        [Column("booking_start_time")]
        public TimeOnly booking_start_time { get; set; }
        [Column("booking_end_time")]
        public TimeOnly booking_end_time { get; set;}
        [Column("booking_date")]
        public DateOnly booking_date { get; set; }
        [Column("booking_status")]
        public int booking_status { get; set;}
        [Column("additional_notes")]
        public string additional_notes { get; set; }
    }
}
