using System.Web;
using System.Web.Optimization;

namespace ProjectPracticeWeb
{
	public class BundleConfig
	{
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
				"~/Scripts/jquery-{version}.js"));
		}
	}
	}