namespace Permit_App.Services
{
    using System.Collections.Generic;
    using System.IO;
    using OfficeOpenXml;

    public class GetMasterDataService
    {
        public List<string> GetMasterData(string filePath)
        {
            var field_names = new List<string>();

            FileInfo fileInfo = new FileInfo(filePath);
            if (!fileInfo.Exists)
                return field_names;

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Required for EPPlus
            using (var package = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Assuming data is in the first sheet
                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++) // Assuming first row is header
                {
                    string field_data = worksheet.Cells[row, 2].Text; // Assuming county names are in column B
                    if (!string.IsNullOrEmpty(field_data))
                    {
                        field_names.Add(field_data);
                    }
                }
            }
            return field_names;
        }
    }
}
