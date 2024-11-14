namespace Order.Service.ApiModels;

public record CreateOrderRequest(List<GetOrderProductResponse> OrderProducts);
