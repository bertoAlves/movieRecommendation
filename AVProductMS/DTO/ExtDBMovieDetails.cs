using Newtonsoft.Json;

namespace AVProduct.DTO
{
    /// <summary>
    /// Movie Details Structure of tmdb api
    /// </summary>
    public class ExtDBMovieDetails : ExtDBMovie
    {
        /// <summary>
        /// Homepage
        /// </summary>
        [JsonProperty(PropertyName = "homepage")]
        public string Homepage { get; set; }

        //add more fields if necessary
    }
}