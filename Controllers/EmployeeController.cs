using LearningWebAPI.Model;
using LearningWebAPI.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningWebAPI.Controllers
{
    // Se utiliza o ControllerBase porque ele não tem suporte à view
    [ApiController]
    [Route("api/v1/employee")]
    public class EmployeeController : ControllerBase
    {

        private readonly IEmployeeRepository _employeeRepository;

        // Precisa criar o construtor para importar o repository
        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        // fromForm para receber atributos como se fosse um formulário
        // AUthorize determina que a rota necessita de autenticação
        [Authorize]
        [HttpPost]
        public IActionResult Add([FromForm]EmployeeViewModel employeeView)
        {
            var filePath = Path.Combine("Storage", employeeView.Photo.FileName);

            // Salvar a imagem dentro da pasta Storage
            using Stream fileStream = new FileStream(filePath, FileMode.Create);
            employeeView.Photo.CopyTo(fileStream);

            var employee = new Employee(employeeView.Name, employeeView.Age, filePath);

            _employeeRepository.Add(employee);

            return Ok();
        }

        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            var employees = _employeeRepository.Get();
            return Ok(employees);
        }

        [Authorize]
        [HttpPost]
        [Route("{id}/download")]
        public IActionResult DownloadPhoto(int id)
        {
            var employee = _employeeRepository.GetById(id);

            var dataBytes = System.IO.File.ReadAllBytes(employee.photo);

            return File(dataBytes, "image/png");
        }

    }
}
