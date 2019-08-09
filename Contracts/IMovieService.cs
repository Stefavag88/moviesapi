using DataObjects;
using DataObjects.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IMovieService
    {
        Task<CreateLanguageResponse> CreateLanguageAsync(CreateLanguageRequest request);

        Task<ResponseBase> BatchInsertEntitiesAsync<TEntity>(IEnumerable<TranslatedRequest> requests, Type type, string idPropName) where TEntity : BaseTranslateModel;

        MovieViewModel GetMovieById(string langCode, int id);

        IEnumerable<object> GetAllMovies(string langCode);
    }
}
