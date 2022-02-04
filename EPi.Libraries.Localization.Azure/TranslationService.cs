// Copyright © 2022 Jeroen Stemerdink.
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
    using System.Net.Http;
    using System.Text;

    using EPiServer.ServiceLocation;
    using EPiServer.Shell.Configuration;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    using Newtonsoft.Json;

    /// <summary>
    ///     Class TranslationService.
    /// </summary>
    [ServiceConfiguration(typeof(ITranslationService))]
    public class TranslationService : ITranslationService
    {
        private const string Endpoint = "https://api.cognitive.microsofttranslator.com/";

        private const string GlobalRegion = "global";

        private readonly IConfiguration configuration;

        /// <summary>
        ///     Initializes the <see cref="ILogger{T}">LogManager</see> for the <see cref="TranslationService" /> class.
        /// </summary>
        private readonly ILogger<TranslationService> logger;

        private string azureEndpoint;

        private string azureRegion;

        private string azureSubscriptionKey;

        public TranslationService(IConfiguration configuration, ILogger<TranslationService> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
        }

        /// <summary>
        /// Gets the azure endpoint.
        /// </summary>
        /// <value>The azure endpoint.</value>
        public string AzureEndpoint
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(value: this.azureEndpoint))
                {
                    return this.azureEndpoint;
                }

                string keyFromConfig = this.configuration["Localization:AzureEndpoint"];

                if (string.IsNullOrWhiteSpace(value: keyFromConfig))
                {
                    keyFromConfig = Endpoint;
                }

                return this.azureEndpoint = keyFromConfig;
            }
        }

        /// <summary>
        /// Gets the azure subscription key.
        /// </summary>
        /// <value>The azure subscription key.</value>
        public string AzureRegion
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(value: this.azureRegion))
                {
                    return this.azureRegion;
                }

                string keyFromConfig = this.configuration["Localization:AzureRegion"];

                if (string.IsNullOrWhiteSpace(value: keyFromConfig))
                {
                    keyFromConfig = GlobalRegion;
                }

                return this.azureRegion = keyFromConfig;
            }
        }

        /// <summary>
        /// Gets the azure subscription key.
        /// </summary>
        /// <value>The azure subscription key.</value>
        /// <exception cref="MissingConfigurationException" accessor="get">No subscription key found for Azure translations</exception>
        public string AzureSubscriptionKey
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(value: this.azureSubscriptionKey))
                {
                    return this.azureSubscriptionKey;
                }

                string keyFromConfig = this.configuration["Localization:AzureSubscriptionKey"];

                if (string.IsNullOrWhiteSpace(value: keyFromConfig))
                {
                    throw new MissingConfigurationException("No subscription key found for Azure translations");
                }

                return this.azureSubscriptionKey = keyFromConfig;
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
                string route = $"/translate?api-version=3.0&from={fromLang}&to={toLang}";

                object[] body = { new { Text = toBeTranslated } };
                string requestBody = JsonConvert.SerializeObject(value: body);

                using (HttpClient client = new HttpClient())
                using (HttpRequestMessage request = new HttpRequestMessage())
                {
                    // Build the request.
                    request.Method = HttpMethod.Post;
                    request.RequestUri = new Uri(Endpoint + route);
                    request.Content = new StringContent(
                        content: requestBody,
                        encoding: Encoding.UTF8,
                        "application/json");

                    request.Headers.Add("Ocp-Apim-Subscription-Key", value: this.AzureSubscriptionKey);

                    if (!this.AzureRegion.Equals(value: GlobalRegion))
                    {
                        request.Headers.Add("Ocp-Apim-Subscription-Region", value: this.AzureRegion);
                    }

                    // Send the request and get response.
                    HttpResponseMessage response = client.Send(request: request);

                    // Read response as a string.
                    string result = response.Content.ReadAsStringAsync().Result;

                    this.logger.LogInformation($"[Localization] Returned result: {result} .");

                    if (!string.IsNullOrWhiteSpace(value: result))
                    {
                        TranslationResult[] deserializedOutput =
                            JsonConvert.DeserializeObject<TranslationResult[]>(value: result);

                        return deserializedOutput[0].Translations[0].Text;
                    }

                    this.logger.LogInformation("[Localization] Getting translations from Azure returned empty result.");

                    return null;
                }
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception: exception, "[Localization] Error getting translations from Azure");
                return null;
            }
        }
    }
}