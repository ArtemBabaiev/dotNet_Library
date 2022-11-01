using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WrittenOffManagement.Domain.Entities;

namespace WrittenOffManagement.Application.CQRS.Command
{
    public class CreateEmployeeCommand : IRequest
    {
        public Employee Employee { get; set; }
    }
}
