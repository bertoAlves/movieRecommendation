using Newtonsoft.Json;

namespace APIGateway.DTO.AVProduct
{
    /// <summary>
    /// AVProduct Recommendation
    /// </summary>
    public class AVProductRecommendation
    {
        /// <summary>
        /// Title
        /// </summary>
        [JsonProperty(PropertyName = "Title")]
        public int Title { get; set; }

        /// <summary>
        /// Overview
        /// </summary>
        [JsonProperty(PropertyName = "Overview")]
        public string Overview { get; set; }

        /// <summary>
        /// Genres IDs
        /// </summary>
        [JsonProperty(PropertyName = "Genres")]
        public List<string> Genres { get; set; }

        /// <summary>
        /// Language
        /// </summary>
        [JsonProperty(PropertyName = "Language")]
        public string Language { get; set; }

        /// <summary>
        /// Original Language
        /// </summary>
        [JsonProperty(PropertyName = "OriginalLanguage")]
        public string OriginalLanguage { get; set; }

        /// <summary>
        /// Release Date
        /// </summary>
        [JsonProperty(PropertyName = "ReleaseDate")]
        public DateTime? ReleaseDate { get; set; }

        /// <summary>
        /// Website
        /// </summary>
        [JsonProperty(PropertyName = "Website")]
        public string Website { get; set; }

        /// <summary>
        /// Keywords
        /// </summary>
        [JsonProperty(PropertyName = "Keywords")]
        public List<string> Keywords { get; set; }
    }
}