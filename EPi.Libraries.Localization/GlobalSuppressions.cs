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
using System.Diagnostics.CodeAnalysis;

[assembly:
    SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace",
        Target = "EPiServer.Libraries.Localization.Models")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace",
        Target = "EPiServer.Libraries.Localization")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member",
        Target = "EPiServer.Libraries.Localization.TranslationFactory.#GetTranslationContainer()")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member",
        Target = "EPiServer.Libraries.Localization.TranslationFactory.#GetXDocument()")]
[assembly:
    SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Scope = "member",
        Target =
            "EPiServer.Libraries.Localization.TranslationFactory.#AddElement(System.Xml.XmlWriter,EPiServer.Core.ContentReference,System.Globalization.CultureInfo)"
        )]
[assembly:
    SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope = "member",
        Target = "EPiServer.Libraries.Localization.TranslationFactory.#GetTranslationContainer()")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member",
        Target = "EPiServer.Libraries.Localization.TranslationFactory.#GetXDocument()")]
[assembly:
    SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Scope = "member",
        Target = "EPiServer.Libraries.Localization.Models.TranslationItem.#LookupKey")]
[assembly:
    SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters",
        MessageId = "EPiServer.Libraries.Localization.TranslationProviderInitialization.RaiseEvent(System.String)",
        Scope = "member",
        Target =
            "EPiServer.Libraries.Localization.TranslationProviderInitialization.#InstanceChangedPage(System.Object,EPiServer.PageEventArgs)"
        )]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member",
        Target =
            "EPiServer.Libraries.Localization.TranslationProvider.#TryGetString(System.String,System.String[],System.Globalization.CultureInfo,System.String&)"
        )]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Scope = "member",
        Target = "EPiServer.Libraries.Localization.Models.BingAccessToken.#access_token")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "token",
        Scope = "member", Target = "EPiServer.Libraries.Localization.Models.BingAccessToken.#access_token")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "access",
        Scope = "member", Target = "EPiServer.Libraries.Localization.Models.BingAccessToken.#access_token")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "in", Scope = "member",
        Target = "EPiServer.Libraries.Localization.Models.BingAccessToken.#expires_in")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Scope = "member",
        Target = "EPiServer.Libraries.Localization.Models.BingAccessToken.#expires_in")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "expires",
        Scope = "member", Target = "EPiServer.Libraries.Localization.Models.BingAccessToken.#expires_in")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "scope",
        Scope = "member", Target = "EPiServer.Libraries.Localization.Models.BingAccessToken.#scope")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Scope = "member",
        Target = "EPiServer.Libraries.Localization.Models.BingAccessToken.#token_type")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "type", Scope = "member",
        Target = "EPiServer.Libraries.Localization.Models.BingAccessToken.#token_type")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "token",
        Scope = "member", Target = "EPiServer.Libraries.Localization.Models.BingAccessToken.#token_type")]
[assembly:
    SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID", Scope = "member",
        Target = "EPiServer.Libraries.Localization.TranslationFactory.#BingClientID")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member",
        Target =
            "EPiServer.Libraries.Localization.TranslationFactory.#BingTranslate(System.String,System.String,System.String)"
        )]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member",
        Target = "EPiServer.Libraries.Localization.TranslationFactory.#GetAccesToken()")]
[assembly:
    SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters",
        MessageId = "EPiServer.Libraries.Localization.TranslationProviderInitialization.RaiseEvent(System.String)",
        Scope = "member",
        Target =
            "EPiServer.Libraries.Localization.TranslationProviderInitialization.#InstancePublishedPage(System.Object,EPiServer.PageEventArgs)"
        )]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member",
        Target = "EPi.Libraries.Localization.TranslationFactory.#GetXDocument()")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member",
        Target = "EPi.Libraries.Localization.TranslationFactory.#GetXDocument()")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member",
        Target =
            "EPi.Libraries.Localization.TranslationProvider.#TryGetString(System.String,System.String[],System.Globalization.CultureInfo,System.String&)"
        )]
[assembly:
    SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters",
        MessageId = "EPi.Libraries.Localization.TranslationProviderInitialization.RaiseEvent(System.String)",
        Scope = "member",
        Target =
            "EPi.Libraries.Localization.TranslationProviderInitialization.#InstanceChangedPage(System.Object,EPiServer.PageEventArgs)"
        )]
[assembly:
    SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters",
        MessageId = "EPi.Libraries.Localization.TranslationProviderInitialization.RaiseEvent(System.String)",
        Scope = "member",
        Target =
            "EPi.Libraries.Localization.TranslationProviderInitialization.#InstancePublishedPage(System.Object,EPiServer.PageEventArgs)"
        )]
[assembly:
    SuppressMessage("Microsoft.Globalization", "CA1304:SpecifyCultureInfo",
        MessageId = "EPiServer.Framework.Localization.MemoryLocalizationProvider.ClearStrings", Scope = "member",
        Target = "EPi.Libraries.Localization.TranslationProviderInitialization.#UpdateTranslations()")]
[assembly:
    SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters",
        MessageId = "EPi.Libraries.Localization.TranslationProviderInitialization.RaiseEvent(System.String)",
        Scope = "member",
        Target =
            "EPi.Libraries.Localization.TranslationProviderInitialization.#InstancePublishedContent(System.Object,EPiServer.ContentEventArgs)"
        )]
[assembly:
    SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters",
        MessageId = "EPi.Libraries.Localization.TranslationProviderInitialization.RaiseEvent(System.String)",
        Scope = "member",
        Target =
            "EPi.Libraries.Localization.TranslationProviderInitialization.#InstanceChangedContent(System.Object,EPiServer.ContentEventArgs)"
        )]
[assembly:
    SuppressMessage("Microsoft.Globalization", "CA1304:SpecifyCultureInfo",
        MessageId = "EPiServer.Framework.Localization.MemoryLocalizationProvider.ClearStrings", Scope = "member",
        Target = "EPi.Libraries.Localization.TranslationProvider.#UpdateTranslations()")]
[assembly:
    SuppressMessage("Microsoft.Globalization", "CA1304:SpecifyCultureInfo",
        MessageId = "EPi.Libraries.Localization.TranslationProvider.UpdateTranslations", Scope = "member",
        Target = "EPi.Libraries.Localization.TranslationProviderInitialization.#UpdateTranslations()")]
[assembly:
    SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters",
        MessageId =
            "EPiServer.Logging.LoggerExtensions.Information(EPiServer.Logging.ILogger,System.String,System.Exception)",
        Scope = "member", Target = "EPi.Libraries.Localization.TranslationFactory.#TranslationService")]
[assembly:
    SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters",
        MessageId = "EPiServer.Logging.LoggerExtensions.Information(EPiServer.Logging.ILogger,System.String)",
        Scope = "member", Target = "EPi.Libraries.Localization.TranslationFactory.#GetTranslationContainer()")]
[assembly:
    SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters",
        MessageId = "EPiServer.Logging.LoggerExtensions.Information(EPiServer.Logging.ILogger,System.String)",
        Scope = "member",
        Target =
            "EPi.Libraries.Localization.TranslationProviderInitialization.#Initialize(EPiServer.Framework.Initialization.InitializationEngine)"
        )]
[assembly:
    SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters",
        MessageId = "EPiServer.Logging.LoggerExtensions.Information(EPiServer.Logging.ILogger,System.String)",
        Scope = "member",
        Target =
            "EPi.Libraries.Localization.TranslationProviderInitialization.#Uninitialize(EPiServer.Framework.Initialization.InitializationEngine)"
        )]
[assembly:
    SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters",
        MessageId = "EPiServer.Logging.LoggerExtensions.Error(EPiServer.Logging.ILogger,System.String,System.Exception)",
        Scope = "member", Target = "EPi.Libraries.Localization.TranslationProviderInitialization.#LoadProvider()")]
[assembly:
    SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters",
        MessageId = "EPiServer.Logging.LoggerExtensions.Information(EPiServer.Logging.ILogger,System.String)",
        Scope = "member",
        Target =
            "EPi.Libraries.Localization.TranslationProviderInitialization.#TranslationsUpdatedEventRaised(System.Object,EPiServer.Events.EventNotificationEventArgs)"
        )]
[assembly:
    SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters",
        MessageId = "EPiServer.Logging.LoggerExtensions.Information(EPiServer.Logging.ILogger,System.String)",
        Scope = "member", Target = "EPi.Libraries.Localization.TranslationProviderInitialization.#UpdateTranslations()")
]
[assembly: SuppressMessage("Microsoft.Usage", "CA2243:AttributeStringLiteralsShouldParseCorrectly")]
[assembly: SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "EPiServer.Logging.LoggerExtensions.Error(EPiServer.Logging.ILogger,System.String,System.Exception)", Scope = "member", Target = "EPi.Libraries.Localization.TranslationFactory.#HasAttribute`1(System.Reflection.PropertyInfo)")]
[assembly: SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "EPiServer.Logging.LoggerExtensions.Error(EPiServer.Logging.ILogger,System.String,System.Exception)", Scope = "member", Target = "EPi.Libraries.Localization.TranslationProviderInitialization.#UpdateTranslations()")]
[assembly: SuppressMessage("Microsoft.Design", "CA1016:MarkAssembliesWithAssemblyVersion")]
[assembly: SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "EPi.Libraries.Localization.TranslationFactory.#HasAttribute`1(System.Reflection.PropertyInfo)")]
[assembly: SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "EPi.Libraries.Localization.TranslationProviderInitialization.#LoadProvider()")]
[assembly: SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "EPi.Libraries.Localization.TranslationProviderInitialization.#UpdateTranslations()")]
[assembly: SuppressMessage("Microsoft.Globalization", "CA1304:SpecifyCultureInfo", MessageId = "EPiServer.Framework.Localization.Internal.MemoryLocalizationProvider.ClearStrings", Scope = "member", Target = "EPi.Libraries.Localization.TranslationProvider.#UpdateTranslations()")]

