using GraphQL.Types;
using SandwichClub.Api.GraphQL.Types;
using SandwichClub.Api.Repositories.Models;
using SandwichClub.Api.Services;

namespace SandwichClub.Api.GraphQL {
    public class SandwichClubMutation : ObjectGraphType {
        public SandwichClubMutation(IScSession session, IWeekUserLinkService weekUserLinkService) {

            Name = "Mutation";
            Field<WeekUserLinkType>(
                "subscribeToWeek",
                arguments: new QueryArguments(
                    new QueryArgument<IntGraphType> { Name = "userId", Description = "UserId of the user" },
                    new QueryArgument<IntGraphType> { Name = "weekId", Description = "WeekId of the week" },
                    new QueryArgument<IntGraphType> { Name = "slices", Description = "WeekId of the week" }
                ),
                resolve: (context) => {
                    var userId = context.GetArgument<int>("userId");
                    var weekId = context.GetArgument<int>("weekId");
                    var slices = context.GetArgument<int>("slices");

                    return weekUserLinkService.UpdateOrInsertAsync(new WeekUserLink { Slices = slices, WeekId = weekId, UserId = userId});
                }
            );
        }
    }
}