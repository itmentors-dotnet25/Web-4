using System.Linq.Expressions;

namespace BookLibrary.Specifications.Book;

public class MyBookSpecification : MySpecification<Models.Book>
{
    private readonly BookFilterParams _filterParams;

    public MyBookSpecification(BookFilterParams filterParams)
    {
        _filterParams = filterParams;
        Initialize();
    }
    
    private void Initialize()
    {
        // Добавляем Include только если запрошено
        if (_filterParams.WithAuthors)
        {
            AddInclude(nameof(Models.Book.Author));  // ← Строковое имя для in-memory
            // AddInclude(b => b.Author!);    // ← Expression для EF Core (опционально)
        }
        
        if (_filterParams.WithCategories)
        {
            AddInclude(nameof(Models.Book.Category));
            // AddInclude(b => b.Category!);
        }        
        ApplySorting(
            _filterParams.SortBy, 
            _filterParams.SortByDesc,
            defaultSort: b => b.Id // Сортировка по умолчанию
            );
    }

    public override Expression<Func<Models.Book, bool>> Criteria
    {
        get
        {
            return book => 
                (_filterParams.Title == null || book.Title.ToLower().Contains(_filterParams.Title.ToLower())) &&
                (_filterParams.Author == null || book.Author != null && book.Author.Name.ToLower().Contains(_filterParams.Author.ToLower())) &&
                (_filterParams.AuthorId == null || book.AuthorId == _filterParams.AuthorId.Value) &&
                (_filterParams.CategoryId == null || book.CategoryId == _filterParams.CategoryId.Value) &&
                (_filterParams.Genre == null || book.Genre == _filterParams.Genre) &&
                (_filterParams.Year == null || book.PublicationYear == _filterParams.Year) &&
                (_filterParams.IsAvailable == null || book.IsAvailable == _filterParams.IsAvailable);
        }
    }

    public override int? Take => _filterParams.PageSize;
    public override int? Skip => _filterParams.PageSize * (_filterParams.PageNumber - 1);
    public override bool IsPagingEnabled => _filterParams.PageSize.HasValue;
    
    protected override Expression<Func<Models.Book, object>>? GetSortExpression(string field)
    {
        return field switch
        {
            "title" => b => b.Title,
            "author" => b => b.Author != null ? b.Author.Name : string.Empty,
            "authorid" => b => b.AuthorId,
            "category" => b => b.Category != null ? b.Category.Name : string.Empty,
            "categoryid" => b => b.CategoryId,
            "publicationyear" => b => b.PublicationYear,
            "genre" => b => b.Genre ?? string.Empty,
            "isavailable" => b => b.IsAvailable,
            "createdat" => b => b.CreatedAt,
            "updatedat" => b => b.UpdatedAt,
            "id" => b => b.Id,
            _ => null
        };
    }
}
