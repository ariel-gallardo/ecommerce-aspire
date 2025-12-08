using AutoMapper;
using Common.Infrastructure.Cache;
using Common.Infrastructure.Repositories;
using MassTransit;

namespace Common.Infrastructure.Messages.Entities
{
    public abstract class Consumer<T> : IConsumer<T> where T : Message
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;
        protected readonly ICacheManagerServices _cache;

        protected Consumer(IUnitOfWork unitOfWork, IMapper mapper, ICacheManagerServices cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }
        public abstract Task OnConsume(ConsumeContext<T> context);
        public async Task Consume(ConsumeContext<T> context) 
        => await OnConsume(context);
    }
}
