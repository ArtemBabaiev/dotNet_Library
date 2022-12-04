using Catalog.API.Protos;
using WrittenOffManagement.API.Protos;

namespace Aggregator.gRPC.Services
{
    public class ExemplarAggregatorService
    {
        Exemplar.ExemplarClient exemplarClient;
        WrittenOff.WrittenOffClient writtenOffClient;
        Literature.LiteratureClient literatureClient;

        public ExemplarAggregatorService(Exemplar.ExemplarClient exemplarClient, WrittenOff.WrittenOffClient writtenOffClient, Literature.LiteratureClient literatureClient)
        {
            this.exemplarClient = exemplarClient;
            this.writtenOffClient = writtenOffClient;
            this.literatureClient = literatureClient;
        }

        public async Task<WrittenOffModel> writeOfExemplar(long id)
        {
            var exemplar = await exemplarClient.GetExemplarByIdAsync(new GetExemplarByIdRequest { Id = id});
            var literature = await literatureClient.GetLiteratureByIdAsync((new GetLiteratureByIdRequest {LiteratureId = exemplar.LiteratureId }));
            exemplarClient.DeleteExemplarById(new DeleteExemplarByIdRequest { Id = id});
            return await writtenOffClient.CreateWrittenOffAsync(new ExemplarRequest
            {
                AuthorDescription = literature.AuthorDescription,
                AuthorName = literature.AuthorName,
                Id = exemplar.Id,
                Isbn = literature.Isbn,
                LiteratureId = exemplar.LiteratureId,
                Name = literature.Name ,
                PublisherDescription = literature.PublisherDescription ,
                PublisherName = literature.PublisherName ,
                PublishingYear = literature.PublishingYear
                
            });
        }
    }
}
