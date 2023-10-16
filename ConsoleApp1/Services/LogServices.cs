using ServerSide.customedAttributes;
using ServerSide.Models;
using ServerSide.Repositories;
using ServerSide.Specifications.ReportingLineloggSpecifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ServerSide.Services
{
    public class LogServices : Service
    {
        private IReportingLineLoggingRepository _reportingLineLoggingRepository;

        public LogServices(IReportingLineLoggingRepository reportingLineLoggingRepository)
        {
            _reportingLineLoggingRepository = reportingLineLoggingRepository;
        }

        public async  Task AddReportingLineRelation(string managerName, string subordinateName)
        {
            ReportingLinelog log = new ReportingLinelog()
            {
                ManagerName = managerName,
                subordinateName = subordinateName,
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.MaxValue)
            }; 
            await _reportingLineLoggingRepository.AddAsync(log);
        }

        [Admin]
        public async Task EndReportingLineRelation (string managerName, string subOrdinateName)
        {
            Console.WriteLine("ReportingLine Relation is being ended"); 
            ReportingLinelog report= await _reportingLineLoggingRepository.GetAsync(managerName, subOrdinateName);
            if (report == null)
            {
                throw new Exception("No such Reporting Relation found"); 
            }
            lock (report)
            {
                report.EndDate= DateOnly.FromDateTime(DateTime.Now);
            }
            await _reportingLineLoggingRepository.SaveChanges(); 
        }

        [Admin]
        public Task<IEnumerable<ReportingLinelog>> GetAllReportingLineRelations()
        {
            return _reportingLineLoggingRepository.GetAll();

        }

        [Admin]
        public async Task<IEnumerable<ReportingLinelog>> GetAllMangersSubordinateHistory(string ManagerName)
        {
            ReportingLineIsManagedbySpecification specification = new ReportingLineIsManagedbySpecification();
            return await _reportingLineLoggingRepository.Filter(specification, ManagerName);
        }
        [Admin]
        public async Task<IEnumerable<String>> GetAllMangersActiveSubordinate(string ManagerName)
        {
            ReportingLineIsManagedbySpecification specification = new ReportingLineIsManagedbySpecification();
            IEnumerable<ReportingLinelog> list = await _reportingLineLoggingRepository.Filter(specification, ManagerName);
            return list.Where(t => t.EndDate== DateOnly.MaxValue).Select(t => t.subordinateName);
        }
        [Admin]
        public async Task<IEnumerable<String>> GetAllPreviosManagersFor(string subordinateName)
        {
            ReportingLinePreviosManagersSpecification filter = new ReportingLinePreviosManagersSpecification();
            IEnumerable<ReportingLinelog>  list = await _reportingLineLoggingRepository.Filter(filter, subordinateName);
            return list.Select(t => t.ManagerName); 

        }
        [Admin]
        public async Task<IEnumerable<ReportingLinelog>> GetAllActiveReportingRelations()
        {
            ReportingLineLogIsActiveAfter filter = new ReportingLineLogIsActiveAfter();
            return await _reportingLineLoggingRepository.Filter(filter, DateOnly.FromDateTime(DateTime.Now));

        }
        [Admin]
        public async Task<IEnumerable<ReportingLinelog>> GetAllActiveReportingRelationAfter(DateOnly date)
        {
            ReportingLineLogIsActiveAfter filter = new ReportingLineLogIsActiveAfter();
            return await _reportingLineLoggingRepository.Filter(filter, date);
        }

    }
}
