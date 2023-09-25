namespace APIGateway.DTO
{
    public class BillboardResponse
    {
        /// <summary>
        /// Week Number and the room - movie association
        /// </summary>
        public Dictionary<int, IEnumerable<WeekPlan>> Billboard { get; set; }
    }

    public class WeekPlan
    {
        public int RoomID { get; set; }

        public int MovieID { get; set; }

        public string MovieTitle { get; set; }
    }
}
