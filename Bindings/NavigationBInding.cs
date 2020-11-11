﻿using System;
using System.Linq;
using Behavioral.Automation.FluentAssertions;
using Behavioral.Automation.Model;
using Behavioral.Automation.Services;
using Behavioral.Automation.Services.Mapping.Contract;
using JetBrains.Annotations;
using TechTalk.SpecFlow;

namespace Behavioral.Automation.Bindings
{
    /// <summary>
    /// Bindings for URL navigation and testing
    /// </summary>
    [Binding]
    public sealed class NavigationBinding
    {
        private readonly IDriverService _driverService;
        private readonly IScopeContextManager _scopeContextManager;

        public NavigationBinding([NotNull] IDriverService driverService,
            [NotNull] IScopeContextManager scopeContextManager)
        {
            _driverService = driverService;
            _scopeContextManager = scopeContextManager;
        }

        /// <summary>
        /// Open URL
        /// </summary>
        /// <param name="url">URL to open</param>
        /// <example>Given URL "http://test" is opened</example>
        [Given("URL \"(.*)\" is opened")]
        [When("user opens URL \"(.*)\"")]
        public void Navigate([NotNull] string url)
        {
            _driverService.Navigate(url);
        }

        /// <summary>
        /// Open Base URL from config
        /// </summary>
        /// <example>When user opens application URL</example>
        [Given("application URL is opened")]
        [When("user opens application URL")]
        public void NavigateToBaseUrl()
        {
            _driverService.Navigate(ConfigServiceBase.BaseUrl);
        }
        
        /// <summary>
        /// Open URL which is relative to the Base URL
        /// </summary>
        /// <param name="url">Relative URL to be opened</param>
        /// <example>When user opens relative URL "/test-url"</example>
        [Given("relative URL \"(.*)\" is opened")]
        [When("user opens relative URL \"(.*)\"")]
        [Then("user opens relative URL \"(.*)\"")]
        public void NavigateToRelativeUrl([NotNull] string url)
        {
            _driverService.NavigateToRelativeUrl(url);
        }

        /// <summary>
        /// Check full page URL
        /// </summary>
        /// <param name="url">Expected URL</param>
        /// <param name="behavior">Assertion behavior (instant or continuous)</param>
        /// <example>Then page "http://test" should become opened</example>
        [Then("page \"(.*)\" should (be|be not|become|become not) opened")]
        public void CheckUrl([NotNull] string url, [NotNull] AssertionBehavior behavior)
        {
            Assert.ShouldBecome(() => _driverService.CurrentUrl, url, behavior,
                $"current URL is {_driverService.CurrentUrl}");
        }

        /// <summary>
        /// Check URL which is relative to Base URL
        /// </summary>
        /// <param name="url">Expected URL</param>
        /// <param name="behavior">Assertion behavior (instant or continuous)</param>
        /// <example>Then relative URL should become "/test-page"</example>
        [Then("relative URL should (be|be not|become|become not) \"(.*)\"")]
        public void CheckRelativeUrl([NotNull] AssertionBehavior behavior, [NotNull] string url)
        {
            Assert.ShouldBecome(() => new Uri(_driverService.CurrentUrl).PathAndQuery, url, behavior,
                $"relative URL is {new Uri(_driverService.CurrentUrl).PathAndQuery}");
        }

        /// <summary>
        /// Check that page URL contains specific string
        /// </summary>
        /// <param name="behavior">Assertion behavior (instant or continuous)</param>
        /// <param name="url">Expected URL substring</param>
        /// <example>Then page should contain "test" URL</example>
        [Given("page (contains|not contains) \"(.*)\" URL")]
        [When("page (contains|not contains) \"(.*)\" URL")]
        [Then("page (should|should not) contain \"(.*)\" URL")]
        public void CheckUrlContains(string behavior, [NotNull] string url)
        {
            Assert.ShouldBecome(() => _driverService.CurrentUrl.Contains(url), !behavior.Contains("not"), $"current URL is {_driverService.CurrentUrl}");
        }

        /// <summary>
        /// Check title of the page
        /// </summary>
        /// <param name="behavior">Assertion behavior (instant or continuous)</param>
        /// <param name="title">Expected title of the page</param>
        /// <example>Then page title should be "Test page"</example>
        [Then("page title should (be|be not|become|become not) \"(.*)\"")]
        public void CheckPageTitle([NotNull] AssertionBehavior behavior, [CanBeNull] string title)
        {
            Assert.ShouldBecome(() => _driverService.Title, title, behavior,
                $"page title is {_driverService.Title}");
        }

        /// <summary>
        /// Resize opened browser window
        /// </summary>
        /// <param name="pageHeight">Desired window height</param>
        /// <param name="pageWidth">Desired window width</param>
        /// <example>When user resize window to 480 height and 640 width</example>
        [Given("user resize window to (.*) height and (.*) width")]
        [When("user resize window to (.*) height and (.*) width")]
        public void CheckOpened([NotNull] int pageHeight, [NotNull] int pageWidth)
        {
            _driverService.ResizeWindow(pageHeight, pageWidth);
        }
    }
}