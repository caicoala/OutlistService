using Moq;
using OutlistService.Application.UseCases;
using OutlistService.Domain.Entities;
using OutlistService.Domain.Interfaces;

public class OutlistUseCaseServiceBDDTests
{
    private readonly Mock<IOutlistRepository> _repositoryMock;
    private readonly OutlistUseCaseService _service;

    public OutlistUseCaseServiceBDDTests()
    {
        _repositoryMock = new Mock<IOutlistRepository>();
        _service = new OutlistUseCaseService(_repositoryMock.Object);
    }

    [Fact]
    public async Task GivenValidProduct_WhenAddAsyncCalled_ThenRepositoryAddAsyncIsCalled()
    {
        // Given
        var product = new OutlistProduct
        {
            ProductCode = "ABC123",
            ValidFrom = DateTime.UtcNow,
            ValidTo = DateTime.UtcNow.AddDays(5)
        };

        // When
        await _service.AddAsync(product);

        // Then
        _repositoryMock.Verify(r => r.AddAsync(product), Times.Once);
    }

    [Fact]
    public async Task GivenProductCodeExists_WhenGetByProductCodeAsyncCalled_ThenReturnsProduct()
    {
        // Given
        var productCode = "ABC123";
        var expectedProduct = new OutlistProduct
        {
            ProductCode = productCode,
            ValidFrom = DateTime.UtcNow,
            ValidTo = DateTime.UtcNow.AddDays(10)
        };

        _repositoryMock.Setup(r => r.GetByProductCodeAsync(productCode))
                       .ReturnsAsync(expectedProduct);

        // When
        var result = await _service.GetByProductCodeAsync(productCode);

        // Then
        Assert.NotNull(result);
        Assert.Equal(productCode, result.ProductCode);
    }

    [Fact]
    public async Task GivenNonExistentProductCode_WhenUpdateValidityCalled_ThenThrowsKeyNotFoundException()
    {
        // Given
        var code = "NOTFOUND";
        var validFrom = DateTime.UtcNow;
        var validTo = DateTime.UtcNow.AddDays(3);

        _repositoryMock.Setup(r => r.GetByProductCodeAsync(code))
                       .ReturnsAsync((OutlistProduct?)null);

        // When & Then
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _service.UpdateValidityAsync(code, validFrom, validTo));

        Assert.Equal($"Produto com código '{code}' não encontrado.", exception.Message);
        _repositoryMock.Verify(r =>
            r.UpdateValidityAsync(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()),
            Times.Never);
    }

    [Fact]
    public async Task Given25Products_WhenGetPagedAsyncCalledWithPage2AndPageSize10_ThenReturnsCorrectPage()
    {
        // Given
        var products = new List<OutlistProduct>();
        for (int i = 1; i <= 25; i++)
        {
            products.Add(new OutlistProduct
            {
                ProductCode = i.ToString(),
                ValidFrom = DateTime.UtcNow.AddDays(-i),
                ValidTo = DateTime.UtcNow.AddDays(i)
            });
        }

        _repositoryMock.Setup(r => r.GetPagedAsync(1, int.MaxValue))
                       .ReturnsAsync(products);

        // When
        var result = await _service.GetPagedAsync(2, 10);

        // Then
        Assert.Equal(10, result.Items.Count);
        Assert.Equal("11", result.Items[0].ProductCode);
        Assert.Equal(3, result.TotalPages);
        Assert.Equal(2, result.CurrentPage);
    }
}
