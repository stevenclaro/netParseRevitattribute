using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NPOI;
using System.Data;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;


namespace includelog4CodefirstRevit.tools
{
    public class npoiexcel
    {
        List<familyModelError> _list;
        public npoiexcel(List<familyModelError> fmrlist)
        {
            _list = fmrlist;
        }
        public void generateexcel()
        {
            // 1、//创建工作簿对象
            IWorkbook workbook = new HSSFWorkbook();
            //2、//创建工作表
            ISheet sheet = workbook.CreateSheet("onesheet");
            IRow row0 = sheet.CreateRow(0);


            row0.CreateCell(0).SetCellValue("guidid");
            row0.CreateCell(1).SetCellValue("family一级族名称");
            row0.CreateCell(2).SetCellValue("family二级族名称");
            row0.CreateCell(3).SetCellValue("familysymbolname族类型名称");
            row0.CreateCell(4).SetCellValue("familyinstancename族实例ID");
            row0.CreateCell(5).SetCellValue("errorinfo");
            row0.CreateCell(6).SetCellValue("correctinfo");


            for (int r = 1; r < _list.Count; r++)
            {
                // 3、//创建行row
                IRow row = sheet.CreateRow(r);
                row.CreateCell(0).SetCellValue(_list[r].guidid);
                row.CreateCell(1).SetCellValue(_list[r].family一级族名称);
                row.CreateCell(2).SetCellValue(_list[r].family二级族名称);
                row.CreateCell(3).SetCellValue(_list[r].familysymbolname族类型名称);
                row.CreateCell(4).SetCellValue(_list[r].familyinstancename族实例ID);
                row.CreateCell(5).SetCellValue(_list[r].errorinfo);
                row.CreateCell(6).SetCellValue(_list[r].correctinfo);


            }

            writeExcelBook(workbook, @"C: \Users\Administrator\Desktop\写入excel.xls");
            //创建流对象并设置存储Excel文件的路径
            //using (FileStream url = File.OpenWrite(@"C:\Users\Administrator\Desktop\写入excel.xls"))
            //{
            //    try
            //    {
            //        //导出Excel文件
            //        workbook.Write(url);
            //        //       Response.Write("<script>alert('写入成功！')</script>");
            //    }
            //    catch (Exception ex)
            //    {
            //        string x = "";
            //    }
            //};

        }
        public  bool writeExcelBook(NPOI.SS.UserModel.IWorkbook book, String outFilePath)
        {
            // 写入到客户端  
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                book.Write(ms);
                try
                {
                    using (FileStream fs = new FileStream(outFilePath, FileMode.Create, FileAccess.Write))
                    {
                        byte[] data = ms.ToArray();
                        fs.Write(data, 0, data.Length);
                        fs.Flush();
                    }
                }
                catch (Exception e)
                {
                    return false;
                }

            }
            return true;
        }
    }
}