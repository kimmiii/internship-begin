using StagebeheerAPI.Contracts;
using StagebeheerAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StagebeheerAPI.Domain
{
    public class InternshipPaginator : IInternshipPaginator
    {
        private IRepositoryWrapper _RepositoryWrapper;

        public InternshipPaginator(IRepositoryWrapper repositoryWrapper)
        {
            _RepositoryWrapper = repositoryWrapper;
        }

        public PagedInternshipsResult GetInternshipsAndPages(List<Internship> internships, PageCriteria pageCriteria)
        {
            return new PagedInternshipsResult
            {
                Internships = _GetPagedInternships(internships, pageCriteria.InternshipsPerPage, pageCriteria.PageNumber),
                Pages = _GetNumberOfPages(internships, pageCriteria.InternshipsPerPage)
            };
        }

        private List<Internship> _GetPagedInternships(List<Internship> internshipList, int internshipsPerPage, int pageNumber)
        {
            var internshipWithlastPagesRemoved = internshipList.Take(internshipsPerPage * pageNumber);
            var pagedInternships = internshipWithlastPagesRemoved.Skip(internshipsPerPage * (pageNumber - 1));

            return pagedInternships.ToList();
        }

        private int _GetNumberOfPages(List<Internship> internshipList, int internshipsPerPage)
        {
            if (internshipList.Count > 0) return (int)Math.Ceiling((double)internshipList.Count / internshipsPerPage);
            return 0;
        }
    }
}
