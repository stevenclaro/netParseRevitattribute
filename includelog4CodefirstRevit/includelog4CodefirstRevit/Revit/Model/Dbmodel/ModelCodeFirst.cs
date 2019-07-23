namespace includelog4CodefirstRevit
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using includelog4CodefirstRevit.Revit.Model.Dbmodel;
    public class ModelCodeFirst : DbContext
    {
        //您的上下文已配置为从您的应用程序的配置文件(App.config 或 Web.config)
        //使用“ModelCodeFirst”连接字符串。默认情况下，此连接字符串针对您的 LocalDb 实例上的
        //“includelog4CodefirstRevit.ModelCodeFirst”数据库。
        // 
        //如果您想要针对其他数据库和/或数据库提供程序，请在应用程序配置文件中修改“ModelCodeFirst”
        //连接字符串。
        public ModelCodeFirst()
            : base("data source = 172.16.0.73; initial catalog = revitbim; user id = sa; password=Aa123456;MultipleActiveResultSets=True;App=EntityFramework" + "providerName=" + "System.Data.SqlClient")
        {
            Database.SetInitializer<ModelCodeFirst>(new DropCreateDatabaseAlways<ModelCodeFirst>());
        }

        public virtual DbSet<familyModelError> familyModelErrors { get; set; }
        public virtual DbSet<ParaModel> ParaModels { get; set; }
        public virtual DbSet<ParaEnum> ParaEnums { get; set; }
        
        //为您要在模型中包含的每种实体类型都添加 DbSet。有关配置和使用 Code First  模型
        //的详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=390109。

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}