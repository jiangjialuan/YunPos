using System.Collections.Generic;
using System.IO;
using System.Web.Optimization;

namespace Web
{
    internal class AsIsBundleOrderer : IBundleOrderer
    {
        public IEnumerable<FileInfo> OrderFiles(BundleContext context, IEnumerable<FileInfo> files)
        {
            return files;
        }
    }

    internal static class BundleExtensions
    {
        /// <summary>
        /// 按传入顺序加载
        /// </summary>
        /// <param name="bundle"></param>
        /// <returns></returns>
        public static Bundle ForceOrdered(this Bundle bundle)
        {
            bundle.Orderer = new AsIsBundleOrderer();
            return bundle;
        }
    }

}