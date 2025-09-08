//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//using BarcodePrintLabel.Models;
//using Microsoft.EntityFrameworkCore;
//using System.Reflection.Emit;

//namespace BarcodePrintLabel.Database
//{
//    public class DatabaseContext : DbContext
//    {
//        private readonly string _connectionString;

//        public DatabaseContext(string connectionString)
//        {
//            _connectionString = connectionString;
//        }

//        public DbSet<TestResult> TestResults { get; set; }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//                optionsBuilder.UseMySQL(_connectionString);
//            }
//        }

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            modelBuilder.Entity<TestResult>(entity =>
//            {
//                entity.ToTable("TestResults");

//                entity.HasKey(e => e.Id);

//                entity.Property(e => e.SerialNumber).HasMaxLength(100);
//                entity.Property(e => e.QRCode).HasMaxLength(200);
//                entity.Property(e => e.RESULT).HasMaxLength(50);
//                entity.Property(e => e.ERROR_CODE).HasMaxLength(50);
//            });
//        }
//    }

//}
