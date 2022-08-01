using System;
using System.Globalization;

namespace Achievers.Models
{
    public class UserInfoModel
    {
        public int AppId { get; set; }
        public string User_Id { get; set; }
        public string Device_Id { get; set; }
        public string Mode { get; set; }
        public bool ShowReview { get; set; }
        public string StartTime { get; set; }
        public string ExprieTime { get; set; }

        public DateTime StartDate
        {
            get
            {
                DateTime.TryParseExact(StartTime, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date);
                return date;
            }
        }

        public DateTime ExpiredDate
        {
            get
            {
                DateTime.TryParseExact(ExprieTime, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date);
                return date;
            }
        }
    }
}