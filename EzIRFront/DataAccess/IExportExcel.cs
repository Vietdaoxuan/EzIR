using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRFront.DataAccess
{
    public interface IExportExcel
    {
        MemoryStream HSTCExcel(JObject jobject, int numofPeriod , string unit);

        MemoryStream KHTCExcel(JObject jObj, string type = "0", string unit = "d");

        MemoryStream CDKTExcel(JObject jObj, int numofPeriod = 10, string unit = "d");

        MemoryStream CommonExcel(JObject jObj, int numofPeriod = 5, string unit = "d", string nameSheet = "Sheet", string title = "Bảng thông tin");

    }
}
