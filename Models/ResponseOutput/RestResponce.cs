namespace HNG_api_project.Models.ResponseOutput
{
    public class RestResponce
    {
        public int Id { get; set; }
        public string @object { get; set; }
        public string created { get; set; }
        public string model { get; set; }
        public List<Choices> choices { get; set; }

    }
    public class Choices
    {
        public int index { get; set; }
        public string text { get; set; }
    }
}
