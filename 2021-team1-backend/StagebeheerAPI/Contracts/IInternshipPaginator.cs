using StagebeheerAPI.Models;
using System.Collections.Generic;

namespace StagebeheerAPI.Contracts
{
    public interface IInternshipPaginator
    {
        PagedInternshipsResult GetInternshipsAndPages(List<Internship> internships, PageCriteria pageCriteria);
    }
}
