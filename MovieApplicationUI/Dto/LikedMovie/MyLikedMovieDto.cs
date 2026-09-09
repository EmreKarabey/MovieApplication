namespace MovieApplicationUI.Dto.LikedMovie
{
    public class MyLikedMovieDto
    {
        public Guid EntityID { get; set; }
        public Guid MovieId { get; set; }
        public string Name { get; set; }
        public string ImageURL { get; set; }
        public string VideoURL { get; set; }
        public string Description { get; set; }
        public string ProducerName { get; set; }
        public int ReleaseDate { get; set; }
        public DateTime CreatedAt { get; set; }

        public IEnumerable<string> CategoryName { get; set; }
    }
}
