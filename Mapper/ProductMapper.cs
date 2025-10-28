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
                stockQuantity = productRequest.stockQuantity,
                description = productRequest.description,
                status = productRequest.status,
                warrantyPeriod = productRequest.warrantyPeriod,
            };
        }

        public static ProductResponse toProductResponse(Products products)
        { 
            return new ProductResponse
            {
                id = products.id,
                productName = products.productName,
                price = products.price??0,
                stockQuantity = products.stockQuantity??0,
                description = products.description,
                thumbnail = products.thumbnail,
                status = products.status,
                brandId = products.brandId!,
                subCategoryId = products.subCategoryId??string.Empty,
                createdAt = products.createdAt,
                brandName = products.brand.brandName,
                subCategoryName = products.subCategory?.subCategoryName??string.Empty,
                warrantyPeriod = products.warrantyPeriod ?? 0
            };
        }
    }
}
