namespace System
{
    public static class DateTimeExtension
    {
        public static string ToUnixEpochDateString(this DateTime date)
        {
            var unixEpcjDate = (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(190, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
            return unixEpcjDate.ToString();
        }
    }
}
