using System.Linq.Expressions;

namespace BookLibrary.Specifications.Book;

public class MyBookSpecification : MySpecification<Models.Book>
{
    private readonly BookFilterParams _filterParams;

    public MyBookSpecification(BookFilterParams filterParams)
    {
        _filterParams = filterParams;
        
        ApplySorting();
    }

    public override Expression<Func<Models.Book, bool>>? Criteria
    {
        get
        {
            return book => 
                (_filterParams.Title == null || book.Title.ToLowerInvariant().Contains(_filterParams.Title.ToLowerInvariant())) &&
                (_filterParams.Author == null || book.Author.ToLowerInvariant().Contains(_filterParams.Author.ToLowerInvariant())) &&
                (_filterParams.Genre == null || book.Genre == _filterParams.Genre) &&
                (_filterParams.Year == null || book.PublicationYear == _filterParams.Year) &&
                (_filterParams.IsAvailable == null || book.IsAvailable == _filterParams.IsAvailable);
        }
    }

    public override int? Take => _filterParams.PageSize;
    public override int? Skip => _filterParams.PageSize * (_filterParams.PageNumber - 1);
    public override bool IsPagingEnabled => _filterParams.PageSize.HasValue;

    private void ApplySorting()
    {
        if (string.IsNullOrEmpty(_filterParams.SortBy))
            return;

        var sortDirection = (_filterParams.SortDirection ?? "asc").ToLowerInvariant();

        switch (_filterParams.SortBy.ToLowerInvariant())
        {
            case "title":
                if (sortDirection == "asc")
                    AddOrderBy(b => b.Title);
                else
                    AddOrderByDescending(b => b.Title);
                break;

            case "author":
                if (sortDirection == "asc")
                    AddOrderBy(b => b.Author);
                else
                    AddOrderByDescending(b => b.Author);
                break;

            case "year":
                if (sortDirection == "asc")
                    AddOrderBy(b => b.PublicationYear);
                else
                    AddOrderByDescending(b => b.PublicationYear);
                break;

            case "createdat":
                if (sortDirection == "asc")
                    AddOrderBy(b => b.CreatedAt);
                else
                    AddOrderByDescending(b => b.CreatedAt);
                break;
        }
    }
}
