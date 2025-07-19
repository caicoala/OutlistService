using Moq;
using OutlistService.Application.DTOs;
using OutlistService.Application.UseCases;
using OutlistService.Domain.Entities;
using OutlistService.Domain.Interfaces;
using Xunit;

public class OutlistUseCaseServiceTests
{
    private readonly Mock<IOutlistRepository> _repositoryMock;
    private readonly OutlistUseCaseService _service;

    public OutlistUseCaseServiceTests()
    {
        _repositoryMock = new Mock<IOutlistRepository>();
        _service = new OutlistUseCaseService(_repositoryMock.Object);
    }

    [Fact]
    public async Task GetPagedAsync_ShouldReturnPagedProducts()
    {
        var mockList = new List<OutlistProduct>
        {
            new OutlistProduct
            {
                ProductCode = "1",
                ValidFrom = DateTime.UtcNow.AddDays(-1),
                ValidTo = DateTime.UtcNow.AddDays(1)
            }
        };

        _repositoryMock
            .Setup(r => r.GetPagedAsync(1, int.MaxValue))
            .ReturnsAsync(mockList);

        var result = await _service.GetPagedAsync(1, 100);

        Assert.NotNull(result);
        Assert.Single(result.Items);
        Assert.Equal("1", result.Items[0].ProductCode);
        Assert.Equal(1, result.TotalItems);
        Assert.Equal(1, result.TotalPages);
        Assert.Equal(1, result.CurrentPage);
        Assert.Equal(100, result.PageSize);
    }

    [Fact]
    public async Task GetPagedAsync_ShouldReturnEmpty_WhenNoProducts()
    {
        _repositoryMock
            .Setup(r => r.GetPagedAsync(1, It.IsAny<int>()))
            .ReturnsAsync(new List<OutlistProduct>());

        var result = await _service.GetPagedAsync(1, 10);

        Assert.NotNull(result);
        Assert.Empty(result.Items);
        Assert.Equal(0, result.TotalItems);
        Assert.Equal(0, result.TotalPages);
        Assert.Equal(1, result.CurrentPage);
        Assert.Equal(10, result.PageSize);
    }

    [Fact]
    public async Task GetPagedAsync_ShouldReturnMultiplePages()
    {
        // Arrange
        var products = Enumerable.Range(1, 25)
            .Select(i => new OutlistProduct
            {
                ProductCode = i.ToString(),
                ValidFrom = DateTime.UtcNow.AddDays(-i),
                ValidTo = DateTime.UtcNow.AddDays(i)
            }).ToList();

        _repositoryMock
            .Setup(r => r.GetPagedAsync(1, int.MaxValue))
            .ReturnsAsync(products);

        // Act
        var pagedResult = await _service.GetPagedAsync(page: 2, pageSize: 10);

        // Assert
        Assert.NotNull(pagedResult);
        Assert.Equal(3, pagedResult.TotalPages);         
        Assert.Equal(2, pagedResult.CurrentPage);        
        Assert.Equal(10, pagedResult.Items.Count);    
        Assert.Equal("11", pagedResult.Items[0].ProductCode); 
    }

    [Fact]
    public async Task GetByProductCodeAsync_ShouldReturnProduct_WhenFound()
    {
        var product = new OutlistProduct
        {
            ProductCode = "ABC123",
            ValidFrom = DateTime.UtcNow,
            ValidTo = DateTime.UtcNow.AddDays(10)
        };

        _repositoryMock
            .Setup(r => r.GetByProductCodeAsync("ABC123"))
            .ReturnsAsync(product);

        var result = await _service.GetByProductCodeAsync("ABC123");

        Assert.NotNull(result);
        Assert.Equal("ABC123", result.ProductCode);
    }

    [Fact]
    public async Task GetByProductCodeAsync_ShouldReturnNull_WhenNotFound()
    {
        _repositoryMock
            .Setup(r => r.GetByProductCodeAsync("NOTFOUND"))
            .ReturnsAsync((OutlistProduct?)null);

        var result = await _service.GetByProductCodeAsync("NOTFOUND");

        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_ShouldCallRepositoryAddAsync()
    {
        var product = new OutlistProduct
        {
            ProductCode = "NEWCODE",
            ValidFrom = DateTime.UtcNow,
            ValidTo = DateTime.UtcNow.AddDays(5)
        };

        await _service.AddAsync(product);

        _repositoryMock.Verify(r => r.AddAsync(product), Times.Once);
    }

    [Fact]
    public async Task RemoveAsync_ShouldCallRepositoryRemoveAsync()
    {
        var code = "REMOVEME";

        await _service.RemoveAsync(code);

        _repositoryMock.Verify(r => r.RemoveAsync(code), Times.Once);
    }

    [Fact]
    public async Task UpdateValidityAsync_ShouldCallRepositoryUpdateValidityAsync_WhenProductExists()
    {
        var code = "P123";
        var validFrom = DateTime.UtcNow;
        var validTo = validFrom.AddDays(7);
        var existingProduct = new OutlistProduct
        {
            ProductCode = code,
            ValidFrom = validFrom,
            ValidTo = validTo
        };

        _repositoryMock
            .Setup(r => r.GetByProductCodeAsync(code))
            .ReturnsAsync(existingProduct);

        await _service.UpdateValidityAsync(code, validFrom, validTo);

        _repositoryMock.Verify(r =>
            r.UpdateValidityAsync(code, validFrom, validTo),
            Times.Once);
    }

    [Fact]
    public async Task UpdateValidityAsync_ShouldNotCallRepositoryUpdate_WhenProductDoesNotExist()
    {
        // Arrange
        var code = "NOT_FOUND";
        var validFrom = DateTime.UtcNow;
        var validTo = validFrom.AddDays(5);

        // Simula que o produto não existe
        _repositoryMock
            .Setup(r => r.GetByProductCodeAsync(code))
            .ReturnsAsync((OutlistProduct?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            await _service.UpdateValidityAsync(code, validFrom, validTo)
        );

        Assert.Equal($"Produto com código '{code}' não encontrado.", exception.Message);

        _repositoryMock.Verify(r =>
            r.UpdateValidityAsync(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()),
            Times.Never);
    }

    [Fact]
    public async Task AddAsync_ShouldThrow_WhenRepositoryThrows()
    {
        var product = new OutlistProduct
        {
            ProductCode = "ERR",
            ValidFrom = DateTime.UtcNow,
            ValidTo = DateTime.UtcNow.AddDays(1)
        };

        _repositoryMock
            .Setup(r => r.AddAsync(product))
            .ThrowsAsync(new Exception("Repository error"));

        await Assert.ThrowsAsync<Exception>(() => _service.AddAsync(product));
    }
}
