using Microsoft.Extensions.Logging;

namespace LoggerComponent
{
    internal static class MySQLLoggerExtensions
    {
        public static ILoggingBuilder AddMySQL(this ILoggingBuilder builder, string connectString)
        {
            builder.AddProvider(new MySQLoggerProvider(connectString));
            return builder;
        } 
    }
}
