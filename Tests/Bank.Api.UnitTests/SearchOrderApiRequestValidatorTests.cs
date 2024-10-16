using Bank.Api.ApiModels.Requests;
using Bank.Api.ApiModels.Validation;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace Bank.Api.UnitTests;

[TestFixture]
public class SearchOrderApiRequestValidatorTests
{
    [SetUp]
    public void Setup()
    {
        _validator = new SearchOrderApiRequestValidator();
    }

    private SearchOrderApiRequestValidator _validator;

    [Test]
    public void Should_Have_Error_When_All_Fields_Are_Null()
    {
        // Arrange
        var request = new SearchOrderApiRequest();

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(r => r.OrderId);
    }

    [Test]
    public void Should_Have_Error_When_OrderId_Is_Null_And_DepartmentAddress_And_ClientId_Are_Empty()
    {
        // Arrange
        var request = new SearchOrderApiRequest("", "");

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(r => r.OrderId);
    }

    [Test]
    public void Should_Have_Error_When_OrderId_Is_Null_And_DepartmentAddress_Is_Empty()
    {
        // Arrange
        var request = new SearchOrderApiRequest("123", "");

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(r => r.DepartmentAddress);
    }

    [Test]
    public void Should_Have_Error_When_OrderId_Is_Null_And_ClientId_Is_Empty()
    {
        // Arrange
        var request = new SearchOrderApiRequest("", "123 Main St");

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(r => r.ClientId);
    }

    [Test]
    public void Should_Not_Have_Error_When_OrderId_Is_Provided()
    {
        // Arrange
        var request = new SearchOrderApiRequest(null, null, 1);

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void Should_Not_Have_Error_When_DepartmentAddress_And_ClientId_Are_Provided_And_OrderId_Is_Null()
    {
        // Arrange
        var request = new SearchOrderApiRequest("123", "123 Main St");

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void Should_Not_Have_Error_When_All_Fields_Are_Provided()
    {
        // Arrange
        var request = new SearchOrderApiRequest("123", "123 Main St", 1);

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}