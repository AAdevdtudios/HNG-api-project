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
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Operations operation_type { get; set; }
        public int x { get; set; } = 0;
        public int y { get; set; } = 0;
    }
}
