using MeterReaderAPI.DTO;
using MeterReaderAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Runtime.Intrinsics.X86;
using System.Text.RegularExpressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MeterReaderAPI.Services
{
    public class TrackRepository : ITrackRepository
    {
        private readonly ApplicationDbContext _context;

        public TrackRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Track item)
        {
            _context.Tracks.Add(item);
            _context.SaveChanges();
        }

        public bool Delete(int id)
        {
            bool isDeleted = false;
            Track? entity = _context.Tracks.FirstOrDefault(p => p.Id == id);

            if (entity != null)
            {
                _context.Tracks.Remove(entity);
                _context.SaveChanges();
                isDeleted = true;
            }
            return isDeleted;
        }

        public Track Get(int id)
        {
            Track? track = _context.Tracks.FirstOrDefault(x => x.Id == id);
            return track;
        }

        public IQueryable<Track> GetAll()
        {
            return _context.Tracks.Include(x => x.Notebook);
        }

        public Dashboard GetDashboardData()
        {
            Dashboard dashboard = new Dashboard();
            dashboard.Called = _context.Tracks.Sum(x => x.Called);
            dashboard.UnCalled = _context.Tracks.Sum(x => x.UnCalled);

            double monthlyUnCalleds = _context.Tracks.Sum(x => x.UnCalled);
            double monthlyCalleds = _context.Tracks.Sum(x => x.Called);
            dashboard.UnCalledPercentage = Math.Round(((double)monthlyUnCalleds / (double)monthlyCalleds) * 100, 2);

            dashboard.LowestUnCalledTrack = _context.Tracks.OrderBy(x => x.UnCalled).First();
            dashboard.HighestUnCalledTrack = _context.Tracks.OrderByDescending(x => x.UnCalled).First();

            dashboard.PopularNotebook = (from track in _context.Tracks
                                         join notebook in _context.Notebooks on track.NotebookId equals notebook.Id
                                         group track by new { notebook.Number, track.Desc, track.NotebookId } into g
                                         select new PopularNotebook() { Id = g.Key.NotebookId, Number = g.Key.Number, Desc = g.Key.Desc, NumberOfRaces = g.Count() }).OrderByDescending(x => x.NumberOfRaces).First();


            dashboard.CalledsPerMonths = (from track in _context.Tracks
                                          group track by new { track.FromDate.Year, track.FromDate.Month } into g
                                          orderby g.Key.Year, g.Key.Month
                                          select new MonthlyData() { Date = $"{g.Key.Month.ToString().PadLeft(2, '0')}/{g.Key.Year}", Count = g.Sum(x => x.Called) }
                                          ).ToList();

            dashboard.UnCalledsPerMonths = (from track in _context.Tracks
                                            group track by new { track.FromDate.Year, track.FromDate.Month } into g
                                            orderby g.Key.Year, g.Key.Month
                                            select new MonthlyData() { Date = $"{g.Key.Month.ToString().PadLeft(2, '0')}/{g.Key.Year}", Count = g.Sum(x => x.UnCalled) }
                                           ).ToList();

            return dashboard;
        }

        public bool Update(Track entity)
        {
            var track = _context.Tracks.FirstOrDefault(x => x.Id == entity.Id);

            if (track != null)
            {
                track.Called = entity.Called;
                track.UnCalled = entity.UnCalled;
                track.Desc = entity.Desc;
                track.FromDate = entity.FromDate;
                track.ToDate = entity.ToDate;

                _context.SaveChanges();
                return true;
            }

            return false;
        }
    }
}





