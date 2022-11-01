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
    public class UpdateWrittenOffCommandHandler : IRequestHandler<UpdateWrittenOffCommand>
    {
        IWrittenOffRepository writtenOffRepository;

        public UpdateWrittenOffCommandHandler(IWrittenOffRepository writtenOffRepository)
        {
            this.writtenOffRepository = writtenOffRepository;
        }

        public async Task<Unit> Handle(UpdateWrittenOffCommand request, CancellationToken cancellationToken)
        {
            if (await writtenOffRepository.GetByIdAsync(request.WrittenOff.Id) == null)
            {
                request.WrittenOff.Quantity = 0;
                await writtenOffRepository.InsertAsync(request.WrittenOff);
            }
            Domain.Entities.WrittenOff writtenOff = await writtenOffRepository.GetByIdAsync(request.WrittenOff.Id);
            writtenOff.Quantity = writtenOff.Quantity + 1;
            await writtenOffRepository.UpdateAsync(writtenOff);
            await writtenOffRepository.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
