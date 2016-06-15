using System.Collections.Generic;
using System.Threading.Tasks;
using SandwichClub.Api.DTO;
using SandwichClub.Api.Services.Mapper;
using SandwichClub.Api.Repositories;
using SandwichClub.Api.Repositories.Models;

namespace SandwichClub.Api.Services
{
    public class WeekUserLinkService : BaseService<WeekUserLinkId, WeekUserLink, WeekUserLinkDto, IWeekUserLinkRepository>, IWeekUserLinkService
    {
        private readonly IWeekUserLinkRepository _weekUserLinkRepository;
        private readonly IUserService _userService;

        public WeekUserLinkService(IWeekUserLinkRepository weekUserLinkRepository, IMapper<WeekUserLink, WeekUserLinkDto> mapper, IUserService userService) : base(weekUserLinkRepository, mapper)
        {
            _weekUserLinkRepository = weekUserLinkRepository;
            _userService = userService;
        }

        public async Task<IEnumerable<WeekUserLinkDto>> GetByWeekIdAsync(int weekId)
        {
            var items = await _repository.GetByWeekIdAsync(weekId);
            return await ToDtosAsync(items);
        }

        public override Task<WeekUserLinkDto> InsertAsync(WeekUserLinkDto linkDto)
        {
            return InsertOrUpdateAsync(linkDto);
        }

        public override Task UpdateAsync(WeekUserLinkDto linkDto)
        {
            return InsertOrUpdateAsync(linkDto);
        }

        public async Task<WeekUserLinkDto> InsertOrUpdateAsync(WeekUserLinkDto linkDto)
        {
            var link = ToModel(linkDto);
            var existingLink = await _repository.GetByIdAsync(new WeekUserLinkId { UserId = linkDto.UserId, WeekId = linkDto.WeekId });
            var exists = existingLink != null;

            if (exists)
                await _repository.UpdateAsync(link);
            else
                link = await _repository.InsertAsync(link);

            return ToDto(link);
        }

        public async override Task HydrateDtoAsync(WeekUserLinkDto dto)
        {
            dto.User = await _userService.GetByIdAsync(dto.UserId);
        }
    }
}