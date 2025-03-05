using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Resources
{
    public static class StringResourceMessage
    {
        #region CommonValueNotValid
        public static readonly string ValueNotValid = "Value not valid";
        #endregion

        #region CommonConflictValue
        public static readonly string ConflictValue = "A product with the same description already exists.";
        #endregion

        #region CommonNotFoundValue
        public static readonly string NotFoundValue = "Product not found.";
        #endregion

        #region CommonNotFoundValue
        public static readonly string ConflictValue1 = "The product cannot be deleted because it is available";
        #endregion

        #region CommonBadValue
        public static readonly string BadValue = "The product cannot be deleted because there is remaining stock.";
        #endregion

        #region CommonConflictValueDiscount
        public static readonly string ConflictValueDiscount = "A discount with the details already exists.";
        #endregion

        #region CommonNotFoundValueDiscount
        public static readonly string NotFoundValueDiscount = "Discount not found.";
        #endregion

        #region CommonNotFoundValueDiscount
        public static readonly string NotFoundValuePD = "Product with the provided ID does not exist.";
        #endregion
        #region CommonNotFoundValueOI
        public static readonly string NotFoundValueOI = "Order Item not found.";
        #endregion

        #region CommonConflictValueOI
        public static readonly string ConflictValueOI = "A order item with the same product already exists.";
        #endregion

        #region CommonNotFoundValueO
        public static readonly string NotFoundValueO = "Order not found.";
        public static readonly string OrderProductEmpty = "Order must contain at least one product.";
        #endregion

        #region NotFoundValueUser
        public static readonly string NotFoundValueUser = "User not found.";
        #endregion

        #region ErrorUsername
        public static readonly string NotFoundUsermane = "Username not found.";
        #endregion

        #region ErrorPassword
        public static readonly string NotFoundPassword = "Invalid password.";
        #endregion

        #region ErrorUsername
        public static readonly string UserAlreadyExists = "A user with this username and role already exists.";
        #endregion

        #region InsufficientStockProduct
        public static readonly string InsufficientStockProduct = "Insufficient stock for product";
        #endregion

        #region UserIDError
        public static readonly string UserIDError = "User ID claim not found in token";
        #endregion

        #region ForbiddenAddUserError
        public static readonly string ForbiddenAddUserError = "You do not have permission to add new users.";
        #endregion

        #region CannotDeleteOwnAccountError
        public static readonly string CannotDeleteOwnAccountError = "You cannot delete your own account.";
        #endregion 



        public static string FormatErrorMessage(string error, object[] formatParameters = null, object[] parameters = null)
        {
            if (formatParameters != null && formatParameters.Length > 0)
            {
                error = string.Format(error, formatParameters);
            }

            if (parameters != null && parameters.Length > 0)
            {
                error += string.Join(CommonStringResources.Separator, parameters);
            }

            return error;
        }
    }
}