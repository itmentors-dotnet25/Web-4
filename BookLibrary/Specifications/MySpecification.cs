using System.Linq.Expressions;
using BookLibrary.Contracts.Specifications;

namespace BookLibrary.Specifications;

public abstract class MySpecification<T> : IMySpecification<T>
{
    private readonly List<Expression<Func<T, object>>> _includes = [];
    private readonly List<string> _includeStrings = [];
    private readonly List<Expression<Func<T, object>>> _orderBy = [];
    private readonly List<Expression<Func<T, object>>> _orderByDescending = [];

    public virtual Expression<Func<T, bool>>? Criteria => null;
    public virtual List<Expression<Func<T, object>>> Includes => _includes;
    public virtual List<string> IncludeStrings => _includeStrings;
    public virtual List<Expression<Func<T, object>>> OrderBy => _orderBy;
    public virtual List<Expression<Func<T, object>>> OrderByDescending => _orderByDescending;
    public virtual int? Take => null;
    public virtual int? Skip => null;
    public virtual bool IsPagingEnabled => false;

    protected virtual void AddInclude(Expression<Func<T, object>> includeExpression)
    {
        _includes.Add(includeExpression);
    }

    protected virtual void AddInclude(string includeString)
    {
        _includeStrings.Add(includeString);
    }

    protected virtual void AddOrderBy(Expression<Func<T, object>> orderByExpression)
    {
        _orderBy.Add(orderByExpression);
    }

    protected virtual void AddOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
    {
        _orderByDescending.Add(orderByDescendingExpression);
    }
}
