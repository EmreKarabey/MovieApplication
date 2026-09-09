namespace MovieApplicationUI.Dto.Movie
{
    public class PublishMovieDto
    {
        public string Name { get; set; }
        public IFormFile VideoFile { get; set; }
        public IFormFile ImageFile { get; set; }
        public string Description { get; set; }
        public string ProducerName { get; set; }
        public int ReleaseDate { get; set; }
    }

    public class PublishMovieCategoryDto
    {
        public string EntityId { get; set; }
        public string CategoryName { get; set; }
    }

    public class PublishTableMovieCategoryDto
    {
        public Guid MovieID { get; set; }
        public Guid CategoryID { get; set; }
    }

}
