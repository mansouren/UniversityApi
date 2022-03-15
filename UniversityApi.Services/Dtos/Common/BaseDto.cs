using AutoMapper;
using UniversityApi.Entities.Common;

namespace UniversityApi.Services.Dtos.Common
{
    public abstract class BaseDto<TDto, TEntity, TKey>
         where TDto : class, new()
         where TEntity : class, IEntity,new()
    {
        public TKey Id { get; set; }

        public TEntity ToEntity(IMapper mapper)
        {
            return mapper.Map<TEntity>(CastToDerivedClass(mapper,this));
        }

        public TEntity ToEntity(IMapper mapper, TEntity entity)
        {
            return mapper.Map(CastToDerivedClass(mapper,this), entity);
        }

        public static TDto FromEntity(IMapper mapper,TEntity entity)
        {
           return mapper.Map<TDto>(entity);
        }

        protected TDto CastToDerivedClass(IMapper mapper,BaseDto<TDto, TEntity, TKey> baseDto)
        {
            return mapper.Map<TDto>(baseDto);
        }
    }

    public abstract class BaseDto<TDto,TEntity> : BaseDto<TDto,TEntity,int>
         where TDto : class, new()
         where TEntity : class, IEntity, new()
    {

    }
}
