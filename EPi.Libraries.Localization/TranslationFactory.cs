// Copyright© 2016 Jeroen Stemerdink. All Rights Reserved.
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

namespace EPi.Libraries.Localization
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    using EPi.Libraries.Localization.DataAnnotations;
    using EPi.Libraries.Localization.Models;

    using EPiServer;
    using EPiServer.Core;
    using EPiServer.DataAbstraction;
    using EPiServer.DataAccess;
    using EPiServer.Security;
    using EPiServer.ServiceLocation;

    using Microsoft.Extensions.Logging;

    /// <summary>
    ///     The TranslationFactory class, used for translation queries.
    /// </summary>
    public sealed class TranslationFactory
    {
        /// <summary>
        ///     The synclock object.
        /// </summary>
        private static readonly object SyncLock = new object();

        /// <summary>
        ///     The one and only TranslationFactory instance.
        /// </summary>
        private static volatile TranslationFactory instance;

        /// <summary>
        ///     The translation service
        /// </summary>
        private ITranslationService translationService;

        /// <summary>
        ///     Gets a value indicating whether [a translation service is activated].
        /// </summary>
        private bool? translationServiceActivated;

        /// <summary>
        ///     Prevents a default instance of the <see cref="TranslationFactory" /> class from being created.
        /// </summary>
        private TranslationFactory()
        {
            this.TranslationContainerReference = this.GetTranslationContainer();
        }

        /// <summary>
        ///     Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static TranslationFactory Instance
        {
            get
            {
                // Double checked locking
                if (instance != null)
                {
                    return instance;
                }

                lock (SyncLock)
                {
                    if (instance == null)
                    {
                        instance = new TranslationFactory();
                    }
                }

                return instance;
            }
        }

        /// <summary>
        ///     Gets or sets the content repository.
        /// </summary>
        /// <value>The content repository.</value>
        public Injected<IContentRepository> ContentRepository { get; set; }

        /// <summary>Gets or sets the logger.</summary>
        /// <value>The logger.</value>
        public Injected<ILogger<TranslationFactory>> Logger { get; set; }

        /// <summary>
        ///     Gets or sets the language branch repository.
        /// </summary>
        /// <value>The language branch repository.</value>
        public Injected<ILanguageBranchRepository> LanguageBranchRepository { get; set; }

        /// <summary>
        ///     Gets the reference to the translation container.
        /// </summary>
        public ContentReference TranslationContainerReference { get; private set; }

        /// <summary>
        ///     Gets a value indicating whether [a translation service is activated].
        /// </summary>
        /// <value><c>true</c> if [translation service activated]; otherwise, <c>false</c>.</value>
        internal bool TranslationServiceActivated
        {
            get
            {
                return this.translationServiceActivated
                       ?? (this.translationServiceActivated = this.TranslationService != null).Value;
            }
        }

        /// <summary>
        ///     Gets or sets the translation service.
        /// </summary>
        /// <value>The translation service.</value>
        private ITranslationService TranslationService
        {
            get
            {
                try
                {
                    return this.translationService
                           ?? (this.translationService = ServiceLocator.Current.GetInstance<ITranslationService>());
                }
                catch (ActivationException activationException)
                {
                    this.Logger.Service.LogInformation("[Localization] No translation service available", activationException);
                }
                catch (InvalidOperationException invalidOperationException)
                {
                    this.Logger.Service.LogInformation("[Localization] No translation service available", invalidOperationException);
                }

                return null;
            }
        }

        /// <summary>
        ///     Gets the missing values.
        /// </summary>
        /// <param name="pageReference">The page reference.</param>
        /// <returns> A ReadOnlyCollection{System.String}.</returns>
        public ReadOnlyCollection<string> GetMissingValues(PageReference pageReference)
        {
            IEnumerable<CultureInfo> availableLanguages =
                this.LanguageBranchRepository.Service.ListEnabled().Select(p => p.Culture);

            IEnumerable<PageData> allLanguages =
                this.ContentRepository.Service.GetLanguageBranches<PageData>(contentLink: pageReference);

            return new ReadOnlyCollection<string>(
                (from availableLanguage in availableLanguages
                 where allLanguages.FirstOrDefault(p => p.Language.Equals(value: availableLanguage)) == null
                 select availableLanguage.NativeName).ToList());
        }

        /// <summary>
        ///     Gets the translated values.
        /// </summary>
        /// <param name="pageReference">The page reference.</param>
        /// <returns>Dictionary&lt;System.String, System.String&gt;.</returns>
        public Dictionary<string, string> GetTranslatedValues(PageReference pageReference)
        {
            IEnumerable<TranslationItem> allLanguages =
                this.ContentRepository.Service.GetLanguageBranches<TranslationItem>(contentLink: pageReference);

            return new Dictionary<string, string>(
                allLanguages.ToDictionary(
                    languageVersion => languageVersion.Language.NativeName,
                    languageVersion => languageVersion.Translation));
        }

        /// <summary>
        ///     Sets the translation container.
        /// </summary>
        public void SetTranslationContainer()
        {
            this.TranslationContainerReference = this.GetTranslationContainer();
        }

        /// <summary>
        ///     Translates them all.
        /// </summary>
        /// <param name="content">The content.</param>
        internal void TranslateThemAll(IContent content)
        {
            if (!this.TranslationServiceActivated)
            {
                return;
            }

            PageData page = content as PageData;

            if (page == null)
            {
                return;
            }

            List<LanguageBranch> enabledLanguages = this.LanguageBranchRepository.Service.ListEnabled().ToList();

            foreach (LanguageBranch languageBranch in enabledLanguages.Where(
                         lb => !lb.Culture.Equals(value: page.Language)))
            {
                this.CreateLanguageBranch(page: page, language: languageBranch.Culture);
            }
        }

        private void CreateLanguageBranch(PageData page, CultureInfo language)
        {
            // Check if language already exists
            bool languageExists = this.ContentRepository.Service
                .GetLanguageBranches<PageData>(contentLink: page.PageLink).Any(p => p.Language.Equals(value: language));

            if (languageExists)
            {
                return;
            }

            TranslationItem translationItem = page as TranslationItem;

            if (translationItem != null)
            {
                TranslationItem languageItemVersion =
                    this.ContentRepository.Service.CreateLanguageBranch<TranslationItem>(
                        contentLink: page.PageLink,
                        language: language);

                languageItemVersion.PageName = page.PageName;
                languageItemVersion.URLSegment = page.URLSegment;

                string translatedText = this.TranslationService.Translate(
                    toBeTranslated: translationItem.OriginalText,
                    page.Language.Name.Split(new char['-'])[0],
                    languageItemVersion.Language.Name.Split(new char['-'])[0]);

                if (string.IsNullOrWhiteSpace(value: translatedText))
                {
                    return;
                }

                languageItemVersion.Translation = translatedText;

                if (!string.IsNullOrWhiteSpace(value: languageItemVersion.Translation))
                {
                    this.ContentRepository.Service.Save(
                        content: languageItemVersion,
                        action: SaveAction.Publish,
                        access: AccessLevel.NoAccess);
                }
            }
            else
            {
                PageData languageVersion = this.ContentRepository.Service.CreateLanguageBranch<PageData>(
                    contentLink: page.PageLink,
                    language: language);

                languageVersion.PageName = page.PageName;
                languageVersion.URLSegment = page.URLSegment;

                this.ContentRepository.Service.Save(
                    content: languageVersion,
                    action: SaveAction.Publish,
                    access: AccessLevel.NoAccess);
            }
        }

        /// <summary>
        ///     Get the translation container.
        /// </summary>
        /// <returns>
        ///     The <see cref="PageReference" /> to the translation container.
        /// </returns>
        private ContentReference GetTranslationContainer()
        {
            ContentReference containerPageReference = null;

            // For a multi site setup there is no other option but to have a global container under the root.
            TranslationContainer containerReference = this.ContentRepository.Service
                .GetChildren<TranslationContainer>(contentLink: ContentReference.RootPage).FirstOrDefault();

            if (containerReference != null)
            {
                this.Logger.Service.LogInformation("[Localization] First translation container under RootPage used.");

                containerPageReference = containerReference.ContentLink;

                return containerPageReference;
            }

            if (ContentReference.IsNullOrEmpty(contentLink: ContentReference.StartPage))
            {
                return ContentReference.EmptyReference;
            }

            ContentData startPageData;

            if (!this.ContentRepository.Service.TryGet(
                    contentLink: ContentReference.StartPage,
                    content: out startPageData))
            {
                return ContentReference.EmptyReference;
            }

            PropertyInfo translationContainerProperty = this.GetTranslationContainerProperty(page: startPageData);

            if (translationContainerProperty != null
                && translationContainerProperty.PropertyType == typeof(PageReference))
            {
                containerPageReference = startPageData.GetPropertyValue(
                    propertyName: translationContainerProperty.Name,
                    defaultValue: ContentReference.StartPage);
            }

            if (containerPageReference == null)
            {
                containerPageReference = startPageData.GetPropertyValue(
                    "TranslationContainer",
                    defaultValue: ContentReference.StartPage);
            }

            if (containerPageReference != ContentReference.StartPage)
            {
                return containerPageReference;
            }

            this.Logger.Service.LogInformation("[Localization] No translation container specified.");

            containerReference = this.ContentRepository.Service
                .GetChildren<TranslationContainer>(contentLink: containerPageReference).FirstOrDefault();

            if (containerReference == null)
            {
                this.Logger.Service.LogInformation("[Localization] No translation container found.");
                return containerPageReference;
            }

            this.Logger.Service.LogInformation("[Localization] First translation container under StartPage used.");

            containerPageReference = containerReference.ContentLink;

            return containerPageReference;
        }

        /// <summary>
        ///     Gets the name of the translation container property.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns>System.Reflection.PropertyInfo</returns>
        private PropertyInfo GetTranslationContainerProperty(ContentData page)
        {
            PropertyInfo translationContainerProperty = page.GetType().GetProperties()
                .Where(predicate: this.HasAttribute<TranslationContainerAttribute>).FirstOrDefault();

            return translationContainerProperty;
        }

        /// <summary>
        ///     Determines whether the specified self has attribute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyInfo">The propertyInfo.</param>
        /// <returns><c>true</c> if the specified self has attribute; otherwise, <c>false</c>.</returns>
        private bool HasAttribute<T>(PropertyInfo propertyInfo)
            where T : Attribute
        {
            try
            {
                T attr = (T)Attribute.GetCustomAttribute(element: propertyInfo, typeof(T));
                return attr != null;
            }
            catch (Exception exception)
            {
                this.Logger.Service.LogError("[Localization] Error checking attribute.", exception);
            }

            return false;
        }
    }
}