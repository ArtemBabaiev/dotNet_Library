using AutoMapper;
using Catalog.BLL.DTO.Request;
using Catalog.BLL.DTO.Response;
using Catalog.BLL.Service.Interface;
using Catalog.DAL.Entity;
using Catalog.DAL.UOW.Interface;
using EventBus.Messages.Events;
using MassTransit;
using MassTransit.Transports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.BLL.Service
{
    public class ExemplarService: IExemplarService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IPublishEndpoint publishEndpoint;
        private readonly IMapper mapper;

        public ExemplarService(IUnitOfWork unitOfWork, IPublishEndpoint publishEndpoint, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.publishEndpoint = publishEndpoint;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ExemplarResponse>> GetAsync()
        {
            var exemplars = await unitOfWork.ExemplarRepository.GetAsync();
            return exemplars?.Select(mapper.Map<Exemplar, ExemplarResponse>);
        }

        public async Task<ExemplarResponse> GetByIdAsync(long id)
        {
            var exemplar = await unitOfWork.ExemplarRepository.GetCompleteEntityAsync(id);
            return mapper.Map<Exemplar, ExemplarResponse>(exemplar);
        }

        public async Task InsertAsync(ExemplarRequest request)
        {
            request.CreatedAt = DateTime.Now;
            request.UpdatedAt = DateTime.Now; 
            var exemplar = mapper.Map<ExemplarRequest, Exemplar>(request);
            await unitOfWork.ExemplarRepository.InsertAsync(exemplar);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(ExemplarRequest request)
        {
            request.UpdatedAt = DateTime.Now;
            var exemplar = mapper.Map<ExemplarRequest, Exemplar>(request);
            await unitOfWork.ExemplarRepository.UpdateAsync(exemplar);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            await unitOfWork.ExemplarRepository.DeleteAsync(id);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteUsingRabbitMQ(long id)
        {
            Exemplar exemplar = await unitOfWork.ExemplarRepository.GetByIdAsync(id);
            if (exemplar is null)
            {
                return;
            }
            Literature literature = await unitOfWork.LiteratureRepository.GetCompleteEntityAsync(exemplar.LiteratureId);
            var eventMessage = new WriteOffExemplarEvent
            {
                LiteratureId = exemplar.LiteratureId,
                Name = literature.Name,
                PublishingYear = literature.PublishingYear is null ? 0 : (int)literature.PublishingYear,
                Quantity = 1,
                AuthorName = literature.Author.Name,
                AuthorDescription = literature.Author.Description,
                PublisherName = literature.Publisher.Name,
                PublisherDescription = literature.Publisher.Description,
                EmployeeId = 1
            };
            await publishEndpoint.Publish(eventMessage);
            await unitOfWork.ExemplarRepository.DeleteAsync(id);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
