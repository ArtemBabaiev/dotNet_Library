using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using WrittenOffManagement.Application.CQRS.Command;
using WrittenOffManagement.Domain.Entities;

namespace WrittenOffManagement.API.EventBusConsumer
{
    public class WriteOffExemplarConsumer : IConsumer<WriteOffExemplarEvent>
    {
        IMediator mediator;
        ILogger<WriteOffExemplarConsumer> logger;
        IMapper mapper;

        public WriteOffExemplarConsumer(IMediator mediator, ILogger<WriteOffExemplarConsumer> logger, IMapper mapper)
        {
            this.mediator = mediator;
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task Consume(ConsumeContext<WriteOffExemplarEvent> context)
        {
            WriteOffExemplarEvent wEvent = context.Message;
            var command = new UpdateWrittenOffCommand {
                Id = wEvent.LiteratureId,
                WrittenOff = mapper.Map<WrittenOff>(wEvent)
            };
            command.WrittenOff.Author = new Domain.ValueObject.Author(wEvent.AuthorName, wEvent.AuthorDescription);
            command.WrittenOff.Publisher = new Domain.ValueObject.Publisher(wEvent.PublisherName, wEvent.PublisherDescription);
            await mediator.Send(command);
            logger.LogInformation("WriteOffExemplarEvent succesfully exxecuted");
        }
    }
}
