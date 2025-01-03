﻿using LearningWebAPI.Domain.DTOs;
using LearningWebAPI.Domain.Model.EmployeeAggregate;

namespace LearningWebAPI.Infraestrutura.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ConnectionContext _context = new ConnectionContext();
        public void Add(Employee employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();

        }

        public List<EmployeeDTO> Get(int pageNumber, int pageQuantity)
        {
            return _context.Employees.Skip(pageNumber * pageQuantity).Take(pageQuantity).Select( b => new EmployeeDTO()
            {
                Id = b.id,
                NameEmployee = b.name,
                Photo = b.photo
            }).ToList();

        }

        public Employee? GetById(int id)
        {
            return _context.Employees.Find(id);
        }
    }
}
