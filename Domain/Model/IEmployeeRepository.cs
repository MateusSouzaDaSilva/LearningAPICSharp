using LearningWebAPI.Domain.DTOs;

namespace LearningWebAPI.Domain.Model
{
    public interface IEmployeeRepository
    {
        void Add(Employee employee);

        List<EmployeeDTO> Get(int pageNumber, int pageQuantity);

        Employee? GetById(int id);
    }
}
