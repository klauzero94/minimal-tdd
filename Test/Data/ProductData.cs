using System.Linq.Expressions;
using App.Outputs;

namespace Test.Data;

public static class ProductData
{
    public static ProductOutput GetProductOutput(Expression<Func<ProductOutput, bool>> expression) =>
        GetProductOutputs().AsQueryable().FirstOrDefault(expression, new ProductOutput());
    public static List<ProductOutput> GetProductOutputs()
    {
        return new List<ProductOutput>()
        {
            new ProductOutput()
            {
                Id = "6381004fd1677fddd851065d",
                CreatedAt = new DateTime(2021, 5, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2021, 5, 2, 0, 0, 0, DateTimeKind.Utc),
                Name = "Product 1",
                Description = "Description 1",
                Price = 10.00m
            },
            new ProductOutput()
            {
                Id = "6381004fd1677fddd851065e",
                CreatedAt = new DateTime(2021, 5, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2021, 5, 2, 0, 0, 0, DateTimeKind.Utc),
                Name = "Product 2",
                Description = "Description 2",
                Price = 20.00m
            },
            new ProductOutput()
            {
                Id = "6381004fd1677fddd851065f",
                CreatedAt = new DateTime(2021, 5, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2021, 5, 2, 0, 0, 0, DateTimeKind.Utc),
                Name = "Product 3",
                Description = "Description 3",
                Price = 30.00m
            },
            new ProductOutput()
            {
                Id = "6381004fd1677fddd8510660",
                CreatedAt = new DateTime(2021, 5, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2021, 5, 2, 0, 0, 0, DateTimeKind.Utc),
                Name = "Product 4",
                Description = "Description 4",
                Price = 40.00m
            },
            new ProductOutput()
            {
                Id = "6381004fd1677fddd8510661",
                CreatedAt = new DateTime(2021, 5, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2021, 5, 2, 0, 0, 0, DateTimeKind.Utc),
                Name = "Product 5",
                Description = "Description 5",
                Price = 50.00m
            }
        };
    }
}