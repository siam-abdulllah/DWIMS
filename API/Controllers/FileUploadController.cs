using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class FileUploadController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        //public void FTPUpload(string fileNameStr, EventArgs e)
        //{
        //    //FTP Server URL.
        //    string ftp = "ftp://yourserver.com/";

        //    //FTP Folder name. Leave blank if you want to upload to root folder.
        //    string ftpFolder = "Uploads/";

        //    byte[] fileBytes = null;

        //    //Read the FileName and convert it to Byte array.
        //    string fileName = Path.GetFileName(fileNameStr);
        //    using (StreamReader fileStream = new StreamReader(FileUpload1.PostedFile.InputStream))
        //    {
        //        fileBytes = Encoding.UTF8.GetBytes(fileStream.ReadToEnd());
        //        fileStream.Close();
        //    }

        //    try
        //    {
        //        //Create FTP Request.
        //        FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftp + ftpFolder + fileName);
        //        request.Method = WebRequestMethods.Ftp.UploadFile;

        //        //Enter FTP Server credentials.
        //        request.Credentials = new NetworkCredential("UserName", "Password");
        //        request.ContentLength = fileBytes.Length;
        //        request.UsePassive = true;
        //        request.UseBinary = true;
        //        request.ServicePoint.ConnectionLimit = fileBytes.Length;
        //        request.EnableSsl = false;

        //        using (Stream requestStream = request.GetRequestStream())
        //        {
        //            requestStream.Write(fileBytes, 0, fileBytes.Length);
        //            requestStream.Close();
        //        }

        //        FtpWebResponse response = (FtpWebResponse)request.GetResponse();
        //        response.Close();
        //    }
        //    catch (WebException ex)
        //    {
        //        throw new Exception((ex.Response as FtpWebResponse).StatusDescription);
        //    }
        //}
    }
}
