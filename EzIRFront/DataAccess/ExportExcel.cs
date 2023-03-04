using Microsoft.AspNetCore.Hosting;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using Newtonsoft.Json.Linq;
using EzIRFront.Models.ModelViews;
using EzIRFront.Models.ExcelModel;
using EzIRFront.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Localization;
using EzIRFront.Models.ExcelModels;

namespace EzIRFront.DataAccess
{
    public class ExportExcel : IExportExcel
    {
        IHostingEnvironment _hostingEnvironment;
        private readonly IHtmlLocalizer<SharedResource> _localizer;
        public ExportExcel(IHostingEnvironment hostingEnvironment, IHtmlLocalizer<SharedResource> localizer)
        {
            _hostingEnvironment = hostingEnvironment;
            _localizer = localizer;
        }
        public ExcelWorksheet Excel(ExcelWorksheet workSheet, int numCol = 11, int numRow = 30, string unit = "d", string title = "Bảng thông tin")
        {
            // workSheet cần thêm teamplate
            // numCol: số cột, numRow: số dòng nội dung bảng, unit: đơn vị

            string pathImg = Path.Combine(_hostingEnvironment.WebRootPath, "images/logo_ngang.png");
            workSheet.Column(1).Width = 50;

            for (int i = 2; i <= numCol; i++)
            {
                workSheet.Column(i).Width = 22;
            }
            //code lại
            //workSheet.Row(6).Height = 18;

            if (File.Exists(pathImg))
            {
                Image logo = Image.FromFile(pathImg);

                var excelImage = workSheet.Drawings.AddPicture("My Logo", logo);

                //add the image to row 20, column E
                excelImage.SetPosition(1, 0, 0, 0);
                excelImage.SetSize(200, 60);
            }

            //Tieu de
            workSheet.Cells[1, 1, numRow + 5, numCol].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[1, 1, numRow + 5, numCol].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[1, 1, numRow + 5, numCol].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[1, 1, numRow + 5, numCol].Style.Border.Left.Style = ExcelBorderStyle.Thin;

            // Assign borders
            workSheet.Cells[1, 1, 5, numCol].Style.Border.Top.Color.SetColor(Color.Transparent);
            workSheet.Cells[1, 1, 5, numCol].Style.Border.Left.Color.SetColor(Color.Transparent);
            workSheet.Cells[1, 1, 5, numCol].Style.Border.Right.Color.SetColor(Color.Transparent);
            workSheet.Cells[1, 1, 5, numCol].Style.Border.Bottom.Color.SetColor(Color.Transparent);
            workSheet.Cells[1, numCol, 5, numCol].Style.Border.Right.Color.SetColor(Color.Black);


            workSheet.Cells["D3"].Value = title;
            workSheet.Cells["D3"].Style.Font.Bold = true;
            workSheet.Cells["D3"].Style.Font.Size = 20;
            workSheet.Cells["D3"].Style.Font.Color.SetColor(Color.Blue);

            workSheet.Cells["C4"].Value = _localizer["Modify_date"].Value + ": " + DateTime.Now.ToString().Substring(0, 10);
            if (unit == "b")
            {
                workSheet.Cells["F4"].Value = _localizer["UNIT"].Value + ": " + _localizer["BILLION"].Value;
            }
            else if (unit == "m")
            {
                workSheet.Cells["F4"].Value = _localizer["UNIT"].Value + ": " + _localizer["MILLION"].Value;
            }
            else if (unit == "t")
            {
                workSheet.Cells["F4"].Value = _localizer["UNIT"].Value + ": " + _localizer["THOUSANDS"].Value;
            }
            else
            {
                workSheet.Cells["F4"].Value = _localizer["UNIT"].Value + ": " + _localizer["DONG"].Value;
            }



            //footer
            workSheet.Cells[numRow + 7, 1].Value = "FPT Securities";
            workSheet.Cells[numRow + 7, 1].Style.Font.Color.SetColor(Color.Blue);
            workSheet.Cells[numRow + 7, 1].Style.Font.Bold = true;
            workSheet.Cells[numRow + 8, 1].Value = _localizer["DiaChiCongTyConLienKet"].Value + ": "+_localizer["Dia_chi"].Value;
            workSheet.Cells[numRow + 9, 1].Value = _localizer["Telephone"].Value + ": 19006446 | Fax: 024 3773 9058";
            workSheet.Cells[numRow + 10, 1].Value = _localizer["Mail"].Value + ": fptsecurities@fpts.com.vn";
            workSheet.Cells[numRow + 7, 1, numRow + 12, 1].Style.Font.Size = 12;
            return workSheet;


        }

        public MemoryStream HSTCExcel(JObject jObj, int numofPeriod = 10, string unit = "d")
        {
            var jArr = jObj.GetValue("table");
            var jArr1 = jObj.GetValue("table1");
            var num = numofPeriod + 1;

            //chuyển dữ liệu từ json sang object
            var tab = jArr.ToObject<List<TableExcelDataModel>>();
            var tab1 = jArr1.ToObject<List<TableExcelDataModel>>();
            var tab2 = jArr1.ToObject<List<HSTCViewModel>>();

            MemoryStream stream = new MemoryStream();
            using (var excelPackage = new ExcelPackage(stream))
            {
                excelPackage.Workbook.Properties.Author = "ThangVH";
                // Tạo title cho file Excel
                excelPackage.Workbook.Properties.Title = _localizer["HSTC"].Value;
                // thêm tí comments vào làm màu 
                excelPackage.Workbook.Properties.Comments = "";
                // Add Sheet vào file Excel
                excelPackage.Workbook.Worksheets.Add("HSTC");
                var workSheet = excelPackage.Workbook.Worksheets[1];
                workSheet = Excel(workSheet, num, tab1.Count + 1, unit, _localizer["HSTC"].Value);

                //data table header title style
                workSheet.Row(6).Height = 18;
                workSheet.Cells[6, 1, 6, num].Style.Border.Top.Color.SetColor(Color.White);
                workSheet.Cells[6, 1, 6, num].Style.Border.Left.Color.SetColor(Color.White);
                workSheet.Cells[6, 1, 6, num].Style.Border.Right.Color.SetColor(Color.White);
                workSheet.Cells[6, 1, 6, num].Style.Border.Bottom.Color.SetColor(Color.Transparent);
                workSheet.Cells[6, 1, 6, num].Style.Font.Bold = true;
                workSheet.Cells[6, 1, 6, num].Style.Font.Size = 12;
                workSheet.Cells[6, 1, 6, num].Style.HorizontalAlignment = ExcelHorizontalAlignment.Justify;

                workSheet.Cells[6, 1, 6, num].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[6, 1, 6, num].Style.Fill.BackgroundColor.SetColor(Color.Blue);
                workSheet.Cells[6, 1, 6, num].Style.Font.Color.SetColor(Color.White);

                //cố định bảng
                workSheet.View.FreezePanes(7, 2);

                //Đổ data vào Excel file
                tab[0].adescription = _localizer["Items"].Value;
                var i = 1;
                foreach (PropertyInfo propertyInfo in tab.FirstOrDefault().GetType().GetProperties())
                {
                    workSheet.Cells[6, i].Value = propertyInfo.GetValue(tab.FirstOrDefault());
                    if (i >= (num))
                    {
                        break;
                    }
                    i++;
                }
                for (int k = 0; k < tab1.Count; k++)
                {
                    if (tab2[k].alev == "1")
                    {
                        workSheet.Cells[k + 7, 1, k + 7, num].Style.Font.Bold = true;
                    }
                    var j = 1;
                    foreach (PropertyInfo propertyInfo in tab1[k].GetType().GetProperties())
                    {
                        if (j == 1)
                        {
                            int level = Int32.Parse(tab2[k].alev.Trim());
                            workSheet.Cells[k + 7, j].Value = new String(' ', (level - 1) * 3) + propertyInfo.GetValue(tab1[k]);

                        }
                        else if (String.IsNullOrEmpty((string)propertyInfo.GetValue(tab1[k])))
                        {
                            workSheet.Cells[k + 7, j].Value = propertyInfo.GetValue(tab1[k]);
                        }
                        else
                        {
                            if (tab2[k].aparentcode == "0501000000" || tab2[k].aparentcode == "0502000000")
                            {
                                if (unit == "b")
                                {
                                    workSheet.Cells[k + 7, j].Value = Double.Parse(propertyInfo.GetValue(tab1[k]).ToString(), System.Globalization.CultureInfo.InvariantCulture) / 1000000000;
                                }
                                else if (unit == "m")
                                {
                                    workSheet.Cells[k + 7, j].Value = Double.Parse(propertyInfo.GetValue(tab1[k]).ToString(), System.Globalization.CultureInfo.InvariantCulture) / 1000000;
                                }
                                else if (unit == "t")
                                {
                                    workSheet.Cells[k + 7, j].Value = Double.Parse(propertyInfo.GetValue(tab1[k]).ToString(), System.Globalization.CultureInfo.InvariantCulture) / 1000;
                                }
                                else
                                {
                                    workSheet.Cells[k + 7, j].Value = Double.Parse(propertyInfo.GetValue(tab1[k]).ToString(), System.Globalization.CultureInfo.InvariantCulture);
                                }

                            }
                            else
                            {
                                workSheet.Cells[k + 7, j].Value = Double.Parse(propertyInfo.GetValue(tab1[k]).ToString(), System.Globalization.CultureInfo.InvariantCulture);
                            }
                        }
                        if (j >= num) break;
                        j++;
                    }
                }

                //format dữ liệu bảng
                if (unit == "b")
                {
                    var numberFormat0 = workSheet.Cells[8, 2, 20, num];
                    numberFormat0.Style.Numberformat.Format = "#,##0.00";
                }
                else
                {
                    var numberFormat0 = workSheet.Cells[8, 2, 20, num];
                    numberFormat0.Style.Numberformat.Format = "#,##0";
                }
                var numberFormat1 = workSheet.Cells[22, 2, 26, num];
                numberFormat1.Style.Numberformat.Format = "#,##0.00";
                var numberFormat2 = workSheet.Cells[28, 2, 35, num];
                numberFormat2.Style.Numberformat.Format = "#,##0.00%";
                var numberFormat3 = workSheet.Cells[37, 2, 54, num];
                numberFormat3.Style.Numberformat.Format = "#,##0.00";
                var numberFormat4 = workSheet.Cells[56, 2, 64, num];
                numberFormat4.Style.Numberformat.Format = "#,##0.00%";

                workSheet.Cells[7, 1, tab1.Count + 6, 1].Style.WrapText = true;

                excelPackage.Save();
                stream = (MemoryStream)excelPackage.Stream;
            }
            return stream;
        }

        public MemoryStream KHTCExcel(JObject jObj, string type = "0", string unit = "d")
        {
            var jArr = jObj.GetValue("table");
            KHTCModelView khtc = new KHTCModelView();

            var num = 18;

            if (jArr != null)
            {
                switch (type)
                {
                    //cty thường & chứng khoán
                    case "0":
                    case "2":
                        var tabSecurity = jArr.ToObject<List<KHTCExcelSecuritiesDataModel>>();
                        khtc.table = tabSecurity.Cast<dynamic>().ToList();

                        num = 18;
                        break;
                    //ngân hàng
                    case "1":
                        var tabBank = jArr.ToObject<List<KHTCExcelBankDataModel>>();
                        khtc.table = tabBank.Cast<dynamic>().ToList();

                        num = 34;
                        break;
                    //bảo hiểm
                    case "3":
                    case "5":
                        var tabInsurance = jArr.ToObject<List<KHTCExcelInsuranceDataModel>>();
                        khtc.table = tabInsurance.Cast<dynamic>().ToList();

                        num = 30;
                        break;
                }
            }
            MemoryStream stream = new MemoryStream();

            using (var excelPackage = new ExcelPackage(stream))
            {
                excelPackage.Workbook.Properties.Author = "ThangVH";
                // Tạo title cho file Excel
                excelPackage.Workbook.Properties.Title = _localizer["KHTC"].Value;
                // thêm tí comments vào làm màu 
                excelPackage.Workbook.Properties.Comments = "";
                // Add Sheet vào file Excel
                excelPackage.Workbook.Worksheets.Add("KHTC");
                var workSheet = excelPackage.Workbook.Worksheets[1];
                workSheet = Excel(workSheet, num, (khtc.table.Count + 2), unit, _localizer["KHTC"].Value);
                //Đổ data vào Excel file
                workSheet.Column(1).Width = 22;
                workSheet.Row(6).Height = 18;
                
                
                workSheet.Cells[6, 1, 7, num].Style.Border.Top.Color.SetColor(Color.White);
                workSheet.Cells[6, 1, 7, num].Style.Border.Left.Color.SetColor(Color.White);
                workSheet.Cells[6, 1, 7, num].Style.Border.Right.Color.SetColor(Color.White);
                workSheet.Cells[6, 1, 7, num].Style.Border.Bottom.Color.SetColor(Color.Transparent);
                workSheet.Cells[6, 1, 7, num].Style.Font.Bold = true;
                workSheet.Cells[6, 1, 7, num].Style.Font.Size = 12;
                workSheet.Cells[6, 1, 7, num].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[6, 1, 7, num].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                workSheet.Cells[6, 1, 7, num].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[6, 1, 7, num].Style.Fill.BackgroundColor.SetColor(Color.Blue);
                workSheet.Cells[6, 1, 7, num].Style.Font.Color.SetColor(Color.White);
                //cố định bảng
                workSheet.View.FreezePanes(8, 3);
                if (type == "0" || type == "2")
                {
                    workSheet.Cells[6, 1, 7, 1].Merge = true;
                    workSheet.Cells[6, 1, 7, 1].Value = _localizer["DCDATE"].Value;

                    workSheet.Cells[6, 2, 7, 2].Merge = true;
                    workSheet.Cells[6, 2, 7, 2].Value = _localizer["YEAR_KHTC"].Value;

                    workSheet.Cells[6, 3, 6, 6].Merge = true;
                    workSheet.Cells[6, 3, 6, 6].Value = _localizer["REVENUE"].Value;
                    workSheet.Cells[7, 3, 7, 3].Value = _localizer["KH"].Value;
                    workSheet.Cells[7, 4, 7, 4].Value = _localizer["KHDC"].Value;
                    workSheet.Cells[7, 5, 7, 5].Value = _localizer["TTKHTC"].Value;
                    workSheet.Cells[7, 6, 7, 6].Value = _localizer["TT_KH"].Value;

                    workSheet.Cells[6, 7, 6, 10].Merge = true;
                    workSheet.Cells[6, 7, 6, 10].Value = _localizer["EARNING_BEFORE_TAX"].Value;
                    workSheet.Cells[7, 7, 7, 7].Value = _localizer["KH"].Value;
                    workSheet.Cells[7, 8, 7, 8].Value = _localizer["KHDC"].Value;
                    workSheet.Cells[7, 9, 7, 9].Value = _localizer["TTKHTC"].Value;
                    workSheet.Cells[7, 10, 7, 10].Value = _localizer["TT_KH"].Value;

                    workSheet.Cells[6, 11, 6, 14].Merge = true;
                    workSheet.Cells[6, 11, 6, 14].Value = _localizer["EARNING_AFTER_TAX"].Value;
                    workSheet.Cells[7, 11, 7, 11].Value = _localizer["KH"].Value;
                    workSheet.Cells[7, 12, 7, 12].Value = _localizer["KHDC"].Value;
                    workSheet.Cells[7, 13, 7, 13].Value = _localizer["TTKHTC"].Value;
                    workSheet.Cells[7, 14, 7, 14].Value = _localizer["TT_KH"].Value;

                    workSheet.Cells[6, 15, 6, 16].Merge = true;
                    workSheet.Cells[6, 15, 6, 16].Value = _localizer["CASH_DIVIDENT"].Value;
                    workSheet.Cells[7, 15, 7, 15].Value = _localizer["KH"].Value;
                    workSheet.Cells[7, 16, 7, 16].Value = _localizer["KHDC"].Value;

                    workSheet.Cells[6, 17, 6, 18].Merge = true;
                    workSheet.Cells[6, 17, 6, 18].Value = _localizer["STOCK_DIVIDENT"].Value;
                    workSheet.Cells[7, 17, 7, 17].Value = _localizer["KH"].Value;
                    workSheet.Cells[7, 18, 7, 18].Value = _localizer["KHDC"].Value;

                    workSheet.Cells[6, 1, 7, 18].Style.WrapText = true;

                    for (int k = 0; k < khtc.table.Count; k++)
                    {
                        //if (khtc1.table[k].alev == "1")
                        //{
                        //    workSheet.Cells[k + 7, 1, k + 7, num].Style.Font.Bold = true;
                        //}
                        var j = 1;
                        foreach (PropertyInfo propertyInfo in khtc.table[k].GetType().GetProperties())
                        {
                            if (j == 1 || j == 2 || propertyInfo.GetValue(khtc.table[k]) == null || propertyInfo.Name.Contains("peR") || propertyInfo.Name.Contains("acotucbang"))
                                workSheet.Cells[k + 8, j].Value = propertyInfo.GetValue(khtc.table[k]);
                            else
                            {
                                if (unit == "b")
                                {
                                    workSheet.Cells[k + 8, j].Value = propertyInfo.GetValue(khtc.table[k]) / 1000000000;
                                }
                                else if (unit == "m")
                                {
                                    workSheet.Cells[k + 8, j].Value = propertyInfo.GetValue(khtc.table[k]) / 1000000;
                                }
                                else if (unit == "t")
                                {
                                    workSheet.Cells[k + 8, j].Value = propertyInfo.GetValue(khtc.table[k]) / 1000;
                                }
                                else
                                {
                                    workSheet.Cells[k + 8, j].Value = propertyInfo.GetValue(khtc.table[k]);
                                }


                            }
                            //if (j >= num) break;
                            j++;
                        }
                    }

                    var numberFormat1 = workSheet.Cells[8, 3, khtc.table.Count + 8, num];
                    numberFormat1.Style.Numberformat.Format = "#,##0";
                }
                else if (type == "1")
                {
                    workSheet.Cells[6, 1, 7, 1].Merge = true;
                    workSheet.Cells[6, 1, 7, 1].Value = _localizer["DCDATE"].Value;

                    workSheet.Cells[6, 2, 7, 2].Merge = true;
                    workSheet.Cells[6, 2, 7, 2].Value = _localizer["YEAR_KHTC"].Value;

                    workSheet.Cells[6, 3, 6, 6].Merge = true;
                    workSheet.Cells[6, 3, 6, 6].Value = _localizer["CHARTERED_CAPITAL"].Value;
                    workSheet.Cells[7, 3, 7, 3].Value = _localizer["KH"].Value;
                    workSheet.Cells[7, 4, 7, 4].Value = _localizer["KHDC"].Value;
                    workSheet.Cells[7, 5, 7, 5].Value = _localizer["TTKHTC"].Value;
                    workSheet.Cells[7, 6, 7, 6].Value = _localizer["TT_KH"].Value;

                    workSheet.Cells[6, 7, 6, 10].Merge = true;
                    workSheet.Cells[6, 7, 6, 10].Value = _localizer["TOTAL_ASSET"].Value;
                    workSheet.Cells[7, 7, 7, 7].Value = _localizer["KH"].Value;
                    workSheet.Cells[7, 8, 7, 8].Value = _localizer["KHDC"].Value;
                    workSheet.Cells[7, 9, 7, 9].Value = _localizer["TTKHTC"].Value;
                    workSheet.Cells[7, 10, 7, 10].Value = _localizer["TT_KH"].Value;

                    workSheet.Cells[6, 11, 6, 14].Merge = true;
                    workSheet.Cells[6, 11, 6, 14].Value = _localizer["MOBILIZING_ECONOMIC_CAPITAL"].Value;
                    workSheet.Cells[7, 11, 7, 11].Value = _localizer["KH"].Value;
                    workSheet.Cells[7, 12, 7, 12].Value = _localizer["KHDC"].Value;
                    workSheet.Cells[7, 13, 7, 13].Value = _localizer["TTKHTC"].Value;
                    workSheet.Cells[7, 14, 7, 14].Value = _localizer["TT_KH"].Value;

                    workSheet.Cells[6, 15, 6, 18].Merge = true;
                    workSheet.Cells[6, 15, 6, 18].Value = _localizer["OUTSTANDING_LOANS"].Value;
                    workSheet.Cells[7, 15, 7, 15].Value = _localizer["KH"].Value;
                    workSheet.Cells[7, 16, 7, 16].Value = _localizer["KHDC"].Value;
                    workSheet.Cells[7, 17, 7, 17].Value = _localizer["TTKHTC"].Value;
                    workSheet.Cells[7, 18, 7, 18].Value = _localizer["TT_KH"].Value;

                    workSheet.Cells[6, 19, 6, 22].Merge = true;
                    workSheet.Cells[6, 19, 6, 22].Value = _localizer["PROFIT_BEFORE_INCOME_TAX"].Value;
                    workSheet.Cells[7, 19, 7, 19].Value = _localizer["KH"].Value;
                    workSheet.Cells[7, 20, 7, 20].Value = _localizer["KHDC"].Value;
                    workSheet.Cells[7, 21, 7, 21].Value = _localizer["TTKHTC"].Value;
                    workSheet.Cells[7, 22, 7, 22].Value = _localizer["TT_KH"].Value;

                    workSheet.Cells[6, 23, 6, 26].Merge = true;
                    workSheet.Cells[6, 23, 6, 26].Value = _localizer["CAPITAL_ADEQUACY_RATIO"].Value;
                    workSheet.Cells[7, 23, 7, 23].Value = _localizer["KH"].Value;
                    workSheet.Cells[7, 24, 7, 24].Value = _localizer["KHDC"].Value;
                    workSheet.Cells[7, 25, 7, 25].Value = _localizer["TTKHTC"].Value;
                    workSheet.Cells[7, 26, 7, 26].Value = _localizer["TT_KH"].Value;

                    workSheet.Cells[6, 27, 6, 30].Merge = true;
                    workSheet.Cells[6, 27, 6, 30].Value = _localizer["BAD_DEBT_RATE"].Value;
                    workSheet.Cells[7, 27, 7, 27].Value = _localizer["KH"].Value;
                    workSheet.Cells[7, 28, 7, 28].Value = _localizer["KHDC"].Value;
                    workSheet.Cells[7, 29, 7, 29].Value = _localizer["TTKHTC"].Value;
                    workSheet.Cells[7, 30, 7, 30].Value = _localizer["TT_KH"].Value;

                    workSheet.Cells[6, 31, 6, 32].Merge = true;
                    workSheet.Cells[6, 31, 6, 32].Value = _localizer["CASH_DIVIDENT"].Value;
                    workSheet.Cells[7, 31, 7, 31].Value = _localizer["KH"].Value;
                    workSheet.Cells[7, 32, 7, 32].Value = _localizer["KHDC"].Value;

                    workSheet.Cells[6, 33, 6, 34].Merge = true;
                    workSheet.Cells[6, 33, 6, 34].Value = _localizer["STOCK_DIVIDENT"].Value;
                    workSheet.Cells[7, 33, 7, 33].Value = _localizer["KH"].Value;
                    workSheet.Cells[7, 34, 7, 34].Value = _localizer["KHDC"].Value;

                    workSheet.Cells[6, 1, 7, 34].Style.WrapText = true;

                    for (int k = 0; k < khtc.table.Count; k++)
                    {
                        //if (khtc1.table[k].alev == "1")
                        //{
                        //    workSheet.Cells[k + 7, 1, k + 7, num].Style.Font.Bold = true;
                        //}
                        var j = 1;
                        foreach (PropertyInfo propertyInfo in khtc.table[k].GetType().GetProperties())
                        {
                            if (j == 1 || j == 2 || propertyInfo.GetValue(khtc.table[k]) == null || propertyInfo.Name.Contains("peR") || propertyInfo.Name.Contains("acotucbang"))
                                workSheet.Cells[k + 8, j].Value = propertyInfo.GetValue(khtc.table[k]);
                            else
                            {
                                if (unit == "b")
                                {
                                    workSheet.Cells[k + 8, j].Value = propertyInfo.GetValue(khtc.table[k]) / 1000000000;
                                }
                                else if (unit == "m")
                                {
                                    workSheet.Cells[k + 8, j].Value = propertyInfo.GetValue(khtc.table[k]) / 1000000;
                                }
                                else if (unit == "t")
                                {
                                    workSheet.Cells[k + 8, j].Value = propertyInfo.GetValue(khtc.table[k]) / 1000;
                                }
                                else
                                {
                                    workSheet.Cells[k + 8, j].Value = propertyInfo.GetValue(khtc.table[k]);
                                }


                            }
                            //if (j >= num) break;
                            j++;
                        }
                    }

                    var numberFormat1 = workSheet.Cells[8, 3, khtc.table.Count + 8, num];
                    numberFormat1.Style.Numberformat.Format = "#,##0";
                }
                else if (type == "3" || type == "5")
                {
                    workSheet.Cells[6, 1, 7, 1].Merge = true;
                    workSheet.Cells[6, 1, 7, 1].Value = _localizer["DCDATE"].Value;

                    workSheet.Cells[6, 2, 7, 2].Merge = true;
                    workSheet.Cells[6, 2, 7, 2].Value = _localizer["YEAR_KHTC"].Value;

                    workSheet.Cells[6, 3, 6, 6].Merge = true;
                    workSheet.Cells[6, 3, 6, 6].Value = _localizer["REVENUE"].Value;
                    workSheet.Cells[7, 3, 7, 3].Value = _localizer["KH"].Value;
                    workSheet.Cells[7, 4, 7, 4].Value = _localizer["KHDC"].Value;
                    workSheet.Cells[7, 5, 7, 5].Value = _localizer["TTKHTC"].Value;
                    workSheet.Cells[7, 6, 7, 6].Value = _localizer["TT_KH"].Value;

                    workSheet.Cells[6, 7, 6, 10].Merge = true;
                    workSheet.Cells[6, 7, 6, 10].Value = _localizer["PREMIUM_FROM_DIRECT_INSURANCE"].Value;
                    workSheet.Cells[7, 7, 7, 7].Value = _localizer["KH"].Value;
                    workSheet.Cells[7, 8, 7, 8].Value = _localizer["KHDC"].Value;
                    workSheet.Cells[7, 9, 7, 9].Value = _localizer["TTKHTC"].Value;
                    workSheet.Cells[7, 10, 7, 10].Value = _localizer["TT_KH"].Value;

                    workSheet.Cells[6, 11, 6, 14].Merge = true;
                    workSheet.Cells[6, 11, 6, 14].Value = _localizer["ASSUMED_PREMIUM"].Value;
                    workSheet.Cells[7, 11, 7, 11].Value = _localizer["KH"].Value;
                    workSheet.Cells[7, 12, 7, 12].Value = _localizer["KHDC"].Value;
                    workSheet.Cells[7, 13, 7, 13].Value = _localizer["TTKHTC"].Value;
                    workSheet.Cells[7, 14, 7, 14].Value = _localizer["TT_KH"].Value;

                    workSheet.Cells[6, 15, 6, 18].Merge = true;
                    workSheet.Cells[6, 15, 6, 18].Value = _localizer["FINANCIAL_INCOME"].Value;
                    workSheet.Cells[7, 15, 7, 15].Value = _localizer["KH"].Value;
                    workSheet.Cells[7, 16, 7, 16].Value = _localizer["KHDC"].Value;
                    workSheet.Cells[7, 17, 7, 17].Value = _localizer["TTKHTC"].Value;
                    workSheet.Cells[7, 18, 7, 18].Value = _localizer["TT_KH"].Value;

                    workSheet.Cells[6, 19, 6, 22].Merge = true;
                    workSheet.Cells[6, 19, 6, 22].Value = _localizer["EARNING_BEFORE_TAX"].Value;
                    workSheet.Cells[7, 19, 7, 19].Value = _localizer["KH"].Value;
                    workSheet.Cells[7, 20, 7, 20].Value = _localizer["KHDC"].Value;
                    workSheet.Cells[7, 21, 7, 21].Value = _localizer["TTKHTC"].Value;
                    workSheet.Cells[7, 22, 7, 22].Value = _localizer["TT_KH"].Value;

                    workSheet.Cells[6, 23, 6, 26].Merge = true;
                    workSheet.Cells[6, 23, 6, 26].Value = _localizer["EARNING_AFTER_TAX"].Value;
                    workSheet.Cells[7, 23, 7, 23].Value = _localizer["KH"].Value;
                    workSheet.Cells[7, 24, 7, 24].Value = _localizer["KHDC"].Value;
                    workSheet.Cells[7, 25, 7, 25].Value = _localizer["TTKHTC"].Value;
                    workSheet.Cells[7, 26, 7, 26].Value = _localizer["TT_KH"].Value;

                    workSheet.Cells[6, 27, 6, 28].Merge = true;
                    workSheet.Cells[6, 27, 6, 28].Value = _localizer["CASH_DIVIDENT"].Value;
                    workSheet.Cells[7, 27, 7, 27].Value = _localizer["KH"].Value;
                    workSheet.Cells[7, 28, 7, 28].Value = _localizer["KHDC"].Value;

                    workSheet.Cells[6, 29, 6, 30].Merge = true;
                    workSheet.Cells[6, 29, 6, 30].Value = _localizer["STOCK_DIVIDENT"].Value;
                    workSheet.Cells[7, 29, 7, 29].Value = _localizer["KH"].Value;
                    workSheet.Cells[7, 30, 7, 30].Value = _localizer["KHDC"].Value;

                    workSheet.Cells[6, 1, 7, 30].Style.WrapText = true;

                    for (int k = 0; k < khtc.table.Count; k++)
                    {
                        //if (khtc1.table[k].alev == "1")
                        //{
                        //    workSheet.Cells[k + 7, 1, k + 7, num].Style.Font.Bold = true;
                        //}
                        var j = 1;
                        foreach (PropertyInfo propertyInfo in khtc.table[k].GetType().GetProperties())
                        {
                            if (j == 1 || j == 2 || propertyInfo.GetValue(khtc.table[k]) == null || propertyInfo.Name.Contains("peR") || propertyInfo.Name.Contains("acotucbang"))
                                workSheet.Cells[k + 8, j].Value = propertyInfo.GetValue(khtc.table[k]);
                            else
                            {
                                if (unit == "b")
                                {
                                    workSheet.Cells[k + 8, j].Value = propertyInfo.GetValue(khtc.table[k]) / 1000000000;
                                }
                                else if (unit == "m")
                                {
                                    workSheet.Cells[k + 8, j].Value = propertyInfo.GetValue(khtc.table[k]) / 1000000;
                                }
                                else if (unit == "t")
                                {
                                    workSheet.Cells[k + 8, j].Value = propertyInfo.GetValue(khtc.table[k]) / 1000;
                                }
                                else
                                {
                                    workSheet.Cells[k + 8, j].Value = propertyInfo.GetValue(khtc.table[k]);
                                }


                            }
                            //if (j >= num) break;
                            j++;
                        }
                    }

                    var numberFormat1 = workSheet.Cells[8, 3, khtc.table.Count + 8, num];
                    numberFormat1.Style.Numberformat.Format = "#,##0";
                }

                excelPackage.Save();
                stream = (MemoryStream)excelPackage.Stream;
            }
            return stream;
        }

        public MemoryStream CDKTExcel(JObject jObj, int numofPeriod = 10, string unit = "d")
        {
            var jArr = jObj.GetValue("table");
            var jArr1 = jObj.GetValue("table1");
            var num = numofPeriod + 1;

            //chuyển dữ liệu từ json sang object
            var tab = jArr.ToObject<List<TableExcelDataModel>>();
            var tab1 = jArr1.ToObject<List<TableExcelDataModel>>();
            var tab2 = jArr1.ToObject<List<CDKTViewModel>>();

            MemoryStream stream = new MemoryStream();
            using (var excelPackage = new ExcelPackage(stream))
            {
                excelPackage.Workbook.Properties.Author = "ThangVH";
                // Tạo title cho file Excel
                excelPackage.Workbook.Properties.Title = _localizer["CDKT"].Value;
                // thêm tí comments vào làm màu 
                excelPackage.Workbook.Properties.Comments = "";
                // Add Sheet vào file Excel
                excelPackage.Workbook.Worksheets.Add("CĐKT");
                var workSheet = excelPackage.Workbook.Worksheets[1];
                workSheet = Excel(workSheet, num, tab1.Count + 1, unit, _localizer["CDKT"].Value);

                //data table header title style
                workSheet.Row(6).Height = 18;
                workSheet.Cells[6, 1, 6, num].Style.Border.Top.Color.SetColor(Color.White);
                workSheet.Cells[6, 1, 6, num].Style.Border.Left.Color.SetColor(Color.White);
                workSheet.Cells[6, 1, 6, num].Style.Border.Right.Color.SetColor(Color.White);
                workSheet.Cells[6, 1, 6, num].Style.Border.Bottom.Color.SetColor(Color.Transparent);
                workSheet.Cells[6, 1, 6, num].Style.Font.Bold = true;
                workSheet.Cells[6, 1, 6, num].Style.Font.Size = 12;
                workSheet.Cells[6, 1, 6, num].Style.HorizontalAlignment = ExcelHorizontalAlignment.Justify;

                workSheet.Cells[6, 1, 6, num].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[6, 1, 6, num].Style.Fill.BackgroundColor.SetColor(Color.Blue);
                workSheet.Cells[6, 1, 6, num].Style.Font.Color.SetColor(Color.White);

                //cố định bảng
                workSheet.View.FreezePanes(7, 2);


                //Đổ data vào Excel file
                tab[0].adescription = _localizer["Items"].Value;
                var i = 1;
                foreach (PropertyInfo propertyInfo in tab.FirstOrDefault().GetType().GetProperties())
                {
                    workSheet.Cells[6, i].Value = propertyInfo.GetValue(tab.FirstOrDefault());
                    if (i >= (num))
                    {
                        break;
                    }
                    i++;
                }
                for (int k = 0; k < tab1.Count; k++)
                {
                    if (tab2[k].alev == "1")
                    {
                        workSheet.Cells[k + 7, 1, k + 7, num].Style.Font.Bold = true;
                    }
                    else if (tab2[k].alev == "2")
                    {
                        workSheet.Cells[k + 7, 1, k + 7, 1].Style.Font.Bold = true;
                    }
                    var j = 1;
                    foreach (PropertyInfo propertyInfo in tab1[k].GetType().GetProperties())
                    {
                        if (j == 1)
                        {
                            int level = Int32.Parse(tab2[k].alev.Trim());
                            workSheet.Cells[k + 7, j].Value = new String(' ', (level - 1) * 3) + propertyInfo.GetValue(tab1[k]);

                        }
                        else if (String.IsNullOrEmpty((string)propertyInfo.GetValue(tab1[k])))
                        {
                            workSheet.Cells[k + 7, j].Value = propertyInfo.GetValue(tab1[k]);
                        }
                        else
                        {

                            if (unit == "b")
                            {
                                workSheet.Cells[k + 7, j].Value = Double.Parse(propertyInfo.GetValue(tab1[k]).ToString(), System.Globalization.CultureInfo.InvariantCulture) / 1000000000;
                            }
                            else if (unit == "m")
                            {
                                workSheet.Cells[k + 7, j].Value = Double.Parse(propertyInfo.GetValue(tab1[k]).ToString(), System.Globalization.CultureInfo.InvariantCulture) / 1000000;
                            }
                            else if (unit == "t")
                            {
                                workSheet.Cells[k + 7, j].Value = Double.Parse(propertyInfo.GetValue(tab1[k]).ToString(), System.Globalization.CultureInfo.InvariantCulture) / 1000;
                            }
                            else
                            {
                                workSheet.Cells[k + 7, j].Value = Double.Parse(propertyInfo.GetValue(tab1[k]).ToString(), System.Globalization.CultureInfo.InvariantCulture);
                            }

                        }
                        if (j >= num) break;
                        j++;
                    }
                }

                //format dữ liệu bảng

                var numberFormat1 = workSheet.Cells[7, 2, tab1.Count + 6, num];
                numberFormat1.Style.Numberformat.Format = "#,##0";

                workSheet.Cells[7, 1, tab1.Count + 6, 1].Style.WrapText = true;
                excelPackage.Save();
                stream = (MemoryStream)excelPackage.Stream;
            }
            return stream;
        }

        public MemoryStream CommonExcel(JObject jObj, int numofPeriod = 5, string unit = "d", string nameSheet = "Sheet", string title = "Bảng thông tin")
        {
            var jArr = jObj.GetValue("table");
            var jArr1 = jObj.GetValue("table1");
            var num = numofPeriod + 1;

            //chuyển dữ liệu từ json sang object
            var tab = jArr.ToObject<List<TableExcelDataModel>>();
            var tab1 = jArr1.ToObject<List<TableExcelDataModel>>();
            var tab2 = jArr1.ToObject<List<KQKDViewModel>>();

            MemoryStream stream = new MemoryStream();
            using (var excelPackage = new ExcelPackage(stream))
            {
                excelPackage.Workbook.Properties.Author = "ThangVH";
                // Tạo title cho file Excel
                excelPackage.Workbook.Properties.Title = title;
                // Add Sheet vào file Excel
                excelPackage.Workbook.Worksheets.Add(nameSheet);
                var workSheet = excelPackage.Workbook.Worksheets[1];
                workSheet = Excel(workSheet, num, tab1.Count + 1, unit, title);

                //data table header title style
                workSheet.Row(6).Height = 18;
                workSheet.Cells[6, 1, 6, num].Style.Border.Top.Color.SetColor(Color.White);
                workSheet.Cells[6, 1, 6, num].Style.Border.Left.Color.SetColor(Color.White);
                workSheet.Cells[6, 1, 6, num].Style.Border.Right.Color.SetColor(Color.White);
                workSheet.Cells[6, 1, 6, num].Style.Border.Bottom.Color.SetColor(Color.Transparent);
                workSheet.Cells[6, 1, 6, num].Style.Font.Bold = true;
                workSheet.Cells[6, 1, 6, num].Style.Font.Size = 12;
                workSheet.Cells[6, 1, 6, num].Style.HorizontalAlignment = ExcelHorizontalAlignment.Justify;

                workSheet.Cells[6, 1, 6, num].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[6, 1, 6, num].Style.Fill.BackgroundColor.SetColor(Color.Blue);
                workSheet.Cells[6, 1, 6, num].Style.Font.Color.SetColor(Color.White);

                //cố định bảng
                workSheet.View.FreezePanes(7, 2);

                //Đổ data vào Excel file
                tab[0].adescription = _localizer["Items"].Value;
                var i = 1;
                foreach (PropertyInfo propertyInfo in tab.FirstOrDefault().GetType().GetProperties())
                {
                    workSheet.Cells[6, i].Value = propertyInfo.GetValue(tab.FirstOrDefault());
                    if (i >= (num))
                    {
                        break;
                    }
                    i++;
                }
                var Check = tab2.Exists(x => x.alev == "3");
                for (int k = 0; k < tab1.Count; k++)
                {
                    if (tab2[k].alev == "1")
                    {
                        workSheet.Cells[k + 7, 1, k + 7, 1].Style.Font.Bold = true;
                    }
                    else if (tab2[k].alev == "2" && Check)
                    {
                        workSheet.Cells[k + 7, 1, k + 7, 1].Style.Font.Bold = true;
                    }
                    var j = 1;
                    foreach (PropertyInfo propertyInfo in tab1[k].GetType().GetProperties())
                    {
                        if (j == 1)
                        {
                            int level = Int32.Parse(tab2[k].alev.Trim());
                            workSheet.Cells[k + 7, j].Value = new String(' ', (level - 1) * 3) + propertyInfo.GetValue(tab1[k]);

                        }
                        else if (String.IsNullOrEmpty((string)propertyInfo.GetValue(tab1[k])))
                        {
                            workSheet.Cells[k + 7, j].Value = propertyInfo.GetValue(tab1[k]);
                        }
                        else
                        {

                            if (unit == "b")
                            {
                                workSheet.Cells[k + 7, j].Value = Double.Parse(propertyInfo.GetValue(tab1[k]).ToString(), System.Globalization.CultureInfo.InvariantCulture) / 1000000000;
                            }
                            else if (unit == "m")
                            {
                                workSheet.Cells[k + 7, j].Value = Double.Parse(propertyInfo.GetValue(tab1[k]).ToString(), System.Globalization.CultureInfo.InvariantCulture) / 1000000;
                            }
                            else if (unit == "t")
                            {
                                workSheet.Cells[k + 7, j].Value = Double.Parse(propertyInfo.GetValue(tab1[k]).ToString(), System.Globalization.CultureInfo.InvariantCulture) / 1000;
                            }
                            else
                            {
                                workSheet.Cells[k + 7, j].Value = Double.Parse(propertyInfo.GetValue(tab1[k]).ToString(), System.Globalization.CultureInfo.InvariantCulture);
                            }

                        }
                        if (j >= num) break;
                        j++;
                    }
                }

                //format dữ liệu bảng

                var numberFormat = workSheet.Cells[7, 2, tab1.Count + 6, num];
                numberFormat.Style.Numberformat.Format = "#,##0";
                workSheet.Cells[7, 1, tab1.Count + 6, 1].Style.WrapText = true;

                excelPackage.Save();
                stream = (MemoryStream)excelPackage.Stream;
            }
            return stream;
        }
    }
}
