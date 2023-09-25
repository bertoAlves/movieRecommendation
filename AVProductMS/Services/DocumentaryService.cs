using Common.AVProductMS.DTO;
using Microsoft.Extensions.Options;
using AVProduct.Configuration;
using AVProduct.Factories.Interfaces;
using AVProduct.Services.Interfaces;
using Common.HttpClientWrapper;

namespace AVProduct.Services
{
    /// <summary>
    /// Documentary Service
    /// </summary>
    public class DocumentaryService : IDocumentaryService
    {
        private readonly IHttpClientWrapper _httpClient;
        private readonly IDocumentaryFactory _factory;
        private readonly IOptions<MovieDBApiOptions> _movieDbApiOptions;

        /// <summary>
        /// Documentary Service
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="factory"></param>
        public DocumentaryService(IHttpClientWrapper httpClient, IDocumentaryFactory factory, IOptions<MovieDBApiOptions> movieDbApiOptions)
        {
            _httpClient = httpClient;
            _factory = factory;
            _movieDbApiOptions = movieDbApiOptions;
        }


        /// <summary>
        /// Get All Time Documentaries Based On Topics
        /// </summary>
        /// <returns>List of Documentaries</returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<DocumentaryDTO>> GetAllTimeDocumentaries(string topics)
        {
            throw new NotImplementedException();
        }
    }
}
