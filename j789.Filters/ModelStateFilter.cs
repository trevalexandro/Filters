﻿using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;
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
                    // Get members of the model that aren't valid.
                    var invalidEntries = context.ModelState.Where(entry => 
                        entry.Value.ValidationState == ModelValidationState.Invalid);
                    ModelStateHelper.ThrowException(invalidEntries.Select(entry => entry.Value.Errors));
                }
            }
            return next();
        }
    }
}
