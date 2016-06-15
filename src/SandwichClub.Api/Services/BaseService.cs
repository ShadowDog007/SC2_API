using System.Collections.Generic;
using System.Threading.Tasks;
using SandwichClub.Api.Services.Mapper;
using SandwichClub.Api.Repositories;

namespace SandwichClub.Api.Services
{
    public class BaseService<TId, T, TDto, TRepo> : IBaseService<TId, TDto> where T : class where TDto : class where TRepo : IBaseRepository<TId, T>
    {
        protected readonly TRepo _repository;
        protected readonly IMapper<T, TDto> _mapper;

        protected BaseService(TRepo repo, IMapper<T, TDto> mapper)
        {
            _repository = repo;
            _mapper = mapper;
        }

        public async virtual Task<IEnumerable<TDto>> GetAsync()
        {
            var items = await _repository.GetAsync();
            return await ToDtosAsync(items);
        }

        public virtual Task<int> CountAsync()
        {
            return _repository.CountAsync();
        }

        public async virtual Task<TDto> GetByIdAsync(TId id)
        {
            var item = await _repository.GetByIdAsync(id);

            // Check for nulls
            if (item == null)
                return null;

            var dto = ToDto(item);
            await HydrateDtoAsync(item, dto);
            return dto;
        }

        public async virtual Task<TDto> InsertAsync(TDto t)
        {
            var model = ToModel(t);
            var updated = await _repository.InsertAsync(model);
            var dto = ToDto(updated);
            await HydrateDtoAsync(updated, dto);
            return dto;
        }

        public virtual Task UpdateAsync(TDto t)
        {
            var model = ToModel(t);
            return _repository.UpdateAsync(model);
        }

        public virtual Task DeleteAsync(TId id)
        {
            return _repository.DeleteAsync(id);
        }

        public TDto ToDto(T t)
        {
            return _mapper.ToDto(t);
        }

        public T ToModel(TDto dto)
        {
            return _mapper.ToModel(dto);
        }

        protected virtual Task HydrateDtoAsync(T t, TDto dto)
        {
            return Task.CompletedTask;
        }

        protected async virtual Task<IList<TDto>> ToDtosAsync(IList<T> items)
        {
            var dtos = new List<TDto>(items.Count);

            // Convert to dtos
            foreach (var item in items)
            {
                var dto = ToDto(item);
                await HydrateDtoAsync(item, dto);
                dtos.Add(dto);
            }

            return dtos;
        }
    }
}
