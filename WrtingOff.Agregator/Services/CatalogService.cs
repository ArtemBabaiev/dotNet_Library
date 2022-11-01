using Aggregator.DTO;
using Catalog.BLL.DTO.Request;
using Catalog.BLL.DTO.Response;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Agregator.Services.Interfaces;

namespace Agregator.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient client;

        public CatalogService(HttpClient client)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client)); ;
        }


        public async Task deleteExemplar(long id)
        {
            HttpResponseMessage httpResponseMessage = await client.DeleteAsync($"/api/Exemplar/{id}");
        }

        public async Task<LiteratureResponse> getLiteratureById(long id)
        {
            var response = await client.GetAsync($"/api/Literature/{id}");
            return await response.Content.ReadFromJsonAsync<LiteratureResponse>();
        }

        public async Task createLiterature(LiteratureAggDTO literature)
        {
            LiteratureRequest request = new LiteratureRequest
            {
                Id = literature.Id,
                CreatedAt = literature.CreatedAt,
                UpdatedAt = literature.UpdatedAt,
                Description = literature.Description,
                Isbn = literature.Isbn,
                Name = literature.Name,
                NumberOfPages = literature.NumberOfPages,
                PublishingYear = literature.PublishingYear,
                PublisherId = literature.PublisherId,
                AuthorId = literature.AuthorId,
                GenreId = literature.GenreId,
                TypeId = literature.TypeId,
                IsLendable = literature.IsLendable,
                LendPeriodInDays = literature.LendPeriodInDays
            };
            HttpResponseMessage httpResponseMessage = await client.PostAsJsonAsync($"/api/Literature", request);
        }

        public async Task deleteLiterature(long id)
        {
            HttpResponseMessage httpResponseMessage = await client.DeleteAsync($"/api/Literature/{id}");
        }

        public async Task updateLiterature(long id, LiteratureAggDTO literature)
        {
            LiteratureRequest request = new LiteratureRequest
            {
                Id = literature.Id,
                CreatedAt = literature.CreatedAt,
                UpdatedAt = literature.UpdatedAt,
                Description = literature.Description,
                Isbn = literature.Isbn,
                Name = literature.Name,
                NumberOfPages = literature.NumberOfPages,
                PublishingYear = literature.PublishingYear,
                PublisherId = literature.PublisherId,
                AuthorId = literature.AuthorId,
                GenreId = literature.GenreId,
                TypeId = literature.TypeId,
                IsLendable = literature.IsLendable,
                LendPeriodInDays = literature.LendPeriodInDays
            };
            HttpResponseMessage httpResponseMessage = await client.PutAsJsonAsync($"/api/Literature/{id}", request);
        }
    }
}
