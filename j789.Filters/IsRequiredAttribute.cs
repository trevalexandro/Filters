using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace j789.Filters
{
    /// <summary>
    /// Adapter pattern for required attribute to validate value types in addition to reference types.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class IsRequiredAttribute : RequiredAttribute
    {
        /// <summary>
        /// Validates property on the model.
        /// </summary>
        /// <param name="value">Property being validated.</param>
        /// <returns>True if the value is valid, false if not.</returns>
        public override bool IsValid(object value)
        {
            var type = value.GetType();

            // If a reference type, null is the default value. Otherwise, value types have a default constructor.
            var defaultValue = type.GetTypeInfo().IsValueType ? Activator.CreateInstance(type) : null;
            return value != defaultValue;
        }
    }
}
