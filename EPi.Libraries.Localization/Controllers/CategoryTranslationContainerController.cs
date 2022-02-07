﻿// Copyright © 2017 Jeroen Stemerdink.
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
namespace EPi.Libraries.Localization.Controllers
{
    using EPi.Libraries.Localization.Models;

    using EPiServer.Web;
    using EPiServer.Web.Mvc;

    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    ///     Class CategoryTranslationContainerController.
    /// </summary>
    public class CategoryTranslationContainerController : PageController<CategoryTranslationContainer>
    {
        /// <summary>
        ///     The default view.
        /// </summary>
        /// <param name="currentPage">The current page.</param>
        /// <returns>The ActionResult.</returns>
        public ActionResult Index(CategoryTranslationContainer currentPage)
        {
            return this.PartialView(this.CreateViewPath(nameof(Index)), currentPage);
        }

        private string CreateViewPath(string action) => UriUtil.Combine("/CmsUIViews/Views/CategoryTranslationContainer/", action + ".cshtml");
    }
}