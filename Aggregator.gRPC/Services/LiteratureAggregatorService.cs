using Catalog.API.Protos;

namespace Aggregator.gRPC.Services
{
    public class LiteratureAggregatorService
    {
        Literature.LiteratureClient catalogClient;

        public LiteratureAggregatorService(Literature.LiteratureClient catalogClient)
        {
            this.catalogClient = catalogClient;
        }

        public LiteratureModel GetById(long id)
        {
            var request = new GetLiteratureByIdRequest { LiteratureId = id };
            return catalogClient.GetLiteratureById(request);
        }

        public Grpc.Core.AsyncServerStreamingCall<LiteratureModel> getAll()
        {
            var request = new GetAllLiteratureRequest();
            return catalogClient.GetAllLiterature(request);
        }
    }
}
