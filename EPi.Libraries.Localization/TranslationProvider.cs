// Copyright © 2017 Jeroen Stemerdink.
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
namespace EPi.Libraries.Localization
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Linq;
    using System.Threading;

    using EPi.Libraries.Localization.Models;

    using EPiServer;
    using EPiServer.Core;
    using EPiServer.DataAbstraction;
    using EPiServer.Framework.Localization;
    using EPiServer.Framework.Localization.Internal;
    using EPiServer.ServiceLocation;

    /// <summary>
    ///     The translation provider.
    /// </summary>
    public class TranslationProvider : MemoryLocalizationProvider, IDisposable
    {
        /// <summary>
        /// The cache lock
        /// </summary>
        private readonly ReaderWriterLockSlim cacheLock = new ReaderWriterLockSlim(
            LockRecursionPolicy.SupportsRecursion);

        /// <summary>
        /// Indicate whether the provider has been disposed.
        /// </summary>
        private bool isDisposed;

        /// <summary>
        ///     Finalizes an instance of the <see cref="TranslationProvider" /> class.
        /// </summary>
        ~TranslationProvider()
        {
            this.Dispose(false);
        }

        /// <summary>
        ///     Gets all available languages from the translation container.
        ///     An available language does not need to contain any translations.
        /// </summary>
        public override IEnumerable<CultureInfo> AvailableLanguages
        {
            get
            {
                return this.LanguageBranchRepository.Service.ListEnabled().Select(p => p.Culture);
            }
        }

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
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <exception cref="SynchronizationLockException">
        ///         <see cref="P:System.Threading.ReaderWriterLockSlim.WaitingReadCount" /> is greater than zero. -or-<see cref="P:System.Threading.ReaderWriterLockSlim.WaitingUpgradeCount" /> is greater than zero. -or-<see cref="P:System.Threading.ReaderWriterLockSlim.WaitingWriteCount" /> is greater than zero. </exception>
        /// <exception cref="ArgumentNullException">Provider is null. </exception>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Gets all localized strings for the specified culture below the specified key.
        /// </summary>
        /// <param name="originalKey">The original key that was passed into any GetString method.</param>
        /// <param name="normalizedKey">The <paramref name="originalKey" /> normalized and split into an array</param>
        /// <param name="culture">The requested culture for the localized strings.</param>
        /// <returns>All localized strings below the specified key.</returns>
        /// <seealso
        ///     cref="M:EPiServer.Framework.Localization.LocalizationService.GetStringByCulture(System.String,System.Globalization.CultureInfo)" />
        /// <exception cref="LockRecursionException">The <see cref="P:System.Threading.ReaderWriterLockSlim.RecursionPolicy" /> property is <see cref="F:System.Threading.LockRecursionPolicy.NoRecursion" /> and the current thread has already entered read mode. -or-The current thread may not acquire the read lock when it already holds the write lock. -or-The recursion number would exceed the capacity of the counter. This limit is so large that applications should never encounter it. </exception>
        /// <exception cref="ObjectDisposedException">The <see cref="T:System.Threading.ReaderWriterLockSlim" /> object has been disposed. </exception>
        /// <exception cref="SynchronizationLockException">The current thread has not entered the lock in read mode. </exception>
        public override IEnumerable<ResourceItem> GetAllStrings(
            string originalKey,
            string[] normalizedKey,
            CultureInfo culture)
        {
            IEnumerable<ResourceItem> allStrings;

            this.cacheLock.EnterReadLock();

            try
            {
                allStrings = base.GetAllStrings(originalKey, normalizedKey, culture);
            }
            finally
            {
                this.cacheLock.ExitReadLock();
            }

            return allStrings;
        }

        /// <summary>
        ///     Gets the localized string for the specified key in the specified culture.
        /// </summary>
        /// <param name="originalKey">The original key that was passed into any GetString method.</param>
        /// <param name="normalizedKey">The <paramref name="originalKey" /> normalized and split into an array</param>
        /// <param name="culture">The requested culture for the localized string.</param>
        /// <returns>A localized string or <c>null</c> if no resource is found for the given key and culture.</returns>
        /// <seealso
        ///     cref="M:EPiServer.Framework.Localization.LocalizationService.GetStringByCulture(System.String,System.Globalization.CultureInfo)" />
        /// <exception cref="LockRecursionException">The <see cref="P:System.Threading.ReaderWriterLockSlim.RecursionPolicy" /> property is <see cref="F:System.Threading.LockRecursionPolicy.NoRecursion" /> and the current thread has already entered read mode. -or-The current thread may not acquire the read lock when it already holds the write lock. -or-The recursion number would exceed the capacity of the counter. This limit is so large that applications should never encounter it. </exception>
        /// <exception cref="ObjectDisposedException">The <see cref="T:System.Threading.ReaderWriterLockSlim" /> object has been disposed. </exception>
        /// <exception cref="SynchronizationLockException">The current thread has not entered the lock in read mode. </exception>
        public override string GetString(string originalKey, string[] normalizedKey, CultureInfo culture)
        {
            string translation;

            this.cacheLock.EnterReadLock();

            try
            {
                translation = base.GetString(originalKey, normalizedKey, culture);
            }
            finally
            {
                this.cacheLock.ExitReadLock();
            }

            return translation;
        }

        /// <summary>
        ///     Initializes the provider.
        /// </summary>
        /// <param name="name">
        ///     The friendly name of the provider.
        /// </param>
        /// <param name="config">
        ///     A collection of the name/value pairs representing the provider-specific attributes specified in the configuration
        ///     for this provider.
        /// </param>
        /// <exception cref="ArgumentNullException">The name of the provider is null.</exception>
        /// <exception cref="ArgumentException">The name of the provider has a length of zero.</exception>
        /// <exception cref="InvalidOperationException">An attempt is made to call <see cref="M:System.Configuration.Provider.ProviderBase.Initialize(System.String,System.Collections.Specialized.NameValueCollection)" /> on a provider after the provider has already been initialized.</exception>
        /// <exception cref="LockRecursionException">The <see cref="P:System.Threading.ReaderWriterLockSlim.RecursionPolicy" /> property is <see cref="F:System.Threading.LockRecursionPolicy.NoRecursion" /> and the current thread has already entered the lock in any mode. -or-The current thread has entered read mode, so trying to enter the lock in write mode would create the possibility of a deadlock. -or-The recursion number would exceed the capacity of the counter. The limit is so large that applications should never encounter it.</exception>
        /// <exception cref="ObjectDisposedException">The <see cref="T:System.Threading.ReaderWriterLockSlim" /> object has been disposed. </exception>
        /// <exception cref="SynchronizationLockException">The current thread has not entered the lock in write mode.</exception>
        public override void Initialize(string name, NameValueCollection config)
        {
            base.Initialize(name, config);

            this.cacheLock.EnterWriteLock();

            try
            {
                this.LoadTranslations();
            }
            finally
            {
                this.cacheLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Updates the translations.
        /// </summary>
        /// <exception cref="SynchronizationLockException">The current thread has not entered the lock in write mode.</exception>
        /// <exception cref="LockRecursionException">The <see cref="P:System.Threading.ReaderWriterLockSlim.RecursionPolicy" /> property is <see cref="F:System.Threading.LockRecursionPolicy.NoRecursion" /> and the current thread has already entered the lock in any mode. -or-The current thread has entered read mode, so trying to enter the lock in write mode would create the possibility of a deadlock. -or-The recursion number would exceed the capacity of the counter. The limit is so large that applications should never encounter it.</exception>
        /// <exception cref="ObjectDisposedException">The <see cref="T:System.Threading.ReaderWriterLockSlim" /> object has been disposed. </exception>
        public void UpdateTranslations()
        {
            this.cacheLock.EnterWriteLock();

            try
            {
                this.ClearStrings();
                this.LoadTranslations();
            }
            finally
            {
                this.cacheLock.ExitWriteLock();
            }
        }

        /// <summary>
        ///     Disposes the specified disposing.
        /// </summary>
        /// <param name="disposing">The disposing.</param>
        /// <exception cref="SynchronizationLockException">
        ///         <see cref="P:System.Threading.ReaderWriterLockSlim.WaitingReadCount" /> is greater than zero. -or-<see cref="P:System.Threading.ReaderWriterLockSlim.WaitingUpgradeCount" /> is greater than zero. -or-<see cref="P:System.Threading.ReaderWriterLockSlim.WaitingWriteCount" /> is greater than zero. </exception>
        protected virtual void Dispose(bool disposing)
        {
            if (this.isDisposed)
            {
                return;
            }

            if (this.cacheLock != null)
            {
                this.cacheLock.Dispose();
            }

            this.isDisposed = true;
        }

        /// <summary>
        ///     Adds the key.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="cultureInfo">The culture information.</param>
        private void AddKey(ContentReference container, CultureInfo cultureInfo)
        {
            if (ContentReference.IsNullOrEmpty(container))
            {
                return;
            }

            List<PageData> children =
                this.ContentRepository.Service.GetChildren<PageData>(container, cultureInfo).ToList();

            foreach (PageData child in children)
            {
                TranslationContainer translationContainer = child as TranslationContainer;

                if (translationContainer != null)
                {
                    this.AddKey(child.PageLink, cultureInfo);
                }

                CategoryTranslationContainer categoryTranslationContainer = child as CategoryTranslationContainer;

                if (categoryTranslationContainer != null)
                {
                    this.AddKey(child.PageLink, cultureInfo);
                }

                TranslationItem translationItem = child as TranslationItem;

                if (translationItem != null)
                {
                    this.AddString(cultureInfo, translationItem.LookupKey, translationItem.Translation);
                }
            }
        }

        /// <summary>
        ///     Load the translations.
        /// </summary>
        private void LoadTranslations()
        {
            List<CultureInfo> availableLanguages = this.AvailableLanguages.ToList();

            foreach (CultureInfo cultureInfo in availableLanguages)
            {
                this.AddKey(TranslationFactory.Instance.TranslationContainerReference, cultureInfo);
            }
        }
    }
}