using AutoMapper;
using Catalog.DAL.Entity;
using Catalog.DAL.UOW.Interface;
using Grpc.Core;
using GrpcServer.Protos;
using System.Collections.Generic;
using Literature = GrpcServer.Protos.Literature;
using LiteratureEntity = Catalog.DAL.Entity.Literature;

namespace GrpcServer.Services
{
    public class LiteratureService : Literature.LiteratureBase
    {
        private readonly ILogger<LiteratureService> _logger;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public LiteratureService(ILogger<LiteratureService> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _logger = logger;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public override async Task<LiteratureModel> GetLiteratureById(GetLiteratureByIdModel request, ServerCallContext context)
        {
            var lit = await unitOfWork.LiteratureRepository.GetByIdAsync(request.LiteratureId);
            if (lit != null)
            {
                return mapper.Map<LiteratureEntity, LiteratureModel>(lit);
            }
            else
            {
                return null;
            }
        }

        public override async Task GetAllLiterature(GetAllLiteratureRequest request, IServerStreamWriter<LiteratureModel> responseStream, ServerCallContext context)
        {
            var allLiterature= await unitOfWork.LiteratureRepository.GetAsync();
            foreach (var lit in allLiterature)
            {
                await responseStream.WriteAsync(mapper.Map<LiteratureEntity, LiteratureModel>(lit));
            }
        }
    }
}
