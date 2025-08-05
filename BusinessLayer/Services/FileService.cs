using Microsoft.AspNetCore.Http;

namespace BusinessLayer.Services
{
    public class FileService
    {
        public string UploadFile(IFormFile file, string destinationFolder)
        {
            var uniqueFileName = string.Empty;

            if (file != null && file.Length > 0)
            {


                //    ./wwwroot/Images   / {fileName}
                //var uploadsFolder = Path.Combine(".\\wwwroot\\", "Images");  // Escape Characters
                //var uploadsFolder = Path.Combine(@".\wwwroot\", "Images");   // Disable Escape Characters

                var uploadsFolder = Path.Combine(@"./wwwroot/", destinationFolder);    //   ===>      "./wwwroot/Images"

                // request.Image.FileName    ==>   Ahmed.jpg
                // request.Image.FileName    ==>   Ahmed_fhiuheifuheifheifuhf.jpg

                //Guid.NewGuid()  ==> Random Guid

                uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;  // ahmed.jpg ===>  hfieuhfiuhuie_ahmed.jpg

                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

            }
            return uniqueFileName;  
        }

        public string UploadFile_Api(IFormFile file, string destinationFolder)
        {
            var uniqueFileName = string.Empty;

            if (file != null && file.Length > 0)
            {


                //    ./wwwroot/Images   / {fileName}
                //var uploadsFolder = Path.Combine(".\\wwwroot\\", "Images");  // Escape Characters
                //var uploadsFolder = Path.Combine(@".\wwwroot\", "Images");   // Disable Escape Characters

                // request.Image.FileName    ==>   Ahmed.jpg
                // request.Image.FileName    ==>   Ahmed_fhiuheifuheifheifuhf.jpg

                //Guid.NewGuid()  ==> Random Guid

                uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;  // ahmed.jpg ===>  hfieuhfiuhuie_ahmed.jpg

                var filePath = Path.Combine(destinationFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

            }
            return uniqueFileName;
        }
    }
}
