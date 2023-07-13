using System.Globalization;
using System.Text.RegularExpressions;
using Terrasoft.Configuration.HaleonReport.Helper;
using Terrasoft.Configuration.HaleonReport.Model;

namespace Samples;

public class SupplyPlanController
{
    public static string ValidateVersion(List<SupplyPlanItem> tableData)
    {
        var version = tableData.First().VersionDate;
        var isFirDateAll = tableData.All(item =>
        {
            return item.VersionDate.Day == 1;
        });
        if(!isFirDateAll) {
            return "Значение версии должно быть на первый день в месяце";
        }
        
        bool isVersionSame = tableData.All(item =>
        {
            Console.WriteLine(item.VersionDate.Day + " " + item.VersionDate.Month + " " + item.VersionDate.Year);
            return item.VersionDate.Equals(version);
        });
        if (!isVersionSame) {
            return "Найдено больше одного значения версии";
        }
        
        return string.Empty;
    }
    public static List<SupplyPlanItem> DictToSupplyItems(List<Dictionary<string, string>> rows)
    {
        var list = new List<SupplyPlanItem>(HaleonReportExcelHelper._initCapacity);
        rows.ForEach(row =>
        {
            try
            {
                var versionString = row["Version"];
                var gmmCode = row["Central Product Code"];
                var receiptQuantityString = row["Receipt Quantity in Customer Local Units"];
                var shipmentDateString = row["Shipment date"];
                var arrivalDateString = row["Arrival date"];

                var version = !string.IsNullOrEmpty(versionString) ? ParseStringToDate(versionString) : default;
                var receiptQuantity = !string.IsNullOrEmpty(receiptQuantityString) ? decimal.Parse(receiptQuantityString) : default;
                var shipmentDate = !string.IsNullOrEmpty(shipmentDateString) ? ParseStringToDate(shipmentDateString) : default;
                var arrivalDate = !string.IsNullOrEmpty(arrivalDateString) ? ParseStringToDate(arrivalDateString) : default;

                var item = new SupplyPlanItem
                {
                    VersionDate = version,
                    GmmCode = gmmCode,
                    ReceiptQuantity = receiptQuantity,
                    DateShipmentFactory = shipmentDate,
                    DateShipmentCustomPost = arrivalDate
                };
                list.Add(item);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        });
        return list;
    }
    
    public static DateTime ParseStringToDate(string date) {
        string[] formats = { "dd?MM?yyyy HH:mm:ss", "MM?dd?yyyy HH:mm:ss", "dd?MM?yyyy", "MM?dd?yyyy" };
        char[] separators = { '/', '-', '.', '@', '#' };
        // Create and populate an array of all acceptable formats
        var acceptableFormats = new List<string>();
        foreach (var format in formats)
        {
            foreach (var separator in separators)
            {
                acceptableFormats.Add(format.Replace('?', separator));
            }
        }

        if (DateTime.TryParseExact(date, acceptableFormats.ToArray(), CultureInfo.InvariantCulture, DateTimeStyles.None, out var resultDate))
        {
            return resultDate;
        }

        return resultDate;
    }
}