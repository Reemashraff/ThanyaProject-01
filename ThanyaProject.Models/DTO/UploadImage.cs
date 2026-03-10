using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThanyaProject.Models.DTO
{
    public class UploadImage
    {
        public IFormFile FormFile { get; set; }
    }
}
