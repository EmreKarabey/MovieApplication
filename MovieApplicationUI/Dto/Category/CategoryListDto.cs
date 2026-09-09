namespace MovieApplicationUI.Dto.Category
{
    public class CategoryListDto
    {
        public string CategoryName { get; set; }
    }

    public class ApiResponse<T>
    {
        public List<T> Items { get; set; }
    }
}
