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
    public class DeleteWrittenOffCommandHandler : IRequestHandler<DeleteWrittenOffCommand>
    {
        IWrittenOffRepository writtenOffRepository;

        public DeleteWrittenOffCommandHandler(IWrittenOffRepository writtenOffRepository)
        {
            this.writtenOffRepository = writtenOffRepository;
        }

        public async Task<Unit> Handle(DeleteWrittenOffCommand request, CancellationToken cancellationToken)
        {
            await writtenOffRepository.DeleteAsync(request.Id);
            await writtenOffRepository.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
