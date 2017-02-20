using System.Collections.Generic;
using System.IO;
using System.Linq;
using Google.Apis.AnalyticsReporting.v4;
using Google.Apis.AnalyticsReporting.v4.Data;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;

namespace GoogleAnalyticsReportingAPIWrapper
{
    public class Report
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="StartDate">string date format "yyyy-MM-dd"</param>
        /// <param name="EndDate">string date format "yyyy-MM-dd"</param>
        /// <param name="expressionOfMetric">metric. example: "ga:hits"</param>
        /// <param name="nameOfDimension">dimension. example: ga:dimension1</param>
        /// <param name="viewId">view ID. example: "ga:113466633"</param>
        /// <param name="jsonFile">Full Path of file</param>
        /// <param name="scopes">scopes for authentication</param>
        /// <param name="applicationName">Your application name</param>
        /// <returns></returns>
        public GetReportsResponse Get(string StartDate, string EndDate, string[] expressionOfMetric, string[] nameOfDimension, string viewId, string jsonFile, string[] scopes, string applicationName)
        {
            DateRange dateRange = new DateRange()
            {
                StartDate = StartDate,
                EndDate = EndDate
            };

            List<Metric> listOfMetric = new List<Metric>();

            if (expressionOfMetric.Length > 0)
            {
                listOfMetric.AddRange(expressionOfMetric.Select(exp => new Metric() { Expression = exp }));
            }

            List<Dimension> listOfDimension = new List<Dimension>();

            if (nameOfDimension.Length > 0)
            {
                listOfDimension.AddRange(expressionOfMetric.Select(exp => new Dimension() { Name = exp }));
            }

            ReportRequest reportRequest = new ReportRequest
            {
                ViewId = viewId,
                DateRanges = new List<DateRange>() { dateRange },
                Dimensions =listOfDimension ,
                Metrics = listOfMetric,
            };

            List<ReportRequest> requests = new List<ReportRequest> {reportRequest};

            GoogleCredential credential;

            using (Stream stream = new FileStream(jsonFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                credential = GoogleCredential.FromStream(stream);
            }

            credential = credential.CreateScoped(scopes);

            GetReportsRequest getReport = new GetReportsRequest() { ReportRequests = requests };

            AnalyticsReportingService analyticsreporting = new AnalyticsReportingService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = applicationName,
            });

            GetReportsResponse response = analyticsreporting.Reports.BatchGet(getReport).Execute();

            return response;
        }
    }
}
