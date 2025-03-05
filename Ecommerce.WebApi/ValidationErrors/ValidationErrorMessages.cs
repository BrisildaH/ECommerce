using Ecommerce.Domain;
using Ecommerce.WebApi.Common;

namespace Ecommerce.WebApi.ValidationErrors
{
    public class ValidationErrorMessages
    {
        #region ProductValidation
        public static readonly string ProductNameCannotBeNullOrEmpty = FormatMessage("Product name is required.");
        public static readonly string ProductNameLength = FormatMessage("Product name must be between 2 and 100 characters.");
        public static readonly string ProductDescCannotBeNullOrEmpty = FormatMessage("Product description is required.");
        public static readonly string ProductDescLength = FormatMessage("Product description must be between 2 and 100 characters.");
        public static readonly string ProductPriceNotValidFormat = FormatMessage("Price must be a valid number with up to two decimal places.");
        public static readonly string ProductPriceNotNegative = FormatMessage("Price must be a valid number greater than zero.");
        public static readonly string ProductIsPublic = FormatMessage("Value for whether the product is public must be specified.");
        public static readonly string ProductIsAvailable = FormatMessage("Value for whether the product is available must be specified.");
        public static readonly string StockInvalidFormat = FormatMessage("The stock must be a valid number");
        public static readonly string StockNotNegative = FormatMessage("Stock quantity must be zero or greater.");
        public static readonly string Page = FormatMessage("Page must be greater than 0.");
        public static readonly string PageSize = FormatMessage("Size must be greater than 0.");
        public static readonly string PageSizeMax = FormatMessage("Size must be less than or equal to 10.");
        #endregion

        #region DiscountValidation
        public static readonly string DiscountDescCannotBeNullOrEmpty = FormatMessage("Discount description is required.");
        public static readonly string DiscountDescLength = FormatMessage("Discount description must be between 2 and 100 characters.");
        public static readonly string PercentageInvalidFormat = FormatMessage("The percentage must be a valid number");
        public static readonly string DiscountPercentageNotNegative = FormatMessage("Percentage must be a valid number greater than zero.");
        public static readonly string DiscountProductIdNotNegative = FormatMessage("ProductId must be a valid number greater than zero.");
        public static readonly string ProductIdInvalidFormat = FormatMessage("The ProductID must be a valid number");
        public static readonly string UserIdInvalidFormat = FormatMessage("The UserID must be a valid number");
        public static readonly string DiscountUserIdNotNegative = FormatMessage("UserId must be a valid number greater than zero.");
        public static readonly string DiscountIdInvalidFormat = FormatMessage("The DiscountId must be a valid number");
        public static readonly string DiscountUIdNotNegative = FormatMessage("DiscountId must be a valid number greater than zero.");
        public static readonly string DiscountIdUnique = FormatMessage("The DiscountId must be unique.");
        #endregion

        #region OrderValidation
        public const string OrderItemsNotEmpty = "Order must have at least one item.";
        public const string ProductIdPositiveNumber = "ProductId must be a positive number.";
        public const string QuantityPositiveNumber = "Quantity must be a positive number.";

        #endregion

        #region UserValidation
        public const string UsernameCannotBeNullOrEmpty = "Username is required.";
        public const string PasswordCannotBeNullOrEmpty = "Password is required.";
        public const string RoleIdCannotBeNegativeOrZero = "RoleId must be greater than zero";
        public const string RoleIdCannotBeNullOrEmpty = "RoleId is required.";
        #endregion

        protected static string FormatMessage(string message)
        {
            return $"{ApiCommonStringResources.Separator}{message}";
        }
    }
}
