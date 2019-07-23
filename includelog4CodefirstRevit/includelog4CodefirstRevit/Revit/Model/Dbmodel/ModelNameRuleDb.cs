namespace includelog4CodefirstRevit
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ModelNameRuleDb : DbContext
    {
        public ModelNameRuleDb()
            
            : base("data source = 172.16.0.73; initial catalog = BI; user id = sa; password=Aa123456;MultipleActiveResultSets=True;App=EntityFramework" + "providerName=" + "System.Data.SqlClient")
        {
        }

        public virtual DbSet<revitnamerule> revitnamerule { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<revitnamerule>()
                .Property(e => e.familytype)
                .IsUnicode(false);

            modelBuilder.Entity<revitnamerule>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<revitnamerule>()
                .Property(e => e.numbe)
                .IsUnicode(false);
        }
    }
}
