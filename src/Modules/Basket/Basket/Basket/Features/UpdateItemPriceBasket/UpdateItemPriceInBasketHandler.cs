namespace Basket.Basket.Features.UpdateItemPriceBasket;

public record UpdateItemPriceInBasketCommand(Guid ProductId, decimal Price)
    : ICommand<UpdateItemPriceInBasketCommandResult>;
public record UpdateItemPriceInBasketCommandResult(bool IsSuccess);

public class UpdateItemPriceInBasketCommandValidator : AbstractValidator<UpdateItemPriceInBasketCommand>
{
    public UpdateItemPriceInBasketCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("Product Id is required");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}
internal class UpdateItemPriceInBasketHandler(BasketDbContext dbContext)
    : ICommandHandler<UpdateItemPriceInBasketCommand, UpdateItemPriceInBasketCommandResult>
{
    public async Task<UpdateItemPriceInBasketCommandResult> Handle(UpdateItemPriceInBasketCommand command, CancellationToken cancellationToken)
    {
        //Find Shopping Cart Items with a give ProductId
        //Iterate items and Update Price of every item with incoming command. Price 
        //save to database
        //return result

        var itemsToUpdate = await dbContext.ShoppingCartItems
            .Where(x => x.ProductId == command.ProductId)
            .ToListAsync(cancellationToken);
        
        if(!itemsToUpdate.Any())
        {
            return new UpdateItemPriceInBasketCommandResult(false);
        }

        foreach (var items in itemsToUpdate)
        {
            items.UpdatePrice(command.Price);
        }
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return new UpdateItemPriceInBasketCommandResult(true); 
    }
}