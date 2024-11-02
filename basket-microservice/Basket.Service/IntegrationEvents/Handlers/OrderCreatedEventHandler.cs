﻿using Basket.Service.Infrastructure.Data;
using ECommerce.Shared.Infrastructure.EventBus.Abstractions;

namespace Basket.Service.IntegrationEvents.Handlers;

internal class OrderCreatedEventHandler : IEventHandler<OrderCreatedEvent>
{
    private readonly IBasketStore _basketStore;

    public OrderCreatedEventHandler(IBasketStore basketStore) => _basketStore = basketStore;

    public Task Handler(OrderCreatedEvent @event)
    {
        _basketStore.DeleteCustomerBasket(@event.CustomerId);
        
        return Task.CompletedTask;
    }
}
