using colab_api.Responses.user;
using Microsoft.EntityFrameworkCore;
namespace colab_api.Models
{
    public partial class SampleDBContext : DbContext
    {
    public SampleDBContext(DbContextOptions
        <SampleDBContext> options)
          : base(options)
        {
        }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Space> Spaces { get; set; }
        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<Chat> Chats { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(k => k.id);
            });
            modelBuilder.Entity<Space>(entity =>
            {
                entity.HasKey(k => k.space_id);
            });
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(k => k.booking_id);
            });
            modelBuilder.Entity<Chat>(entity =>
            {
                entity.HasKey(k => k.chat_id);
            });
            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
