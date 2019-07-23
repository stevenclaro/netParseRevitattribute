using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace includelog4CodefirstRevit.Revit.BLL
{
   
        public class buildchain
        {
          public  List<parent> chainlist = new List<parent>();

        public void getAllSubclass()
        {
            GetAllsysFamilySubClass();
            GetAllNonsysFamilySubClass();
        }
            private void GetAllsysFamilySubClass()
            {


                List<Type> lstTypeSubClass = this.GetType().Assembly.GetTypes().ToList().FindAll(the => the.BaseType == typeof(systemfamily)).ToList();
                foreach (Type typeSubClass in lstTypeSubClass)
                {
                    //得到当前父类的子类
                    parent Subchr = this.GetType().Assembly.CreateInstance(typeSubClass.FullName) as parent;
                    chainlist.Add(Subchr);
                }
            }
        private void GetAllNonsysFamilySubClass()
        {


            List<Type> lstTypeSubClass = this.GetType().Assembly.GetTypes().ToList().FindAll(the => the.BaseType == typeof(nonsystemfamily)).ToList();
            foreach (Type typeSubClass in lstTypeSubClass)
            {
                //得到当前父类的子类
                parent Subchr = this.GetType().Assembly.CreateInstance(typeSubClass.FullName) as parent;
                chainlist.Add(Subchr);
            }
            
        }
    }
    }

