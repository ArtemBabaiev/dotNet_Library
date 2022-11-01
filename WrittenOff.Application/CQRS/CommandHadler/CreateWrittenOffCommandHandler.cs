using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WrittenOffManagement.Application.CQRS.Command;
using WrittenOffManagement.Domain.Interface;

namespace WrittenOffManagement.Application.CQRS.CommandHadler
{
    public class CreateWrittenOffCommandHandler : IRequestHandler<CreateWrittenOffCommand>
    {
        IWrittenOffRepository writtenOffRepository;

        public CreateWrittenOffCommandHandler(IWrittenOffRepository writtenOffRepository)
        {
            this.writtenOffRepository = writtenOffRepository;
        }

        public async Task<Unit> Handle(CreateWrittenOffCommand request, CancellationToken cancellationToken)
        {
            await writtenOffRepository.InsertAsync(request.WrittenOff);
            await writtenOffRepository.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
