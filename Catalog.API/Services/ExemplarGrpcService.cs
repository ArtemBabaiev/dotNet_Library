using AutoMapper;
using Catalog.API.Protos;
using Catalog.DAL.UOW.Interface;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using static Catalog.API.Protos.Exemplar;

namespace Catalog.API.Services
{
    public class ExemplarGrpcService: ExemplarBase
    {
        ILogger<ExemplarGrpcService> logger;
        IMapper mapper;
        IUnitOfWork unitOfWork;

        public ExemplarGrpcService(ILogger<ExemplarGrpcService> logger, IUnitOfWork unitOfWork)
        {
            this.logger = logger;
            this.unitOfWork = unitOfWork;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DAL.Entity.Exemplar, ExemplarModel>()
                .ForMember(model => model.CreatedAt,
                conf => conf.MapFrom(
                    entity => Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.SpecifyKind(entity.CreatedAt ?? DateTime.Now, DateTimeKind.Utc))))
                .ForMember(model => model.UpdatedAt,
                conf => conf.MapFrom(
                    entity => Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.SpecifyKind(entity.UpdatedAt ?? DateTime.Now, DateTimeKind.Utc))));

            });
            mapper = new Mapper(config);
        }

        public override async Task<Empty> DeleteExemplarById(DeleteExemplarByIdRequest request, ServerCallContext context)
        {
            await unitOfWork.ExemplarRepository.DeleteAsync(request.Id);
           await unitOfWork.SaveChangesAsync();
            return new Google.Protobuf.WellKnownTypes.Empty();
        }

        public override async Task<ExemplarModel> GetExemplarById(GetExemplarByIdRequest request, ServerCallContext context)
        {
            var toReturn = await unitOfWork.ExemplarRepository.GetByIdAsync(request.Id);
            if (toReturn == null)
            {
                logger.LogError($"gRPC exemplar with id={request.Id} not found");
                throw new RpcException(
                    new Status(StatusCode.NotFound, $"exemplar with id={request.Id} not found"));
            }
            logger.LogInformation($"gRPC returned exemplar with id={request.Id}");
            return mapper.Map<ExemplarModel>(toReturn);
        }
    }
}
