using Catalog.BLL.DTO.Request;
using Catalog.BLL.DTO.Response;
using Gateway.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WrittenOffManagement.Application.DTO.Request;
using WrittenOffManagement.Application.DTO.Response;

namespace Gateway.API.Services
{
    public class WrittenOffService: IWrittenOffService
    {
        private readonly HttpClient client;

        public WrittenOffService(HttpClient client)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client)); ;
        }

        public async Task createWrittenOff(ExemplarRequest request, LiteratureResponse literature)
        {
            WrittenOffRequest writtenOffRequest = new WrittenOffRequest
            {
                Author = new WrittenOffManagement.Domain.ValueObject.Author(literature.Author.Name, literature.Author.Description),
                CreatedAt = DateTime.Now,
                EmployeeId = 1,
                Id = literature.Id,
                Isbn = literature.Isbn,
                Name = literature.Name,
                Publisher = new WrittenOffManagement.Domain.ValueObject.Publisher(literature.Publisher.Name, literature.Publisher.Description),
                PublishingYear = (int)literature.PublishingYear,
                Quantity = 1,
                UpdatedAt = DateTime.Now
            };
            await client.PostAsJsonAsync("/api/WrittenOff", writtenOffRequest);

        }

        public async Task<IEnumerable<WrittenOffResponse>> GetWrittenOffs()
        {
            var response = await client.GetAsync("/api/WrittenOff");
            return await response.Content.ReadFromJsonAsync<List<WrittenOffResponse>>();
        }
    }
}
