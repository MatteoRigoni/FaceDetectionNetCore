using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace FaceApiTest
{
    class ImageUtility
    {
        public byte[] ConvertToByte(string imagePath)
        {
            MemoryStream memoryStream = new MemoryStream();
            using (FileStream stream = new FileStream(imagePath, FileMode.Open))
            {
                stream.CopyTo(memoryStream);
            }
            var bytes = memoryStream.ToArray();
            return bytes;
        }

        public void FromBytesToImage(byte[] imageBytes, string fielname)
        {
            using (var ms = new MemoryStream(imageBytes))
            {
                Image img = Image.FromStream(ms);
                img.Save(fielname + ".jpg", ImageFormat.Jpeg);
            }
        }
    }
}
