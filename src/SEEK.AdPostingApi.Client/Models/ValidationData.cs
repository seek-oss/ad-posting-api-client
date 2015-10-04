namespace SEEK.AdPostingApi.Client.Models
{
    public class ValidationData
    {
        public ValidationSeverity Severity { get; set; } = ValidationSeverity.Error;

        public string Code { get; set; }

        public string Message { get; set; }
    }
}
