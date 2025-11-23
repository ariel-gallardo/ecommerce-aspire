namespace Common.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime ToDate(this string dateTimeString)
        => DateTime.ParseExact(dateTimeString, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
    }
}
