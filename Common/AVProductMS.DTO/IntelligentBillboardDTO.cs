namespace Common.AVProductMS.DTO
{
    /// <summary>
    /// Billboard
    /// </summary>
    public class Billboard
    {
        /// <summary>
        /// Week Number and the movie list
        /// </summary>
        public Dictionary<int, WeekPlan> billboard { get; set; }
    }

    /// <summary>
    /// Week Plan
    /// </summary>
    public class WeekPlan
    {
        /// <summary>
        /// Movies
        /// </summary>
        public IEnumerable<MovieDTO> Movies { get; set; }
    }
}
