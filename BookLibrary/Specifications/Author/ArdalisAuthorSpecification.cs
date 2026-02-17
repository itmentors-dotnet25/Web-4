using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.Specifications.Author;

public class ArdalisAuthorSpecification : MySpecification<Models.Author>
{
    // Поля разрешенные для сортировки
    private readonly AuthorFilterParams _filterParams;
    private readonly string? _nameFilterPattern; // Шаблон для ILike: %значение%

    public ArdalisAuthorSpecification(AuthorFilterParams filterParams)
    {
        _filterParams = filterParams;
        _nameFilterPattern = !string.IsNullOrEmpty(filterParams.Name) 
            ? $"%{filterParams.Name}%" 
            : null;
        Initialize();
    }

    private void Initialize()
    {
        if (_filterParams.WithBooks)
        {
            AddInclude(nameof(Models.Author.Books)); // Строковое имя для совместимости
        }

        ApplySorting(
            _filterParams.SortBy,
            _filterParams.SortByDesc,
            defaultSort: a => a.Id // Сортировка по умолчанию
        );
    }

    public override Expression<Func<Models.Author, bool>>? Criteria =>
        author => string.IsNullOrEmpty(_nameFilterPattern) || 
                  EF.Functions.ILike(author.Name, _nameFilterPattern);

    protected override Expression<Func<Models.Author, object>>? GetSortExpression(string field) =>
        field switch
        {
            "name" => a => a.Name,
            "country" => a => a.Country ?? string.Empty,
            "birthyear" => a => a.BirthYear ?? int.MaxValue,
            "createdat" => a => a.CreatedAt,
            "updatedat" => a => a.UpdatedAt,
            "id" => a => a.Id,
            _ => null
        };
}
