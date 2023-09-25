using Common.CinemaMS.DTO;
using System.Text.Json.Serialization;

namespace Common.AVProductMS.DTO
{
    /// <summary>
    /// Audio Visual Product DTO
    /// </summary>
    public class AudioVisualProductDTO
    {
        [JsonIgnore]
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Title
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Overview
        /// </summary>
        public string Overview { get; set; }
        /// <summary>
        /// Genres
        /// </summary>
        public List<int> Genres { get; set; }
        /// <summary>
        /// Language
        /// </summary>
        public string Language { get; set; }
        /// <summary>
        /// ReleaseDate
        /// </summary>
        public DateTime? ReleaseDate { get; set; }
        /// <summary>
        /// Website
        /// </summary>
        public string Website { get; set; }
        /// <summary>
        /// Keywords
        /// </summary>
        public List<KeywordDTO> Keywords { get; set; }
    }
}
