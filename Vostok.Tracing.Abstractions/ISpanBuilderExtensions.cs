using JetBrains.Annotations;
using Vostok.Commons.Formatting;

namespace Vostok.Tracing.Abstractions
{
    [PublicAPI]
    public static class ISpanBuilderExtensions
    {
        public static void SetAnnotation(
            [NotNull] this ISpanBuilder builder,
            [NotNull] string key,
            [CanBeNull] object value,
            bool allowOverwrite = true)
        {
            builder.SetAnnotation(key, FormatValue(value), allowOverwrite);
        }

        [CanBeNull]
        private static string FormatValue([CanBeNull] object value)
        {
            if (value == null)
                return null;

            return ObjectValueFormatter.Format(value);
        }
    }
}