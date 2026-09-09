namespace CorePersistence.Dynamic
{
    public class DynamicQuery
    {
        public IEnumerable<Sort>? Sorts { get; set; }
        public Filter? Filter { get; set; }

        public DynamicQuery() { }
        public DynamicQuery(IEnumerable<Sort>? sorts, Filter?filter)
        {
            Sorts = sorts;
            Filter = filter;
        }
    }
}
