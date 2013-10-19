using System;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ThinkGeo.MapSuite.WmsServerEdition
{
    /// <summary>
    /// Summary description for SecureServerRequestsWmsHandler
    /// </summary>
    public class SecureServerRequestsWmsHandler : WmsHandler
    {
        // Here in the constructor we are going to set the path to the custom
        // capabilities XML for this particular handler
        public SecureServerRequestsWmsHandler()
        {
            this.CapabilitiesFilePathName = HttpContext.Current.Server.MapPath(@"~\CapabilitiesXml\SecureServerRequestsWmsHandlerCapabilities.xml");
        }

        // The ProcessRequestCore is the first method called in the cycle.  This is the first place you
        // put code in the WMS request process.  What we are going to do here is to check
        // to see if the URL is signed and if not then do not bother calling the base just return
        // an error tile to the user.
        protected override void ProcessRequestCore(HttpContext context)
        {
            // Grab the clientId and signiture from the query string
            NameValueCollection nameValueCollection = context.Request.QueryString;
            string requestType = nameValueCollection["REQUEST"].ToUpperInvariant();

            if (requestType.Equals("GETCAPABILITIES", StringComparison.OrdinalIgnoreCase))
            {
                base.ProcessRequestCore(context);
            }
            else
            {
                string clientId = nameValueCollection["ClientId"];
                string signature = nameValueCollection["Signature"];

                // Lookup the private key for this client
                string privateKey = GetClientsPrivateKey(clientId);

                // Get part of the URL minus the signiture itself
                string requestPathExceptSignature = context.Request.Url.AbsoluteUri.Substring(0, context.Request.Url.AbsoluteUri.LastIndexOf('&'));

                // Hash the URL so we can compare it to the signiture in the request
                string signedUrl = SignUrl(requestPathExceptSignature, privateKey);

                //  If the signiture matches the has then it is good and we call the base
                //  If the signiture does not match then we cannot gaurentee the request came
                //  from our client so we return an exception tile
                if (!signedUrl.Equals(context.Request.Url.AbsoluteUri, StringComparison.OrdinalIgnoreCase) && !requestType.Equals("GETCAPABILITIES", StringComparison.OrdinalIgnoreCase))
                {
                    DrawExceptionMessage(context, "Failed Authentication");
                }
                else
                {
                    base.ProcessRequestCore(context);
                }
            }
        }

        // Normally in this method you would lookup the key based on the clientId from
        // a SQL server or a cached list of clients.  For this example we are hard coding
        // the one client we accept
        private string GetClientsPrivateKey(string clientId)
        {
            string privateKey = string.Empty;

            if (clientId.Equals("ThinkGeo", StringComparison.OrdinalIgnoreCase))
            {
                privateKey = "lkjf()*9LKJ8afjLKJ098)(*sdfh";
            }

            return privateKey;
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

        // This method signs the URL based on the privatekey you passed in
        public static string SignUrl(string url, string privateKey)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
                        
            byte[] privateKeyBytes = encoding.GetBytes(privateKey); //Convert.FromBase64String(usablePrivateKey);

            Uri uri = new Uri(url);
            byte[] encodedPathAndQueryBytes = encoding.GetBytes(uri.LocalPath + uri.Query);

            // compute the hash
            HMACSHA1 algorithm = new HMACSHA1(privateKeyBytes);
            byte[] hash = algorithm.ComputeHash(encodedPathAndQueryBytes);

            // convert the bytes to string and make url-safe by replacing '+' and '/' characters
            string signature = Convert.ToBase64String(hash).Replace("+", "-").Replace("/", "_");

            // Add the signature to the existing URI.
            return uri.Scheme + "://" + uri.Authority + uri.LocalPath + uri.Query + "&signature=" + signature;
        }
    }
}