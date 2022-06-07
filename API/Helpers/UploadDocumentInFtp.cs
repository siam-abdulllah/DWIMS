using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace API.Helpers
{
    public static class UploadDocumentInFtp
    {
        public static bool UploadFile(IFormFile formFile, string fileName)
        {
            try
            {
                FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create("ftp://172.16.201.161:21/" + fileName);
                //FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create("ftp://172.16.242.31:21/"+fileName);
                //ftp.Credentials = new NetworkCredential("Administrator", "Asdf321");
                ftp.Proxy = null;
                ftp.KeepAlive = true;
                ftp.UseBinary = true;
                ftp.Method = WebRequestMethods.Ftp.UploadFile;
                Stream ftpstream = ftp.GetRequestStream();

                byte[] data;
                using (Stream inputStream = formFile.OpenReadStream())
                {
                    MemoryStream memoryStream = inputStream as MemoryStream;
                    if (memoryStream == null)
                    {
                        memoryStream = new MemoryStream();
                        inputStream.CopyTo(memoryStream);
                    }

                    data = memoryStream.ToArray();
                }

                ftpstream.WriteAsync(data, 0, data.Length);
                ftpstream.Close();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static void MakeFTPDir(string ftpAddress, string port, string pathToCreate, string login, string password)
        {
            FtpWebRequest reqFTP = null;
            Stream ftpStream = null;

            string[] subDirs = pathToCreate.Split('/');

            string currentDir = string.Format("ftp://{0}:" + port, ftpAddress);

            foreach (string subDir in subDirs)
            {
                try
                {
                    currentDir = currentDir + "/" + subDir;


                    reqFTP = (FtpWebRequest)FtpWebRequest.Create(currentDir);
                    reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
                    reqFTP.UseBinary = true;
                    reqFTP.Credentials = new NetworkCredential(login, password);
                    reqFTP.Proxy = new WebProxy();
                    FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                    ftpStream = response.GetResponseStream();
                    ftpStream.Close();
                    response.Close();
                }
                catch (Exception ex)
                {
                    continue;
                    //directory already exist I know that is weak but there is no way to check if a folder exist on ftp...
                }
            }
        }

        public static bool DeleteFileFromFTP(string fileName)
        {
            FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create("ftp://172.16.201.161:21/" + fileName);
            //request.Credentials = new NetworkCredential("silsoft", "adm!N@123");

            //request.Credentials = new NetworkCredential(userName, password);

            request.Method = WebRequestMethods.Ftp.DeleteFile;
            request.Proxy = null;
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            if (response.StatusCode.ToString() == "FileActionOK")
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public static byte[] Download(string fileName)
        {
           WebClient request = new WebClient();
            string url = "ftp://172.16.201.161:21/Square Pharmaceuticals Limited/PMD/2022/Jun/" +"nid.jpg";
            //request.Credentials = new NetworkCredential("anonymous", "anonymous@example.com");
           
            try
            {
                byte[] newFileData = request.DownloadData(url);
                string fileString = System.Text.Encoding.UTF8.GetString(newFileData);
                return newFileData;
            }
            catch (WebException e)
            {
                // Do something such as log error, but this is based on OP's original code
                // so for now we do nothing.
                return new byte[0];
            }
        
        }
  


    }
    
}