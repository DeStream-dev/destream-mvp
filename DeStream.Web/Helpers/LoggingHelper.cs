using NLog;
using System;
using System.Globalization;
using System.Net.Http;
using System.Web;

namespace DeStream.Web.Helpers
{
    public static class LoggingHelper
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private static readonly bool IsDebugEnabled = Log.IsDebugEnabled;

        public static void LogRequest(HttpRequestMessage request)
        {
            try
            {
                var context=request.Properties["MS_HttpContext"] as System.Web.HttpContextWrapper;
                string userInfo = $"({context.Request.UserHostAddress} | {request.Headers.UserAgent})";
                LogRequest(request.Method.ToString(), request.RequestUri, request.Content, userInfo);
            }
            catch (Exception e)
            {
                Log.Error(e, "Can't log request");
            }
        }

        public static void LogRequest(HttpRequestBase request)
        {
            try
            {
                LogRequest(request.HttpMethod, request.Url, new StreamContent(request.InputStream));
            }
            catch (Exception e)
            {
                Log.Error("Can't log request", e);
            }
        }

        private static void LogRequest(string httpMethod, Uri url, HttpContent content, string userInfo=null)
        {
            var shouldLogData = IsDebugEnabled;
            var method = httpMethod.ToUpper();
            var pathAndQuery = HttpUtility.UrlDecode(url.PathAndQuery);
            var requestInfo = $"{method} {pathAndQuery} {userInfo}";

            Log.Info(requestInfo);
            if (shouldLogData && (content != null))
            {
                if (content.Headers.ContentType == null || !content.Headers.ContentType.ToString().StartsWith("image"))
                {
                    var requestContentString = content.ReadAsStringAsync().Result;
                    if (!string.IsNullOrEmpty(requestContentString)) Log.Debug("{0} <- {1}", requestInfo, requestContentString);
                }
            }
        }

        public static void LogResponse(HttpRequestMessage request, HttpResponseMessage response, TimeSpan? requestDuration = null)
        {
            try
            {
                var shouldLogData = IsDebugEnabled;
                var method = request.Method.ToString().ToUpper();
                var pathAndQuery = HttpUtility.UrlDecode(request.RequestUri.PathAndQuery);
                var requestInfo = string.Format("{0} {1}", method, pathAndQuery);
                var responseInfo = string.Format("{0} {1}", (int)response.StatusCode, response.ReasonPhrase);
                var responseContent = response.Content;
                var responseContentString = string.Empty;
                if (shouldLogData && (responseContent != null))
                {
                    if ((responseContent.Headers.ContentType == null) || !responseContent.Headers.ContentType.ToString().StartsWith("image"))
                    {
                        responseContentString = responseContent.ReadAsStringAsync().Result;
                    }
                }
                var durationString = requestDuration != null ? $" [{requestDuration.Value.TotalSeconds.ToString("F3", CultureInfo.InvariantCulture)}]" : "";
                Log.Info($"{requestInfo}{durationString} -> {responseInfo} {responseContentString}");
            }
            catch (Exception e)
            {
                Log.Error("Can't log response", e);
            }
        }

        public static void LogResponse(HttpRequestBase request, int status, string statusDescription, string body)
        {
            try
            {
                var shouldLogData = IsDebugEnabled;
                var method = request.HttpMethod.ToUpper();
                var pathAndQuery = HttpUtility.UrlDecode(request.Url.PathAndQuery);
                var requestInfo = string.Format("{0} {1}", method, pathAndQuery);
                var responseInfo = string.Format("{0} {1}", status, statusDescription);
                var responseContentString = shouldLogData ? body : "";
                Log.Info("{0} -> {1} {2}", requestInfo, responseInfo, responseContentString);
            }
            catch (Exception e)
            {
                Log.Error("Can't log response", e);
            }
        }
    }
}