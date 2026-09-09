namespace MovieApplicationUI.Dto.History
{
    public class UpdateHistoryProgressDto
    {
        public Guid Id { get; set; }
        public Guid MovieID { get; set; }
        public int TotalSeconds { get; set; }
        public int WatchedSeconds { get; set; }
    }
}
