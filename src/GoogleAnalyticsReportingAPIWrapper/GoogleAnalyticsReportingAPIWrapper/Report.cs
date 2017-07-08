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
        /// Gets Google Analytics Reports by sending custom variables
        /// </summary>
        /// <param name="startDate">report start date. format "yyyy-MM-dd"</param>
        /// <param name="endDate">report start date. format "yyyy-MM-dd"</param>
        /// <param name="metrics">metrics. example: "ga:hits, ga:eventValue etc."</param>
        /// <param name="dimensions">dimensions. example: "ga:dimension1," for events "ga:eventCategory, ga:eventAction, ga:eventLabel"</param>
        /// <param name="viewId">view id of your property. example: "ga:113466633"</param>
        /// <param name="jsonFile">Full Path of file</param>
        /// <param name="scopes">scopes for authentication</param>
        /// <param name="applicationName">Your application name</param>
        /// <returns></returns>
        public GetReportsResponse Get(string startDate, string endDate, string[] metrics, string[] dimensions,
            string viewId, string jsonFile, string[] scopes, string applicationName)
        {
            DateRange dateRange = new DateRange()
                                  {
                                      StartDate = startDate,
                                      EndDate = endDate
                                  };

            List<Metric> listOfMetric = new List<Metric>();

            if (metrics.Length > 0)
            {
                listOfMetric.AddRange(metrics.Select(exp => new Metric() {Expression = exp}));
            }

            List<Dimension> listOfDimension = new List<Dimension>();

            if (dimensions.Length > 0)
            {
                listOfDimension.AddRange(dimensions.Select(exp => new Dimension() {Name = exp}));
            }

            ReportRequest reportRequest = new ReportRequest
                                          {
                                              ViewId = viewId,
                                              DateRanges = new List<DateRange>() {dateRange},
                                              Dimensions = listOfDimension,
                                              Metrics = listOfMetric,
                                          };

            List<ReportRequest> requests = new List<ReportRequest> {reportRequest};

            GoogleCredential credential;

            using (Stream stream = new FileStream(jsonFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                credential = GoogleCredential.FromStream(stream);
            }

            credential = credential.CreateScoped(scopes);

            GetReportsRequest getReport = new GetReportsRequest() {ReportRequests = requests};

            AnalyticsReportingService analyticsreporting =
                new AnalyticsReportingService(new BaseClientService.Initializer()
                                              {
                                                  HttpClientInitializer = credential,
                                                  ApplicationName = applicationName,
                                              });

            GetReportsResponse response = analyticsreporting.Reports.BatchGet(getReport).Execute();

            return response;
        }
    }
}
