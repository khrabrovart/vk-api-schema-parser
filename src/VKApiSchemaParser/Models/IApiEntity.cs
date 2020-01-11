namespace VKApiSchemaParser.Models
{
    /// <summary>
    /// Represents an API entity.
    /// </summary>
    public interface IApiEntity
    {
        /// <summary>
        /// Gets API entity's name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets API entity's description.
        /// </summary>
        string Description { get; }
    }
}
