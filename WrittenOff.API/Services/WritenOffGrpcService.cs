using AutoMapper;
using Grpc.Core;
using WrittenOffManagement.API.Protos;
using WrittenOffManagement.Domain.Entities;
using WrittenOffManagement.Domain.Interface;
using WrittenOffManagement.Infrastructure.Data.Repository;

namespace WrittenOffManagement.API.Services
{
    public class WritenOffGrpcService : Protos.WrittenOff.WrittenOffBase
    {
        ILogger<WritenOffGrpcService> logger;
        IMapper mapper;
        IWrittenOffRepository repository;

        public WritenOffGrpcService(ILogger<WritenOffGrpcService> logger, IWrittenOffRepository repository)
        {
            this.logger = logger;
            this.repository = repository;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Domain.Entities.WrittenOff, WrittenOffModel>()
                    .ForMember(model => model.AuthorName,
                        conf => conf.MapFrom(entity => entity.Author.Name))
                    .ForMember(model => model.AuthorDescription,
                        conf => conf.MapFrom(entity => entity.Author.Descritption))
                    .ForMember(model => model.PublisherName,
                        conf => conf.MapFrom(entity => entity.Publisher.Name))
                    .ForMember(model => model.PublisherDescription,
                        conf => conf.MapFrom(entity => entity.Publisher.Descritption))
                    .ForMember(model => model.CreatedAt,
                        conf => conf.MapFrom(
                            entity => Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.SpecifyKind(entity.CreatedAt, DateTimeKind.Utc))))
                    .ForMember(model => model.UpdatedAt,
                        conf => conf.MapFrom(
                            entity => Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.SpecifyKind(entity.UpdatedAt, DateTimeKind.Utc))));

            });
            mapper = new Mapper(config);
        }

        public override async Task<WrittenOffModel> CreateWrittenOff(ExemplarRequest request, ServerCallContext context)
        {
            if (await repository.GetByIdAsync(request.LiteratureId) == null)
            {
                await repository.InsertAsync(new Domain.Entities.WrittenOff()
                {
                    Id = request.LiteratureId,
                    Author = new Domain.ValueObject.Author(request.AuthorName, request.AuthorDescription),
                    CreatedAt = DateTime.Now,
                    EmployeeId = 1,
                    Isbn = request.Isbn,
                    Name = request.Name,
                    Publisher = new Domain.ValueObject.Publisher(request.PublisherName, request.PublisherDescription),
                    PublishingYear =request.PublishingYear,
                    Quantity = 0,
                    UpdatedAt = DateTime.Now
                });
            }
            Domain.Entities.WrittenOff writtenOff = await repository.GetByIdAsync(request.LiteratureId);
            writtenOff.Quantity = writtenOff.Quantity + 1;
            await repository.UpdateAsync(writtenOff);
            await repository.SaveChangesAsync();
            return mapper.Map<WrittenOffModel>(await repository.GetByIdAsync(request.LiteratureId));
        }
    }
}
