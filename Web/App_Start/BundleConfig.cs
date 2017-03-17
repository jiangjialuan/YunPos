using System.Web.Optimization;
using CySoft.Utility;

namespace Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = true;
            BundleTable.EnableOptimizations = true;

            string jquery_1_9_1_cdn = AppConfig.GetValue("jquery_1_9_1_cdn");
            bundles.Add(new ScriptBundle("~/scripts/jquery_1_9_1", jquery_1_9_1_cdn).Include("~/scripts/jquery/1.9.1/jquery.min.js"));
        }
    }
}