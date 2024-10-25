namespace Order.Service.ApiModels;

public record CreateOrderRequest(List<OrderProductDto> OrderProducts);
