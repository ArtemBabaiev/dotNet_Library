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
using Type = Catalog.DAL.Entity.Type;

namespace Catalog.BLL.Service
{
    public class TypeService: ITypeService
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IMapper mapper;

        public TypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<TypeResponse>> GetAsync()
        {
            var types = await unitOfWork.TypeRepository.GetAsync();
            return types?.Select(mapper.Map<Type, TypeResponse>);
        }

        public async Task<TypeResponse> GetByIdAsync(long id)
        {
            var type = await unitOfWork.TypeRepository.GetCompleteEntityAsync(id);
            return mapper.Map<Type, TypeResponse>(type);
        }

        public async Task InsertAsync(TypeRequest request)
        {
            request.CreatedAt = DateTime.Now;
            request.UpdatedAt = DateTime.Now;
            var type = mapper.Map<TypeRequest, Type>(request);
            await unitOfWork.TypeRepository.InsertAsync(type);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(TypeRequest request)
        {
            request.UpdatedAt = DateTime.Now;
            var type = mapper.Map<TypeRequest, Type>(request);
            await unitOfWork.TypeRepository.UpdateAsync(type);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            await unitOfWork.TypeRepository.DeleteAsync(id);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
