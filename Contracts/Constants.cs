using System.Collections.Generic;

namespace Contracts
{
    public static class Constants
    {
        public const string CONTRIBTYPE_SUFFIX = "CONTRIBTYPE";
        public const string CONTRIB_SUFFIX = "CONTRIBUTOR";
        public const string MOVIE_SUFFIX = "MOVIE";
        public const string GENRE_SUFFIX = "GENRE";

        public static Dictionary<string, string> SuffixToPropertyMap = new Dictionary<string, string>
        {
            {"ContribId", CONTRIB_SUFFIX},
            {"GenreId", GENRE_SUFFIX},
            {"MovieId", MOVIE_SUFFIX},
            {"ContribTypeId", CONTRIBTYPE_SUFFIX}
        };
    }
}
