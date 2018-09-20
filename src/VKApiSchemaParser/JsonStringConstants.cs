namespace VKApiSchemaParser
{
    internal static class JsonStringConstants
    {
        public const string AccessTokenType = "access_token_type";
        public const string AdditionalProperties = "additionalProperties";
        public const string AllOf = "allOf";
        public const string Code = "code";
        public const string Default = "default";
        public const string Definitions = "definitions";
        public const string Description = "description";
        public const string Enum = "enum";
        public const string EnumNames = "enumNames";
        public const string Errors = "errors";
        public const string Items = "items";
        public const string MaxItems = "maxItems";
        public const string MaxLength = "maxLength";
        public const string MaxProperties = "maxProperties";
        public const string Maximum = "maximum";
        public const string Methods = "methods";
        public const string MinLength = "minLength";
        public const string MinProperties = "minProperties";
        public const string Minimum = "minimum";
        public const string Name = "name";
        public const string OneOf = "oneOf";
        public const string Parameters = "parameters";
        public const string PatternProperties = "patternProperties";
        public const string Properties = "properties";
        public const string Reference = "$ref";
        public const string Required = "required";
        public const string Response = "response";
        public const string Responses = "responses";
        public const string Type = "type";

        #region Types

        public const string Array = "array";
        public const string Boolean = "boolean";
        public const string Integer = "integer";
        public const string Number = "number";
        public const string Object = "object";
        public const string String = "string";

        // For [integer, string] types
        public const string Multiple = "multiple";

        #endregion

        #region AccessToken Types

        public const string Group = "group";
        public const string Open = "open";
        public const string Service = "service";
        public const string User = "user";

        #endregion
    }
}
