namespace VKApiSchemaParser.Models
{
    public enum ApiAccessTokenType
    {
        /// <summary>
        /// User token.
        /// </summary>
        User,

        /// <summary>
        /// Open token.
        /// </summary>
        Open,

        /// <summary>
        /// Service token.
        /// </summary>
        Service,

        /// <summary>
        /// Community token.
        /// </summary>
        Group,

        /// <summary>
        /// Undefined token type.
        /// </summary>
        Undefined
    }
}
