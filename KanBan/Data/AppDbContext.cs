using Microsoft.EntityFrameworkCore;
using KanBan.Models;

namespace KanBan.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Board> Boards { get; set; }
        public DbSet<BoardMember> BoardMembers { get; set; }
        public DbSet<BoardColumn> Columns { get; set; }

        public DbSet<ColumnTask> ColumnTasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Board>()
               .HasOne(b => b.Creator)
               .WithMany(u => u.CreatedBoards)
               .HasForeignKey(b => b.CreatedBy)
               .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<BoardMember>()
                .HasOne(bm => bm.User)
                .WithMany(u => u.BoardMemberships)
                .HasForeignKey(bm => bm.UserId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<BoardMember>()
                .HasOne(bm => bm.Board)
                .WithMany(b => b.Members)
                .HasForeignKey(bm => bm.BoardId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
