using JetBrains.Annotations;

namespace Vostok.Tracing.Abstractions
{
    [PublicAPI]
    public static class WellKnownStatuses
    {
        /// <summary>
        /// Operation finished successfully.
        /// </summary>
        public const string Success = "success";

        /// <summary>
        /// Operation finished with some warning.
        /// </summary>
        public const string Warning = "warning";

        /// <summary>
        /// Operation finished with some error.
        /// </summary>
        public const string Error = "error";
    }
}