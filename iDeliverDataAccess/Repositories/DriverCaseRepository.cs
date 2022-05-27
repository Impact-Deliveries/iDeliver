﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using iDeliverDataAccess.Base;
using IDeliverObjects.DTO;
using IDeliverObjects.Enum;
using IDeliverObjects.Objects;
using Microsoft.EntityFrameworkCore;

namespace iDeliverDataAccess.Repositories
{
    internal class DriverCaseRepository : IDriverCaseRepository
    {

        private readonly IDeliverDbContext _context;

        public DriverCaseRepository(IDeliverDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DriverCase>> GetAll() =>
            await _context.DriverCases.ToListAsync();

        public async Task<DriverCase?> GetFirstRow() =>
            await _context.DriverCases.OrderBy(o => o.Id).FirstOrDefaultAsync();

        public async Task<DriverCase?> GetLastRow() =>
            await _context.DriverCases.OrderByDescending(o => o.Id).FirstOrDefaultAsync();

        public async Task<DriverCase?> GetByID(long id) =>
            await _context.DriverCases.Where(w => w.Id == id).FirstOrDefaultAsync();

        public async Task<IEnumerable<DriverCase>> Find(Expression<Func<DriverCase, bool>> where) =>
            await _context.DriverCases.Where(where).ToListAsync();

        public async Task<DriverCase?> FindRow(Expression<Func<DriverCase, bool>> where) =>
            await _context.DriverCases.Where(where).FirstOrDefaultAsync();

        public bool IsExists(Expression<Func<DriverCase, bool>> where) =>
             _context.DriverCases.Any(where);

        public async Task Add(DriverCase DriverCase)
        {
            _context.DriverCases.Add(DriverCase);
            try
            {
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public async Task Update(DriverCase DriverCase)
        {
            _context.Entry(DriverCase).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task Delete(DriverCase DriverCase)
        {
            try
            {
                _context.DriverCases.Remove(DriverCase);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public async Task<List<DriverCaseDTO>> GetOnlineDrivers()
        {
            try
            {
                var data = (from c in _context.DriverCases
                            join d in _context.Drivers on c.DriverId equals d.Id
                            where c.IsOnline == true && c.IsDeleted == false
                            && d.IsActive == true && d.IsDeleted == false && c.Status != 3
                            select new DriverCaseDTO
                            {
                                Latitude = c.Latitude,
                                Longitude = c.Longitude,
                                DriveCaseID = c.Id,
                                Status = c.Status,
                                phone = d.Phone,
                                name = d.FirstName + ' ' + d.LastName,
                            }).ToListAsync();

                return await data;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}