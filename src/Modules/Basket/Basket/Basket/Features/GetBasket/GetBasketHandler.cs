
namespace Basket.Basket.Features.GetBasket;

public record GetBasketCommand(string UserName)
    : IQuery<GetBasketResult>;

public record GetBasketResult(ShoppingCartDto ShoppingCart);

internal class GetBasketHandler(IBasketRepository repository)
    : IQueryHandler<GetBasketCommand, GetBasketResult>
{
    public async Task<GetBasketResult> Handle(GetBasketCommand query, CancellationToken cancellationToken)
     {
        // Get Basket with UserName 
       var basket = await repository.GetBasket(query.UserName, true, cancellationToken);
        
        // mapping basket entity to shoppingCartDto
        var basketDto = basket.Adapt<ShoppingCartDto>();
        
        return new GetBasketResult(basketDto);
    }
}