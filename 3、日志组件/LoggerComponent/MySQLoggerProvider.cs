using Microsoft.Extensions.Logging;

namespace LoggerComponent
{
    internal class MySQLoggerProvider : ILoggerProvider
    {
        private string _ConnectionString {  get; set; }

        public MySQLoggerProvider(string connectionString)
        {
            this._ConnectionString = connectionString;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new MySQLogger(_ConnectionString);
        }

        public void Dispose()
        {
            
        }
    }
}
