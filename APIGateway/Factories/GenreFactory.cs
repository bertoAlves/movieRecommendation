using APIGateway.Factories.Interfaces;
using Common.CinemaMS.DTO;

namespace APIGateway.Factories
{
    /// <summary>
    /// Auxiliary Class to map genres
    /// </summary>
    public class GenreMapping
    {
        /// <summary>
        /// BeezyGenreID
        /// </summary>
        public int BeezyGenreId { get; set; }
        /// <summary>
        /// MovieDBGenreId
        /// </summary>
        public int MovieDBGenreId { get; set; }
        /// <summary>
        /// GenreName
        /// </summary>
        public string GenreName { get; set; }
    }

    /// <summary>
    /// Genre Factory
    /// </summary>
    public class GenreFactory : IGenreFactory
    {
        private readonly List<GenreMapping> genreMappings;

        /// <summary>
        /// Genre Factory
        /// </summary>
        public GenreFactory() {
            #region genre mappings
                genreMappings = new List<GenreMapping>
                {
                    new GenreMapping
                    {
                        BeezyGenreId = 1,
                        MovieDBGenreId = 28,
                        GenreName = "Action"
                    },
                    new GenreMapping
                    {
                        BeezyGenreId = 2,
                        MovieDBGenreId = 12,
                        GenreName = "Adventure"
                    },
                    new GenreMapping
                    {
                        BeezyGenreId = 3,
                        MovieDBGenreId = 16,
                        GenreName = "Animation"
                    },
                    new GenreMapping
                    {
                        BeezyGenreId = 4,
                        MovieDBGenreId = 35,
                        GenreName = "Comedy"
                    },
                    new GenreMapping
                    {
                        BeezyGenreId = 5,
                        MovieDBGenreId = 80,
                        GenreName = "Crime"
                    },
                    new GenreMapping
                    {
                        BeezyGenreId = 6,
                        MovieDBGenreId = 99,
                        GenreName = "Documentary"
                    },
                    new GenreMapping
                    {
                        BeezyGenreId = 7,
                        MovieDBGenreId = 18,
                        GenreName = "Drama"
                    },
                    new GenreMapping
                    {
                        BeezyGenreId = 8,
                        MovieDBGenreId = 10751,
                        GenreName = "Family"
                    },
                    new GenreMapping
                    {
                        BeezyGenreId = 9,
                        MovieDBGenreId = 14,
                        GenreName = "Fantasy"
                    },
                    new GenreMapping
                    {
                        BeezyGenreId = 10,
                        MovieDBGenreId = 36,
                        GenreName = "History"
                    },
                    new GenreMapping
                    {
                        BeezyGenreId = 11,
                        MovieDBGenreId = 27,
                        GenreName = "Horror"
                    },
                    new GenreMapping
                    {
                        BeezyGenreId = 12,
                        MovieDBGenreId = 10402,
                        GenreName = "Music"
                    },
                    new GenreMapping
                    {
                        BeezyGenreId = 13,
                        MovieDBGenreId = 9648,
                        GenreName = "Mystery"
                    },
                    new GenreMapping
                    {
                        BeezyGenreId = 14,
                        MovieDBGenreId = 10749,
                        GenreName = "Romance"
                    },
                    new GenreMapping
                    {
                        BeezyGenreId = 15,
                        MovieDBGenreId = 878,
                        GenreName = "Science Fiction"
                    },
                    new GenreMapping
                    {
                        BeezyGenreId = 16,
                        MovieDBGenreId = 10770,
                        GenreName = "TV Movie"
                    },
                    new GenreMapping
                    {
                        BeezyGenreId = 17,
                        MovieDBGenreId = 53,
                        GenreName = "Thriller"
                    },
                    new GenreMapping
                    {
                        BeezyGenreId = 18,
                        MovieDBGenreId = 10752,
                        GenreName = "War"
                    },
                    new GenreMapping
                    {
                        BeezyGenreId = 19,
                        MovieDBGenreId = 37,
                        GenreName = "Western"
                    }
                };
            #endregion
        }

        /// <summary>
        /// Map Beezy genres ids to MovieDB GenreId
        /// </summary>
        /// <param name="genres"></param>
        /// <returns></returns>
        public async Task<IEnumerable<int>> MapBeezyGenre(IEnumerable<GenreDTO> genres)
        {
            var mappedGenreIds = new List<int>();

            foreach (var genre in genres)
            {
                var mapping = genreMappings.FirstOrDefault(mapping => mapping.GenreName.Equals(genre.Name, StringComparison.OrdinalIgnoreCase));

                if (mapping != null)
                {
                    mappedGenreIds.Add(mapping.MovieDBGenreId);
                }
            }

            return mappedGenreIds;
        }

    }
}
