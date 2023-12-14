
using Core.Interfaces.IModel;
using Core.ViewModel.PagingAndFiltration;
using Microsoft.EntityFrameworkCore;

namespace Core.Staff;

public static class Extension
{
    public static List<T> BuildTrees<T>(int? pId, List<T> items) where T : ITreeModel<T>
    {
        var childs = items.Where(c => c.ParentId == pId).ToList();
        if (!childs.Any())
        {
            return new List<T>();  // required an empty list instead of a null ! 
        }
        foreach (var i in childs)
        {
            i.Children = BuildTrees(i.Id, items);
        }
        return childs;
    }

    public static async Task<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> source, PagingOptions pagingOptions)
    {
        var count = await source.CountAsync();
        var items = await source.Skip((pagingOptions.Page - 1) * pagingOptions.PageSize).Take(pagingOptions.PageSize).ToListAsync();
        return new PagedList<T>(items, count, pagingOptions.Page, pagingOptions.PageSize);
    }

    public static PagedList<T> ToPagedList<T>(this IEnumerable<T> source, PagingOptions pagingOptions)
    {
        var count = source.Count();
        var items = source.Skip((pagingOptions.Page - 1) * pagingOptions.PageSize).Take(pagingOptions.PageSize).ToList();
        return new PagedList<T>(items, count, pagingOptions.Page, pagingOptions.PageSize);
    }
}
