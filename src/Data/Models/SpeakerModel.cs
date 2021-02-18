namespace CoreCodeCamp.Data.Models
{
    public class SpeakerModel
    {
        //in models we give id to the things that we know we want to update in the future, cause
        //we need then identify it in some way
        public int SpeakerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Company { get; set; }
        public string CompanyUrl { get; set; }
        public string BlogUrl { get; set; }
        public string Twitter { get; set; }
        public string GitHub { get; set; }
    }
}