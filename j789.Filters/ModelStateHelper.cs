using j789.ExceptionHandler.CustomExceptions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace j789.Filters
{
    /// <summary>
    /// Helper class for model state validation.
    /// </summary>
    internal static class ModelStateHelper
    {
        /// <summary>
        /// Throws a business logic exception when the model state isn't valid.
        /// </summary>
        /// <param name="invalidEntries">All the invalid members of the model with an error message. 
        /// If there are none, the default message will be returned.</param>
        internal static void ThrowException(IEnumerable<ModelStateEntry> invalidEntries)
        {
            string finalErrorMessage = "Bad request due to invalid request parameter(s).";
            if (invalidEntries.Any())
            {
                var stringBuilder = new StringBuilder(finalErrorMessage);

                // Append custom error messages.
                invalidEntries.Aggregate(stringBuilder, (sb, entry) => 
                    sb.Append(entry.Errors.Select(error => 
                    $" {error.ErrorMessage}")), sb => finalErrorMessage = sb.ToString());
            }
            throw new BusinessLogicException(finalErrorMessage);
        }
    }
}
