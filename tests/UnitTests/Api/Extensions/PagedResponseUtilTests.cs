using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using static Api.Extensions.PagedResponseUtil;

namespace UnitTests.Api.Request.Validation;

public class PagedResponseUtilTests {

    public PagedResponseUtilTests() {

    }

    [Fact]
    [Trait("category", "api")]
    public void GetBaseUrl_Returns_Empty_When_Request_Null() {
        // Arrange
        HttpRequest? req = null;

        // Act
        var baseUrl = req.GetBaseUrl();

        // Assert
        baseUrl.Should().BeEmpty();
    }

    [Fact]
    [Trait("category", "api")]
    public void GetBaseUrl_Returns_Url_When_Valid_Request() {
        // Arrange
        var req = Substitute.For<HttpRequest>();
        req.Scheme.Returns("https");
        req.Host.Returns(new HostString("localhost:7104"));
        req.Path.Returns(new PathString("/api/Employees"));

        // Act
        var baseUrl = req.GetBaseUrl();

        // Assert
        baseUrl.Should().Be("https://localhost:7104/api/Employees");
    }

    [Fact]
    [Trait("category", "api")]
    public void BuildPagedUrl_Returns_Valid_Uri_With_Params() {
        // Arrange
        var baseUrl = "https://localhost:7104/api/Employee/GetEmployees";
        int pageNumber = 1;
        int pageSize = 10;

        // Act
        var uri = baseUrl.BuildPagedUrl(pageNumber, pageSize);

        // Assert
        uri.Should().Be("https://localhost:7104/api/Employee/GetEmployees?pageNumber=1&pageSize=10");
    }
}
