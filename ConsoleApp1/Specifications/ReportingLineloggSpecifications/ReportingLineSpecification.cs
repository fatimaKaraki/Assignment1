using ServerSide.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ServerSide.Specifications.ReportingLineloggSpecifications
{
    public abstract class ReportingLineSpecification <T> 
    {
        public bool IsSatisfiedBy(T criteria, ReportingLinelog log )
        {
            Func<T, ReportingLinelog , bool> predicate = ToExpression().Compile();
            return predicate(criteria, log);
           
        }
        public abstract Expression<Func<T, ReportingLinelog, bool>> ToExpression(); 
        
    }
}
