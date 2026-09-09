namespace MovieApplicationUI.Dto.History
{
    public class CreatedHistoryResponseDto
    {
        public Guid EntityID { get; set; }
        public Guid MovieID { get; set; }
        public int UserID { get; set; }
        public int TotalSeconds { get; set; }
        public int WatchedSeconds { get; set; }
    }
}
