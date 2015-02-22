using System;
using System.ComponentModel.DataAnnotations;

using EPi.Libraries.Localization.DataAnnotations;

using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.SpecializedProperties;

using Machine.Specifications.Annotations;

namespace EPi.Libraries.Localization.UnitTests.Models
{
    [ContentType(DisplayName = "StartPage", GUID = "93188469-5ee2-444b-b6b6-4194090079f6", Description = "Unittest StartPage", AvailableInEditMode = true)]
    public class StartPage : PageData
    {
        [NotNull]
        [TranslationContainer]
        [Display(GroupName = SystemTabNames.Content, Order = 1)]
        public virtual ContentReference TranslationContainer { get; set; }
    }
}