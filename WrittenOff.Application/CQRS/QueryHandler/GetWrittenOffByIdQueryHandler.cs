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
    public class GetWrittenOffByIdQueryHandler : IRequestHandler<GetWrittenOffByIdQuery, WrittenOff>
    {
        IWrittenOffRepository writtenOffRepository;

        public GetWrittenOffByIdQueryHandler(IWrittenOffRepository writtenOffRepository)
        {
            this.writtenOffRepository = writtenOffRepository;
        }

        public async Task<WrittenOff> Handle(GetWrittenOffByIdQuery request, CancellationToken cancellationToken)
        {
            return await writtenOffRepository.GetCompleteEntityAsync(request.Id);
        }
    }
}
