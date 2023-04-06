using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TST_API_MySql.Db.DO;

namespace TST_API_MySql.Db
{
    /// <summary>
    /// Defines the class of Employee Database Context
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.DbContext" />
    public class EmployeeDbContext : DbContext
    {
        #region Protected Properties

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        protected IConfiguration Configuration { get; }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the employees.
        /// </summary>
        /// <value>
        /// The employees.
        /// </value>
        public DbSet<Employee> Employees { get; set; }

        /// <summary>
        /// Gets or sets the positions.
        /// </summary>
        /// <value>
        /// The positions.
        /// </value>
        public DbSet<Position> Positions { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeDbContext"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public EmployeeDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        #endregion

        #region Overriden Methods

        /// <summary>
        /// <para>
        /// Override this method to configure the database (and other options) to be used for this context.
        /// This method is called for each instance of the context that is created.
        /// The base implementation does nothing.
        /// </para>
        /// <para>
        /// In situations where an instance of <see cref="T:Microsoft.EntityFrameworkCore.DbContextOptions" /> may or may not have been passed
        /// to the constructor, you can use <see cref="P:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.IsConfigured" /> to determine if
        /// the options have already been set, and skip some or all of the logic in
        /// <see cref="M:Microsoft.EntityFrameworkCore.DbContext.OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder)" />.
        /// </para>
        /// </summary>
        /// <param name="optionsBuilder">A builder used to create or modify options for this context. Databases (and other extensions)
        /// typically define extension methods on this object that allow you to configure the context.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(Configuration.GetConnectionString("EmployeeDatabase"),
                new MySqlServerVersion(new Version(8, 0))
            );
        }

        /// <summary>
        /// Override this method to further configure the model that was discovered by convention from the entity types
        /// exposed in <see cref="T:Microsoft.EntityFrameworkCore.DbSet`1" /> properties on your derived context. The resulting model may be cached
        /// and re-used for subsequent instances of your derived context.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context. Databases (and other extensions) typically
        /// define extension methods on this object that allow you to configure aspects of the model that are specific
        /// to a given database.</param>
        /// <remarks>
        /// If a model is explicitly set on the options for this context (via <see cref="M:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.UseModel(Microsoft.EntityFrameworkCore.Metadata.IModel)" />)
        /// then this method will not be run.
        /// </remarks>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Positions)
                .WithMany(e => e.Employees)
                .UsingEntity<Dictionary<string, object>>("EmployeePosition",
                    j => j.HasOne<Position>().WithMany().OnDelete(DeleteBehavior.Restrict),
                    j => j.HasOne<Employee>().WithMany().OnDelete(DeleteBehavior.Cascade));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Recreates the database base entities.
        /// </summary>
        public void RecreateDbBaseEntities()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();

            var p1 = new Position { Id = Guid.NewGuid(), Grade = 1, Name = "Position1" };
            var p2 = new Position { Id = Guid.NewGuid(), Grade = 2, Name = "Position2" };
            var p3 = new Position { Id = Guid.NewGuid(), Grade = 3, Name = "Position3" };
            var p4 = new Position { Id = Guid.NewGuid(), Grade = 4, Name = "Position4" };

            Positions.AddRange(p1, p2, p3, p4);

            var e1 = new Employee() { Id = Guid.NewGuid(), Fio = "Employee1", Birthday = new DateTime(2001, 01, 01) };
            var e2 = new Employee() { Id = Guid.NewGuid(), Fio = "Employee2", Birthday = new DateTime(2002, 02, 02) };
            var e3 = new Employee() { Id = Guid.NewGuid(), Fio = "Employee3", Birthday = new DateTime(2003, 03, 03) };

            Employees.AddRange(e1, e2, e3);

            e1.Positions.Add(p1);
            
            e2.Positions.Add(p1);
            e2.Positions.Add(p2);
            
            e3.Positions.Add(p1);
            e3.Positions.Add(p2);
            e3.Positions.Add(p3);
            e3.Positions.Add(p4);

            SaveChanges();
        }

        #endregion
    }
}