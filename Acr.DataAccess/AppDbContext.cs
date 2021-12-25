using Acr.Entities.Abstract;
using Acr.Entities.Concrete;
using Acr.Entities.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;

namespace Acr.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=SALIH; Initial Catalog=test-log-db;Integrated Security=True;
                MultipleActiveResultSets = True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.AddGlobalFilter(); // isdeleted filtresi
        }

        #region Entities | Tablolar
        public DbSet<Log> Log { get; set; }
        public DbSet<TestTable> TestTable { get; set; }
        public DbSet<TestJsonTable> TestJsonTable { get; set; }
        #endregion


        #region Views | Sql Viewlar
        public DbSet<ViewTestTable> ViewTestTable { get; set; }
        #endregion

    }
    public static class ModelBuilderExtensions
    {
        public static void AddGlobalFilter(this ModelBuilder modelBuilder)
        {
            /*
                Model içerisindeki tüm Entity tiplerine bak ve içerisinde ISoftDeletable olanları bul ve
                SetSoftDeleteFilter method'unu çağır.
            */
            foreach (var type in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDeletable).IsAssignableFrom(type.ClrType))
                    modelBuilder.SetSoftDeleteFilter(type.ClrType);
            }
        }

        public static void SetSoftDeleteFilter(this ModelBuilder modelBuilder, Type entityType)
        {
            SetSoftDeleteFilterMethod.MakeGenericMethod(entityType)
                .Invoke(null, new object[] { modelBuilder });
        }

        static readonly MethodInfo SetSoftDeleteFilterMethod = typeof(ModelBuilderExtensions)
                   .GetMethods(BindingFlags.Public | BindingFlags.Static)
                   .Single(t => t.IsGenericMethod && t.Name == "SetSoftDeleteFilter");

        public static void SetSoftDeleteFilter<TEntity>(this ModelBuilder modelBuilder)
            where TEntity : class, ISoftDeletable
        {
            modelBuilder.Entity<TEntity>().HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
