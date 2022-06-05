

namespace CookiesAuthenticationPOC.Infrastructure.Extensions
{
    /// <summary>
    /// ConvertExtensions contains extensions for data convertion for premitive types.
    /// </summary>
    public static class ConvertExtensions
    {

        /// <summary>
        /// GetStringValue Converts the sourceValue to string.
        /// </summary>
        /// <param name="sourceValue"></param>
        /// <returns></returns>
        public static string GetStringValue(this object sourceValue)
        {
            var value = string.Empty;

            if (sourceValue != null)
            {
                if (sourceValue is string)
                {
                    value = (string)sourceValue;
                }
                else
                {
                    value = Convert.ToString(sourceValue);
                }
            }

            return value;
        }



    }
}
