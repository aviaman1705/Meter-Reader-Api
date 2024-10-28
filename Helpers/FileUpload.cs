using Microsoft.AspNetCore.Hosting;

namespace MeterReaderAPI.Helpers
{
    public class FileUpload
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly HttpContext _httpContext;

        public FileUpload(IWebHostEnvironment webHostEnvironment, HttpContext httpContext)
        {
            _webHostEnvironment = webHostEnvironment;
            _httpContext = httpContext;
        }
        public string Save(IFormFile file, string container)
        {
            var saveImg = Path.Combine(_webHostEnvironment.WebRootPath, container, file.FileName);
            string extention = Path.GetExtension(saveImg);
            if (extention == ".jpg" || extention == ".png")
            {
                using (var uploadImg = new FileStream(saveImg, FileMode.Create))
                {
                    file.CopyToAsync(uploadImg);
                }
            }

            string imgPat = $"{_httpContext.Request.Scheme}://{_httpContext.Request.Host.Value}\\{container}\\{file.FileName}";

            return imgPat;
        }
    }
}
