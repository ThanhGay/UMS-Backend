﻿using Microsoft.EntityFrameworkCore;
using Server.Entities;

namespace Server.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<LopHP> ClassHPs { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<LopHP_Room> lopHP_Rooms { get; set; }
        public DbSet<ChuongTrinhKhung> ChuongTrinhKhungs { get; set; }
        public DbSet<MonHoc_ChuongTrinhKhung> DetailCTKs { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LopHP>().HasOne<Subject>().WithMany().HasForeignKey(e => e.SubjectId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MonHoc_ChuongTrinhKhung>().HasOne<ChuongTrinhKhung>().WithMany().HasForeignKey(_ => _.ChuongTrinhKhungId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<MonHoc_ChuongTrinhKhung>().HasOne<Subject>().WithMany().HasForeignKey(_ => _.SubjectId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LopHP_Room>().HasOne<LopHP>().WithMany().HasForeignKey(s => s.LopHpId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<LopHP_Room>().HasOne<Room>().WithMany().HasForeignKey(s => s.RoomId).OnDelete(DeleteBehavior.Restrict);

            

            base.OnModelCreating(modelBuilder);
        }
    }
}