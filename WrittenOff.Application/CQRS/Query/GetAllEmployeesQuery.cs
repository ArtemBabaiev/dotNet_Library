using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WrittenOffManagement.Domain.Entities;

namespace WrittenOffManagement.Application.CQRS.Query
{
    public class GetAllEmployeesQuery:IRequest<IEnumerable<Employee>>
    {
    }
}
