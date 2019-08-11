using DataObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IAdminService
    {
        Task<CreateLanguageResponse> CreateLanguageAsync(CreateLanguageRequest request);

        /// <summary>
        /// Insert to database a list of Entities of type TEntity
        /// </summary>
        /// <typeparam name="TEntity">An entity that has its text on LangText</typeparam>
        /// <param name="requests">Array of fields based on Language Code</param>
        /// <param name="type">Actual DB model class Type</param>
        /// <param name="idPropName">primary key of TEntity Table</param>
        /// <returns></returns>
        Task<ResponseBase> BatchInsertEntitiesAsync<TEntity>(IEnumerable<TranslatedRequest> requests, Type type, string idPropName)
            where TEntity : BaseTranslateModel;

        Task<CreateContributorTypePerMovieResponse> CreateContributorTypesPerMovieAsync(IEnumerable<CreateContributorTypePerMovieRequest> requests);

        Task<CreateMovieGenreResponse> InsertMovieGenresAsync(IEnumerable<CreateMovieGenreRequest> requests);

    }
}
