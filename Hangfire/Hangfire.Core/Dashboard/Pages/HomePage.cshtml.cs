#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Hangfire.Dashboard.Pages
{
    
    #line 2 "..\..\Dashboard\Pages\HomePage.cshtml"
    using System;
    
    #line default
    #line hidden
    
    #line 3 "..\..\Dashboard\Pages\HomePage.cshtml"
    using System.Collections.Generic;
    
    #line default
    #line hidden
    using System.Linq;
    using System.Text;
    
    #line 4 "..\..\Dashboard\Pages\HomePage.cshtml"
    using Hangfire.Dashboard;
    
    #line default
    #line hidden
    
    #line 5 "..\..\Dashboard\Pages\HomePage.cshtml"
    using Hangfire.Dashboard.Pages;
    
    #line default
    #line hidden
    
    #line 6 "..\..\Dashboard\Pages\HomePage.cshtml"
    using Hangfire.Dashboard.Resources;
    
    #line default
    #line hidden
    
    #line 7 "..\..\Dashboard\Pages\HomePage.cshtml"
    using Newtonsoft.Json;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    internal partial class HomePage : RazorPage
    {
#line hidden

        public override void Execute()
        {


WriteLiteral("\r\n");









            
            #line 9 "..\..\Dashboard\Pages\HomePage.cshtml"
  
    Layout = new LayoutPage(Strings.HomePage_Title);
    IDictionary<DateTime, long> succeeded = null;
    IDictionary<DateTime, long> failed = null;

    var period = Query("period") ?? "day";

    var monitor = Storage.GetMonitoringApi();
    if ("week".Equals(period, StringComparison.OrdinalIgnoreCase))
    {
        succeeded = monitor.SucceededByDatesCount();
        failed = monitor.FailedByDatesCount();
    }
    else if ("day".Equals(period, StringComparison.OrdinalIgnoreCase))
    {
        succeeded = monitor.HourlySucceededJobs();
        failed = monitor.HourlyFailedJobs();
    }


            
            #line default
            #line hidden
WriteLiteral("\r\n<div class=\"row\">\r\n    <div class=\"col-md-12\">\r\n        <h1 class=\"page-header\"" +
">");


            
            #line 31 "..\..\Dashboard\Pages\HomePage.cshtml"
                           Write(Strings.HomePage_Title);

            
            #line default
            #line hidden
WriteLiteral("</h1>\r\n");


            
            #line 32 "..\..\Dashboard\Pages\HomePage.cshtml"
         if (Metrics.Count > 0)
        {

            
            #line default
            #line hidden
WriteLiteral("            <div class=\"row\">\r\n");


            
            #line 35 "..\..\Dashboard\Pages\HomePage.cshtml"
                 foreach (var metric in Metrics)
                {

            
            #line default
            #line hidden
WriteLiteral("                    <div class=\"col-md-2\">\r\n                        ");


            
            #line 38 "..\..\Dashboard\Pages\HomePage.cshtml"
                   Write(Html.BlockMetric(metric));

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </div>\r\n");


            
            #line 40 "..\..\Dashboard\Pages\HomePage.cshtml"
                }

            
            #line default
            #line hidden
WriteLiteral("            </div>\r\n");


            
            #line 42 "..\..\Dashboard\Pages\HomePage.cshtml"
        }

            
            #line default
            #line hidden
WriteLiteral("        <h3>");


            
            #line 43 "..\..\Dashboard\Pages\HomePage.cshtml"
       Write(Strings.HomePage_RealtimeGraph);

            
            #line default
            #line hidden
WriteLiteral("</h3>\r\n        <canvas width=\"1140\" height=\"250\" id=\"realtimeGraph\" data-succeede" +
"d=\"");


            
            #line 44 "..\..\Dashboard\Pages\HomePage.cshtml"
                                                                        Write(Statistics.Succeeded);

            
            #line default
            #line hidden
WriteLiteral("\" data-failed=\"");


            
            #line 44 "..\..\Dashboard\Pages\HomePage.cshtml"
                                                                                                            Write(Statistics.Failed);

            
            #line default
            #line hidden
WriteLiteral("\"\r\n             data-succeeded-string=\"");


            
            #line 45 "..\..\Dashboard\Pages\HomePage.cshtml"
                               Write(Strings.HomePage_GraphHover_Succeeded);

            
            #line default
            #line hidden
WriteLiteral("\"\r\n             data-failed-string=\"");


            
            #line 46 "..\..\Dashboard\Pages\HomePage.cshtml"
                            Write(Strings.HomePage_GraphHover_Failed);

            
            #line default
            #line hidden
WriteLiteral(@"""></canvas>
        <div style=""display: none;"">
            <span data-metric=""succeeded:count""></span>
            <span data-metric=""failed:count""></span>
        </div>

        <h3>
            <div class=""btn-group pull-right"" style=""margin-top: 2px;"">
                <a href=""?period=day"" class=""btn btn-sm btn-default ");


            
            #line 54 "..\..\Dashboard\Pages\HomePage.cshtml"
                                                                Write("day".Equals(period, StringComparison.OrdinalIgnoreCase) ? "active" : null);

            
            #line default
            #line hidden
WriteLiteral("\">");


            
            #line 54 "..\..\Dashboard\Pages\HomePage.cshtml"
                                                                                                                                              Write(Strings.Common_PeriodDay);

            
            #line default
            #line hidden
WriteLiteral("</a>\r\n                <a href=\"?period=week\" class=\"btn btn-sm btn-default ");


            
            #line 55 "..\..\Dashboard\Pages\HomePage.cshtml"
                                                                 Write("week".Equals(period, StringComparison.OrdinalIgnoreCase) ? "active" : null);

            
            #line default
            #line hidden
WriteLiteral("\">");


            
            #line 55 "..\..\Dashboard\Pages\HomePage.cshtml"
                                                                                                                                                Write(Strings.Common_PeriodWeek);

            
            #line default
            #line hidden
WriteLiteral("</a>\r\n            </div>\r\n            ");


            
            #line 57 "..\..\Dashboard\Pages\HomePage.cshtml"
       Write(Strings.HomePage_HistoryGraph);

            
            #line default
            #line hidden
WriteLiteral("\r\n        </h3>\r\n\r\n");


            
            #line 60 "..\..\Dashboard\Pages\HomePage.cshtml"
         if (succeeded != null && failed != null)
        {

            
            #line default
            #line hidden
WriteLiteral("            <canvas width=\"1140\" height=\"250\" id=\"historyGraph\"\r\n                " +
" data-succeeded=\"");


            
            #line 63 "..\..\Dashboard\Pages\HomePage.cshtml"
                            Write(JsonConvert.SerializeObject(succeeded));

            
            #line default
            #line hidden
WriteLiteral("\"\r\n                 data-failed=\"");


            
            #line 64 "..\..\Dashboard\Pages\HomePage.cshtml"
                         Write(JsonConvert.SerializeObject(failed));

            
            #line default
            #line hidden
WriteLiteral("\"\r\n                 data-succeeded-string=\"");


            
            #line 65 "..\..\Dashboard\Pages\HomePage.cshtml"
                                   Write(Strings.HomePage_GraphHover_Succeeded);

            
            #line default
            #line hidden
WriteLiteral("\"\r\n                 data-failed-string=\"");


            
            #line 66 "..\..\Dashboard\Pages\HomePage.cshtml"
                                Write(Strings.HomePage_GraphHover_Failed);

            
            #line default
            #line hidden
WriteLiteral("\"\r\n                 data-period=\"");


            
            #line 67 "..\..\Dashboard\Pages\HomePage.cshtml"
                         Write(period);

            
            #line default
            #line hidden
WriteLiteral("\">\r\n            </canvas>\r\n");


            
            #line 69 "..\..\Dashboard\Pages\HomePage.cshtml"
        }

            
            #line default
            #line hidden
WriteLiteral("    </div>\r\n</div>");


        }
    }
}
#pragma warning restore 1591
