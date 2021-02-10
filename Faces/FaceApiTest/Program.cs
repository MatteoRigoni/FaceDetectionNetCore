using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace FaceApiTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var imagePath = @"test.jpg";
            var urlAddress = @"http://localhost:6000/api/faces";

            ImageUtility imgUtility = new ImageUtility();
            var bytes = imgUtility.ConvertToByte(imagePath);

            List<byte[]> faceList = null;

            var byteContent = new ByteArrayContent(bytes);
            byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.PostAsync(urlAddress, byteContent))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    faceList = JsonConvert.DeserializeObject<List<byte[]>>(apiResponse);

                    int k = 0;
                    foreach (var face in faceList)
                    {
                        k++;
                        imgUtility.FromBytesToImage(face, "face_" + k + ".jpg");
                    }
                }
            }
        }
    }
}
