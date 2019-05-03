using System.Data.Fluent.Abstraction;
using System.Globalization;

namespace System.Data.Fluent.Impl
{
    internal sealed class DefaultDbValueProvider : IDbValueConverter
    {
        public object ConvertDbValue(object value, Type type)
        {
            Check.IsNull(type, nameof(type));

            if (value == null || Convert.IsDBNull(value))
            {
                return null;
            }

            var valueType = value.GetType();

            if(type.IsAssignableFrom(valueType))
            {
                return value;
            }

            if(type.IsEnum)
            {
                return ConvertDbValue(value, Enum.GetUnderlyingType(type));
            }

            if(type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return ConvertDbValue(value, Nullable.GetUnderlyingType(type));
            }

            if(value is IConvertible)
            {
                return Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
            }

            throw new InvalidCastException($"Can not convert {valueType.Name} to {type.Name}");
        }

        public T ConvertDbValue<T>(object value)
        {
            return (T)ConvertDbValue(value, typeof(T));
        }
    }
}
