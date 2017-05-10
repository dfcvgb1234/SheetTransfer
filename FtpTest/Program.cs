using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Net;



namespace FtpTest
{
    class Program
    {
        static string serverURL;
        static string ApplicationName = "Google Sheets API .NET Quickstart";
        static string[] Scopes = { SheetsService.Scope.Spreadsheets };
        static public object[] processList = new object[5000];

        static public object[] gamesList = new object[5000];

        static public object[] checkedState = new object[5000];

        static public int range = 0;
        // ftp://127.0.0.1
        static void Main(string[] args)
        {
            start:
            Console.Write("Insert Server URL (Ex: 127.0.0.1, default): ");
            string tmpURL = Console.ReadLine();
            if (tmpURL.ToLower() == "default")
            {
                serverURL = "ftp://127.0.0.1/";
            }
            else if (tmpURL.ToLower() == "exit")
            {
                Environment.Exit(1);
            }
            else
            {
                serverURL = "ftp://" + tmpURL + "/";
            }
            newStart:
            Console.Write("Action (Ex: status, update files): ");
            string tmpStatus = Console.ReadLine();
            if (tmpStatus.ToLower() == "status")
            {
                foreach (string file in Directory.GetFiles(@"C:\Users\InsertName\Desktop\TEMP FTP"))
                {

                    string[] filename = file.Split('\\');
                    string fileend = filename[filename.Length - 1];
                    bool tf = FtpFileCheck(fileend);
                    Console.WriteLine("is file {0} on FTP server? {1}", fileend, tf);
                    if(!tf)
                    {
                        Console.WriteLine("CODE 4: File does not exists or there is no connection to server");
                        Console.WriteLine(" ");
                    }
                    else
                    {
                        Console.WriteLine(" ");
                    }
                    
                }
                goto newStart;
            }
            else if (tmpStatus.ToLower() == "update")
            {
                if (!UpdateFromFolder(@"C:\Users\InsertName\Desktop\TEMP FTP"))
                {
                    goto start;
                }
                else
                {
                    goto newStart;
                }
            }
            else if (tmpStatus.ToLower() == "sheet")
            {
                if(!UpdateFromFolder(@"C:\Users\InsertName\Desktop\TEMP FTP\SheetData\"))
                {
                    goto start;
                }
                else
                {
                    goto newStart;
                }
            }
            else if (tmpStatus.ToLower() == "files")
            {
                if (!UpdateFromFolder(@"C:\Users\InsertName\Desktop\TEMP FTP"))
                {
                    goto start;
                }
                else
                {
                    goto newStart;
                }
            }
            else if (tmpStatus.ToLower() == "update files")
            {
                if (!UpdateFromFolder(@"C:\Users\InsertName\Desktop\TEMP FTP"))
                {
                    goto start;
                }
                else
                {
                    goto newStart;
                }
            }
            else if (tmpStatus.ToLower() == "ip")
            {
                goto start;
            }
            else if (tmpStatus.ToLower() == "change ip")
            {
                goto start;
            }
            else if (tmpStatus.ToLower() == "ip change")
            {
                goto start;
            }
            else if (tmpURL.ToLower() == "exit")
            {
                Environment.Exit(1);
            }
            else
            {
                Console.WriteLine("Invalid Command");
                goto newStart;
            }
        }

        static public bool FtpFileCheck(string fileName)
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    client.Credentials = new NetworkCredential("Default", "1234");
                    client.OpenRead(serverURL + fileName);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public static bool UpdateFromFolder(string directory)
        {
            foreach (string file in Directory.GetFiles(@"C:\Users\InsertName\Desktop\TEMP FTP"))
            {

                string[] filename = file.Split('\\');
                string fileend = filename[filename.Length - 1];
                switch (FtpFileCheck(fileend))
                {
                    case true:
                        try
                        {

                            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(serverURL + fileend);

                            request.Credentials = new NetworkCredential("Default", "1234");

                            request.Method = WebRequestMethods.Ftp.DeleteFile;

                            FtpWebResponse respone = (FtpWebResponse)request.GetResponse();
                            respone.Close();
                            //Console.WriteLine("File Deleted: " + fileend);
                            Console.WriteLine(" ");
                        }
                        catch
                        {
                            Console.WriteLine("Could not find server on " + serverURL);
                            return false;
                        }
                        break;

                    case false:
                        try
                        {
                            using (WebClient client = new WebClient())
                            {
                                client.Credentials = new NetworkCredential("Default", "1234");
                                client.UploadFile(serverURL + fileend, "STOR", @"C:\Users\InsertName\Desktop\TEMP FTP\" + fileend);
                                //Console.WriteLine("File Created: " + fileend);
                                Console.WriteLine(" ");
                            }
                        }
                        catch
                        {
                            Console.WriteLine("Could not find server on: " + serverURL);
                            return false;
                        }
                        break;
                }
                Console.WriteLine("is file {0} on FTP server? {1}", fileend, FtpFileCheck(fileend));
            }
            return true;
            
        }
    }
}
