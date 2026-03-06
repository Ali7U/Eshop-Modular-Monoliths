namespace Basket.Basket.Features.CreateBasket;

public record CreateBasketCommand(ShoppingCartDto ShoppingCart)
    : ICommand<CreateBasketResult>;
public record CreateBasketResult(Guid Id);


public class CreateBasketValidator : AbstractValidator<CreateBasketCommand>
{
    public CreateBasketValidator()
    {
        RuleFor(x => x.ShoppingCart.UserName).NotEmpty().WithMessage("UserName is required!.");
    }
}

internal class CreateBasketHandler(IBasketRepository repository) 
    : ICommandHandler<CreateBasketCommand, CreateBasketResult>
{
    public async Task<CreateBasketResult> Handle(CreateBasketCommand command, CancellationToken cancellationToken)
    {
        // Create Basket entity from command object
        // Save to database
        // Return result
        
        var shoppingCart = CreateNewBasket(command.ShoppingCart);
        
        await repository.CreateBasket(shoppingCart, cancellationToken);

        return new CreateBasketResult(shoppingCart.Id);
    }

    private ShoppingCart CreateNewBasket(ShoppingCartDto shoppingCartDto)
    {
        // create new basket
        var newBasket = ShoppingCart.Create(
            Guid.NewGuid(),
            shoppingCartDto.UserName);

        shoppingCartDto.items.ForEach(Item =>
        {
            newBasket.AddItem(
                Item.ProductId,
                Item.Quantity,
                Item.Color,
                Item.Price,
                Item.ProductName);
        });
        
        return newBasket;
    }
}