using Bank.Common;

namespace Bank.Api.ApiModels.Responses;

public record SearchOrdersResponse(IEnumerable<Order> Orders);