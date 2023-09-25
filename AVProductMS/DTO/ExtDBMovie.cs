using Newtonsoft.Json;

namespace AVProduct.DTO
{
    /// <summary>
    /// Movie Structure of tmdb api
    /// </summary>
    public class ExtDBMovie
    {
        /// <summary>
        /// ID
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public int ID { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        /// <summary>
        /// Overview
        /// </summary>
        [JsonProperty(PropertyName = "overview")]
        public string Overview { get; set; }

        /// <summary>
        /// Genres IDs
        /// </summary>
        [JsonProperty(PropertyName = "genre_ids")]
        public List<int> GenresIDs { get; set; }

        /// <summary>
        /// Original Language
        /// </summary>
        [JsonProperty(PropertyName = "original_language")]
        public string OriginalLanguage { get; set; }

        /// <summary>
        /// Release Date
        /// </summary>
        [JsonProperty(PropertyName = "release_date")]
        public DateTime? ReleaseDate { get; set; }
    }

    /// <summary>
    /// Response of tmdb api
    /// </summary>
    public class MovieDBResponse
    {
        /// <summary>
        /// ID
        /// </summary>
        [JsonProperty(PropertyName = "Page")]
        public int Page { get; set; }
        public IEnumerable<ExtDBMovie> Results { get; set; }
    }
}