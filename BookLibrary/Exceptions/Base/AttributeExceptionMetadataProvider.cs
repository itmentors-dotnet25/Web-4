using System.Collections.Concurrent;
using System.Reflection;
using BookLibrary.Config;
using BookLibrary.Exceptions.Contracts;
using Microsoft.Extensions.Options;

namespace BookLibrary.Exceptions.Base;

public class AttributeExceptionMetadataProvider(IOptions<ApplicationSettings> settings) : IExceptionMetadataProvider
{
    private readonly ConcurrentDictionary<Type, ExceptionMetadataAttribute> _cache = new();
    private readonly ExceptionHandlingSettings _config = settings.Value.Exceptions;

    public ExceptionMetadataAttribute GetMetadata(Exception ex)
    {
        var type = ex.GetType();
        
        // Кэшируем атрибуты для производительности
        return _cache.GetOrAdd(type, t =>
        {
            // ВАЖНО: нужно передать 'true' для поиска атрибутов в базовых классах!
            var attr = t.GetCustomAttribute<ExceptionMetadataAttribute>(inherit: true);
            return MergeMetadata(attr);
        });
    }
    
    private ExceptionMetadataAttribute MergeMetadata(ExceptionMetadataAttribute? attr)
    {
        var defaultAttribute = new ExceptionMetadataAttribute();
        
        return new ExceptionMetadataAttribute
        {
            MessageCategory = attr?.MessageCategory 
                              ?? _config.MessageCategory 
                              ?? defaultAttribute.MessageCategory,
            ErrorMessage = attr?.ErrorMessage 
                           ?? _config.ErrorMessage 
                           ?? defaultAttribute.ErrorMessage,
            ErrorCode = attr?.ErrorCode 
                        ?? _config.ErrorCode 
                        ?? defaultAttribute.ErrorCode,
            StatusCode = attr?.StatusCode 
                         ?? _config.StatusCode 
                         ?? defaultAttribute.StatusCode,
            LogEnabled = attr?.LogEnabled 
                         ?? _config.LogEnabled 
                         ?? defaultAttribute.LogEnabled,
            LogLevel = attr?.LogLevel 
                       ?? _config.LogLevel 
                       ?? defaultAttribute.LogLevel,
            EmptyResponseBody = attr?.EmptyResponseBody 
                                ?? _config.EmptyResponseBody 
                                ?? defaultAttribute.EmptyResponseBody,
        };
    }

    public bool ShouldLog(Exception ex)
    {
        var metadata = GetMetadata(ex);
        return metadata.LogEnabled;
    }

    public bool ShouldReturnEmptyBody(Exception ex)
    {
        var metadata = GetMetadata(ex);
        return metadata.EmptyResponseBody;
    }
}
