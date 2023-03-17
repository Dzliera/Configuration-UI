using System.Text.Json.Serialization;
using ConfigurationUi.Shared.ConfigurationSchema.SchemaTokenTypes;

namespace ConfigurationUi.Shared.ConfigurationSchema;


[JsonDerivedType(typeof(ArraySchemaToken), nameof(SchemaTokenType.Array))]
[JsonDerivedType(typeof(BoolSchemaToken), nameof(SchemaTokenType.Bool))]
[JsonDerivedType(typeof(DecimalSchemaToken), nameof(SchemaTokenType.Decimal))]
[JsonDerivedType(typeof(IntegerSchemaToken), nameof(SchemaTokenType.Integer))]
[JsonDerivedType(typeof(ObjectSchemaToken), nameof(SchemaTokenType.Object))]
[JsonDerivedType(typeof(StringSchemaToken), nameof(SchemaTokenType.String))]
public record SchemaTokenBase
{
}