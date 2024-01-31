using ArtworkService.Services.IServices;
using System.Text;
using System.Text.Json;

namespace ArtworkService.Services
{
    public class BidsService : IBid
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "";
        public BidsService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> UpdateBidStatus(List<string> artworkIds)
        {
            string jsoncontent = JsonSerializer.Serialize(artworkIds);
            HttpContent content = new StringContent(jsoncontent, Encoding.UTF8, "application/json");
            await _httpClient.PostAsync(_apiUrl, content);
            return "updated";
        }
    }
}
