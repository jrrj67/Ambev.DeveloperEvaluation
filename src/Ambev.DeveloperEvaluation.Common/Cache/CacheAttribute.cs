using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Ambev.DeveloperEvaluation.Common.Cache;

/// <summary>
/// An attribute that provides caching functionality for ASP.NET Core actions.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class CacheAttribute : Attribute, IAsyncActionFilter
{
    private readonly string _cacheKeyTemplate;
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    };

    /// <summary>
    /// Gets or sets the duration (in minutes) for which the response should be cached.
    /// Default value is 60 minutes.
    /// </summary>
    public int DurationInMinutes { get; set; } = 60;

    /// <summary>
    /// Initializes a new instance of the <see cref="CacheAttribute"/> class.
    /// </summary>
    /// <param name="cacheKeyTemplate">
    /// A template for generating the cache key. Use placeholders (e.g., {parameterName}) to include action parameters in the key.
    /// </param>
    public CacheAttribute(string cacheKeyTemplate)
    {
        _cacheKeyTemplate = cacheKeyTemplate;
    }

    /// <summary>
    /// Executes the caching logic before and after the action is executed.
    /// </summary>
    /// <param name="context">The context for the action being executed.</param>
    /// <param name="next">The delegate to execute the next action filter or the action itself.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the cache service is not properly configured.</exception>
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        ICacheService? cacheService = context.HttpContext.RequestServices.GetService(typeof(ICacheService)) as ICacheService
            ?? throw new InvalidOperationException("Cache service not configured properly.");

        var cacheKey = GenerateCacheKey(_cacheKeyTemplate, context);

        var cachedValue = await cacheService.GetAsync<string>(cacheKey);
        if (!string.IsNullOrEmpty(cachedValue))
        {
            var contentResult = new ContentResult
            {
                Content = cachedValue,
                ContentType = "application/json",
                StatusCode = 200
            };
            context.Result = contentResult;
            return;
        }

        var executedContext = await next();

        if (executedContext.Result is ObjectResult objectResult && objectResult.StatusCode == 200)
        {
            var serializedValue = JsonSerializer.Serialize(objectResult.Value, _jsonSerializerOptions);

            await cacheService.SetAsync(cacheKey, serializedValue, TimeSpan.FromMinutes(DurationInMinutes));
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