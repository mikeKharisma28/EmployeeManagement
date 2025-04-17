using Azure;
using EmployeeManagement.DTOs;
using EmployeeManagement.DTOs.Employee;
using Newtonsoft.Json;

namespace EmployeeManagement.Services
{
    public class ExternalAPIServices
    {
        public async Task<string> UploadImage(IFormFile fileImg)
        {
            byte[] imageBytes = null;
            if (fileImg.ContentType.ToLower().StartsWith("image/"))
            // Check whether the selected file is image
            {
                using (BinaryReader br = new BinaryReader(fileImg.OpenReadStream()))
                {
                    imageBytes = br.ReadBytes((int)fileImg.OpenReadStream().Length);
                    // Convert the image in to bytes
                }
            }

            var requestData = new Dictionary<String, String>()
            {
                { "key", "331008efb2d4d42184c1420dd0716c04" },
                { "image", Convert.ToBase64String(imageBytes) }
            };

            var client = new HttpClient();
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://api.imgbb.com/1/upload"),
                Content = new FormUrlEncodedContent(requestData)
            };

            var response = await client.SendAsync(request);
            var str = await response.Content.ReadAsStringAsync();
            ImageUploadResponseDto responseObj = JsonConvert.DeserializeObject<ImageUploadResponseDto>(str);

            return responseObj.Data.Url;
        }
    }
}
