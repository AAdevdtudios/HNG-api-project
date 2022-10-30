namespace HNG_api_project.Models
{
    public class Bio
    {
        public int Id { get; set; }
        public bool backend { get; set; } = true;
        public int age { get; set; }
        public string slackUsername { get; set; }
        public string bio { get; set; }
    }
}
