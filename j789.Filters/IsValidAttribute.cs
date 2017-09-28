using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace j789.Filters
{
    /// <summary>
    /// Attribute class for action methods that allow only certain properties to be considered 
    /// when validating a model.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class IsValidAttribute : ActionFilterAttribute
    {
        private readonly IEnumerable<string> _properties;

        public IsValidAttribute(params string[] properties)
        {
            // These are the properties on the model to be considered for the executing action.
            _properties = properties;
        }

        /// <summary>
        /// Validates the object before executing the desired action.
        /// </summary>
        /// <param name="context">Contains information about the model state, controller, etc.</param>
        /// <param name="next">Executes subsequent filters and the action method.</param>
        /// <returns>Returns a task since this is an asynchronous method.</returns>
        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Disregard string values that don't equate to a property name on the model.
            var propertiesToCheck = context.ModelState.Where(entry => _properties.Contains(entry.Key));
            var modelStateEntries = propertiesToCheck.Where(modelState =>
                modelState.Value.ValidationState == ModelValidationState.Invalid);
            if (modelStateEntries.Any())
            {
                ModelStateHelper.ThrowException(modelStateEntries.Select(entry => entry.Value.Errors));
            }
            return next();
        }
    }
}
