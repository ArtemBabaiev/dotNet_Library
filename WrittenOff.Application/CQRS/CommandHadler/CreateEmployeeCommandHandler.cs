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
    public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand>
    {
        IEmployeeRepository employeeRepository;

        public CreateEmployeeCommandHandler(IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        public async Task<Unit> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            await employeeRepository.InsertAsync(request.Employee);
            await employeeRepository.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
