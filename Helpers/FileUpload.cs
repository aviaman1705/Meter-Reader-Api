using Microsoft.AspNetCore.Hosting;

namespace MeterReaderAPI.Helpers
{
    public class FileUpload
    {
        private  readonly IWebHostEnvironment _webHostEnvironment;

        public FileUpload(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public string Save(IFormFile file,string container)
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

            return saveImg;
        }
    }
}
