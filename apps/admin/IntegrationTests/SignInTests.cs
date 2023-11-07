﻿using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using IntegrationTests.Utilities;
using System.Net;
using Web.Pages;
using Xunit;

namespace IntegrationTests;

public class SignInTests : IClassFixture<AdminTestsFixture>
{
    private readonly AdminTestsFixture _fixture;
    private readonly string _signInUrl;

    public SignInTests(AdminTestsFixture fixture)
    {
        _fixture = fixture;
        _signInUrl = "/signin";
    }

    [Fact]
    public async Task SignInPage_ForAnonymousUser_ReturnsOk()
    {
        // Arrange
        var url = "/signin";
        var applicationFactory = _fixture.ApplicationFactory!;
        var client = applicationFactory.CreateClient();

        // Act
        var response = await client.GetAsync(url);

        // Assert
        response.Should().NotBeNull();
        response.RequestMessage?.RequestUri?.PathAndQuery.Should().BeEquivalentTo(url);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory]
    [InlineData("/")]
    [InlineData("/Index")]
    [InlineData("/ManageData")]
    [InlineData("/ManageUsers")]
    public async Task ProtectedPages_ForAnonymousUser_RedirectsToSignin(string url)
    {
        // Arrange
        var applicationFactory = _fixture.ApplicationFactory!;
        var client = applicationFactory.CreateClient();

        // Act
        var response = await client.GetAsync(url);

        // Assert
        response.Should().NotBeNull();
        response.RequestMessage?.RequestUri?.PathAndQuery.Should().BeEquivalentTo(_signInUrl);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task SignInPage_WithWrongCredentials_ShowsError()
    {
        // Arrange
        var expectedErrorMessage = "Invalid credentials or permissions";
        var applicationFactory = _fixture.ApplicationFactory!;
        var client = applicationFactory.CreateClient();
        var signInPage = await client.GetAsync(_signInUrl);
        var content = await HtmlHelpers.GetDocumentAsync(signInPage);
        var form = content.QuerySelector<IHtmlFormElement>("form");
        var submitButton = content.QuerySelector<IHtmlInputElement>("input[id='signInButton']");

        List<KeyValuePair<string, string>> formValues = new()
        {
            { new(nameof(SignInModel.Username), Constants.DefaultTextValue) },
            { new(nameof(SignInModel.Password), Constants.DefaultTextValue) }
        };

        // Act
        var response = await client.SendAsync(form, submitButton, formValues);
        var responseContent = await HtmlHelpers.GetDocumentAsync(response);
        var errorAlert = responseContent.QuerySelector<IHtmlDivElement>("div[id='errorAlert']");

        // Assert
        errorAlert?.InnerHtml.Should().Be(expectedErrorMessage);
    }

    [Fact]
    public async Task SignInPage_WithValidCredentialsAndInvalidPermissions_RedirectsToForbidden()
    {
        // Arrange
        var expectedResponseUrl = "http://localhost/Forbidden";
        var applicationFactory = _fixture.ApplicationFactory!;
        var client = applicationFactory.CreateClient();
        var signInPage = await client.GetAsync(_signInUrl);
        var content = await HtmlHelpers.GetDocumentAsync(signInPage);
        var form = content.QuerySelector<IHtmlFormElement>("form");
        var submitButton = content.QuerySelector<IHtmlInputElement>("input[id='signInButton']");
        var username = Constants.DefaultEmailValue;
        var password = Constants.DefaultPasswordValue;
        await _fixture.CreateIntegrationTestsUser(username, password, false);

        List<KeyValuePair<string, string>> formValues = new()
        {
            { new(nameof(SignInModel.Username), username) },
            { new(nameof(SignInModel.Password), password) }
        };

        // Act
        var response = await client.SendAsync(form, submitButton, formValues);
        var responseContent = await HtmlHelpers.GetDocumentAsync(response);

        // Assert
        responseContent?.Url.Should().Be(expectedResponseUrl);
    }

    [Fact]
    public async Task SignInPage_WithValidCredentialsAndValidPermissions_RedirectsToIndex()
    {
        // Arrange
        var expectedResponseUrl = "http://localhost/";
        var applicationFactory = _fixture.ApplicationFactory!;
        var client = applicationFactory.CreateClient();
        var signInPage = await client.GetAsync(_signInUrl);
        var content = await HtmlHelpers.GetDocumentAsync(signInPage);
        var form = content.QuerySelector<IHtmlFormElement>("form");
        var submitButton = content.QuerySelector<IHtmlInputElement>("input[id='signInButton']");
        var username = Constants.DefaultEmailValue;
        var password = Constants.DefaultPasswordValue;
        await _fixture.CreateIntegrationTestsUser(username, password, true);

        List<KeyValuePair<string, string>> formValues = new()
        {
            { new(nameof(SignInModel.Username), username) },
            { new(nameof(SignInModel.Password), password) }
        };

        // Act
        var response = await client.SendAsync(form, submitButton, formValues);
        var responseContent = await HtmlHelpers.GetDocumentAsync(response);

        // Assert
        responseContent?.Url.Should().Be(expectedResponseUrl);
    }
}
