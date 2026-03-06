using Mapster;
using Shared.Pagination;

namespace Ordering.Orders.Features.GetOrders;

public record GetOrdersRequest(PaginationRequest PaginationRequest)
    : IQuery<GetOrdersResult>;

public record GetOrdersResult(PaginationResult<OrderDto> Orders);

public class GetOrdersHandler(OrderingDbContext dbContext) 
    : IQueryHandler<GetOrdersRequest, GetOrdersResult>
{
    public async Task<GetOrdersResult> Handle(GetOrdersRequest query, CancellationToken cancellationToken)
    {
        var pageIndex = query.PaginationRequest.PageIndex;
        var pageSize = query.PaginationRequest.PageSize;
        
        var totalCount = await dbContext.Orders.LongCountAsync(cancellationToken);

        var orders = await dbContext.Orders
            .AsNoTracking()
            .Include(x => x.Items)
            .OrderBy(x => x.OrderName)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var orderDtos = orders.Adapt<List<OrderDto>>();

        return new GetOrdersResult(new PaginationResult<OrderDto>(
            pageIndex,
            pageSize, 
            totalCount,
            orderDtos));
    }
}