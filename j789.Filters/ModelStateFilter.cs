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
            if (!context.ModelState.IsValid)
            {
                // Get error messages specified in the model.
                var errorMessages = context.ModelState.Where(arg => 
                    arg.Value.ValidationState == ModelValidationState.Invalid && arg.Value.Errors.Any());
                var stringBuilder = new StringBuilder("Bad request due to invalid request parameter(s).");

                // Append custom error messages.
                string finalErrorMessage = errorMessages.Aggregate(stringBuilder, (sb, errorMsg) => 
                    sb.Append($" {errorMsg}"), sb => sb.ToString());
                throw new BusinessLogicException(finalErrorMessage);
            }
            return next();
        }
    }
}
