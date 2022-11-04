using ClosedXML.Excel;
using CsvHelper;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Office2016.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using HNG_api_project.Models;
using HNG_api_project.Models.ResponseOutput;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Protocol;
using OpenAI;
using OpenAI_API;
using RestSharp;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.RegularExpressions;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

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

        routes.MapPost("/calculation", (MathsWorkings mathsWorkings, HttpRequest request) =>
        {
            var responce = new MathCalResponse<MathsWorkings>();
            var list = new string[] { "Add", "Addition", "Plus", "+", "Sum", "Total", "Subtraction","Minus", "-","*", "Difference","Take Away", "Deduct","Multiply", "Product", "By", "Times"};
            var addition = new List<string>(new string[] { "Add", "Addition", "Plus", "+", "Sum", "Total", });
            var subtract = new List<string>(new string[] { "Subtraction", "Minus", "-","Difference", "Take Away", "Deduct", });
            var multiply = new List<string>(new string[] { "Product", "By", "Times","*" });
            //var list = Enum.GetNames(typeof(Operations));
            string value = mathsWorkings.operation_type.ToLower();
            string[] seperator = new string[] { ",", ".", "!", "\\"," ","\'s"};
            List<string> words = mathsWorkings.operation_type.Split(seperator, StringSplitOptions.RemoveEmptyEntries).ToList();

            if(words.Count > 1)
            {
                //Do something
                OpenAiModelLab bio = new OpenAiModelLab()
                {
                    model = "text-curie-001",
                    prompt = mathsWorkings.operation_type
                };

                string answer = callOpenAI(150, bio.prompt, bio.model, 0.7, 1, 0, 0);
                string[] numbers = Regex.Split(answer, @"\D+");
                List<int> values = new List<int>();

                foreach (string outputsInt in numbers)
                {
                    if (!string.IsNullOrEmpty(outputsInt))
                    {
                        int i = int.Parse(outputsInt);
                        values.Add(i);
                    }
                }

                var matched = list.Where(keyword =>
                    Regex.IsMatch(value, Regex.Escape(keyword), RegexOptions.IgnoreCase));

                foreach (string item in matched)
                {
                    if(addition.Exists(i=> i.Equals(item)))
                    {
                        value = "addition";
                    }else if(subtract.Exists(i => i.Equals(item)))
                    {
                        value = "subtraction";
                    }
                    else
                    {
                        value = "multiplication";
                    }

                }
                responce.operation_type = Enum.Parse<Operations>(value);
                responce.result = values.Last();
                return Results.Ok(responce);
            }

            try
            {
                int answer = 0;

                switch (value)
                {
                    case "addition":
                        answer = mathsWorkings.x + mathsWorkings.y;
                        break;
                    case "subtraction":
                        answer = mathsWorkings.x - mathsWorkings.y;
                        break;
                    case "multiplication":
                        answer = mathsWorkings.x * mathsWorkings.y;
                        break;
                    default:
                        answer = mathsWorkings.x + mathsWorkings.y;
                        break;
                }
                responce.result = answer;
                responce.operation_type = Enum.Parse<Operations>(value);
                return Results.Ok(responce);
            }
            catch (Exception e)
            {
                return Results.BadRequest(e.Message);
            }
        });

        routes.MapPost("/cals", async () =>
        {
            //var api = new OpenAI.OpenAIClient(authentication:OpenAI.OpenAIAuthentication.LoadFromEnv());
            var api = new OpenAI_API.OpenAIAPI(APIAuthentication.LoadFromEnv(), OpenAI_API.Engine.Ada);
            var request = new SearchRequest()
            {
                Query = "Washington DC",
                Documents = new List<string> { "Canada", "China", "USA", "Spain" }
            };
            var result = await api.Search.GetBestMatchAsync(request);
            return Results.Ok(result);
        });
        static string callOpenAI(int tokens, string input, string engine,
                  double temperature, int topP, int frequencyPenalty, int presencePenalty)
        {

            var openAiKey = "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";

            var apiCall = "https://api.openai.com/v1/engines/" + engine + "/completions";

            try
            {

                using (var httpClient = new HttpClient())
                {
                    using (var request = new HttpRequestMessage(new HttpMethod("POST"), apiCall))
                    {
                        request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + openAiKey);
                        request.Content = new StringContent("{\n  \"prompt\": \"" + input + "\",\n  \"temperature\": " +
                                                            temperature.ToString(CultureInfo.InvariantCulture) + ",\n  \"max_tokens\": " + tokens + ",\n  \"top_p\": " + topP +
                                                            ",\n  \"frequency_penalty\": " + frequencyPenalty + ",\n  \"presence_penalty\": " + presencePenalty + "\n}");

                        request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                        var response = httpClient.SendAsync(request).Result;
                        var json = response.Content.ReadAsStringAsync().Result;

                        dynamic dynObj = JsonConvert.DeserializeObject(json);

                        if (dynObj != null)
                        {
                            return dynObj.choices[0].text.ToString();
                        }


                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }

            return null;


        }

    }
}
