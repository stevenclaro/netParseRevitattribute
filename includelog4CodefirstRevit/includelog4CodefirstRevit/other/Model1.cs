namespace includelog4CodefirstRevit
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("data source = 172.16.0.73; initial catalog = BI; user id = sa; password=Aa123456;MultipleActiveResultSets=True;App=EntityFramework" +"providerName="+"System.Data.SqlClient")
        {
            //���ó�����дbase�������ַ���������Ѿ��ɹ�ʵ�ִ����ݿ���ȡֵ��
        }

        public virtual DbSet<employee> employee { get; set; }
    //    public virtual DbSet<familyModel> familyModels { get; set; }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    //modelBuilder.Entity<employee>()
        //    //    .Property(e => e.Empid)
        //    //    .IsUnicode(false);

        //    //modelBuilder.Entity<employee>()
        //    //    .Property(e => e.name)
        //    //    .IsUnicode(false);
        //}
    }
}
