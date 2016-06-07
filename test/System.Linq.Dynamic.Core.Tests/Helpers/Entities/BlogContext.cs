﻿#if EFCORE
using System.Linq.Dynamic.Core.Tests.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
#else
using System.Data.Entity;
using SQLite.CodeFirst;
#endif

namespace System.Linq.Dynamic.Core.Tests.Helpers.Entities
{
    public class BlogContext : DbContext
    {
#if NET4 || EF
        public BlogContext(): base("SQLiteDefaultConnection") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var sqliteConnectionInitializer = new SqliteDropCreateDatabaseAlways<BlogContext>(modelBuilder);
            Database.SetInitializer(sqliteConnectionInitializer);
        }
#else
        public BlogContext(DbContextOptions options)
            : base(options)
        {
        }
#endif

#if (!NET4 && !NET452)
        public void EnableLogging()
        {
            var serviceProvider = this.GetInfrastructure();
            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            loggerFactory.AddProvider(new DbLoggerProvider());
        }
#endif

        public DbSet<Blog> Blogs { get; set; }

        public DbSet<Post> Posts { get; set; }
    }
}