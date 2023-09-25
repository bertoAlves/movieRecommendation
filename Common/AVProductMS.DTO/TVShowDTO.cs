namespace Common.AVProductMS.DTO
{
    /// <summary>
    /// TVShow DTO
    /// </summary>
    public class TVShowDTO : AudioVisualProductDTO
    {
        /// <summary>
        /// Number Of Seasons
        /// </summary>
        public int NumberOfSeasons { get; set; }
        /// <summary>
        /// Number Of Episodes
        /// </summary>
        public int NumberOfEpisodes { get; set; }
        /// <summary>
        /// HasConcluded
        /// </summary>
        public bool HasConcluded { get; set; }
    }
}
