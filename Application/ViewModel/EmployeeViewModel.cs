namespace LearningWebAPI.Application.ViewModel
{
    public class EmployeeViewModel
    {
        public string Name { get; set; }
        public int Age { get; set; }
        // propriedade IFormFile serve para acessar todas as propriedades do arquivo e o próprio arquivo
        public IFormFile Photo { get; set; }
    }
}
