using DataObjects;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IMovieService
    {
        Task<CreateLanguageResponse> CreateLanguageAsync(CreateLanguageRequest request);
    }
}
