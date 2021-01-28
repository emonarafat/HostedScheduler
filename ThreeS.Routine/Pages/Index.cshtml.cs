namespace ThreeS.Routine.Pages
{

    using Hangfire;

    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;

    using System;

    using ThreeS.Jobs;

    /// <summary>
    /// Defines the <see cref="IndexModel" />
    /// </summary>
    public class IndexModel : PageModel
    {
        /// <summary>
        /// Defines the _logger
        /// </summary>
        private readonly ILogger<IndexModel> _logger;
        private readonly ISQLJobs _sQLJobs;

        public IndexModel(ILogger<IndexModel> logger, ISQLJobs sQLJobs)
        {
            _logger = logger;
            _sQLJobs = sQLJobs;
        }


        /// <summary>
        /// The OnGet
        /// </summary>
        public void OnGet()
        {
            //RecurringJob.RemoveIfExists("RebuildIndex");
            //RecurringJob.AddOrUpdate("RebuildIndex", () => _sQLJobs.RebuildAllIndex(), Cron.Weekly(), timeZone: TimeZoneInfo.Local);
            //RecurringJob.RemoveIfExists("RebuildOrReorganizeIndexes");
            //RecurringJob.AddOrUpdate("RebuildOrReorganizeIndexes", () => _sQLJobs.RebuildOrReorganizeIndexes(), Cron.Weekly(), timeZone: TimeZoneInfo.Local);
            //RecurringJob.RemoveIfExists("ShrinkDatabase");
            //RecurringJob.AddOrUpdate("ShrinkDatabase", () => _sQLJobs.ShrinkDataBases(), Cron.Weekly(), timeZone: TimeZoneInfo.Local);
        }
    }
}
