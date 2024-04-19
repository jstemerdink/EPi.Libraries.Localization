using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using EPiServer.ContentApi.Core.Internal;
using EPiServer.Framework.Localization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EPi.Libraries.Localization.Api
{
    /// <summary>
    /// The translation api controller.
    /// </summary>
    [DisplayName("Translation Api")]
    [ApiController]
    [Authorize(Policy = "ContentDeliveryAuthorizationPolicy")]
    [ContentLanguageFilter]
    [Route("api/translations")]
    public class TranslationApiController : ControllerBase
    {
        private readonly LocalizationService localizationService;

        public TranslationApiController(LocalizationService localizationService)
        {
            this.localizationService = localizationService;
        }

        /// <summary>
        /// Gets all translations for all or the requested language(s).
        /// </summary>
        /// <returns>The translations for all available languages.</returns>
        [HttpGet]
        [Route("")]
        [Produces(typeof(Dictionary<string, Dictionary<string, string>>))]
        [ProducesResponseType(404)]
        public IActionResult GetTranslations([FromHeader(Name = "Accept-Language")] List<string> languages)
        {
            Dictionary<string, Dictionary<string, string>> keyValues = GetKeyValues(languages, null);

            if (keyValues.Count == 0)
            {
                return NotFound();
            }

            return Ok(keyValues);
        }

        /// <summary>
        /// Gets all translations below the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="languages">The languages to get the translations for the specified <paramref name="key"/> for.</param>
        /// <returns>An array of translations below the specified <paramref name="key"/>, by culture.</returns>
        [HttpGet]
        [Route("{key}")]
        [Produces(typeof(Dictionary<string, Dictionary<string, string>>))]
        [ProducesResponseType(404)]
        public IActionResult GetTranslations(string key, [FromHeader(Name = "Accept-Language")] List<string> languages)
        {
            Dictionary<string, Dictionary<string, string>> keyValues = GetKeyValues(languages, key);

            if (keyValues.Count == 0)
            {
                return NotFound();
            }

            return Ok(keyValues);
        }

        /// <summary>
        /// Gets a single translation.
        /// </summary>
        /// <param name="language">The language</param>
        /// <param name="key">The key.</param>
        /// <returns>The translation for the <paramref name="key"/> in the specified <paramref name="language"/>.</returns>
        [HttpGet]
        [Route("{language}/{key}")]
        [Produces(typeof(string))]
        [ProducesResponseType(404)]
        public IActionResult GetTranslation(string language, string key)
        {
            string resourceKey = $"/{key.Replace('.', '/')}";

            if (localizationService.TryGetStringByCulture(resourceKey, new CultureInfo(language), out var translation))
            {
                return Ok(translation);
            }

            return NotFound();
        }
        
        private static void AddKeyValue(ResourceItem resourceItem, Dictionary<string, string> keyValues)
        {
            string resourceKey = resourceItem.Key.Replace('/', '.').TrimStart('.');
            string resourceValue = resourceItem.Value;

            keyValues.TryAdd(key: resourceKey, value: resourceValue);
        }

        private Dictionary<string, Dictionary<string, string>> GetKeyValues(List<string> languages, string key)
        {
            List<ResourceItem> resourceItems = new List<ResourceItem>();

            if (languages.Count <= 0)
            {
                languages = localizationService.AvailableLocalizations.Select(ci => ci.Name).ToList();
            }

            string resourceKey = string.Empty;

            if (!string.IsNullOrWhiteSpace(key))
            {
                resourceKey = $"/{key.Replace('.', '/')}";
            }

            foreach (string language in languages)
            {
                resourceItems.AddRange(localizationService.GetAllStringsByCulture(resourceKey, new CultureInfo(language)));
            }
            
            List<IGrouping<string, ResourceItem>> groupedTranslationsList = resourceItems
                .GroupBy(u => u.SourceCulture.Name).ToList();

            Dictionary<string, Dictionary<string, string>> keyValues =
                new Dictionary<string, Dictionary<string, string>>();

            foreach (IGrouping<string, ResourceItem> group in groupedTranslationsList)
            {
                Dictionary<string, string> cultureDictionary = new Dictionary<string, string>();

                foreach (ResourceItem resourceItem in group)
                {
                    AddKeyValue(resourceItem: resourceItem, keyValues: cultureDictionary);
                }

                keyValues.Add(key: group.Key, value: cultureDictionary);
            }

            return keyValues;
        }
    }
}