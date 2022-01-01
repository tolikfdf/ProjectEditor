using Microsoft.EntityFrameworkCore;
using System;

namespace Entities
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Models.Task> Tasks { get; set; }

        public DbSet<Models.Project> Projects { get; set; }
    }
}
