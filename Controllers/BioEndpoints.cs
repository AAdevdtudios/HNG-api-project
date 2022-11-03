using ClosedXML.Excel;
using CsvHelper;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Office2016.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using HNG_api_project.Models;
using HNG_api_project.Models.ResponseOutput;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using System.Globalization;

namespace HNG_api_project.Controllers;

public static class BioEndpoints
{
    public static void MapBioEndpoints (this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/Bio", () =>
        {
            return new Bio()
            {
                backend = true,
                age = 23,
                bio = "My name is Oladele Joseph, am a software developer",
                slackUsername = "DevTherapy",
            };
        })
        .WithName("GetAllBios")
        .Produces<Bio[]>(StatusCodes.Status200OK);

        /*routes.MapPost("/uploadIt", (IFormFile file, HttpRequest request) =>
        {
            var csv = new List<string[]>();
            using (var reader = new StreamReader(request.Body, System.Text.Encoding.UTF8))
            {
                return Results.Ok(reader);
            }
        })
        .WithName("UploadingFiles");*/

        byte[] Create_the_File(string users)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Users");
            var currentRow = 1;
            worksheet.Cell(currentRow, 1).Value = "Id";
            worksheet.Cell(currentRow, 2).Value = "Title";
            worksheet.Cell(currentRow, 3).Value = "FirstName";

            foreach (var user in users)
            {
                currentRow++;
                worksheet.Cell(currentRow, 1).Value = user;
                worksheet.Cell(currentRow, 2).Value = user;
            }
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();
            return content;
        }
        routes.MapPost("/calculation", (MathsWorkings mathsWorkings, HttpRequest request) =>
        {
            var responce = new MathCalResponse<MathsWorkings>();
            try
            {
                int answer = 0;

                switch (mathsWorkings.operation_type)
                {
                    case Operations.addition:
                        answer = mathsWorkings.x + mathsWorkings.y;
                        break;
                    case Operations.subtraction:
                        answer = mathsWorkings.x - mathsWorkings.y;
                        break;
                    case Operations.multiplication:
                        answer = mathsWorkings.x * mathsWorkings.y;
                        break;
                    default:
                        answer = mathsWorkings.x + mathsWorkings.y;
                        break;
                }
                responce.result = answer;
                responce.operation_type = mathsWorkings.operation_type;
                return Results.Ok(responce);
            }
            catch (Exception e)
            {
                return Results.BadRequest(e.Message);
            }
        });
        /*routes.MapPost("/upload",(HttpRequest request) =>
        {
            using (var reader = new StreamReader(request.Body, System.Text.Encoding.UTF8))

            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<CSVdata>();
                return Results.Ok(csv.ToJson(Newtonsoft.Json.Formatting.Indented));
            }
            //return Results.File(Create_the_File("Somedata"), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "someting.csv");

        }).Accepts<IFormFile>("application/json");*/
    }
}
