using Catalog.BLL.DTO.Response;
using Gateway.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Gateway.API.Services
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
            HttpResponseMessage httpResponseMessage = await client.DeleteAsync($"/api/Literature/{id}");
        }

        public async Task<LiteratureResponse> getById(long id)
        {
            var response = await client.GetAsync($"/api/Literature/{id}");
            return await response.Content.ReadFromJsonAsync<LiteratureResponse>();
        }
    }
}
