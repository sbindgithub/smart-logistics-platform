using MediatR;

namespace SmartLogistics.Application.Orders.Commands.ConfirmOrder;

public sealed record ConfirmOrderCommand(Guid OrderId) : IRequest;
