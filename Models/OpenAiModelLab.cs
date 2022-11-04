namespace HNG_api_project.Models
{
    public class OpenAiModelLab
    {
        public string model { get; set; }
        public string prompt { get; set; }
        public float temperature { get; set; } = 0.7f;
        public int max_tokens { get; set; } = 256;
        public int top_p { get; set; } = 1;
        public int frequency_penalty { get; set; } = 0;
        public int presence_penalty { get; set; } = 0;
    }
}
