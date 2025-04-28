using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace Ambev.DeveloperEvaluation.Common.Cache;

/// <summary>
/// An attribute that invalidates a specific cache entry after an action is executed.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class InvalidateCacheAttribute : Attribute, IAsyncActionFilter
{
    private readonly string _cacheKeyTemplate;

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidateCacheAttribute"/> class.
    /// </summary>
    /// <param name="cacheKeyTemplate">
    /// A template for generating the cache key. Use placeholders (e.g., {parameterName}) to include action parameters in the key.
    /// </param>
    public InvalidateCacheAttribute(string cacheKeyTemplate)
    {
        _cacheKeyTemplate = cacheKeyTemplate;
    }

    /// <summary>
    /// Executes the logic to invalidate a cache entry after the action is executed.
    /// </summary>
    /// <param name="context">The context for the action being executed.</param>
    /// <param name="next">The delegate to execute the next action filter or the action itself.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the cache service is not properly configured.</exception>
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var logger = context.HttpContext.RequestServices.GetService(typeof(ILogger<InvalidateCacheAttribute>)) as ILogger<InvalidateCacheAttribute>;

        if (context.HttpContext.RequestServices.GetService(typeof(ICacheService)) is not ICacheService cacheService)
        {
            logger?.LogError("Cache service not configured properly.");
            throw new InvalidOperationException("Cache service not configured properly.");
        }

        var cacheKey = GenerateCacheKey(_cacheKeyTemplate, context);

        await next();

        if (!string.IsNullOrEmpty(cacheKey))
        {
            bool removed = await cacheService.RemoveAsync(cacheKey);
            if (!removed)
            {
                logger?.LogWarning("Failed to remove cache for key: {CacheKey}. The key might not exist.", cacheKey);
            }
        }
    }

    /// <summary>
    /// Generates a cache key by replacing placeholders in the template with action parameter values.
    /// </summary>
    /// <param name="template">The cache key template.</param>
    /// <param name="context">The context for the action being executed.</param>
    /// <returns>A string representing the generated cache key.</returns>
    private static string GenerateCacheKey(string template, ActionExecutingContext context)
    {
        var key = template;
        foreach (var parameter in context.ActionArguments)
        {
            key = Regex.Replace(key, $@"\{{{parameter.Key}\}}", parameter.Value?.ToString() ?? string.Empty);
        }
        return key;
    }
}