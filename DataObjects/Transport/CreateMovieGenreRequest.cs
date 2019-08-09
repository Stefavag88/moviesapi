namespace DataObjects
{
    public class CreateMovieGenreRequest : RequestBase
    {
        public int MovieId { get; set; }
        public int GenreId { get; set; }
    }
}
