using API.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class FileUploadController :  BaseApiController
    {

        [HttpPost("uploadBudgetYearlyFile/{file}")]
        public void UploadBudgetYearlyFile(IFormFile file)
        {
            
        }
        [Route("SaveFile")]
        [HttpPost, DisableRequestSizeLimit]
        public void SaveFile()
        {
            try
            {
                 
                IFormFile file = Request.Form.Files[0];
                 string monthname = DateTime.Now.ToString("MMM")+"/";
                UploadDocumentInFtp.MakeFTPDir("172.16.201.161", "21", file.Name+monthname , null, null);
               
                var filerName = file.Name+monthname+file.FileName;
                UploadDocumentInFtp.UploadFile(file, filerName);             
                UploadDocumentInFtp.Download(filerName);             
                       
            }
            catch (Exception ex)
            {
              
            }
        }

        // public  void DownloadFile(object sender, EventArgs e)
        // {
        //     try
        //     {
        //         string url = "ftp://172.16.201.161:21/Square Pharmaceuticals Limited/PMD/2022/Jun/" + "nid.jpg";
        //         /* Create an FTP Request */
        //         var ftpRequest = (FtpWebRequest)FtpWebRequest.Create(url);
        //         /* Log in to the FTP Server with the User Name and Password Provided */
        //         // ftpRequest.Credentials = new NetworkCredential(user, pass);
        //         /* When in doubt, use these options */
        //         ftpRequest.UseBinary = true;
        //         ftpRequest.UsePassive = true;
        //         ftpRequest.KeepAlive = true;
        //         /* Specify the Type of FTP Request */
        //         ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
        //         /* Establish Return Communication with the FTP Server */
        //         var ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
        //         /* Get the FTP Server's Response Stream */
        //         var ftpStream = ftpResponse.GetResponseStream();

        //         // TODO: you might need to extract these settings from the FTP response
        //         const string contentType = "application/zip";
        //         const string fileNameDisplayedToUser = "FileName.zip";

        //         return File(ftpStream, contentType, fileNameDisplayedToUser);
        //     }
        //     catch (Exception ex)
        //     {
        //         // _logger.Error(ex);
        //     }
        // }



    }
}
