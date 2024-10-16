using Bank.Api.ApiModels.Requests;
using Bank.Api.ApiModels.Validation;
using Bank.Common;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace Bank.Api.UnitTests;

[TestFixture]
public class CreateOrderRequestValidatorTests
{
    [SetUp]
    public void Setup()
    {
        _validator = new CreateOrderRequestValidator();
    }

    private CreateOrderRequestValidator _validator;

    [Test]
    public void Should_Have_Error_When_Amount_Is_Less_Than_100()
    {
        // Arrange
        var request = new CreateOrderRequest("123", "123 Main St", 50, Currency.USD);

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(r => r.Amount);
    }

    [Test]
    public void Should_Have_Error_When_Amount_Is_Greater_Than_100000()
    {
        // Arrange
        var request = new CreateOrderRequest("123", "123 Main St", 100001, Currency.USD);

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(r => r.Amount);
    }

    [Test]
    public void Should_Have_Error_When_ClientId_Is_Empty()
    {
        // Arrange
        var request = new CreateOrderRequest("", "123 Main St", 500, Currency.USD);

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(r => r.ClientId);
    }

    [Test]
    public void Should_Have_Error_When_DepartmentAddress_Is_Empty()
    {
        // Arrange
        var request = new CreateOrderRequest("123", "", 500, Currency.USD);

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(r => r.DepartmentAddress);
    }

    [Test]
    public void Should_Have_Error_When_Currency_Is_Invalid()
    {
        // Arrange
        var request = new CreateOrderRequest("123", "123 Main St", 500, (Currency)999);

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(r => r.Currency);
    }

    [Test]
    public void Should_Not_Have_Error_When_Request_Is_Valid()
    {
        // Arrange
        var request = new CreateOrderRequest("123", "123 Main St", 500, Currency.USD);

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}