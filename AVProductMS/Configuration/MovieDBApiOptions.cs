namespace AVProduct.Configuration
{
    /// <summary>
    /// MovieDBApi Options
    /// </summary>
    public class MovieDBApiOptions
    {
        /// <summary>
        /// Base Uri
        /// </summary>
        public string BaseUri { get; set; }
        /// <summary>
        /// AllTimeMovies Endpoint
        /// </summary>
        public string AllTimeMoviesEndpoint { get; set; }
        /// <summary>
        /// AllTimeMovies Params
        /// </summary>
        public string AllTimeMoviesParams { get; set; }
        /// <summary>
        /// UpcomingMovies Endpoint
        /// </summary>
        public string UpcomingMoviesEndpoint { get; set; }
        /// <summary>
        /// UpcomingMovies Params
        /// </summary>
        public string UpcomingMoviesParams { get; set; }
        /// <summary>
        /// AllTimeTVShows Endpoint
        /// </summary>
        public string AllTimeTVShowsEndpoint { get; set; }
        /// <summary>
        /// AllTimeTVShows Params
        /// </summary>
        public string AllTimeTVShowsParams { get; set; }
        /// <summary>
        /// AllTimeDocumentaries Endpoint
        /// </summary>
        public string AllTimeDocumentariesEndpoint { get; set; }
        /// <summary>
        /// AllTimeDocumentaries Params
        /// </summary>
        public string AllTimeDocumentariesParams { get; set; }
        /// <summary>
        /// Blockbusters Endpoint
        /// </summary>
        public string BlockbustersEndpoint { get; set; }
        /// <summary>
        /// Blockbusters Params
        /// </summary>
        public string BlockbustersParams { get; set; }
        /// <summary>
        /// MinorityGenres Endpoint
        /// </summary>
        public string MinorityGenresEndpoint { get; set; }
        /// <summary>
        /// MinorityGenres Params
        /// </summary>
        public string MinorityGenresParams { get; set; }
        /// <summary>
        /// MinorityGenres Params
        /// </summary>
        public int MaxNumberOfMovies { get; set; } = 20;
        /// <summary>
        /// MovieDetails Endpoint
        /// </summary>
        public string MovieDetailsEndpoint { get; set; }
        /// <summary>
        /// MovieDetails Params
        /// </summary>
        public string MovieDetailsParams { get; set; }
        /// <summary>
        /// Keywords Endpoint
        /// </summary>
        public string KeywordsEndpoint { get; set; }
    }
}
