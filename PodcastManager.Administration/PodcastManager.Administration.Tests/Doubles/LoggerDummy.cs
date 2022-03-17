using Serilog;
using Serilog.Events;

namespace PodcastManager.Administration.Doubles;

public class LoggerDummy : ILogger
{
    public void Write(LogEvent logEvent)
    {
    }
}