using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApplication.Responses
{
    public class GetListResponse<T>:BasePageableModel
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

    public class BasePageableModel
    {
    }
}
