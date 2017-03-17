using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Diagnostics;

namespace CySoft.Utility
{
    public class BrowserHelper
    {
        protected static List<BrowserModel> browserModelList = new List<BrowserModel>();

        protected static void Init()
        {
            if (browserModelList == null || browserModelList.Count < 1)
            {
                browserModelList = new List<BrowserModel>();
            }
            //Mozilla/5.0 (iPhone 5SGLOBAL; CPU iPhone OS 8_1_2 like Mac OS X) AppleWebKit/600.1.4 (KHTML, like Gecko) Version/6.0 MQQBrowser/5.6 Mobile/12B440 Safari/8536.25
            browserModelList.Add(new BrowserModel(@"mqqbrowser/(?<version>[\d\.]+).*mobile/", "mobile qq", "${version}"));//识别Mobile QQBrowser
            //Mozilla/5.0 (iPhone; CPU iPhone OS 8.1.2 like Mac OS X) AppleWebKit/534.46 (KHTML, like Gecko) Version/5.1 Mobile/9A334 Safari/7534.48.3 QHBrowser/1.2.1
            browserModelList.Add(new BrowserModel(@"mobile/.*qhbrowser/(?<version>[\d\.]+)", "mobile 360", "${version}"));//识别Mobile 360browser
            //Mozilla/5.0 (iPhone; CPU iPhone OS 8_1_2 like Mac OS X; zh-CN) AppleWebKit/537.51.1 (KHTML, like Gecko) Mobile/12B440 UCBrowser/10.1.5.526 Mobile
            browserModelList.Add(new BrowserModel(@"mobile/.*ucbrowser/(?<version>[\d\.]+)", "mobile uc", "${version}"));//识别Mobile UCBrowser
            //Mozilla/5.0 (iPhone; CPU iPhone OS 8_1_2 like Mac OS X) AppleWebKit/600.1.4 (KHTML, like Gecko) CriOS/39.0.2171.50 Mobile/12B440 Safari/600.1.4
            browserModelList.Add(new BrowserModel(@"crios/(?<version>[\d\.]+).*mobile", "mobile chrome", "${version}"));//识别Mobile Chrome
            //Mozilla/5.0 (iPhone; CPU iPhone OS 8_1_2 like Mac OS X) AppleWebKit/600.1.4 (KHTML, like Gecko) Coast/4.00.86810 Mobile/12B440 Safari/7534.48.3
            browserModelList.Add(new BrowserModel(@"coast/(?<version>[\d\.]+).*mobile", "mobile opera", "${version}"));//识别Mobile Opera Coast
            //Mozilla/5.0 (iPhone; CPU iPhone OS 8_1_2 like Mac OS X) AppleWebKit/600.1.4 (KHTML, like Gecko) OPiOS/9.1.0.86723 Mobile/12B440 Safari/9537.53
            browserModelList.Add(new BrowserModel(@"opios/(?<version>[\d\.]+).*mobile", "mobile opera", "${version}"));//识别Mobile Opera for iOS
            //Mozilla/5.0 (Linux; Android 4.4.4; SM-N9150 Build/KTU84P) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.59 Mobile Safari/537.36 OPR/26.0.1656.87080
            browserModelList.Add(new BrowserModel(@"mobile.*safari/.*opr/(?<version>[\d\.]+)", "mobile opera", "${version}"));//识别Mobile Opera for Android
            //Mozilla/5.0 (Linux; U; Android 4.1.1; en-us; Google Nexus S - 4.1.1 - API 16 - 480x800 Build/JRO03S) AppleWebKit/534.30 (KHTML, like Gecko) Version/4.0 Mobile Safari/534.30
            browserModelList.Add(new BrowserModel(@"android.*version/(?<version>[\d\.]+).*mobile", "mobile android", "${version}"));//识别Mobile Android
            //Mozilla/5.0 (iPhone; CPU iPhone OS 8_1_2 like Mac OS X) AppleWebKit/600.1.4 (KHTML, like Gecko) Version/8.0 Mobile/12B440 Safari/600.1.4
            browserModelList.Add(new BrowserModel(@"version/(?<version>[\d\.]+).*mobile.*safari/", "mobile safari", "${version}"));//识别Mobile Safari

            browserModelList.Add(new BrowserModel(@"(?<browser>opr)/(?<version>[\d\.]+)", "opera", "${version}"));//识别opera 20.0以上
            browserModelList.Add(new BrowserModel(@"(?<browser>chrome)/(?<version>[\d\.]+)", "${browser}", "${version}"));//识别chrome
            browserModelList.Add(new BrowserModel(@"version/(?<version>[\d\.]+)\s(?<browser>safari)/(?<devVersion>[\d\.]+)", "${browser}", "${version}/${devVersion}"));//识别safari
            browserModelList.Add(new BrowserModel(@"(?<browser>webkit)/(?<version>\S+)", "${browser}", "${version}"));//识别webkit
            browserModelList.Add(new BrowserModel(@"(?<browser>opera).*version/(?<version>[\d\.]+)", "${browser}", "${version}"));//识别opera 6.0以上
            browserModelList.Add(new BrowserModel(@"(?<browser>msie)\s(?<version>[\d\.]+)", "${browser}", "${version}"));//识别IE6-IE10
            browserModelList.Add(new BrowserModel(@"(?<browser>netscape)/(?<version>[\d\.]+)", "${browser}", "${version}"));//识别netscape
            browserModelList.Add(new BrowserModel(@"(?<browser>navigator)/(?<version>[\d\.]+)", "${browser}", "${version}"));//识别navigator
            //Mozilla/5.0 (Windows NT 6.1; Trident/7.0; rv:11.0) like Gecko
            browserModelList.Add(new BrowserModel(@"(.+rv:(?<version>[\d\.]+)).*like.*gecko$", "msie", "${version}"));//识别IE11以上版本
            browserModelList.Add(new BrowserModel(@"(?<browser>mozilla)/.*(.*rv:(?<version>[\d\.]+))", "${browser}", "${version}"));//识别mozilla
        }

        public static BrowserInfo GetBrowserInfo(string userAgent)
        {
            if (browserModelList.Count < 1)
            {
                lock (browserModelList)
                {
                    if (browserModelList.Count < 1)
                    {
                        Init();
                    }
                }
            }
            BrowserInfo model = new BrowserInfo();
            if (String.IsNullOrEmpty(userAgent))
            {
                return model;
            }
            userAgent = userAgent.ToLower();

            foreach (BrowserModel browserModel in browserModelList)
            {
                if (browserModel.regex.IsMatch(userAgent))
                {
                    Match m = browserModel.regex.Match(userAgent);
                    model.browser = m.Result(browserModel.replacBrowser);
                    model.version = m.Result(browserModel.replacVersion);
                    return model;
                }
            }

            return model;
        }
    }

    [DebuggerDisplay("browser = {browser}, version = {version}")]
    public class BrowserInfo
    {
        private string _browser = "unknown";
        private string _version = "unknown";

        public string browser
        {
            get { return this._browser; }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    this._browser = value;
                }
                else
                {
                    this._browser = String.Empty;
                }
            }
        }

        public string version
        {
            get { return this._version; }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    this._version = value;
                }
                else
                {
                    this._version = String.Empty;
                }
            }
        }
    }

    [DebuggerDisplay("regexPattern = {regexPattern}, replacBrowser = {replacBrowser}, replacVersion = {replacVersion}")]
    public class BrowserModel
    {
        public BrowserModel() { }
        public BrowserModel(string regexPattern, string replacBrowser, string replacVersion)
        {
            this.regexPattern = regexPattern;
            this.regex = new Regex(this.regexPattern, RegexOptions.Compiled);
            this.replacBrowser = replacBrowser;
            this.replacVersion = replacVersion;
        }

        protected string regexPattern { get; set; }
        public Regex regex { get; set; }
        public string replacBrowser { get; set; }
        public string replacVersion { get; set; }
    }
}
