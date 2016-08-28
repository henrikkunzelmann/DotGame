namespace DotGame.Graphics
{
    /// <summary>
    /// Gibt an wie ein Buffer benutzt wird.
    /// </summary>
    public enum ResourceUsage
    {
        /// <summary>
        /// Am besten geeignet für Resourcen, die sich nie ändern
        /// </summary>
        Immutable,

        /// <summary>
        /// Am besten geeignet für Resourcen, die sich nicht oder nicht oft ändern. 
        /// </summary>
        Normal,

        /// <summary>
        /// Am besten geeignet für Resourcen von denen sich Teile oft ändern
        /// </summary>
        Staging,

        /// <summary>
        /// Am besten geeignet für Resourcen, die sich komplett oft ändern.
        /// </summary>
        Dynamic
    }
}
