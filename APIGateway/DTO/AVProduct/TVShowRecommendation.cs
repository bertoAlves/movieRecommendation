using Newtonsoft.Json;

namespace APIGateway.DTO.AVProduct
{
    /// <summary>
    /// Recommendation TVShow
    /// </summary>
    public class TVShowRecommendation : AVProductRecommendation
    {
        /// <summary>
        /// Number Of Seasons
        /// </summary>
        [JsonProperty(PropertyName = "NumberOfSeasons")]
        public int NumberOfSeasons { get; set; }
        /// <summary>
        /// Number Of Episodes
        /// </summary>
        [JsonProperty(PropertyName = "NumberOfEpisodes")]
        public int NumberOfEpisodes { get; set; }
        /// <summary>
        /// Has Concluded
        /// </summary>
        [JsonProperty(PropertyName = "HasConcluded")]
        public bool HasConcluded { get; set; }
    }
}