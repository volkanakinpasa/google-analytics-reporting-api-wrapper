# google-analytics-reporting-api-wrapper

![](https://ci.appveyor.com/api/projects/status/4r4midm4t20hs7ja?svg=true)

It covers Google Analytics Reporting API v4 by sending custom variables

## Installation
#### **Nuget**
      Install-Package GoogleAnalyticsReportingAPIWrapper
#### **Google Analytics**
1. Create a Service Account
https://console.developers.google.com/projectselector/iam-admin/serviceaccounts
Select Role: Project / Viewer
Check: Furnish a new private key / JSON

2. Attach this file with the (secret) private key to your project.

3. Enable Google Analytics Reporting API 
https://console.developers.google.com/apis/api/analyticsreporting.googleapis.com/overview

4. The service account will have an email. Add that email to your Google Analytics users, preferably only on the specific view you're interested in. (Administration / View / User Management)
Read more: https://support.google.com/analytics/answer/1009702?hl=en


## Usage

#### **Javascript, send example data**

```javascript
ga('send', 'event', [eventCategory], [eventAction], [eventLabel], [eventValue], [fieldsObject]);
```
```javascript
ga('send', {
  hitType: 'event',
  eventCategory: 'Product',
  eventAction: 'click',
  eventLabel: 'ProductId'
});
``` 
for more info how to send events to Google Analytics click [here](https://developers.google.com/analytics/devguides/collection/analyticsjs/events/) 

to find which dimensions & metrics you need to pass as parameter, please click [here](https://developers.google.com/analytics/devguides/reporting/core/dimsmets#q=event&cats=user,session,traffic_sources,adwords,goal_conversions,platform_or_device,geo_network,system,social_activities,page_tracking,content_grouping,internal_search,site_speed,app_tracking,event_tracking,ecommerce,social_interactions,user_timings,exceptions,content_experiments,custom_variables_or_columns,time,doubleclick_campaign_manager,audience,adsense,ad_exchange,doubleclick_for_publishers,doubleclick_for_publishers_backfill,lifetime_value_and_cohorts,channel_grouping,related_products,doubleclick_bid_manager,doubleclick_search)


#### **C# get Analytics data**


```C#
using System;
using System.Linq;
using Google.Apis.AnalyticsReporting.v4.Data;
using Report = GoogleAnalyticsReportingAPIWrapper.Report;

namespace WrapperConsole
{
	class Program
	{
		static void Main(string[] args)
		{

			Report reportWrapper = new Report();

			string start = DateTime.UtcNow.AddDays(-7).ToString("yyyy-MM-dd");
			string stop = DateTime.UtcNow.ToString("yyyy-MM-dd");
			var metrics = new[] { "ga:users" };
			var dimensions = new[] { "ga:date" };
			string viewId = "123456";
			string jsonFile = "serivce-account-secret.json";
			var scopes = new[] { "https://www.googleapis.com/auth/analytics.readonly" };
			string applicationName = "google-analytics-reporting-api-wrapper";

			GetReportsResponse response = reportWrapper.Get(start, stop, metrics, dimensions, viewId, jsonFile, scopes, applicationName);

			foreach (Google.Apis.AnalyticsReporting.v4.Data.Report report in response.Reports) {
				Console.WriteLine("{0,8} {1,8}", dimensions[0], metrics[0]);
				report.Data.Rows.ToList().ForEach(row => Console.WriteLine("{0,8} {1,8}", row.Dimensions.First(), row.Metrics.First().Values.First()));
			}

			Console.ReadLine();

		}
	}
}
```
