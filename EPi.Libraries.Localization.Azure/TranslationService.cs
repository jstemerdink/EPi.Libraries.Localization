// Copyright © 2016 Jeroen Stemerdink.
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
namespace EPi.Libraries.Localization.Azure
{
    using System;
    using System.Configuration;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using System.Xml;

    using EPiServer.Logging;
    using EPiServer.ServiceLocation;

    /// <summary>
    ///     Class TranslationService.
    /// </summary>
    [ServiceConfiguration(typeof(ITranslationService))]
    public class TranslationService : ITranslationService
    {

        /// <summary>
        /// Header name used to pass the subscription key to translation service
        /// </summary>
        private const string OcpApimSubscriptionKeyHeader = "Ocp-Apim-Subscription-Key";

        /// <summary>
        /// The translate URL template
        /// </summary>
        private const string TranslateUrlTemplate = "http://api.microsofttranslator.com/v2/http.svc/translate?text={0}&from={1}&to={2}&category={3}";

        /// <summary>
        ///     Initializes the <see cref="LogManager">LogManager</see> for the <see cref="TranslationService" /> class.
        /// </summary>
        private static readonly ILogger Logger = LogManager.GetLogger(typeof(TranslationService));

        /// <summary>
        /// The azure subscription key
        /// </summary>
        private static string azureSubscriptionKey;

        /// <summary>
        /// Gets the azure subscription key.
        /// </summary>
        /// <value>The azure subscription key.</value>
        public static string AzureSubscriptionKey
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(azureSubscriptionKey))
                {
                    return azureSubscriptionKey;
                }

                return azureSubscriptionKey = ConfigurationManager.AppSettings["localization.azure.subscriptionkey"];
            }
        }

        /// <summary>
        ///     Translates the specified text.
        /// </summary>
        /// <param name="toBeTranslated">The text to translate.</param>
        /// <param name="fromLang">From language.</param>
        /// <param name="toLang">To language.</param>
        /// <returns>The translated string.</returns>
        public string Translate(string toBeTranslated, string fromLang, string toLang)
        {
            try
            {
                Uri uri = new Uri(string.Format(CultureInfo.InvariantCulture, TranslateUrlTemplate, HttpUtility.UrlEncode(toBeTranslated), fromLang, toLang, "generalnn"));

                WebRequest translationWebRequest = WebRequest.Create(uri);
                translationWebRequest.Headers.Add(OcpApimSubscriptionKeyHeader, AzureSubscriptionKey);

                using (WebResponse response = translationWebRequest.GetResponse())
                {
                    Stream stream = response.GetResponseStream();

                    if (stream == null)
                    {
                        return null;
                    }

                    using (StreamReader translatedStream = new StreamReader(stream, Encoding.UTF8))
                    {
                        XmlDocument xTranslation = new XmlDocument();
                        xTranslation.LoadXml(translatedStream.ReadToEnd());
                        return xTranslation.InnerText;
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Error("[Localization] Error getting translations from Azure", exception);
                return null;
            }
        }

        /// <summary>
        /// Translates the request.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>The translate task.</returns>
        private static async Task<HttpResponseMessage> TranslateRequestAsync(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add(OcpApimSubscriptionKeyHeader, AzureSubscriptionKey);
                return await client.GetAsync(url).ConfigureAwait(true);
            }
        }

        /// <summary>
        /// translate as an asynchronous operation.
        /// </summary>
        /// <param name="toBeTranslated">To be translated.</param>
        /// <param name="fromLang">From language.</param>
        /// <param name="toLang">To language.</param>
        /// <returns>The translation Task.</returns>
        private static async Task TranslateAsync(string toBeTranslated, string fromLang, string toLang)
        {
            try
            {
                HttpResponseMessage translateResponse = await TranslateRequestAsync(string.Format(CultureInfo.InvariantCulture, TranslateUrlTemplate, HttpUtility.UrlEncode(toBeTranslated), fromLang, toLang, "generalnn")).ConfigureAwait(true);

                string translateResponseContent = await translateResponse.Content.ReadAsStringAsync().ConfigureAwait(true);

                if (translateResponse.IsSuccessStatusCode)
                {
                    Logger.Debug("[Localization] Translation result: {0}", translateResponseContent);
                }
                else
                {
                    Logger.Error("[Localization] Failed to translate. Response: {0}", translateResponseContent);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("[Localization] Failed to translate. Exception: {0}", ex.Message);
            }
        }
    }
}