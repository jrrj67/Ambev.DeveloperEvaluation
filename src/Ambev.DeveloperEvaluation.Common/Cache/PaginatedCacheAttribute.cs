using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Ambev.DeveloperEvaluation.Common.Cache;

/// <summary>
/// An attribute that provides caching functionality for paginated responses in ASP.NET Core actions.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class PaginatedCacheAttribute : Attribute, IAsyncActionFilter
{
    private readonly string _cacheKeyTemplate;

    /// <summary>
    /// Gets or sets the duration (in minutes) for which the response should be cached.
    /// Default value is 60 minutes.
    /// </summary>
    public int DurationInMinutes { get; set; } = 60;

    /// <summary>
    /// Initializes a new instance of the <see cref="PaginatedCacheAttribute"/> class.
    /// </summary>
    /// <param name="cacheKeyTemplate">
    /// A template for generating the cache key. Use placeholders (e.g., {parameterName}) to include action parameters in the key.
    /// </param>
    public PaginatedCacheAttribute(string cacheKeyTemplate)
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
        var cacheService = context.HttpContext.RequestServices.GetService(typeof(ICacheService)) as ICacheService;
        if (cacheService == null) throw new InvalidOperationException("Cache service not configured properly.");

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
            var serializedValue = JsonSerializer.Serialize(objectResult.Value, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            });

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
            if (parameter.Value == null)
            {
                key = key.Replace($"{{{parameter.Key}}}", "null");
                continue;
            }

            if (IsSimpleType(parameter.Value.GetType()))
            {
                key = key.Replace($"{{{parameter.Key}}}", parameter.Value.ToString() ?? "null");
            }
            else
            {
                // Generates a detailed representation of the complex object in the cache key
                var complexKey = GenerateComplexObjectKey(parameter.Value);
                key = key.Replace($"{{{parameter.Key}}}", string.IsNullOrEmpty(complexKey) ? "null" : complexKey);
            }
        }

        // Safely removes unfilled placeholders, avoiding unnecessary underscores
        key = Regex.Replace(key, @"\{.*?\}", "null");
        key = Regex.Replace(key, "_null", ""); // Removes unnecessary "_null" from the end

        return key;
    }

    /// <summary>
    /// Determines if a type is a simple type (e.g., primitive, string, or decimal).
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <returns>True if the type is simple; otherwise, false.</returns>
    private static bool IsSimpleType(Type type)
    {
        return type.IsPrimitive || type.IsEnum || type.Equals(typeof(string)) || type.Equals(typeof(decimal));
    }

    /// <summary>
    /// Generates a cache key representation for a complex object by concatenating its readable property names and values.
    /// </summary>
    /// <param name="complexObject">The complex object to generate the key for.</param>
    /// <returns>A string representing the complex object in the cache key.</returns>
    private static string GenerateComplexObjectKey(object complexObject)
    {
        var properties = complexObject.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        var keyValuePairs = properties
            .Where(p => p.CanRead && p.GetValue(complexObject) != null)
            .Select(p => $"{p.Name}={p.GetValue(complexObject)?.ToString()}")
            .ToList();

        return string.Join("_", keyValuePairs);
    }
}