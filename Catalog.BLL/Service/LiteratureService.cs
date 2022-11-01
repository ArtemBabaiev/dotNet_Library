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
    public class LiteratureService : ILiteratureService
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IMapper mapper;

        public LiteratureService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<LiteratureResponse>> GetAsync()
        {
            var literatures = await unitOfWork.LiteratureRepository.GetAsync();
            return literatures?.Select(mapper.Map<Literature, LiteratureResponse>);
        }

        public async Task<LiteratureResponse> GetByIdAsync(long id)
        {
            var literature = await unitOfWork.LiteratureRepository.GetCompleteEntityAsync(id);
            return mapper.Map<Literature, LiteratureResponse>(literature);
        }

        public async Task InsertAsync(LiteratureRequest request)
        {
            request.CreatedAt = DateTime.Now;
            request.UpdatedAt = DateTime.Now;
            var literature = mapper.Map<LiteratureRequest, Literature>(request);
            await unitOfWork.LiteratureRepository.InsertAsync(literature);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(LiteratureRequest request)
        {
            request.UpdatedAt = DateTime.Now;
            var literature = mapper.Map<LiteratureRequest, Literature>(request);
            await unitOfWork.LiteratureRepository.UpdateAsync(literature);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            await unitOfWork.LiteratureRepository.DeleteAsync(id);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<LiteratureResponse>> GetAllWithAuthor(long authorId)
        {
            Author author = await unitOfWork.AuthorRepository.GetCompleteEntityAsync(authorId);
            await unitOfWork.LiteratureRepository.GetLiteratureWithAuthor(authorId);
            return author.Literatures.Select(el => mapper.Map<Literature, LiteratureResponse>(el));
        }
    }
}
