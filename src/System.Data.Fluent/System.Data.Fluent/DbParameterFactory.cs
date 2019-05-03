using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Data.Fluent
{
    public abstract class DbParameterFactory : IDbParameterFactory
    {
        public virtual DbParameter CreateCursorParameter(string name) => throw new NotSupportedException();

        public virtual DbParameter CreateInputOutputParameter(string name, object value)
        {
            Check.IsNull(name, nameof(name));

            var parameter = CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value;
            parameter.Direction = ParameterDirection.InputOutput;
            return parameter;
        }

        public virtual DbParameter CreateInputParameter(string name, object value)
        {
            Check.IsNull(name, nameof(name));

            var parameter = CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value;
            parameter.Direction = ParameterDirection.Input;
            return parameter;
        }

        public virtual DbParameter CreateOutputParameter(string name, Type type)
        {
            Check.IsNull(name, nameof(name));
            Check.IsNull(type, nameof(type));

            var parameter = CreateParameter();
            parameter.ParameterName = name;
            parameter.DbType = GetDbTypeFromType(type);
            parameter.Direction = ParameterDirection.Output;
            return parameter;
        }

        protected abstract DbParameter CreateParameter();

        protected virtual DbType GetDbTypeFromType(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                return GetDbTypeFromType(Nullable.GetUnderlyingType(type));
            }

            if (Enum.TryParse<DbType>(type.Name, out DbType dbType))
            {
                return dbType;
            }
            else if (typeToDbTypeMap.ContainsKey(type))
            {
                return typeToDbTypeMap[type];
            }
            else
            {
                return DbType.Object;
            }
        }

        private IDictionary<Type, DbType> typeToDbTypeMap = new Dictionary<Type, DbType>
        {
            { typeof(Char), DbType.String },
            { typeof(Byte[]), DbType.Binary }
        };
    }
}
