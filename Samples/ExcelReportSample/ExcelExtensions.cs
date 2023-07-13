using System.Reflection;
using OfficeOpenXml;
using OfficeOpenXml.Table;

namespace Terrasoft.Configuration.SummaryTableVisa;

public static class ExcelExtensions
{
    public static DateTime ConvertToDateTime(double excelDate)
    {
        if (excelDate < 1)
            throw new ArgumentException("Excel dates cannot be smaller than 0.");

        var dateOfReference = new DateTime(1900, 1, 1);

        if (excelDate > 60d)
            excelDate -= 2;
        else
            excelDate -= 1;
        return dateOfReference.AddDays(excelDate);
    }
    public static void SetDateTimeValue(this PropertyInfo prop, object? obj, double value)
    {
        prop.SetValue(obj, ConvertToDateTime(value));
    }

    public static List<IGrouping<int, ExcelRangeBase>> GetGroupCellList(this ExcelTable table)
    {
        //Get the cells based on the table address
        var start = table.Address.Start;
        var end = table.Address.End;
        var cells = new List<ExcelRangeBase>();

        //Have to use for loops insteadof worksheet.Cells to protect against empties
        for (var row = start.Row; row <= end.Row; row++)
        {
            for (var column = start.Column; column <= end.Column; column++)
            {
                cells.Add(table.WorkSheet.Cells[row, column]);
            }
        }
        return cells
                .GroupBy(cell => cell.Start.Row)
                .ToList();
    }
    
    public static List<ExcelColumn> GetExcelColumns(this List<IGrouping<int, ExcelRangeBase>> groups)
    {
        var columnNames = groups
            .First()
            .Select((hcell, idx) => new  ExcelColumn(hcell.Value.ToString(), idx))
            .ToList();
        return columnNames;
    }
    
    public static IEnumerable<List<object>> GetExcelValues(this List<IGrouping<int, ExcelRangeBase>> groups)
    {
        var rowValues = groups
            .Skip(1) //Exclude header
            .Select(cg => cg.Select(c => c.Value).ToList());
        return rowValues;
    }
    
    public static List<Type> GetExcelTypes(this List<IGrouping<int, ExcelRangeBase>> groups)
    {
        var types = groups
            .Skip(1)
            .First()
            .Select(rcell => rcell.Value.GetType())
            .ToList();
        return types;
    }

    public static void GetExcelData(IEnumerable<List<object>> rowValues, List<ExcelColumn> colnames, List<Type> types)
    {
        foreach (var row in rowValues) 
        {
            colnames.ForEach(columnName =>
            {
                //This is the real wrinkle to using reflection - Excel stores all numbers as double including int
                var val = row[columnName.Index];
                var type = types[columnName.Index];
                Console.WriteLine($"{columnName.Name} | {val} | {type}");
            });
        }
    }
      public static IEnumerable<T> ConvertTableToObjects<T>(this ExcelTable table) where T : new()
    {
        //DateTime Conversion
        var convertDateTime = new Func<double, DateTime>(excelDate =>
        {
            if (excelDate < 1)
                throw new ArgumentException("Excel dates cannot be smaller than 0.");

            var dateOfReference = new DateTime(1900, 1, 1);

            if (excelDate > 60d)
                excelDate = excelDate - 2;
            else
                excelDate = excelDate - 1;
            return dateOfReference.AddDays(excelDate);
        });

        //Get the properties of T
        var tprops = (new T())
            .GetType()
            .GetProperties()
            .ToList();

        //Get the cells based on the table address
        var start = table.Address.Start;
        var end = table.Address.End;
        var cells = new List<ExcelRangeBase>();

        //Have to use for loops insteadof worksheet.Cells to protect against empties
        for (var r = start.Row; r <= end.Row; r++)
        for (var c = start.Column; c <= end.Column; c++)
            cells.Add(table.WorkSheet.Cells[r, c]);

        var groups = cells
            .GroupBy(cell => cell.Start.Row)
            .ToList();

        //Assume the second row represents column data types (big assumption!)
        var types = groups
            .Skip(1)
            .First()
            .Select(rcell => rcell.Value.GetType())
            .ToList();

        //Assume first row has the column names
        var colnames = groups
            .First()
            .Select((hcell, idx) => new { Name = hcell.Value.ToString(), index = idx })
            .Where(o => tprops.Select(p => p.Name).Contains(o.Name))
            .ToList();

        //Everything after the header is data
        var rowvalues = groups
            .Skip(1) //Exclude header
            .Select(cg => cg.Select(c => c.Value).ToList());

        //Create the collection container
        var collection = rowvalues
            .Select(row =>
            {
                var tnew = new T();
                colnames.ForEach(colname =>
                {
                    //This is the real wrinkle to using reflection - Excel stores all numbers as double including int
                    var val = row[colname.index];
                    var type = types[colname.index];
                    var prop = tprops.First(p => p.Name == colname.Name);

                    //If it is numeric it is a double since that is how excel stores all numbers
                    if (type == typeof(double))
                    {
                        if (!string.IsNullOrWhiteSpace(val?.ToString()))
                        {
                            //Unbox it
                            var unboxedVal = (double)val;

                            //FAR FROM A COMPLETE LIST!!!
                            if (prop.PropertyType == typeof(Int32))
                                prop.SetValue(tnew, (int)unboxedVal);
                            else if (prop.PropertyType == typeof(double))
                                prop.SetValue(tnew, unboxedVal);
                            else if (prop.PropertyType == typeof(DateTime))
                                prop.SetValue(tnew, convertDateTime(unboxedVal));
                            else
                                throw new NotImplementedException(
                                    $"Type '{prop.PropertyType.Name}' not implemented yet!");
                        }
                    }
                    else
                    {
                        //Its a string
                        prop.SetValue(tnew, val);
                    }
                });

                return tnew;
            });


        //Send it back
        return collection;
    }
}