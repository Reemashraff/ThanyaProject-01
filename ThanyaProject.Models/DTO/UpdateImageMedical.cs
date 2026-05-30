using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThanyaProject.Models.DTO
{
    public class UpdateImageMedical
    {
        public List<IFormFile>? NewImages { get; set; }
        public List<int>? DeleteImageIds { get; set; }
    }
}
