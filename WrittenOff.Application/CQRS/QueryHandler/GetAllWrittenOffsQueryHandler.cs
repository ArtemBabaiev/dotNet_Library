using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WrittenOffManagement.Application.CQRS.Query;
using WrittenOffManagement.Domain.Entities;
using WrittenOffManagement.Domain.Interface;

namespace WrittenOffManagement.Application.CQRS.QueryHandler
{
    public class GetAllWrittenOffsQueryHandler : IRequestHandler<GetAllWrittenOffsQuery, IEnumerable<WrittenOff>>
    {
        IWrittenOffRepository writtenOffRepository;

        public GetAllWrittenOffsQueryHandler(IWrittenOffRepository writtenOffRepository)
        {
            this.writtenOffRepository = writtenOffRepository;
        }

        public async Task<IEnumerable<WrittenOff>> Handle(GetAllWrittenOffsQuery request, CancellationToken cancellationToken)
        {
            return await writtenOffRepository.GetAsync();
        }
    }
}
