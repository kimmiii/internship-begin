using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventAPI.Domain.Models;

namespace EventAPI.Tests.Builders
{
    public interface IEventBuilder
    {
        EventBuilder WithId();
        EventBuilder WithName();
        EventBuilder WithAcademicYear(Guid academicYearId);
        EventBuilder IsActivated();
    }
}
