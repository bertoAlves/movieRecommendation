using APIGateway.DTO.AVProduct;
using APIGateway.Factories.Interfaces;
using APIGateway.Services.Interfaces;
using Common.CinemaMS.DTO;
using Common.AVProductMS.DTO;

namespace CinemaMS.Services
{
    /// <summary>
    /// Recommendation Service
    /// </summary>
    public class RecommendationService : IRecommendationService
    {
        private readonly ICinemaMSService _cinemaMS;
        private readonly IGenreFactory _factory;
        private readonly IAVProductMSService _avproductMS;

        /// <summary>
        /// Recommendation Service
        /// </summary>
        /// <param name="cinemaMS"></param>
        /// <param name="factory"></param>
        /// <param name="avproductMS"></param>
        public RecommendationService(ICinemaMSService cinemaMS, IGenreFactory factory, IAVProductMSService avproductMS)
        {
            _cinemaMS = cinemaMS;
            _factory = factory;
            _avproductMS = avproductMS;
        }

        public Task<IEnumerable<DocumentaryRecommendation>> GetAllTimeDocumentariesRecommendation(string topics)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MovieRecommendation>> GetAllTimeMoviesRecommendation(string? keywords, string? genre)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TVShowRecommendation>> GetAllTimeTVShowsRecommendation(string keywords, string genres)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MovieRecommendation>> GetUpcomingMoviesRecommendation(string? keywords, string? genres, int maxDate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MovieRecommendation>> GetUpcomingMoviesRecommendationByAgeRate(string genres, int maxDate, string ageRate)
        {
            throw new NotImplementedException();
        }

        public Task<Billboard> GetSuggestedBillboard(int numberScreens, int numberWeeks)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get Intelligent Billboard
        /// </summary>
        /// <param name="numberWeeks"></param>
        /// <param name="bigScreens"></param>
        /// <param name="smallScreens"></param>
        /// <param name="useSuccessfulGenres"></param>
        /// <returns></returns>
        public async Task<IntelligentBillboard> GetIntelligentBillboard(int numberWeeks, int bigScreens, int smallScreens, bool useSuccessfulGenres)
        {
            Dictionary<int, IntelligentWeekPlan> billboard = new Dictionary<int, IntelligentWeekPlan>();

            if (numberWeeks <= 0)
            {
                return new IntelligentBillboard
                {
                    billboard = billboard,
                };
            }

            if (smallScreens <= 0 && bigScreens <= 0)
            {
                return new IntelligentBillboard
                {
                    billboard = billboard,
                };
            }

            IEnumerable<GenreDTO>? genres = null;
            string genreIds = "";
            string without_genreIds = "";

            

            if (useSuccessfulGenres)
            {
                genres = await _cinemaMS.GetSuccessfulGenres();
            }

            if(genres != null) {
                IEnumerable<int> moviedbgenreids = await _factory.MapBeezyGenre(genres);
                genreIds = string.Join("|", moviedbgenreids); //movies that have atleast one genre id
                without_genreIds = string.Join(",", moviedbgenreids);
            }     

            IEnumerable<MovieDTO> blockbusters = (bigScreens <= 0) ? new List<MovieDTO>() : await _avproductMS.GetBlockbusterMovies(genreIds, numberWeeks, bigScreens);
            IEnumerable<MovieDTO> minorityGenres = (smallScreens <= 0) ? new List<MovieDTO>() :  await _avproductMS.GetMinorityGenresMovies(without_genreIds, numberWeeks, smallScreens);

            List<MovieDTO> blocks = blockbusters.ToList();
            List<MovieDTO> mins = minorityGenres.ToList();

            if(blocks.Count == 0 && mins.Count == 0)
            {
                return new IntelligentBillboard
                {
                    billboard = billboard,
                };
            }

            int week = 0;
            while(week < numberWeeks)
            {
                IntelligentWeekPlan weekPlan = new IntelligentWeekPlan();
                weekPlan.BigScreens = blocks.Take(bigScreens);
                weekPlan.SmallScreens = mins.Take(smallScreens);

                blocks = blocks.Skip(bigScreens).ToList();
                mins = mins.Skip(smallScreens).ToList();

                billboard[week] = weekPlan;
                week++;
            }

            return new IntelligentBillboard
            {
                billboard = billboard,
            };
        }

    }
}
