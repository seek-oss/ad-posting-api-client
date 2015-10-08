using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEEK.AdPostingApi.Client.Models
{
    public class Video
    {
        public string Url { get; set; }

        public VideoPosition? Position { get; set; }
    }

}
