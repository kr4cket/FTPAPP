using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Policy;
using System.Windows.Media.Animation;

namespace FTPAPP
{
    class FTP_Client
    {
        private readonly string _host;
        private readonly string _user;
        private readonly string _password;
        private string _path;
        private string _temp;
        private int _bufferSize;
        private int _timeout;
        private bool _ssl;
        private bool _passive;
        private bool _binary;
        private FtpWebRequest _webRequest;

        public FTP_Client (string host, int timeout, bool ssl, string user = "anonymous", string password = "anonymous")
        {
            _host = host;
            _user = user;
            _password = password;
            _timeout = timeout;
            _ssl = ssl;
            _passive = true;
            _binary = false;
            _path = _host;
        }

        public bool CreateRequest()
        {
            try
            {
                _webRequest = (FtpWebRequest)WebRequest.Create(_host);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool CreateRequest(string path)
        {
            try
            {
                _webRequest = (FtpWebRequest)WebRequest.Create(path);
                _path = path;
                return true;
            }
            catch
            {
                return false;
            }
        }

        private FtpWebRequest MakeRequest(string method)
        {
            _webRequest.Method = method;
            return _webRequest;
        }

        public bool Authorization()
        {
            try
            {
                _webRequest.Credentials = new NetworkCredential(_user, _password);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<DataFile> ListDirectory()
        {
            var list = new List<string>();

            var request = MakeRequest(WebRequestMethods.Ftp.ListDirectoryDetails);

            using (var response = (FtpWebResponse)request.GetResponse())
            {
                using (var stream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream, true))
                    {
                        while (!reader.EndOfStream)
                        {
                            list.Add(reader.ReadLine());
                        }
                    }
                }
            }
            return CreateViewList(list);
        }

        private List<DataFile> CreateViewList(List<string> oldList)
        {
            var newList = new List<DataFile>();
            for (int i = 0; i < oldList.Count; i++)
            {
                var line = oldList[i].Split(' ');
                line = RemoveSpaces(line);

                // File_size File_name File_type
                DataFile dataFile = new DataFile(line[0], line[1], line[2]);
                newList.Add(dataFile);
            }
            return newList;
        }

        private string Combine(string file_name)
        {
            return _path + "/" + file_name;
        }

        private string[] RemoveSpaces(string[] line)
        {
            List<string> newline = new List<string>(); 
            for (int i = 0; i < line.Length; i++)
            {
                if (!(line[i] == ""))
                    newline.Add(line[i]);
            }
            for(int i = 9; i < newline.Count; i++)
            {
                newline[8] += " ";
                newline[8] += newline[i];
            }
            newline.Add(GetFileType(newline[8]));
            return new string[] { newline[4], newline[8], newline[newline.Count-1] };
        }

        private string GetFileType(string file_name)
        {
            var line = file_name.Split('.');
            if (line.Length == 1)
                    return "dir";
            else
                return line[line.Length - 1];
        }

        public List<DataFile> ChangeDirectory(string file_name)
        {
            try
            {
                string directoryPath = _path + '/' + file_name;
                _temp = '/' + file_name;
                if (CreateRequest(directoryPath))
                {
                    return ListDirectory();
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public List<DataFile> BackDirectory()
        {
            try
            {
                string directoryPath = _path.Substring(0,_path.Length - _temp.Length);
                if (CreateRequest(directoryPath))
                {
                    return ListDirectory();
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public bool DownloadFile(string source, string size, string file_name)
        {
            try
            {
                var buffer = new byte[Convert.ToInt32(size)];
                var path = Combine(file_name);
                Authorization();
                if (CreateRequest(path))
                {
                    var request = MakeRequest(WebRequestMethods.Ftp.DownloadFile);
                    using (var resonse = (FtpWebResponse)request.GetResponse())
                    {
                        using (var stream = resonse.GetResponseStream())
                        {
                            using (var filestream = new FileStream(source, FileMode.OpenOrCreate))
                            {
                                int readCount = stream.Read(buffer, 0, Convert.ToInt32(size));

                                while (readCount > 0)
                                {
                                    filestream.Write(buffer, 0, readCount);
                                    readCount = stream.Read(buffer, 0, Convert.ToInt32(size));
                                }
                            }
                        }
                    }
                    return true;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
            
        }


    }
}
