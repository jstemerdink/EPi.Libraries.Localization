// Copyright© 2015 Jeroen Stemerdink. All Rights Reserved.
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

using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Web.UI;

using EPi.Libraries.Localization.Models;
using EPi.Libraries.Localization.UnitTests.Models;
using EPi.Libraries.UnitTests.Base;

using EPiServer.Core;

using Machine.Specifications;
using Machine.Specifications.Annotations;

namespace EPi.Libraries.Localization.UnitTests.Specs
{
    /// <summary>
    ///     The translation specs.
    /// </summary>
    public abstract class TranslationSpecs
    {
        #region Fields

        /// <summary>
        ///     The context.
        /// </summary>
        [UsedImplicitly]
        private Establish context = () =>
            {
                CmsContext = new CmsContext();

                CmsContext.CreatePageType(typeof(StartPage));
                CmsContext.CreatePageType(typeof(TranslationContainer));
                CmsContext.CreatePageType(typeof(TranslationItem));
                CmsContext.CreatePageType(typeof(CategoryTranslationContainer));

                LanguageSelector masterLanguageSelector = new LanguageSelector(CmsContext.MasterLanguage.Name);
                LanguageSelector secondLanguageSelector = new LanguageSelector(CmsContext.SecondLanguage.Name);

                CmsContext.CreateContent<StartPage>("StartPage", ContentReference.RootPage);

                ContainerReference =
                    CmsContext.CreateContent<TranslationContainer>("Translations", ContentReference.RootPage)
                        .ContentLink;

                CmsContext.CreateLanguageVersionOfContent<TranslationContainer>(ContainerReference, secondLanguageSelector);

                TranslationReference =
                    CreateTranslationItem("TextOne", "Translation One", ContainerReference, masterLanguageSelector)
                        .ContentLink;
                AddLanguageVersionToTranslationItem(TranslationReference, "Vertaling Een", secondLanguageSelector);

                ContentReference subContainerReference =
                    CmsContext.CreateContent<TranslationContainer>("SubTranslations", ContainerReference)
                        .ContentLink;
                CmsContext.CreateLanguageVersionOfContent<TranslationContainer>(
                    subContainerReference,
                    secondLanguageSelector);

                ContentReference subItemReference =
                    CreateTranslationItem(
                        "SubTextOne",
                        "SubTranslation One",
                        subContainerReference,
                        masterLanguageSelector).ContentLink;
                AddLanguageVersionToTranslationItem(subItemReference, "Sub Vertaling Een", secondLanguageSelector);

                ContentReference categoryContainerReference =
                    CmsContext.CreateContent<CategoryTranslationContainer>("Categories", ContainerReference)
                        .ContentLink;
                CmsContext.CreateLanguageVersionOfContent<CategoryTranslationContainer>(
                    categoryContainerReference,
                    secondLanguageSelector);

                ContentReference categoryReference =
                    CreateTranslationItem(
                        "CategoryOne",
                        "CategoryTranslation One",
                        categoryContainerReference,
                        masterLanguageSelector).ContentLink;
                AddLanguageVersionToTranslationItem(
                    categoryReference,
                    "CategorieVertaling Een",
                    secondLanguageSelector);

                NameValueCollection configValues = new NameValueCollection { { "containerid", "0" } };

                LocalizationProvider = new TranslationProvider();

                // Instanciate the provider
                LocalizationProvider.Initialize("Translations", configValues);

                // Add it at the end of the list of providers.
                CmsContext.ProviderBasedLocalizationService.Providers.Add(LocalizationProvider);
            };

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the fake EPiServer context.
        /// </summary>
        [NotNull]
        public static CmsContext CmsContext { get; set; }

        /// <summary>
        ///     Gets or sets the container reference.
        /// </summary>
        /// <value>The container reference.</value>
        [NotNull]
        public static ContentReference ContainerReference { get; set; }

        /// <summary>
        ///     Gets or sets localization provider.
        /// </summary>
        [NotNull]
        public static TranslationProvider LocalizationProvider { get; set; }

        /// <summary>
        ///     Gets or sets the translation reference.
        /// </summary>
        /// <value>The container reference.</value>
        [NotNull]
        public static ContentReference TranslationReference { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     Adds the language version to translation item.
        /// </summary>
        /// <param name="contentLink">The content link.</param>
        /// <param name="translation">The translation.</param>
        /// <param name="languageSelector">The language selector.</param>
        /// <returns>The <see cref="ContentReference" />.</returns>
        /// <exception cref="EPiServer.Core.ContentNotFoundException">TranslationItem not found.</exception>
        /// <exception cref="EPiServer.Core.EPiServerException">Creating a copy of the item did not succeed.</exception>
        [NotNull]
        protected static TranslationItem AddLanguageVersionToTranslationItem(
            [NotNull] ContentReference contentLink,
            [NotNull] string translation,
            [NotNull] LanguageSelector languageSelector)
        {
            // Create the base language version
            TranslationItem translationItem = CmsContext.CreateLanguageVersionOfContent<TranslationItem>(
                contentLink,
                languageSelector);

            // Change the properties that need changing for this version.
            translationItem.Translation = translation;

            return translationItem;
        }

        /// <summary>
        ///     The create translation item.
        /// </summary>
        /// <param name="originalText">The original text.</param>
        /// <param name="translation">The translation.</param>
        /// <param name="parentLink">The parent link.</param>
        /// <param name="languageSelector">The language selector.</param>
        /// <returns>The <see cref="ContentReference" />.</returns>
        [NotNull]
        protected static TranslationItem CreateTranslationItem(
            [NotNull] string originalText,
            [NotNull] string translation,
            [NotNull] ContentReference parentLink,
            [NotNull] LanguageSelector languageSelector)
        {
            // Create the base item
            TranslationItem translationItem = CmsContext.CreateContent<TranslationItem>(originalText, parentLink);

            // Set the additional properties for this type.
            translationItem.OriginalText = originalText;
            translationItem.Translation = translation;

            return translationItem;
        }

        [NotNull]
        protected static void UpdateTranslationItem(
            [NotNull] ContentReference contentLink,
            [NotNull] string translation,
            [NotNull] LanguageSelector languageSelector)
        {

            TranslationItem translationItem =
              CmsContext.ContentRepository.Get<TranslationItem>(contentLink, new LanguageSelector(CultureInfo.CurrentUICulture.Name));
            
            // Set the additional properties for this type.
            translationItem.Translation = translation;
            CmsContext.UpdateContent(translationItem, new LanguageSelector(CultureInfo.CurrentUICulture.Name));
        }

        /// <summary>
        ///     Gets the rendered text.
        /// </summary>
        /// <param name="c">
        ///     The c.
        /// </param>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        [NotNull]
        public static string GetRenderedText([NotNull] Control c)
        {
            using (HtmlTextWriter writer = new HtmlTextWriter(new StringWriter()))
            {
                c.RenderControl(writer);
                return writer.InnerWriter.ToString();
            }
        }
        #endregion
    }
}