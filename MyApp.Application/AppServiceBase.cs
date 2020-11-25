using MyApp.Application.Interface;
using MyApp.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;

namespace MyApp.Application
{
    public class AppServiceBase<TEntity> : IDisposable, IAppServiceBase<TEntity> where TEntity : class
    {
        private readonly IServiceBase<TEntity> _webScrapingResultServiceBase;


        public AppServiceBase(IServiceBase<TEntity> webScrapingResultServiceBase)
        {
            _webScrapingResultServiceBase = webScrapingResultServiceBase;
        }

        public void Add(TEntity obj)
        {
            _webScrapingResultServiceBase.Add(obj);
        }

        public void Dispose()
        {
            _webScrapingResultServiceBase.Dispose();
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _webScrapingResultServiceBase.GetAll();
        }

        public TEntity GetById(int Id)
        {
            return _webScrapingResultServiceBase.GetById(Id);
        }

        public void Remove(TEntity obj)
        {
            _webScrapingResultServiceBase.Remove(obj);
        }

        public void Update(TEntity obj)
        {
            _webScrapingResultServiceBase.Update(obj);
        }
    }
}
