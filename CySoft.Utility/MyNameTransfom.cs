using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CySoft.Utility
{
    public class MyNameTransfom : ICSharpCode.SharpZipLib.Core.INameTransform
    {
        public string TransformDirectory(string name)
        {
            return null;
        }

        public string TransformFile(string name)
        {
            return Path.GetFileName(name);
        }
    }
}
