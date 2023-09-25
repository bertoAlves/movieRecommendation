namespace Common.CinemaMS.DTO
{
    /// <summary>
    /// Cinema DTO
    /// </summary>
    public class CinemaDTO
    {
        /// <summary>
        /// Cinema ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Cinema Name
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Open Since
        /// </summary>
        public DateTime OpenSince { get; set; }

        /// <summary>
        /// City ID
        /// </summary>
        public int CityId { get; set; }
    }
}