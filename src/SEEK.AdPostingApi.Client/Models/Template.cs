using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEEK.AdPostingApi.Client.Models
{
    public class Template
    {
        public int? Id { get; set; }

        public TemplateItemModel[] Items { get; set; }
    }
}
