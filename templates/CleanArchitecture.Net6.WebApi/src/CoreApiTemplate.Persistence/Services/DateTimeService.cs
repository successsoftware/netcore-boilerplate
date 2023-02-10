using SSS.CommonLib.Interfaces;
using System;

namespace CoreApiTemplate.Persistence.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    }
}
