namespace LearningWebAPI.Model
{
    public interface IEmployeeRepository
    {
        void Add(Employee employee);

        List<Employee> Get();

        Employee? GetById(int id);
    }
}
