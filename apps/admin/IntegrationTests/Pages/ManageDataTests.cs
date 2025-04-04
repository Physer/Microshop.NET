﻿using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using IntegrationTests.Utilities;
using Shouldly;
using System.Net;
using Xunit;

namespace IntegrationTests.Pages;

[Collection(nameof(AdminCollectionFixture))]
public class ManageDataTests(AdminFixture fixture)
{
    private readonly AdminFixture _fixture = fixture;

    [Fact]
    public async Task ManageDataPage_WithAdminUser_ShowsContent()
    {
        // Arrange
        var expectedHeader = "Use this page to perform actions on the data of Microshop.NET";
        var expectedGenerateDataContent = "Generate data";
        var expectedClearDataContent = "Clear data";
        var applicationFactory = _fixture.ValidApplicationFactory!;
        var client = applicationFactory.CreateClient();
        _ = await _fixture.SendSignInRequestForAdminUserAsync(client);

        // Act
        var response = await client.GetAsync("/ManageData");
        var document = await HtmlHelpers.GetDocumentAsync(response);

        // Assert
        response.ShouldNotBeNull();
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        document.Body?.InnerHtml.ShouldContain(expectedHeader);
        document.Body?.InnerHtml.ShouldContain(expectedGenerateDataContent);
        document.Body?.InnerHtml.ShouldContain(expectedClearDataContent);
    }

    [Theory]
    [InlineData("Generate")]
    [InlineData("Clear")]
    public async Task Handlers_WithAnonymousUser_RedirectsToSignin(string handler)
    {
        // Arrange
        var applicationFactory = _fixture.ValidApplicationFactory!;
        var client = applicationFactory.CreateClient();

        // Act
        var response = await client.PostAsync($"/ManageData?Handler={handler}", default);

        // Assert
        response.ShouldNotBeNull();
        response.RequestMessage?.RequestUri?.PathAndQuery.ShouldBeEquivalentTo("/signin");
    }

    [Theory]
    [InlineData("Generate")]
    [InlineData("Clear")]
    public async Task Handlers_WithForbiddenUser_RedirectsToForbidden(string handler)
    {
        // Arrange
        var applicationFactory = _fixture.ValidApplicationFactory!;
        var client = applicationFactory.CreateClient();
        _ = await _fixture.SendSignInRequestForForbiddenUserAsync(client);

        // Act
        var response = await client.PostAsync($"/ManageData?Handler={handler}", default);

        // Assert
        response.ShouldNotBeNull();
        response.RequestMessage?.RequestUri?.PathAndQuery.ShouldBeEquivalentTo("/forbidden");
    }

    [Fact]
    public async Task GenerateHandler_WithAdminUser_SuccesfullyGeneratesProducts()
    {
        // Arrange
        var manageDataUrl = "/ManageData";
        var expectedMessage = "Succesfully executed at";
        var applicationFactory = _fixture.ValidApplicationFactory!;
        var client = applicationFactory.CreateClient();
        _ = await _fixture.SendSignInRequestForAdminUserAsync(client);
        var manageDataPage = await client.GetAsync(manageDataUrl);
        var manageDataPageContent = await HtmlHelpers.GetDocumentAsync(manageDataPage);
        var manageDataForm = manageDataPageContent.QuerySelector<IHtmlFormElement>("form") ?? throw new Exception("Unable to find the manage data form");
        var generateDataButton = manageDataPageContent.QuerySelector<IHtmlButtonElement>("button[id='generateDataButton']") ?? throw new Exception("Unable to find the generate data button on the manage data form");

        // Act
        var response = await client.SendAsync(manageDataForm, generateDataButton);
        var document = await HtmlHelpers.GetDocumentAsync(response);

        // Assert
        response.ShouldNotBeNull();
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        document?.Body?.InnerHtml.ShouldContain(expectedMessage);
    }

    [Fact]
    public async Task ClearDataHandler_WithAdminUser_SuccesfullyClearsData()
    {
        // Arrange
        var manageDataUrl = "/ManageData";
        var expectedMessage = "Succesfully executed at";
        var applicationFactory = _fixture.ValidApplicationFactory!;
        var client = applicationFactory.CreateClient();
        _ = await _fixture.SendSignInRequestForAdminUserAsync(client);
        var manageDataPage = await client.GetAsync(manageDataUrl);
        var manageDataPageContent = await HtmlHelpers.GetDocumentAsync(manageDataPage);
        var manageDataForm = manageDataPageContent.QuerySelector<IHtmlFormElement>("form") ?? throw new Exception("Unable to find the manage data form");
        var clearDataButton = manageDataPageContent.QuerySelector<IHtmlButtonElement>("button[id='clearDataButton']") ?? throw new Exception("Unable to find the clear data button on the manage data form");

        // Act
        var response = await client.SendAsync(manageDataForm, clearDataButton);
        var document = await HtmlHelpers.GetDocumentAsync(response);

        // Assert
        response.ShouldNotBeNull();
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        document?.Body?.InnerHtml.ShouldContain(expectedMessage);
    }
}
