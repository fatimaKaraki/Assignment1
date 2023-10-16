using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ServerSide.customedAttributes;
using ServerSide.Models;
using ServerSide.Modle;
using ServerSide.Specifications.ReportingLineloggSpecifications;
using ServerSide.Support;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSide.Repositories
{
    public interface IReportingLineLoggingRepository
    {
        Task AddAsync(ReportingLinelog reportingLinelog);
        Task<ReportingLinelog> GetAsync(string managerName, string subordinateName);
        Task<IEnumerable<ReportingLinelog>> GetAll();
        Task<IEnumerable<ReportingLinelog>> Filter<T>(ReportingLineSpecification<T> filter, T criteria);
        Task Initialize();
        Task SaveChanges();

    }

    public class ReportingLineLoggingRepository : IReportingLineLoggingRepository
    {
        private List<ReportingLinelog> _reportingLines;
        private static ReportingLineLoggingRepository _reportingLineLoggingRepository;
        private readonly IFileService _fileService;
        private readonly IConfiguration _configuration;


        public async Task AddAsync(ReportingLinelog reportingLinelog)
        {
            if (_reportingLines == null) await Initialize();
            lock (_reportingLines)
            {
                _reportingLines.Add(reportingLinelog);
            }
            await SaveChanges();
            Console.WriteLine("Reporting line relatio added"); 

        }
        public async Task<ReportingLinelog> GetAsync(string managerName, string subordinateName)
        {
            if (_reportingLines == null) await Initialize();
            return _reportingLines.
                FirstOrDefault(t => t.ManagerName == managerName && t.subordinateName == subordinateName);
        }
        public async Task<IEnumerable<ReportingLinelog>> GetAll()
        {
            if (_reportingLines == null) await Initialize();
            lock (_reportingLines)
            {
                return _reportingLines;
            }

        }

        public async Task<IEnumerable<ReportingLinelog>> Filter<T>(ReportingLineSpecification<T> filter, T criteria)
        {
            if (_reportingLines == null) await Initialize();
            return _reportingLines.Where(t => filter.IsSatisfiedBy(criteria, t)).ToList();
        }
        public async Task Initialize()
        {
            string json = _fileService.GetFromFile(_configuration["ReportingLineLogPath"]).GetAwaiter().GetResult();
            _reportingLines = JsonConvert.DeserializeObject<List<ReportingLinelog>>(json);
        }

        public async Task SaveChanges()
        {
            await _fileService.Save(_reportingLines, _configuration["ReportingLineLogPath"]);
        }

        private ReportingLineLoggingRepository(IFileService fileService, IConfiguration configuration)
        {
            _configuration = configuration;
            _fileService = fileService;
        }

        public static ReportingLineLoggingRepository GetReportingLineLoggingRepository()
        {
            if (_reportingLineLoggingRepository == null)
            {
                var builder = new ConfigurationBuilder();
                builder.SetBasePath("C:\\Users\\Fatima.Karaki\\source\\repos\\ConsoleApp1\\ConsoleApp1")
                       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                IConfiguration config = builder.Build();
                _reportingLineLoggingRepository = new ReportingLineLoggingRepository(new FileService(), config);
            }
            return _reportingLineLoggingRepository;
        }


    }
}
