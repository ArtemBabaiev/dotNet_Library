using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WrittenOffManagement.Application.CQRS.Command
{
    public class DeleteWrittenOffCommand : IRequest
    {
        public long Id { get; set; }

    }
}
