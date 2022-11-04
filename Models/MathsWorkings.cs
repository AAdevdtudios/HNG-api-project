using System.Text.Json.Serialization;

namespace HNG_api_project.Models
{
    public enum Operations
    {
        addition,
        subtraction,
        multiplication
    }
    public class MathsWorkings
    {
        //[JsonConverter(typeof(JsonStringEnumConverter))]
        public string operation_type { get; set; } = Operations.addition.ToString();
        public int x { get; set; }
        public int y { get; set; }
    }
}
