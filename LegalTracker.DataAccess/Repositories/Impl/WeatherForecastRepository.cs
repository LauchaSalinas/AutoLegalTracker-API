using LegalTracker.DataAccess.Persistence;
using LegalTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegalTracker.DataAccess.Repositories.Impl
{
    public class WeatherForecastRepository : BaseRepository<WeatherForecast>
    {
        public WeatherForecastRepository(ScrapContext context) : base(context) { }
    }
}
