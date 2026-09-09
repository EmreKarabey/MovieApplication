namespace MovieApplicationUI.Dto.Movie
{
    public class MovieCategoryDetail
    {
        public Guid EntityId { get; set; }
        public string Name { get; set; }
        public string ImageURL { get; set; }
        public Guid MovieId { get; set; }
        public string MovieName { get; set; }
        public string VideoURL { get; set; }
        public string Description { get; set; }
        public string ProducerName { get; set; }
        public int ReleaseDate { get; set; }

        public ICollection<string> CategoryName { get; set; }
    }
}
