using System.Text.Json.Serialization;

namespace HNG_api_project.Models.ResponseOutput
{
    public class MathCalResponse <T>
    {
        public T Data { get; set; }
        public string slackUsername { get; set; } = "DevTherapy";
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Operations operation_type { get; set; }
        public int result { get; set; }

    }
}
