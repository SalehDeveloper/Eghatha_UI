using BlazorApp2.Models;

namespace BlazorApp2.Extensions
{
    public static class ProblemDetailsExtensions
    {
        private const string DefaultErrorMessage = "An unknown error occurred.";

        public static string ToError(this ProblemDetails? source)
        {
            return source switch
            {
                null => DefaultErrorMessage,
                _ => source.Title!
            };
        }
    }
}
