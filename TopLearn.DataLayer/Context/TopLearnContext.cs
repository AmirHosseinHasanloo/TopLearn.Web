using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TopLearn.DataLayer.Entities.Course;
using TopLearn.DataLayer.Entities.Order;
using TopLearn.DataLayer.Entities.Permission;
using TopLearn.DataLayer.Entities.Question;
using TopLearn.DataLayer.Entities.User;
using TopLearn.DataLayer.Entities.Wallet;

namespace TopLearn.DataLayer.Context
{
    public class TopLearnContext : DbContext
    {
        public TopLearnContext(DbContextOptions<TopLearnContext> options) : base(options) { }

        #region Filters

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // if you want return list of users run this query on that =>

            modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsBanned);

            // Filter the list of roles => if IsDelete == true => Do not show

            modelBuilder.Entity<Role>().HasQueryFilter(r => !r.IsDelete);

            // Filter the list of CourseGroups => if IsDelete == true => Do not show

            modelBuilder.Entity<Entities.Course.CourseGroup>().HasQueryFilter(cg => !cg.IsDeleted);

            // Filter the list of Discount => if IsDelete == true => Do not show

            modelBuilder.Entity<Discount>().HasQueryFilter(d => !d.IsDeleted);


            // Filter the list of CourseComment => if IsDelete == true => Do not show

            modelBuilder.Entity<CourseComment>().HasQueryFilter(d => d.IsAdminRead && !d.IsDeleted);

            // disable cascade delete =>
            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
       .SelectMany(t => t.GetForeignKeys())
       .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }

            base.OnModelCreating(modelBuilder);
        }

        #endregion

        #region User

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserCourse> UserCourse { get; set; }
        public DbSet<UserDiscountCode> UserDiscountCode { get; set; }

        #endregion

        #region Wallet

        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<WalletType> WalletType { get; set; }

        #endregion

        #region Permission

        public DbSet<Permission> Permission { get; set; }
        public DbSet<RolePermission> RolePermission { get; set; }

        #endregion

        #region Course

        public DbSet<CourseGroup> CourseGroup { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<CourseLevel> CourseLevel { get; set; }
        public DbSet<CourseStatus> CourseStatus { get; set; }
        public DbSet<CourseEpisode> CourseEpisode { get; set; }
        public DbSet<CourseComment> CourseComment { get; set; }
        public DbSet<CourseVote> CourseVote { get; set; }

        #endregion

        #region Questions
        public DbSet<Question> Question { get; set; }
        public DbSet<Answer> Answer { get; set; }
        #endregion

        #region Order   
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<Discount> Discount { get; set; }
        #endregion
    }
}
