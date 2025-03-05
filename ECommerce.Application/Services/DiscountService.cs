using AutoMapper;
using Ecommerce.Application.DTO.DiscountDto;
using Ecommerce.Application.Interface;
using Ecommerce.Application.Resources;
using Ecommerce.Common.Exceptions;
using Ecommerce.Domain;
using Ecommerce.Domain.Interface;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace Ecommerce.Application.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public DiscountService(IDiscountRepository discountRepository, IMapper mapper, IProductRepository productRepository, IUserRepository userRepository)
        {
            _discountRepository = discountRepository;
            _mapper = mapper;
            _productRepository = productRepository;
            _userRepository = userRepository;
        }

        public async Task<GetDiscountByIdDtoResponse> GetDiscountById(int id)
        {
            var discount = await _discountRepository.GetDiscountById(id);
            if (discount == null)

            {
                throw new NotFoundException(StringResourceMessage.NotFoundValueDiscount);
            }

            return _mapper.Map<GetDiscountByIdDtoResponse>(discount);

        }

        public async Task<GetAllDiscountsDtoResponse> GetAllDiscounts()
        {
            var discounts = await _discountRepository.GetAllDiscounts().ToListAsync();
            return _mapper.Map<GetAllDiscountsDtoResponse>(discounts);
        }

        public async Task<AddDiscountDtoResponse> AddDiscount(AddDiscountDtoRequest addDiscountDto)
        {
            var existingDiscount = await _discountRepository.GetDiscountByProductUserId(addDiscountDto.ProductId, addDiscountDto.UserId);
            if (existingDiscount != null)
            {
                throw new ConflictException(StringResourceMessage.ConflictValueDiscount);
            }

            var productExists = await _productRepository.GetProductById(addDiscountDto.ProductId);
            if (productExists == null)
            {
                throw new NotFoundException(StringResourceMessage.NotFoundValuePD);
            }

            var userExists = await _userRepository.GetUserById(addDiscountDto.UserId);
            if (userExists == null)
            {
                throw new NotFoundException(StringResourceMessage.NotFoundValueUser);
            }

            var discount = _mapper.Map<Discount>(addDiscountDto);
            await _discountRepository.AddDiscount(discount);

            return _mapper.Map<AddDiscountDtoResponse>(discount);
        }

        public async Task<UpdateDiscountDtoResponse> UpdateDiscount(int id, UpdateDiscountDtoRequest updateDiscountDto)
        {
            var existingDiscount = await _discountRepository.GetDiscountById(id);

            if (existingDiscount == null)
            {
                throw new NotFoundException(StringResourceMessage.NotFoundValueDiscount);
            }


            var productExists = await _productRepository.GetProductById(updateDiscountDto.ProductId);
            if (productExists == null)
            {
                throw new NotFoundException(StringResourceMessage.NotFoundValuePD);
            }

            var userExists = await _userRepository.GetUserById(updateDiscountDto.UserId);
            if (userExists == null)
            {
                throw new NotFoundException(StringResourceMessage.NotFoundValueUser);
            }

            var existingDiscountProductUser = await _discountRepository.GetDiscountByProductUserId(updateDiscountDto.ProductId, updateDiscountDto.UserId);
            if (existingDiscountProductUser != null)
            {
                throw new ConflictException(StringResourceMessage.ConflictValueDiscount);
            }

            existingDiscount.Description = updateDiscountDto.Description;
            existingDiscount.Percentage = updateDiscountDto.Percentage;
            existingDiscount.UserId = updateDiscountDto.UserId;
            existingDiscount.ProductId = updateDiscountDto.ProductId;

            await _discountRepository.UpdateDiscount(existingDiscount);
            return _mapper.Map<UpdateDiscountDtoResponse>(existingDiscount);
        }

        public async Task<DeleteDiscountDtoResponse> DeleteDiscount(int id)
        {
            var existingDiscount = await _discountRepository.GetDiscountById(id);

            if (existingDiscount == null)
            {
                throw new NotFoundException(StringResourceMessage.NotFoundValueDiscount);
            }

            await _discountRepository.DeleteDiscount(id);
            return _mapper.Map<DeleteDiscountDtoResponse>(existingDiscount);

        }
        public async Task AddDiscounts(AddDiscountsDtoRequest addDiscountDtoRequest)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    foreach (var discountDto in addDiscountDtoRequest.Discounts)
                    {
                        var existingDiscount = await _discountRepository.GetDiscountByProductUserId(discountDto.ProductId, discountDto.UserId);
                        if (existingDiscount != null)
                        {
                            throw new ConflictException(StringResourceMessage.ConflictValueDiscount);
                        }


                        var productExists = await _productRepository.GetProductById(discountDto.ProductId);
                        if (productExists == null)
                        {
                            throw new NotFoundException(StringResourceMessage.NotFoundValuePD);
                        }

                        var userExists = await _userRepository.GetUserById(discountDto.UserId);
                        if (userExists == null)
                        {
                            throw new NotFoundException(StringResourceMessage.NotFoundValueUser);
                        }


                        var discount = new Discount
                        {
                            Description = discountDto.Description,
                            Percentage = discountDto.Percentage,
                            ProductId = discountDto.ProductId,
                            UserId = discountDto.UserId
                        };

                        await _discountRepository.AddDiscount(discount);

                    }
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateDiscounts(UpdateDiscountsDtoRequest updateDiscountDto)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    foreach (var discountDto in updateDiscountDto.Discounts)
                    {
                        var existingDiscount = await _discountRepository.GetDiscountById(discountDto.Id);

                        if (existingDiscount == null)
                        {
                            throw new NotFoundException(StringResourceMessage.NotFoundValueDiscount);
                        }

                        var discountByProductUser = await _discountRepository.GetDiscountByProductUserId(discountDto.ProductId, discountDto.UserId);
                        if (discountByProductUser != null && discountByProductUser.Id != discountDto.Id)
                        {
                            throw new ConflictException(StringResourceMessage.ConflictValueDiscount);
                        }
                        var productExists = await _productRepository.GetProductById(discountDto.ProductId);
                        if (productExists == null)
                        {
                            throw new NotFoundException(StringResourceMessage.NotFoundValuePD);
                        }

                        var userExists = await _userRepository.GetUserById(discountDto.UserId);
                        if (userExists == null)
                        {
                            throw new NotFoundException(StringResourceMessage.NotFoundValueUser);
                        }

                        existingDiscount.Description = discountDto.Description;
                        existingDiscount.Percentage = discountDto.Percentage;
                        existingDiscount.UserId = discountDto.UserId;
                        existingDiscount.ProductId = discountDto.ProductId;

                        await _discountRepository.UpdateDiscount(existingDiscount);

                    }
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
