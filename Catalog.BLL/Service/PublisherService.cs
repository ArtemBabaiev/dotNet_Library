using AutoMapper;
using Catalog.BLL.DTO.Request;
using Catalog.BLL.DTO.Response;
using Catalog.BLL.Service.Interface;
using Catalog.DAL.Entity;
using Catalog.DAL.UOW.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.BLL.Service
{
    public class PublisherService: IPublisherService
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IMapper mapper;

        public PublisherService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<PublisherResponse>> GetAsync()
        {
            var publishers = await unitOfWork.PublisherRepository.GetAsync();
            return publishers?.Select(mapper.Map<Publisher, PublisherResponse>);
        }

        public async Task<PublisherResponse> GetByIdAsync(long id)
        {
            var publisher = await unitOfWork.PublisherRepository.GetCompleteEntityAsync(id);
            return mapper.Map<Publisher, PublisherResponse>(publisher);
        }

        public async Task InsertAsync(PublisherRequest request)
        {
            request.CreatedAt = DateTime.Now;
            request.UpdatedAt = DateTime.Now; 
            var publisher = mapper.Map<PublisherRequest, Publisher>(request);
            await unitOfWork.PublisherRepository.InsertAsync(publisher);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(PublisherRequest request)
        {
            request.UpdatedAt = DateTime.Now;
            var publisher = mapper.Map<PublisherRequest, Publisher>(request);
            await unitOfWork.PublisherRepository.UpdateAsync(publisher);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            await unitOfWork.PublisherRepository.DeleteAsync(id);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
