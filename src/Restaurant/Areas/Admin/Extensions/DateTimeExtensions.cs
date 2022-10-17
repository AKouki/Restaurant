namespace Restaurant.Areas.Admin.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToRelativeString(this DateTime dateTime, string defaultFormat = "dd/MM/yyyy hh:mm tt")
        {
            if (dateTime.Date == DateTime.Today)
                return $"Today, {dateTime.ToString("hh:mm tt")}";

            if (dateTime.Date == DateTime.Today.AddDays(1))
                return $"Tommorow, {dateTime.ToString("hh:mm tt")}";

            return dateTime.ToString(defaultFormat);
        }
    }
}
