namespace DataObjects
{
    public class CreateContributorTypePerMovieRequest : RequestBase
    {
        public int MovieId { get; set; }
        public int ContributorId { get; set; }
        public int ContribTypeId { get; set; }
    }
}
