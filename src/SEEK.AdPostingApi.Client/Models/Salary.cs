using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEEK.AdPostingApi.Client.Models
{
    public class Salary
    {
        public SalaryType Type { get; set; }

        public int Minimum { get; set; }

        public int Maximum { get; set; }

        public string Details { get; set; }
    }
}
