using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using System.IO;

namespace RestaurantBookingApp.Function
{
    public class BlobTriggerFunction
    {
        [FunctionName("BlobTriggerFunction")]
        // ContainerName and AzureStorageConnectionString is available in local.settings.json
        // In Prod, it should be configured in respective Function App
        public void Run([BlobTrigger("%ContainerName%", Connection = "AzureStorageConnectionString")] Stream myBlob, string name, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");

            //Get the file extension
            string extension = Path.GetExtension(name);

            //Check if the file extension is an Excel file
            if (extension == ".xls" || extension == ".xlsx")
            {

                //Process excel file
                using (var package = new ExcelPackage(myBlob)) // package -> EPPlus
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    var worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;
                    var colCount = worksheet.Dimension.Columns;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var phoneNumber = worksheet.Cells[row, 1].Value?.ToString();
                        if (!string.IsNullOrEmpty(phoneNumber) && phoneNumber.Length == 10)
                        {
                            var firstName = worksheet.Cells[row, 2].Value?.ToString();
                            var lastName = worksheet.Cells[row, 3].Value?.ToString();
                            var email = worksheet.Cells[row, 4].Value?.ToString();
                            var address = worksheet.Cells[row, 5].Value?.ToString();
                            var groupName = worksheet.Cells[row, 6].Value?.ToString();

                            var model = new MyModel
                            {
                                PhoneNumber = phoneNumber,
                                FirstName = firstName,
                                LastName = lastName,
                                Address = address,
                                GroupName = groupName
                            };

                            log.LogInformation($"Processed row {row - 1}: {model}");
                        }
                    }
                }
            }
        }
    }

    public class MyModel
    {
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string GroupName { get; set; }

        public override string ToString()
        {
            return $"{{PhoneNumber={PhoneNumber},FirstName={FirstName}, LastName={LastName},Email={Email}, Address={Address}, GroupName={GroupName}}}";
        }
    }
}
