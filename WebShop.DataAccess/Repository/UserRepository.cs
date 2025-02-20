﻿using System.Collections.Generic;
using System.Threading.Tasks;
using WebShop.Models.Models;
using WebShop.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using WebShop.DataAccess.Data;

namespace WebShop.DataAccess.Repository
{
    public class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<List<ApplicationUser>> GetAllAsync()
        {
            return _context.ApplicationUsers.ToListAsync();
        }

        public void Update(ApplicationUser user)
        {
            var existingEntity = _context.Users.Local.FirstOrDefault(x => x.Id == user.Id);
            if (existingEntity == null)
            {
                _context.Users.Attach(user);
            }
            else
            {
                _context.Entry(existingEntity).CurrentValues.SetValues(user);
            }
        }
    }
}
