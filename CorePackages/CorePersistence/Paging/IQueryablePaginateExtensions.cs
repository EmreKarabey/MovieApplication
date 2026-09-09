using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CorePersistence.Paginate;
using Microsoft.EntityFrameworkCore;

namespace CorePersistence.Paging
{
    public static class IQueryablePaginateExtensions
    {
        public static async Task<Paginate<TEntity>> ToPaginateAsync<TEntity>(this IQueryable<TEntity> source,
            int index,
            int size,
            CancellationToken cancellationToken)
        {
            int count = await source.CountAsync(cancellationToken).ConfigureAwait(false);

            int pages = (int)Math.Ceiling(count / (double)size);

            List<TEntity> items = await source.Skip(index * size).Take(size).ToListAsync(cancellationToken).ConfigureAwait(false);

            var Paginate = new Paginate<TEntity>()
            {
                Count = count,
                Index = index,
                Items = items,
                Pages = pages,
                Size = size
            };
            return Paginate;
        }
    }
}
