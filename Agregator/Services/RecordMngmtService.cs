using Aggregator.DTO;
using RecordManagment.BLL.DTO;
using Agregator.Services.Interfaces;

namespace Agregator.Services
{
    public class RecordMngmtService : IRecordMngmtService
    {
        private readonly HttpClient client;

        public RecordMngmtService(HttpClient client)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task deleteExemplar(long id)
        {
            HttpResponseMessage httpResponseMessage = await client.DeleteAsync($"/api/Exemplar/{id}");
        }
        public async Task createLiterature(LiteratureAggDTO literature)
        {
            LiteratureDTO request = new LiteratureDTO
            {
                Id = literature.Id,
                Name = literature.Name,
                Isbn = literature.Isbn,
                LendTimeInDays = (int)literature.LendPeriodInDays,
                CreatedAt = literature.CreatedAt,
                UpdatedAt = literature.UpdatedAt,
            };
            HttpResponseMessage httpResponseMessage = await client.PostAsJsonAsync($"/api/Literature", request);
        }

        public async Task deleteLiterature(long id)
        {
            HttpResponseMessage httpResponseMessage = await client.DeleteAsync($"/api/Literature/{id}");
        }

        public async Task updateLiterature(long id, LiteratureAggDTO literature)
        {
            LiteratureDTO request = new LiteratureDTO
            {
                Id = literature.Id,
                Name = literature.Name,
                Isbn = literature.Isbn,
                LendTimeInDays = (int)literature.LendPeriodInDays,
                CreatedAt = literature.CreatedAt,
                UpdatedAt = literature.UpdatedAt,
            };
            HttpResponseMessage httpResponseMessage = await client.PutAsJsonAsync($"/api/Literature/{id}", request);
        }
    }
}
