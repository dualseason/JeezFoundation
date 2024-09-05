using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LoggerComponent
{
    internal static class LoggerFormat
    {
        // 日志记录方法，包括调用者信息
        public static void LogTrace(ILogger logger, string message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            logger.LogTrace($"[{memberName} in {filePath}:{lineNumber}] {message}");
        }

        public static void LogDebug(ILogger logger, string message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            logger.LogDebug($"[{memberName} in {filePath}:{lineNumber}] {message}");
        }

        public static void LogInformation(ILogger logger, string message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            logger.LogInformation($"[{memberName} in {filePath}:{lineNumber}] {message}");
        }

        public static void LogWarning(ILogger logger, string message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            logger.LogWarning($"[{memberName} in {filePath}:{lineNumber}] {message}");
        }

        public static void LogError(ILogger logger, string message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            logger.LogError($"[{memberName} in {filePath}:{lineNumber}] {message}");
        }

        public static void LogCritical(ILogger logger, string message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            logger.LogCritical($"[{memberName} in {filePath}:{lineNumber}] {message}");
        }
    }
}
