using Common.CinemaMS.DTO;
using Newtonsoft.Json;

namespace AVProduct.DTO
{
    /// <summary>
    /// Movie Keywords
    /// </summary>
    public class ExtDBMovieKeywords
    {
        /// <summary>
        /// Movie Id
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        /// <summary>
        /// Keywords
        /// </summary>
        [JsonProperty(PropertyName = "keywords")]
        public IEnumerable<ExtKeywod> Keywords { get; set; }
    }

    /// <summary>
    /// Keyword
    /// </summary>
    public class ExtKeywod
    {
        /// <summary>
        /// Keyword Id
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        /// <summary>
        /// Keyword Name
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}