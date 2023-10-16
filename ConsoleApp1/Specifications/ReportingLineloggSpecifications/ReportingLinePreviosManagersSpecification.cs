using ServerSide.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ServerSide.Specifications.ReportingLineloggSpecifications
{
    public  class ReportingLinePreviosManagersSpecification: ReportingLineSpecification<string>
    {
        public override Expression<Func<string, ReportingLinelog, bool>> ToExpression()
        {
            return (subordinatename, log) => log.subordinateName== subordinatename;

        }
    }
}
