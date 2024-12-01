using AutoMapper;
using LearningWebAPI.Application.ViewModel;
using LearningWebAPI.Domain.DTOs;
using LearningWebAPI.Domain.Model.EmployeeAggregate;
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
        private readonly ILogger<EmployeeController> _logger;
        private readonly IMapper _mapper;

        public EmployeeController(IEmployeeRepository employeeRepository, ILogger<EmployeeController> logger, IMapper mapper)
        {
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        // Precisa criar o construtor para importar o repository


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

        //[Authorize]
        [HttpGet]
        public IActionResult Get(int pageNumber, int pageQuantity)
        {
            _logger.Log(LogLevel.Error, "Teve um erro");


            var employees = _employeeRepository.Get(pageNumber, pageQuantity);
            
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

        [HttpGet]
        [Route("{id}")]
        public IActionResult Search(int id)
        {

            var employees = _employeeRepository.GetById(id);

            var employeesDTOS = _mapper.Map<EmployeeDTO>(employees);
            return Ok(employeesDTOS);
        }

    }
}
