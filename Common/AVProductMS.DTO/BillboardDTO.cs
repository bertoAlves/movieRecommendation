namespace Common.AVProductMS.DTO
{
    /// <summary>
    /// Intelligent Billboard
    /// </summary>
    public class IntelligentBillboard
    {
        /// <summary>
        /// Week Number and the big screen movie and small screen movie list
        /// </summary>
        public Dictionary<int, IntelligentWeekPlan> billboard { get; set; }
    }

    /// <summary>
    /// Week Plan
    /// </summary>
    public class IntelligentWeekPlan
    {
        /// <summary>
        /// Movies for the big screens
        /// </summary>
        public IEnumerable<MovieDTO> BigScreens { get; set; }

        /// <summary>
        /// Movies for the small screens
        /// </summary>
        public IEnumerable<MovieDTO> SmallScreens { get; set; }
    }
}
