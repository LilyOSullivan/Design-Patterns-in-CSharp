using System;
using Autofac;

namespace DecoratorInDependencyInjection
{
    public interface IReportingService
    {
        void Report();
    }

    public class ReportingService : IReportingService
    {
        public void Report()
        {
            Console.WriteLine("Here is your report");
        }
    }

    public class ReportingServiceWithLogging : IReportingService
    {
        private IReportingService _reportingService;

        public ReportingServiceWithLogging(IReportingService reportingService)
        {
            _reportingService = reportingService;
        }

        public void Report()
        {
            Console.WriteLine("Begin Logging..");
            _reportingService.Report();
            Console.WriteLine("Finish Logging..");

        }
    }

    class Program
    {
        static void Main()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<ReportingService>().Named<IReportingService>("reporting");
            containerBuilder.RegisterDecorator<IReportingService>(
                (context, service) => new ReportingServiceWithLogging(service),
                "reporting"
            );

            using (var container = containerBuilder.Build())
            {
                var reportingService = container.Resolve<IReportingService>();
                reportingService.Report();
            }
        }
    }
}
