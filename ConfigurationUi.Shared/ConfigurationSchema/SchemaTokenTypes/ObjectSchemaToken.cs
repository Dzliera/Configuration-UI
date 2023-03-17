namespace ConfigurationUi.Shared.ConfigurationSchema.SchemaTokenTypes;

public record ObjectSchemaToken(Dictionary<string, ObjectPropertySchema> PropertySchemas) : SchemaTokenBase;

public record ObjectPropertySchema(string Name, bool IsRequired, SchemaTokenBase ValueSchema);