using AutoMapper;
using Catalog.BLL.DTO.Request;
using Catalog.BLL.DTO.Response;
using Catalog.BLL.Service.Interface;
using Catalog.DAL.Entity;
using Catalog.DAL.Repository.Interface;
using Catalog.DAL.UOW.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.BLL.Service
{
    public class WritingService : IWritingService
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IMapper mapper;

        public WritingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<WritingResponse>> GetAsync()
        {
            var writings = await unitOfWork.WritingRepository.GetAsync();
            return writings?.Select(mapper.Map<Writing, WritingResponse>);
        }

        public async Task<WritingResponse> GetByIdAsync(long id)
        {
            var writing = await unitOfWork.WritingRepository.GetCompleteEntityAsync(id);
            return mapper.Map<Writing, WritingResponse>(writing);
        }

        public async Task InsertAsync(WritingRequest request)
        {
            request.CreatedAt = DateTime.Now;
            request.UpdatedAt = DateTime.Now;
            var writing = mapper.Map<WritingRequest, Writing>(request);
            await unitOfWork.WritingRepository.InsertAsync(writing);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(WritingRequest request)
        {
            request.UpdatedAt = DateTime.Now;
            var writing = mapper.Map<WritingRequest, Writing>(request);
            await unitOfWork.WritingRepository.UpdateAsync(writing);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            await unitOfWork.WritingRepository.DeleteAsync(id);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<WritingResponse>> GetAllWritingsWithAuthor(long authorId)
        {
            return (await unitOfWork.WritingRepository.GetWritingsWithAuthors(authorId)).Select(el => mapper.Map<Writing, WritingResponse>(el));
        }
    }
}
