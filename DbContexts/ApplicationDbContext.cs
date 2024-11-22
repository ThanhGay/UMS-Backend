using Microsoft.EntityFrameworkCore;
using Server.Entities;

namespace Server.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<LopHP> ClassHPs { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<LopHP_Room> LopHP_Rooms { get; set; }
        public DbSet<ChuongTrinhKhung> ChuongTrinhKhungs { get; set; }
        public DbSet<MonHoc_ChuongTrinhKhung> DetailCTKs { get; set; }
        public DbSet<LopHP_Student> LopHP_Students { get; set; }
        public DbSet<LopHP_Teacher> LopHP_Teachers { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<MonHoc_ChuongTrinhKhung>().HasOne<ChuongTrinhKhung>().WithMany().HasForeignKey(_ => _.ChuongTrinhKhungId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<LopHP_Room>().HasOne<LopHP>().WithMany().HasForeignKey(s => s.LopHpId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<LopHP_Room>().HasOne<Room>().WithMany().HasForeignKey(s => s.RoomId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LopHP_Student>().HasOne<LopHP>().WithMany().HasForeignKey(_ => _.LopHpId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LopHP_Teacher>().HasOne<LopHP>().WithMany().HasForeignKey(_ => _.LopHpId).OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
