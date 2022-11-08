using AutoMapper;
using Catalog.API.Protos;
using Catalog.DAL.UOW.Interface;
using Grpc.Core;
using static Catalog.API.Protos.Literature;

namespace Catalog.API.Services
{
    public class LiteratureGrpcService : LiteratureBase
    {
        ILogger<LiteratureGrpcService> logger;
        IMapper mapper;
        IUnitOfWork unitOfWork;


        public LiteratureGrpcService(ILogger<LiteratureGrpcService> logger, IUnitOfWork unitOfWork)
        {
            this.logger = logger;
            this.unitOfWork = unitOfWork;
           
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DAL.Entity.Literature, LiteratureModel>()
                .ForMember(model => model.AuthorName,
                    conf => conf.MapFrom(entity => entity.Author.Name))
                .ForMember(model => model.AuthorDescription,
                    conf => conf.MapFrom(entity => entity.Author.Description))
                .ForMember(model => model.PublisherName,
                    conf => conf.MapFrom(entity => entity.Publisher.Name))
                .ForMember(model => model.PublisherDescription,
                    conf => conf.MapFrom(entity => entity.Publisher.Description))
                .ForMember(model => model.CreatedAt,
                    conf => conf.MapFrom(
                        entity => Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.SpecifyKind(entity.CreatedAt?? DateTime.Now, DateTimeKind.Utc))))
                .ForMember(model => model.UpdatedAt,
                    conf => conf.MapFrom(
                        entity => Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.SpecifyKind(entity.UpdatedAt ?? DateTime.Now, DateTimeKind.Utc))));

            });
            mapper = new Mapper(config);
        }

        public override async Task GetAllLiterature(GetAllLiteratureRequest request, IServerStreamWriter<LiteratureModel> responseStream, ServerCallContext context)
        {
            var allLiterature = await unitOfWork.LiteratureRepository.GetAsync();
            foreach (var literature in allLiterature)
            {
                await responseStream.WriteAsync(mapper.Map<LiteratureModel>(literature));
            }
            logger.LogInformation($"gRPC returned all literature");
        }

        public override async Task<LiteratureModel> GetLiteratureById(GetLiteratureByIdRequest request, ServerCallContext context)
        {
            var toReturn = await unitOfWork.LiteratureRepository.GetCompleteEntityAsync(request.LiteratureId);
            if (toReturn == null)
            {
                logger.LogError($"gRPC literature with id={request.LiteratureId} not found");
                throw new RpcException(
                    new Status(StatusCode.NotFound, $"Literature with id={request.LiteratureId} not found"));
            }
            logger.LogInformation($"gRPC returned literature with id={request.LiteratureId}");
            return mapper.Map<LiteratureModel>(toReturn);
        }
    }
}
