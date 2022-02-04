using StagebeheerAPI.Contracts;
using StagebeheerAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace StagebeheerAPI.FilterPattern
{
    public class CombinedFilter : ICombinedFilter
    {

        public List<Internship> comboFiltering(List<Internship> internships, Internship criteria)
        {
            var filters = new List<IFilter>();
            if (criteria.UserFavourites.Count != 0)
            {
                var _favouritesFilter = new FavouritesFilter(criteria.UserFavourites.FirstOrDefault().UserId);
                filters.Add(_favouritesFilter);
            }
            if (criteria.UserInternships.Count != 0)
            {
                var _userInternshipsFilter = new UserInternshipsFilter(criteria.UserInternships.FirstOrDefault().UserId);
                filters.Add(_userInternshipsFilter);
            }
            var _companyFilter = new CompanyFilter(criteria.CompanyId);
            filters.Add(_companyFilter);
            var _expectationFilter = new ExpectationCombinedFilter(criteria.InternshipExpectation.ToList());
            filters.Add(_expectationFilter);
            var _periodFilter = new PeriodCombinedFilter(criteria.InternshipPeriod.ToList());
            filters.Add(_periodFilter);
            var _specialisationFilter = new SpecialisationCombinedFilter(criteria.InternshipSpecialisation.ToList());
            filters.Add(_specialisationFilter);
            var _descriptionFilter = new DescriptionFilter(criteria.AssignmentDescription);
            filters.Add(_descriptionFilter);
            List<IFilter> environmentOrFilter = new List<IFilter>();
            if (criteria.InternshipEnvironmentOthers != "")
            {
                var _otherEnvironmentsFilter = new OtherEnvironmentsFilter(criteria.InternshipEnvironmentOthers);
                environmentOrFilter.Add(_otherEnvironmentsFilter);
            }
            if (criteria.InternshipEnvironment.Count() != 0)
            {
                var _environmentFilter = new EnvironmentCombinedFilter(criteria.InternshipEnvironment.ToList());
                environmentOrFilter.Add(_environmentFilter);
            }
            if (environmentOrFilter.Count() > 0)
            {
                var _environmentsOrFilter = new OrFilters(environmentOrFilter);
                filters.Add(_environmentsOrFilter);
            }
            var combinedFilter = new AndFilters(filters);
            //var internships = _RepositoryWrapper.Internship.FindAll()
            //    .Include(c => c.Company)
            //    .Include(e => e.InternshipEnvironment)
            //    .Include(x => x.InternshipExpectation)
            //    .Include(p => p.InternshipPeriod)
            //    .Include(s => s.InternshipSpecialisation)
            //    .ToList<Internship>();
            return combinedFilter.meetFilter(internships);
        }
        //public List<Internship> meetFilter(List<Internship> internships)
        //{
        //    return combinedFilter.meetFilter(internships);
        //    //    AndFilters combinedFilter = new AndFilters(filters);
        //    //    return combinedFilter.meetFilter(internships);

        //    //    //AndFilters expectationSpecialisation = new AndFilters(_expectationFilter, _specialisationFilter);
        //    //    //AndFilters descriptionOtherEnvironment = new AndFilters(_descriptionFilter, _otherEnvironmentsFilter);
        //    //    //List<Internship> filteredList = companyEnvironment.meetFilter(internships);
        //    //    //filteredList = descriptionOtherEnvironment.meetFilter(companyEnvironment.meetFilter(expectationSpecialisation.meetFilter(_periodFilter.meetFilter(internships))));


        //}






    }
}
