using System;
using System.Collections.Generic;
using System.Linq;

using Aliencube.AzureFunctions.Extensions.OpenApi.Extensions;

using Microsoft.OpenApi.Models;

using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Aliencube.AzureFunctions.Extensions.OpenApi.Visitors
{
    /// <summary>
    /// This represents the type visitor for <see cref="long"/> type enum.
    /// </summary>
    public class Int64EnumTypeVisitor : TypeVisitor
    {
        /// <inheritdoc />
        public override bool IsVisitable(Type type)
        {
            var isVisitable = this.IsVisitable(type, TypeCode.Int64) &&
                              type.IsUnflaggedEnumType() &&
                              !type.HasJsonConverterAttribute<StringEnumConverter>() &&
                              Enum.GetUnderlyingType(type) == typeof(long)
                              ;

            return isVisitable;
        }

        /// <inheritdoc />
        public override void Visit(IAcceptor acceptor, KeyValuePair<string, Type> type, NamingStrategy namingStrategy, params Attribute[] attributes)
        {
            var name = type.Key;

            var instance = acceptor as OpenApiSchemaAcceptor;
            if (instance.IsNullOrDefault())
            {
                return;
            }

            var enums = type.Value.ToOpenApiInt64Collection();

            var schema = new OpenApiSchema()
            {
                Type = "integer",
                Format = "int64",
                Enum = enums,
                Default = enums.First()
            };

            instance.Schemas.Add(name, schema);
        }
    }
}
