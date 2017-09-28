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
        /// <param name="errorsForEntries">Collection of errors for each invalid property from the model.</param>
        internal static void ThrowException(IEnumerable<ModelErrorCollection> errorsForEntries)
        {
            var stringBuilder = new StringBuilder("Bad request due to invalid request parameter(s).");

            // Append error messages.
            errorsForEntries.Select(entryErrors => 
                (stringBuilder = entryErrors.Aggregate(stringBuilder, (sb, error) => 
                sb.Append($" {error.ErrorMessage}"))));
            throw new BusinessLogicException(stringBuilder.ToString());
        }
    }
}
