using System.Net;

namespace ShopPC.Exceptions
{
    public class ErrorCode
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public HttpStatusCode Status { get; set; }

        public ErrorCode(int code, string message, HttpStatusCode status)
        {
            Code = code;
            Message = message;
            Status = status;
        }   

        public static readonly ErrorCode INTERNAL_SERVER_ERROR
            = new ErrorCode(1000, "Uncategorized Exception", HttpStatusCode.InternalServerError);
        //category error codes
        public static readonly ErrorCode CATEGORY_ALREADY_EXISTS
            = new ErrorCode(1001, "Category already exists", HttpStatusCode.BadRequest);
        public static readonly ErrorCode CATEGORY_NOT_FOUND
            = new ErrorCode(1002, "Category not found", HttpStatusCode.NotFound);
        public static readonly ErrorCode CATEGORY_NOT_EXISTS
            = new ErrorCode(1008, "Category not exists", HttpStatusCode.BadRequest);


        //file error codes
        public static readonly ErrorCode FILE_TOO_LARGE
            = new ErrorCode(1003, "File size is too large", HttpStatusCode.BadRequest);
        public static readonly ErrorCode FILE_EMPTY
            = new ErrorCode(1004, "File is empty", HttpStatusCode.BadRequest);
        public static readonly ErrorCode INVALID_FILE_TYPE
            = new ErrorCode(1005, "Invalid file type", HttpStatusCode.BadRequest);
        public static readonly ErrorCode FILE_UPLOAD_FAILED
            = new ErrorCode(1006, "File upload failed", HttpStatusCode.InternalServerError);
        public static readonly ErrorCode DELETE_FILE_FAILED
            = new ErrorCode(1007, "Delete file failed", HttpStatusCode.InternalServerError);


        // SubCategory error codes
        public static readonly ErrorCode SUBCATEGORY_ALREADY_EXISTS
            = new ErrorCode(2001, "SubCategory already exists", HttpStatusCode.BadRequest);
        public static readonly ErrorCode SUB_CATEGORY_NOT_EXISTS
            = new ErrorCode(2002, "SubCategory not exists", HttpStatusCode.BadRequest);

        //brand error codes
        public static readonly ErrorCode BRAND_ALREADY_EXISTS
            = new ErrorCode(3001, "Brand already exists", HttpStatusCode.BadRequest);
        public static readonly ErrorCode BRAND_NOT_EXISTS
            = new ErrorCode(3002, "Brand not exists", HttpStatusCode.BadRequest);

        //attribute error codes
        public static readonly ErrorCode ATTRIBUTE_NOT_EXISTS
            = new ErrorCode(4001, "Attribute not exists", HttpStatusCode.BadRequest);
        public static readonly ErrorCode ATTRIBUTE_ALREADY_EXISTS
            = new ErrorCode(4002, "Attribute already exists", HttpStatusCode.BadRequest);

        //product error codes
        public static readonly ErrorCode PRODUCT_NOT_EXISTS
            = new ErrorCode(5001, "Product not exists", HttpStatusCode.BadRequest);
        public static readonly ErrorCode PRODUCT_ALREADY_EXISTS
            = new ErrorCode(5002, "Product already exists", HttpStatusCode.BadRequest);
        public static readonly ErrorCode PRODUCT_OUT_OF_STOCK
            = new ErrorCode(5003, "Product out of stock", HttpStatusCode.BadRequest);
        public static readonly ErrorCode INVALID_PRODUCT_QUANTITY
            = new ErrorCode(5004, "Invalid product quantity", HttpStatusCode.BadRequest);
        public static readonly ErrorCode INVALID_PRODUCT_PRICE
            = new ErrorCode(5005, "Invalid product price", HttpStatusCode.BadRequest);

        //product img
        public static readonly ErrorCode PRODUCT_IMG_NOT_EXISTS
            = new ErrorCode(6001, "ProductImg not exists", HttpStatusCode.BadRequest);

        //product attribute
        public static readonly ErrorCode PRODUCT_ATTRIBUTE_NOT_EXISTS
            = new ErrorCode(7001, "ProductAttribute not exists", HttpStatusCode.BadRequest);
        public static readonly ErrorCode PRODUCT_ATTRIBUTE_ALREADY_EXISTS
            = new ErrorCode(7002, "ProductAttribute already exists", HttpStatusCode.BadRequest);
    }
}
