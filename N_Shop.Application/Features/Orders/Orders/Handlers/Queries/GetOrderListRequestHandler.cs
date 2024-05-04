﻿using AutoMapper;
using MediatR;
using N_Shop.Application.Contracts.Accessor;
using N_Shop.Application.Contracts.Persistence.Orders;
using N_Shop.Application.DTOs.Orders.Order;
using N_Shop.Application.Exceptions;
using N_Shop.Application.Features.Orders.Orders.Requests.Queries;

namespace N_Shop.Application.Features.Orders.Orders.Handlers.Queries
{
    public class GetOrderListRequestHandler : IRequestHandler<GetOrderListRequest, List<OrderDto>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly IUserAccessor _userAccessor;
        public GetOrderListRequestHandler(IMapper mapper, IOrderRepository orderRepository, IUserAccessor userAccessor)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _userAccessor = userAccessor;
        }
        public async Task<List<OrderDto>> Handle(GetOrderListRequest request, CancellationToken cancellationToken)
        {
            if (request.ShowAdmin == false)
            {
                #region Cheak HttpContext

                var uid = request.UserId;

                if (_userAccessor.UserId != uid)
                    throw new BadRequestException($"Get Order Failed {_userAccessor.UserId} != {uid}");

                #endregion
            }

            var orders = await _orderRepository.GetOrdersByUserId(request.UserId);
            return _mapper.Map<List<OrderDto>>(orders);
        }
    }
}
