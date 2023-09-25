using Common.AVProductMS.DTO;

namespace AVProduct.Services.Interfaces
{
    public interface IDocumentaryService
    {
        Task<IEnumerable<DocumentaryDTO>> GetAllTimeDocumentaries(string topics);
    }
}
