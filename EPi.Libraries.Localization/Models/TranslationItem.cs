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
namespace EPi.Libraries.Localization.Models
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;

    using EPiServer;
    using EPiServer.Core;
    using EPiServer.DataAbstraction;
    using EPiServer.DataAnnotations;
    using EPiServer.ServiceLocation;

    using Newtonsoft.Json;

    /// <summary>
    ///     The translation PageType.
    /// </summary>
    [ContentType(GUID = "{A691F851-6C6E-4C06-B62E-8FBC5A038A68}", AvailableInEditMode = true,
        Description = "Translation", DisplayName = "Translation", GroupName = "Localization")]
    [ImageUrl("~/Content/images/translation-thumbnail.png")]
    [AvailableContentTypes(Exclude = new[] { typeof(PageData) })]
    public class TranslationItem : PageData
    {
        /// <summary>
        ///     Gets the lookup key this item.
        /// </summary>
        /// <remarks>
        ///     Only for normal translation items. Displays the path to use in e.g. the translate control.
        ///     <![CDATA[
        ///         <EPiServer:Translate runat="server" Text="/jeroenstemerdink/textone" />
        ///     ]]>
        /// </remarks>
        [JsonIgnore]
        public string LookupKey
        {
            get
            {
                // If this is a category translation, no need to calculate a path.
                CategoryTranslationContainer categoryTranslationContainer;
                this.ContentRepository.Service.TryGet(this.ParentLink, out categoryTranslationContainer);

                if (categoryTranslationContainer != null)
                {
                    return string.Format(
                        CultureInfo.InvariantCulture,
                        "/categories/category[@name=\"{0}\"]/description",
                        this.OriginalText);
                }

                // Use the masterlanguage branch, that one is always available.
                TranslationItem masterLanguagePage;

                if (!this.ContentRepository.Service.TryGet(
                    this.PageLink,
                    this.MasterLanguage, out masterLanguagePage))
                {
                    return "-";
                }

                // Get the ancestors
                IEnumerable<IContent> ancestors =
                    this.ContentRepository.Service.GetAncestors(masterLanguagePage.PageLink).Reverse();

                // Get all translation containers, skip the main one.
                List<string> keyParts =
                    ancestors.OfType<TranslationContainer>()
                        .Select(
                            ancestor =>
                            Regex.Replace(ancestor.ContainerName.ToLowerInvariant(), @"[^A-Za-z0-9]+", string.Empty))
                        .Skip(1)
                        .ToList();

                // Add this file
                keyParts.Add(Regex.Replace(this.OriginalText.ToLowerInvariant(), @"[^A-Za-z0-9]+", string.Empty));

                return string.Format(CultureInfo.InvariantCulture, "/{0}", string.Join("/", keyParts));
            }
        }

        /// <summary>
        ///     Gets the missing translations for this item.
        /// </summary>
        [JsonIgnore]
        public ReadOnlyCollection<string> MissingValues
        {
            get
            {
                return TranslationFactory.Instance.GetMissingValues(this.PageLink);
            }
        }

        /// <summary>
        ///     Gets or sets the original text.
        /// </summary>
        /// <remarks>
        ///     To reference this use the text without spaces and special characters, all lower case.
        ///     So if the OriginalText is e.g. Text One, the key in the xml will be textone.
        ///     Note that this is only for normal translations. For translations beneath a
        ///     <see
        ///         cref="CategoryTranslationContainer" />
        ///     the OriginalText text will be used, as the generated xml is different for Category translations.
        ///     You can display the translated category name on a page by using the LocalizedDescription property of the Category.
        /// </remarks>
        /// <example>
        ///     <![CDATA[
        ///         <EPiServer:Translate runat="server" Text="/jeroenstemerdink/textone" />
        ///     ]]>
        /// </example>
        [Display(GroupName = SystemTabNames.Content, Description = "The text to translate.", Name = "Original text")]
        [CultureSpecific(false)]
        [Required(AllowEmptyStrings = false)]
        public virtual string OriginalText { get; set; }

        /// <summary>
        ///     Gets the translated values for this item.
        /// </summary>
        [JsonIgnore]
        public Dictionary<string, string> TranslatedValues
        {
            get
            {
                return TranslationFactory.Instance.GetTranslatedValues(this.PageLink);
            }
        }

        /// <summary>
        ///     Gets or sets the translation.
        /// </summary>
        [Display(GroupName = SystemTabNames.Content, Description = "The translation of the original text.",
            Name = "Translation")]
        [CultureSpecific]
        [Required(AllowEmptyStrings = false)]
        public virtual string Translation { get; set; }

        /// <summary>
        ///     Gets or sets the content repository.
        /// </summary>
        /// <value>The content repository.</value>
        protected Injected<IContentRepository> ContentRepository { get; set; }

        /// <summary>
        ///     Gets or sets the language branch repository.
        /// </summary>
        /// <value>The language branch repository.</value>
        protected Injected<ILanguageBranchRepository> LanguageBranchRepository { get; set; }

        /// <summary>
        ///     Sets the default property values on the page data.
        /// </summary>
        /// <param name="contentType">
        ///     The type of content.
        /// </param>
        /// <example>
        ///     <code source="../CodeSamples/EPiServer/Core/PageDataSamples.aspx.cs" region="DefaultValues">
        /// </code>
        /// </example>
        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            this.VisibleInMenu = false;
        }
    }
}