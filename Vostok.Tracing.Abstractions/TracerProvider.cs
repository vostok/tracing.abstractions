using System;
using JetBrains.Annotations;

namespace Vostok.Tracing.Abstractions
{
    /// <summary>
    /// <para><see cref="TracerProvider"/> is a static shared configuration point that allows to decouple configuration of tracing in libraries from calling code.</para>
    /// <para>It is intended to be used primarily by library developers who must not force their users to explicitly provide <see cref="ITracer"/> instances.</para>
    /// <para>It is expected to be configured by a hosting system or just directly in the application entry point.</para>
    /// </summary>
    [PublicAPI]
    public static class TracerProvider
    {
        private static readonly ITracer DefaultInstance = new DevNullTracer();

        private static volatile ITracer instance;

        /// <summary>
        /// Returns <c>true</c> if a global <see cref="ITracer"/> instance has already been configured with <see cref="Configure"/> method. Returns <c>false</c> otherwise.
        /// </summary>
        public static bool IsConfigured => instance != null;

        /// <summary>
        /// <para>Returns the global default instance of <see cref="ITracer"/> if it's been configured.</para>
        /// <para>If nothing has been configured yet, falls back to an instance of <see cref="DevNullTracer"/>.</para>
        /// </summary>
        [NotNull]
        public static ITracer Get() => instance ?? DefaultInstance;

        /// <summary>
        /// <para>Configures the global default <see cref="ITracer"/> with given instance, which will be returned by all subsequent <see cref="Get"/> calls.</para>
        /// <para>By default, this method fails when trying to overwrite a previously configured instance. This behaviour can be changed with <paramref name="canOverwrite"/> parameter.</para>
        /// </summary>
        /// <exception cref="ArgumentNullException">Provided instance was <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">Attempted to overwrite previously configured instance.</exception>
        public static void Configure([NotNull] ITracer tracer, bool canOverwrite = false)
        {
            if (!canOverwrite && instance != null)
                throw new InvalidOperationException($"Can't overwrite existing configured implementation of type '{instance.GetType().Name}'.");

            instance = tracer ?? throw new ArgumentNullException(nameof(tracer));
        }
    }
}