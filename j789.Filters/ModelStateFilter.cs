using j789.ExceptionHandler.CustomExceptions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace j789.Filters
{
    /// <summary>
    /// Filter used to validate request on all HTTP requests.
    /// </summary>
    public class ModelStateFilter : IAsyncActionFilter
    {
        /// <summary>
        /// Validates the object before executing the desired action.
        /// </summary>
        /// <param name="context">Contains information about the model state, controller, etc.</param>
        /// <param name="next">Executes subsequent filters and the action method.</param>
        /// <returns>Returns a task since this is an asynchronous method.</returns>
        public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // If the IsValid attribute filter is used for the action, don't use this filter.
            if (!context.Filters.Any(filter => filter.GetType() == typeof(IsValidAttribute)))
            {
                if (!context.ModelState.IsValid)
                {
                    // Get members of the model with error messages.
                    var modelStateEntries = context.ModelState.Select(entry => entry.Value);
                    var entriesWithMessages = modelStateEntries.Where(entry => 
                        entry.ValidationState == ModelValidationState.Invalid && 
                        entry.Errors.Any(error => !string.IsNullOrWhiteSpace(error.ErrorMessage)));
                    ModelStateHelper.ThrowException(entriesWithMessages);
                }
            }
            return next();
        }
    }
}
