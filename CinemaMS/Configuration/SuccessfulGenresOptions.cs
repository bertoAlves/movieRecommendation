namespace CinemaMS.Configuration
{
    /// <summary>
    /// Successful Genres Algorithm Options
    /// </summary>
    public class SuccessfulGenresOptions
    {
        /// <summary>
        /// Algorithm Name
        /// </summary>
        public string Algorithm { get; set; }
        /// <summary>
        /// For Algorithm 'SELL-OUTS' minimum number of sell-outs (default = 0)
        /// </summary>
        public int? MinNumberSellouts { get; set; }
    }
}
