 
using FoodHub.Menu.Application.Dtos;
using FoodHub.Menu.Application.Interfaces;
using FoodHub.Menu.Application.Queries;
 
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FoodHub.Api.GraphQL.Queries;

[Authorize]
[ExtendObjectType("Query")]
public sealed class RestaurantQuery
{
    public async Task<MenuDto?> GetMenuById(
        Guid id,
        [Service] IMenuRepository repository,
        [Service] Serilog.ILogger logger,
        [Service] Serilog.ILogger queryLogger,
        CancellationToken cancellationToken)
    {
        logger.ForContext<RestaurantQuery>().Information("Begin: GetMenuById query for {MenuId}", id);

        try
        {
            var query = new GetMenuByIdQuery(repository, queryLogger.ForContext<GetMenuByIdQuery>());
            var menu = await query.ExecuteAsync(id, cancellationToken);

            if (menu is null)
            {
                logger.ForContext<RestaurantQuery>().Information("Success: Menu with Id {MenuId} not found", id);
            }
            else
            {
                logger.ForContext<RestaurantQuery>().Information("Success: Found menu {MenuId}", menu.Id);
            }

            return menu;
        }
        catch (Exception ex)
        {
            logger.ForContext<RestaurantQuery>().Error(ex, "Error: Failed to get menu for Id {MenuId}", id);
            throw;
        }
    }

    public async Task<MenuDto?> GetMenusByRestaurant(
        Guid restaurantId,
        [Service] IMenuRepository repository,
        [Service] Serilog.ILogger logger,
        [Service] Serilog.ILogger queryLogger,
        CancellationToken cancellationToken)
    {
        logger.ForContext<RestaurantQuery>().Information("Begin: GetMenusByRestaurant query for {RestaurantId}", restaurantId);

        try
        {
            var query = new GetMenuByRestaurantIdQuery(repository, queryLogger.ForContext<GetMenuByRestaurantIdQuery>());
            var menu = await query.ExecuteAsync(restaurantId, cancellationToken);

            if (menu is null)
            {
                logger.ForContext<RestaurantQuery>().Information("Success: Menu for Restaurant {RestaurantId} not found", restaurantId);
            }
            else
            {
                logger.ForContext<RestaurantQuery>().Information("Success: Found menu {MenuId} for Restaurant {RestaurantId}", menu.Id, menu.RestaurantId);
            }

            return menu;
        }
        catch (Exception ex)
        {
            logger.ForContext<RestaurantQuery>().Error(ex, "Error: Failed to get menu for Restaurant {RestaurantId}", restaurantId);
            throw;
        }
    }


}