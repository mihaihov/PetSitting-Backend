namespace PetSitting.Domain.Entities.PostManagement
{
    public class JobPost : Post
    {
        public string? Title { get; set; }
        public string? Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? Payment { get; set; }
        public bool IsOpen { get; set; } = true;

        public ICollection<JobApplication> Applications { get; set; } = new List<JobApplication>();
    }

}