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
    public class GenreService: IGenreService
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IMapper mapper;

        public GenreService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<GenreResponse>> GetAsync()
        {
            var genres = await unitOfWork.GenreRepository.GetAsync();
            return genres?.Select(mapper.Map<Genre, GenreResponse>);
        }

        public async Task<GenreResponse> GetByIdAsync(long id)
        {
            var genre = await unitOfWork.GenreRepository.GetCompleteEntityAsync(id);
            return mapper.Map<Genre, GenreResponse>(genre);
        }

        public async Task InsertAsync(GenreRequest request)
        {
            request.CreatedAt = DateTime.Now;
            request.UpdatedAt = DateTime.Now;
            var genre = mapper.Map<GenreRequest, Genre>(request);
            await unitOfWork.GenreRepository.InsertAsync(genre);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(GenreRequest request)
        {
            request.UpdatedAt = DateTime.Now;
            var genre = mapper.Map<GenreRequest, Genre>(request);
            await unitOfWork.GenreRepository.UpdateAsync(genre);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            await unitOfWork.GenreRepository.DeleteAsync(id);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
