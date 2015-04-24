namespace YorkshireDigital.Web.Feedback.Models
{
    public class FeedbackPostModel
    {
        public string Name { get; set; }
        public string Contact { get; set; }
        public string Details { get; set; }
        public string Site { get; set; }

        public string SlackUpdate
        {
            get { return string.Format("{0} ({1}) has raised an issue at {2}", Name, Contact, Site); }
        }
    }
}