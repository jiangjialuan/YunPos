using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MsdpApi;

namespace CySoft.Utility
{
    public class MSDPHelper
    {
        public static bool isInti = false;

        public static int Init()
        {
            int resultCode = Msdp.SkInit(AppConfig.GetValue("msdpHost"), AppConfig.GetValue("msdpUserNname"), AppConfig.GetValue("msdpPassword"), 0, null, 0, null, null);
            if (resultCode == 0)
            {
                isInti = true;
            }
            return resultCode;
        }

        public static int End()
        {
            int resultCode = Msdp.SkLogout();
            isInti = false;
            return resultCode;
        }

        public static int Send(string phone, string message)
        {
            if (!isInti)
            {
                Init();
            }
            string successList = "                                                                                  ";
            string failureList = "                                                                                  ";
            int successCount = 0;
            int failureCount = 0;
            int resultCode = -1;
            try
            {
                resultCode = Msdp.SkMultiSend(101, "", "", phone, message, out successList, out failureList, out successCount, out failureCount);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return resultCode;
        }
    }
}
