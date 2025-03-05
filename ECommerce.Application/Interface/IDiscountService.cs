using Ecommerce.Application.DTO.DiscountDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Interface
{
    public interface IDiscountService
    {
        Task<GetDiscountByIdDtoResponse> GetDiscountById(int id);
        Task<GetAllDiscountsDtoResponse> GetAllDiscounts();
        Task<AddDiscountDtoResponse> AddDiscount(AddDiscountDtoRequest addDiscountDto);
        Task<UpdateDiscountDtoResponse> UpdateDiscount(int id, UpdateDiscountDtoRequest updateDiscountDto);
        Task<DeleteDiscountDtoResponse> DeleteDiscount(int id);
        Task AddDiscounts(AddDiscountsDtoRequest addDiscountDtoRequest);
        Task UpdateDiscounts( UpdateDiscountsDtoRequest updateDiscountDto);
    }
}
