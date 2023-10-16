using ServerSide.Models;
using ServerSide.Specifications.ReportingLineloggSpecifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ServerSide.Specifications.ReportingLineloggSpecifications
{
    public  class ReportingLineLogIsActiveAfter: ReportingLineSpecification<DateOnly>
    {

        public override Expression<Func<DateOnly, ReportingLinelog, bool>> ToExpression()
        {
            return (Date, log) => log.EndDate > Date; 
            
        }
    }
}

