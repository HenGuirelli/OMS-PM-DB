using Serilog;

namespace OMS
{
    public class ExecutionReportCounter
    {
        private readonly System.Timers.Timer _timer = new();
        private volatile uint _executionReportCount = 0;
        private volatile uint _executionReportTotalCount = 0;

        public ExecutionReportCounter()
        {
            _timer.Elapsed += (o, e) =>
            {
                Log.Information(
                    "ER count (p/ sec): {executionReportCount}\t| Total: {executionReportTotalCount}",
                    _executionReportCount, _executionReportTotalCount);
                //Console.WriteLine("ER count (p/ sec): " + _executionReportCount + "\t| Total: " + _executionReportTotalCount);
                _executionReportCount = 0;
            };
            _timer.AutoReset = true;
            _timer.Interval = 1000;
        }

        public void AddEr()
        {
            if (_timer.Enabled == false)
            {
                _timer.Enabled = true;
            }
            _executionReportCount++;
            _executionReportTotalCount++;
        }
    }
}
