using Microsoft.AspNetCore.Mvc;

namespace Basket.Basket.Features.RemoveItemFromBasket;

// public record RemoveItemFromBasketRequest(string UserName, string ProductId);
public record RemoveItemFromBasketResponse(Guid Id);

public class RemoveItemFromBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/basket/{username}/items/{productId}",
            async ([FromRoute] string username,
                [FromRoute] Guid productId,
                ISender sender) =>
            {
                var command = new RemoveItemFromBasketCommand(username, productId);
                
                var result = await sender.Send(command);
                
                var response = result.Adapt<RemoveItemFromBasketResponse>();

                return Results.Ok(response);
            })
            .Produces<RemoveItemFromBasketResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Remove Item From Basket.")
            .WithDescription("Removes item from basket.")
            .RequireAuthorization();
    }
}