using BookLibrary.Exceptions.Base;

namespace BookLibrary.Exceptions.Contracts;

public interface IExceptionMetadataProvider
{
    ExceptionMetadataAttribute GetMetadata(Exception ex);
    bool ShouldLog(Exception ex);
    bool ShouldReturnEmptyBody(Exception ex);
}
