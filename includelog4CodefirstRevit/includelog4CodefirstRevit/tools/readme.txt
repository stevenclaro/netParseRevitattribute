把Model抛弃掉配置文件
	 public Model1()
            : base("data source = 172.16.0.73; initial catalog = BI; user id = sa; password=Aa123456;MultipleActiveResultSets=True;App=EntityFramework" +"providerName="+"System.Data.SqlClient")
        {
            //采用程序来写base的连接字符串，这个已经成功实现从数据库中取值了
        }


把Log4抛弃掉配置文件
	 增加一个log.cs文件
		在这个文件中，就可以定义文件名称及Log的级别
		如果采用Debug级别，那么后面的
		都可以
		参考https://www.cnblogs.com/chucklu/p/5404813.html


	 然后在Command的命令中，首先执行这个
	 includelog4CodefirstRevit.Logger.Setup();

	 问题：
		第一次执行之后，第二次就不执行了。除非换一个文件名
		，我估计是原来的文件，被上个进程锁住了。
		无法打开

		提示这个文件已经存在

		为什么Familyinstance过滤，是柱子，但是不是墙？墙需要单独进行过滤？
		bugReport

		2018年8月20日
		存在的问题，进行继续完成
		族的名称，有一级，二级之分
				其中一级为 墙，二级，为叠墙
			但是这个与类别 有什么关系呢？
		如何取到墙上的厚度，这个厚度，与名称进行匹配
		如何采用regex的方法，固定的名称进行Split
			什么时候用split的match
			regex的好的网站参考
			https://www.cnblogs.com/viviancc/p/3448272.html

		2018年8月21日
			程序的结构，先进行wall等实体的设置，方便将Revit的模型的数据取到之后，纳入到wall实体中。
			在取到revit的数据的时候，就需要采用责任链的方式来获取

			然后后面的程序代码，就是字符串的解析规则，单独作为一个service

		2018年8月24日
			新增了Excel的文件输出

			经过验证，该dll已经它所依赖的dll，Copy给李兵。能够执行，并在桌面产生Excel目标文件。
		  
		  未来考虑，将 匹配的规则，开放成Xml的文件，让用户来进行调节。

