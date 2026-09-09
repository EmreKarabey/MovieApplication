namespace MovieApplicationUI.Paginate
{
    public class PaginateList<T> where T : class
    {
        private IList<T> _items;

        public IList<T> Items
        {
            get => _items ??= new List<T>();
            set => _items = value;
        }

        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
    }
}
