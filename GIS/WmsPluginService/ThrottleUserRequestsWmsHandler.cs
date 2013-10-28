using System;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Caching;

namespace ThinkGeo.MapSuite.WmsServerEdition
{
    /// <summary>
    /// Summary description for ThrottleUserRequestsWmsHandler
    /// </summary>
    public class ThrottleUserRequestsWmsHandler : WmsHandler
    {

        // Here in the constructor we are going to set the path to the custom
        // capabilities XML for this particular handler
        public ThrottleUserRequestsWmsHandler()
        {
            this.CapabilitiesFilePathName = HttpContext.Current.Server.MapPath(@"~\CapabilitiesXml\ThrottleUserRequestsWmsHandlerCapabilities.xml");
        }

        // The ProcessRequestCore is the first method called in the cycle.  This is the first place you
        // put code in the WMS request process.  What we are going to do here is to check
        // how many requests the user is making over a certain period and if it is too many then
        // we throttle him
        protected override void ProcessRequestCore(HttpContext context)
        {
            // Get the requesters IP
            string requestIp = context.Request.UserHostAddress;

            // We check our cache and see if the users IP is there, if it is this is not his first
            // request in 10 seconds.  If the cache is null then it is his first request in 10 seconds
            if (HttpRuntime.Cache["Throttling-" + requestIp] == null)
            {
                // Add the IP to the cache and make the cache expire in 10 seconds
                HttpRuntime.Cache.Add("Throttling-" + requestIp, true, null, DateTime.Now.AddSeconds(10), Cache.NoSlidingExpiration, CacheItemPriority.Low, null);
                context.Application["Throttling-" + requestIp] = 1;

                // Call the base and let the request proceed as normal
                base.ProcessRequestCore(context);
            }
            else
            {
                // Add one to the users total number of request for this 10 second period
                context.Application["Throttling-" + requestIp] = GetThrottleValue(requestIp, context) + 1;

                // If the user has more than 25 requests in 10 seconds then throttle him and if not
                // then call the base and let the request proceed as normal
                if (GetThrottleValue(requestIp, context) > 25)
                {
                    // Draw an excpetion return image that lets the user know they are throttled
                    int requestCount = GetThrottleValue(requestIp, context);
                    string warningText = string.Format("Limit 25 tiles per 10s \r\nRequests: {0}", requestCount.ToString(CultureInfo.InvariantCulture));
                    DrawExceptionMessage(context, warningText);
                }
                else
                {
                    base.ProcessRequestCore(context);
                }
            }
        }

        private int GetThrottleValue(string requestIp, HttpContext context)
        {
            int throttleValue = 0;

            if (context.Application["Throttling-" + requestIp] == null)
            {
                context.Application["Throttling-" + requestIp] = 0;
            }
            else
            {
                throttleValue = Convert.ToInt32(context.Application["Throttling-" + requestIp]);
            }

            return throttleValue;
        }

        // This method creates a tile with an exception message on it and write it to
        // the response which sends it back to the client
        private static void DrawExceptionMessage(HttpContext context, string message)
        {
            Bitmap bitmap = null;
            MemoryStream stream = null;
            try
            {
                // Get the width & height from the query string.  This is the size of the tile
                // we need to return for our exception message
                NameValueCollection nameValueCollection = context.Request.QueryString;
                int width = Convert.ToInt32(nameValueCollection["WIDTH"], CultureInfo.InvariantCulture);
                int height = Convert.ToInt32(nameValueCollection["HEIGHT"], CultureInfo.InvariantCulture);

                // Create the exception bitmap
                bitmap = new Bitmap(width, height);

                // Draw the exception message on the bitmap
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                    Font wartermarkFont = new Font("Arial", 11, FontStyle.Bold);
                    Color wartermarkColor = Color.FromArgb(100, Color.Red);
                    graphics.DrawString(message, wartermarkFont, new SolidBrush(wartermarkColor), new PointF(0, 0));
                }
                stream = new MemoryStream();
                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);

                // Write the bitmap back to the client via the response
                context.Response.ContentType = nameValueCollection["FORMAT"];
                context.Response.BinaryWrite(stream.GetBuffer());
            }
            finally
            {
                // Cleanup our disposable objects
                if (stream != null) { stream.Dispose(); };
                if (bitmap != null) { bitmap.Dispose(); };
            }
        }
    }
}