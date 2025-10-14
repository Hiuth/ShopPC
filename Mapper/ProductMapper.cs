using ShopPC.DTO.Request;
using ShopPC.DTO.Response;
using ShopPC.Models;

namespace ShopPC.Mapper
{
    public class ProductMapper
    {
        public static Products toProducts(ProductRequest productRequest)
        {
            return new Products
            { 
                productName = productRequest.productName,
                price = productRequest.price,
                quantity = productRequest.quantity,
                stockQuantity = productRequest.stockQuantity,
                description = productRequest.description,
                thumbnail = productRequest.thumbnail,
                status = productRequest.status
            };
        }

        public static ProductResponse toProductResponse(Products products)
        { 
            return new ProductResponse
            {
                id = products.id,
                productName = products.productName,
                price = products.price,
                quantity = products.quantity,
                stockQuantity = products.stockQuantity,
                description = products.description,
                thumbnail = products.thumbnail,
                status = products.status,
                brandId = products.brandId,
                subCategoryId = products.subCategoryId,
                createdAt = products.createdAt,
                brandName = products.brand.brandName,
                subCategoryName = products.subCategory.subCategoryName
            };
        }
    }
}
