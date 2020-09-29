using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestComm.Helper
{
    public class ExcelHelper
    {
        public static ICellStyle GetTitleCellStyle(HSSFWorkbook workbook)
        {
            ICellStyle style = workbook.CreateCellStyle();
            IFont font = workbook.CreateFont(); //创建一个字体样式对象       
            font.FontName = "微软雅黑"; //和excel里面的字体对应 
            font.FontHeightInPoints = 14;
            style.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            style.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            style.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            style.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            style.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
            style.SetFont(font);

            return style;
        }

        public static ICellStyle GetCellStyle(HSSFWorkbook workbook)
        {
            ICellStyle style = workbook.CreateCellStyle();
            IFont font = workbook.CreateFont(); //创建一个字体样式对象       
            font.FontHeightInPoints = 12;
            style.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            style.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            style.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            style.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            style.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
            style.SetFont(font);
            style.IsLocked = false;

            return style;
        }

        public static ICellStyle GetLockCellStyle(HSSFWorkbook workbook)
        {
            ICellStyle style = workbook.CreateCellStyle();
            IFont font = workbook.CreateFont(); //创建一个字体样式对象       
            font.FontName = "微软雅黑"; //和excel里面的字体对应 
            font.FontHeightInPoints = 12;
            style.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            style.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            style.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            style.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            style.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
            style.SetFont(font);

            style.IsLocked = true;


            return style;
        }

        /// <summary>
        /// 获取工作本
        /// </summary>
        /// <param name="filePath">Excel文件路径</param>
        /// <returns></returns>
        public static IWorkbook GetHssfWorkBook(string filePath)
        {
            IWorkbook hssfWorkBook;
            try
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    if (filePath.EndsWith(".xls"))
                    {
                        hssfWorkBook = new HSSFWorkbook(fileStream);
                    }
                    else
                    {
                        hssfWorkBook = new XSSFWorkbook(fileStream);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return hssfWorkBook;
        }

        /// <summary>
        /// 设置字体颜色
        /// </summary>
        /// <param name="style"></param>
        /// <param name="color"></param>
        public static ICellStyle SetFontColor(HSSFWorkbook workbook, ICellStyle style, short color)
        {
            //创建一个字体颜色
            IFont font = workbook.CreateFont();
            //设置字体颜色
            font.Color = color;
            font.FontHeightInPoints = 12;
            style.BorderTop = BorderStyle.Thin;
            style.BorderBottom = BorderStyle.Thin;
            style.BorderLeft = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
            style.SetFont(font);
            return style;
        }

        //设置单元格对齐方式,字体 Edit by fanzhang in 20200920
        public static ICellStyle SetFontAlign(HSSFWorkbook workbook, string fontStyle = "Arial", short fontSize = 10, HorizontalAlignment align0 = HorizontalAlignment.Center, VerticalAlignment align1 = VerticalAlignment.Center)
        {
            ICellStyle style = workbook.CreateCellStyle();
            IFont font = workbook.CreateFont(); //创建一个字体样式对象       
            font.FontName = fontStyle; //和excel里面的字体对应 
            font.FontHeightInPoints = fontSize;
            style.Alignment = align0;
            style.VerticalAlignment = align1;
            style.SetFont(font);
            style.IsLocked = true;
            return style;
        }

        //绑定下拉列表值--数据项较少 Edit by fanzhang in 20200920   
        public static void SetCellDropdownList(ISheet sheet, int firstrow, int lastrow, int firstcol, int lastcol, string[] vals, bool bol = false)
        {
            if (bol)
            {
                for (int i = firstrow; i < lastrow; i++)
                {
                    for (int j = firstcol; j < lastcol; j++)
                    {
                        sheet.CreateRow(i).CreateCell(j).CellStyle.IsLocked = false;
                    }
                }
            }

            //设置生成下拉框的行和列
            var cellRegions = new CellRangeAddressList(firstrow, lastrow, firstcol, lastcol);
            //设置 下拉框内容
            DVConstraint constraint = DVConstraint.CreateExplicitListConstraint(vals);
            //绑定下拉框和作用区域，并设置错误提示信息
            HSSFDataValidation dataValidate = new HSSFDataValidation(cellRegions, constraint);
            dataValidate.CreateErrorBox("输入不合法", "请输入或选择下拉列表中的值。");
            dataValidate.ShowPromptBox = true;
            sheet.AddValidationData(dataValidate);
        }


        //绑定下拉列表值--数据项较多 Edit by fanzhang in 20200920
        public static void SetCellDropdownListMul(HSSFWorkbook workbook, ISheet sheet, string name, int firstrow, int lastrow, int firstcol, int lastcol, string[] vals, int sheetindex = 1)
        {
            //先创建一个Sheet专门用于存储下拉项的值
            ISheet sheet2 = workbook.CreateSheet(name);
            //隐藏
            workbook.SetSheetHidden(sheetindex, true);
            int index = 0;
            foreach (var item in vals)
            {
                sheet2.CreateRow(index).CreateCell(0).SetCellValue(item);
                index++;
            }
            //创建的下拉项的区域：
            var rangeName = name + "Range";
            IName range = workbook.CreateName();
            range.RefersToFormula = name + "!$A$1:$A$" + index;
            range.NameName = rangeName;
            CellRangeAddressList regions = new CellRangeAddressList(firstrow, lastrow, firstcol, lastcol);

            DVConstraint constraint = DVConstraint.CreateFormulaListConstraint(rangeName);
            HSSFDataValidation dataValidate = new HSSFDataValidation(regions, constraint);
            dataValidate.CreateErrorBox("输入不合法", "请输入或选择下拉列表中的值。");
            dataValidate.ShowPromptBox = true;
            sheet.AddValidationData(dataValidate);
        }

    }
}
