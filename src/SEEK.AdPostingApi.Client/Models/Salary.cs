namespace SEEK.AdPostingApi.Client.Models
{
    public class Salary
    {
        public SalaryType Type { get; set; }

        public decimal Minimum { get; set; }

        public decimal Maximum { get; set; }

        public string Details { get; set; }
    }
}