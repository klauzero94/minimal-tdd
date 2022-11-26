using App.Outputs;
using AutoMapper;
using Domain.Collections;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using Repository.Interfaces;
using Service.Implementations;
using Service.Interfaces;
using Test.Data;
using Xunit;

namespace Test;

public class ProductServiceTest
{
    [Fact]
    public void Should_ReturnProductAll()
    {
        #region Mapper
        IMapper mapper = new Mock<IMapper>().Object;
        #endregion

        #region Arrange
        var mockProductRepository = new Mock<IProductRepository>();
        var mockUoW = new Mock<IUoW>();
        var output = ProductData.GetProductOutputs();
        mockProductRepository.Setup(x => x.FindAsync<ProductOutput>(It.IsAny<FilterDefinition<ProductCollection>>(),
            It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync((output, output.Count));
        IProductService service = new ProductService(mockProductRepository.Object, mockUoW.Object, mapper);
        #endregion

        #region Act
        var result = service.GetProducts(string.Empty, 1, 10);
        #endregion

        #region Assert
        Assert.NotNull(result);
        Assert.Equal(output, result.Result.Item1);
        Assert.Equal(output.Count, result.Result.Item2);
        #endregion
    }

    [Fact]
    public void Should_ReturnProductById()
    {
        #region Mapper
        IMapper mapper = new Mock<IMapper>().Object;
        #endregion

        #region Arrange
        var mockProductRepository = new Mock<IProductRepository>();
        var mockUoW = new Mock<IUoW>();
        var output = ProductData.GetProductOutput(x => x.Id == "6381004fd1677fddd851065e");
        mockProductRepository.Setup(x => x.GetByIdAsync<ProductOutput>(It.IsAny<ObjectId>())).ReturnsAsync(output);
        IProductService service = new ProductService(mockProductRepository.Object, mockUoW.Object, mapper);
        #endregion

        #region Act
        var result = service.GetProduct("6381004fd1677fddd851065e");
        #endregion

        #region Assert
        Assert.NotNull(result);
        Assert.Equal(output, result.Result);
        #endregion
    }
}