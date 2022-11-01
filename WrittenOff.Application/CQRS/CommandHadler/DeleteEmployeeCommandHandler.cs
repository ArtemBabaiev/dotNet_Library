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
    public class DeleteEmployeeCommandHandler: IRequestHandler<DeleteEmployeeCommand>
    {
        IEmployeeRepository employeeRepository;

        public DeleteEmployeeCommandHandler(IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        public async Task<Unit> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            await employeeRepository.DeleteAsync(request.Id);
            await employeeRepository.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
