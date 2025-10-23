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
        public static readonly ErrorCode PRODUCT_UNIT_NOT_EXISTS
            = new ErrorCode(5006, "ProductUnit not exists", HttpStatusCode.BadRequest);
        public static readonly ErrorCode PRODUCT_UNIT_ALREADY_EXISTS
            = new ErrorCode(5007, "ProductUnit already exists", HttpStatusCode.BadRequest);
        public static readonly ErrorCode PRODUCT_UNIT_IMEI_ALREADY_EXISTS
            = new ErrorCode(5008, "ProductUnit IMEI already exists", HttpStatusCode.BadRequest);
        public static readonly ErrorCode PRODUCT_UNIT_SERIALNUMBER_ALREADY_EXISTS
            = new ErrorCode(5009, "ProductUnit SerialNumber already exists", HttpStatusCode.BadRequest);
        public static readonly ErrorCode QUANTITY_EXCEEDS_STOCK
            = new ErrorCode(5010, "Quantity exceeds stock", HttpStatusCode.BadRequest);
        public static readonly ErrorCode PC_BUILD_NOT_EXISTS
            = new ErrorCode(5011, "PC Build not exists", HttpStatusCode.BadRequest);
        public static readonly ErrorCode PC_BUILD_ITEM_NOT_EXISTS
            = new ErrorCode(5012, "PC Build Item not exists", HttpStatusCode.BadRequest);
        //product img
        public static readonly ErrorCode PRODUCT_IMG_NOT_EXISTS
            = new ErrorCode(6001, "ProductImg not exists", HttpStatusCode.BadRequest);

        //product attribute
        public static readonly ErrorCode PRODUCT_ATTRIBUTE_NOT_EXISTS
            = new ErrorCode(7001, "ProductAttribute not exists", HttpStatusCode.BadRequest);
        public static readonly ErrorCode PRODUCT_ATTRIBUTE_ALREADY_EXISTS
            = new ErrorCode(7002, "ProductAttribute already exists", HttpStatusCode.BadRequest);

        //account error codes
        public static readonly ErrorCode ACCOUNT_NOT_EXISTS
            = new ErrorCode(8001, "Account not exists", HttpStatusCode.BadRequest);
        public static readonly ErrorCode ACCOUNT_ALREADY_EXISTS
            = new ErrorCode(8002, "Account already exists", HttpStatusCode.BadRequest);
        public static readonly ErrorCode EMAIL_ALREADY_EXISTS
            = new ErrorCode(8003, "Email already exists", HttpStatusCode.BadRequest);
        public static readonly ErrorCode EMAIL_NOT_EXISTS
            = new ErrorCode(8004, "Email not exists", HttpStatusCode.BadRequest);
        public static readonly ErrorCode INVALID_OTP
            = new ErrorCode(8005, "Invalid OTP", HttpStatusCode.BadRequest);


        //cart error codes
        public static readonly ErrorCode CART_NOT_EXISTS
            = new ErrorCode(9001, "Cart not exists", HttpStatusCode.BadRequest);
        public static readonly ErrorCode INVALID_QUANTITY
            = new ErrorCode(9002, "Invalid quantity", HttpStatusCode.BadRequest);

        //Order and Order detail error codes
        public static readonly ErrorCode ORDER_NOT_EXISTS
            = new ErrorCode(10001,"Order not exists",HttpStatusCode.BadRequest);
        public static readonly ErrorCode ORDER_ALREADY_EXISTS
            = new ErrorCode(10002, "Order already exists", HttpStatusCode.BadRequest);
        public static readonly ErrorCode ORDER_DETAIL_NOT_EXISTS
            = new ErrorCode(10003, "Order detail not exists", HttpStatusCode.BadRequest);
        public static readonly ErrorCode ORDER_DETAIL_ALREADY_EXISTS
            = new ErrorCode(10004, "Order detail already exists", HttpStatusCode.BadRequest);

        //Comment error codes
        public static readonly ErrorCode COMMENT_NOT_EXISTS
            = new ErrorCode(11001, "Comment not exists", HttpStatusCode.BadRequest);
        public static readonly ErrorCode COMMENT_ALREADY_EXISTS
            = new ErrorCode(11002, "Comment already exists", HttpStatusCode.BadRequest);
        public static readonly ErrorCode RATING_INVALID
            = new ErrorCode(11003, "Rating is invalid", HttpStatusCode.BadRequest);

        //WarrantyRecord error codes
        public static readonly ErrorCode WARRANTY_RECORD_NOT_EXISTS
            = new ErrorCode(12001, "Warranty record not exists", HttpStatusCode.BadRequest);
        public static readonly ErrorCode WARRANTY_RECORD_ALREADY_EXISTS
            = new ErrorCode(12002, "Warranty record already exists", HttpStatusCode.BadRequest);

    }
}
