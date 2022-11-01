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
    public class AuthorService : IAuthorService
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IMapper mapper;

        public AuthorService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<AuthorResponse>> GetAsync()
        {
            var authors = await unitOfWork.AuthorRepository.GetAsync();
            return authors?.Select(mapper.Map<Author, AuthorResponse>);
        }

        public async Task<AuthorResponse> GetByIdAsync(long id)
        {
            var author = await unitOfWork.AuthorRepository.GetCompleteEntityAsync(id);
            return mapper.Map<Author, AuthorResponse>(author);
        }

        public async Task InsertAsync(AuthorRequest request)
        {
            request.CreatedAt = DateTime.Now;
            request.UpdatedAt = DateTime.Now;
            var author = mapper.Map<AuthorRequest, Author>(request);
            await unitOfWork.AuthorRepository.InsertAsync(author);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(AuthorRequest request)
        {
            request.UpdatedAt = DateTime.Now;
            var author = mapper.Map<AuthorRequest, Author>(request);
            await unitOfWork.AuthorRepository.UpdateAsync(author);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            await unitOfWork.AuthorRepository.DeleteAsync(id);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
