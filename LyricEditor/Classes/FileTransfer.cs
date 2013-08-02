using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace LyricEditor
{
    public class FileTransfer
    {
        public class FtpState
        {
            private ManualResetEvent wait;
            private FtpWebRequest request;
            private string fileName;
            private Exception operationException = null;
            string status;

            public FtpState()
            {
                wait = new ManualResetEvent(false);
            }

            public ManualResetEvent OperationComplete
            {
                get { return wait; }
            }

            public FtpWebRequest Request
            {
                get { return request; }
                set { request = value; }
            }

            public string FileName
            {
                get { return fileName; }
                set { fileName = value; }
            }
            public Exception OperationException
            {
                get { return operationException; }
                set { operationException = value; }
            }
            public string StatusDescription
            {
                get { return status; }
                set { status = value; }
            }
        }

        public const string MINUS_ONE = "-1";

        public const string EXT_TXT = ".txt";
        public const string EXT_MP3 = ".mp3";
        public const string EXT_JPG = ".jpg";

        public static void UploadFile (string fileName)//, string ftpServer, string ftpDirectory, string ftpUsername, string ftpPassword, string songId)
        {
            // Get the object used to communicate with the server.
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://172.16.1.254/assets/songs/" + fileName);
            request.Method = WebRequestMethods.Ftp.UploadFile;

            // This example assumes the FTP site uses anonymous logon.
            request.Credentials = new NetworkCredential("anonymous", "puthutp@agatestudio.com");

            // Copy the contents of the file to the request stream.
            StreamReader sourceStream = new StreamReader(fileName);
            byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
            sourceStream.Close();
            request.ContentLength = fileContents.Length;

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(fileContents, 0, fileContents.Length);
            requestStream.Close();

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            Console.WriteLine("Upload File Complete, status {0}", response.StatusDescription);

            response.Close();
        }

        public static void UploadFile(string fileName, string ftpServer, string ftpDirectory, string ftpUsername, string ftpPassword, string songId, string fileExtension)
        {
            // Get the object used to communicate with the server.
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + ftpServer + ftpDirectory + songId + fileExtension);
            request.Method = WebRequestMethods.Ftp.UploadFile;

            // This example assumes the FTP site uses anonymous logon.
            request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

            byte[] fileContents = File.ReadAllBytes(fileName);

            request.ContentLength = fileContents.Length;

            //try
            //{
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(fileContents, 0, fileContents.Length);
                requestStream.Close();

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                Console.WriteLine("Upload File Complete, status {0}", response.StatusDescription);

                response.Close();
            //}
            //catch (WebException e)
            //{
            //    MessageBox.Show(e.Message, "Upload File Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}

/********************************************/

            //ManualResetEvent waitObject;

            //FtpState state = new FtpState();
            //FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + ftpServer + ftpDirectory + songId + fileExtension);
            //request.Method = WebRequestMethods.Ftp.UploadFile;

            //// This example uses anonymous logon. 
            //// The request is anonymous by default; the credential does not have to be specified.  
            //// The example specifies the credential only to 
            //// control how actions are logged on the server.

            //request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

            //// Store the request in the object that we pass into the 
            //// asynchronous operations.
            //state.Request = request;
            //state.FileName = fileName;

            //// Get the event to wait on.
            //waitObject = state.OperationComplete;

            //// Asynchronously get the stream for the file contents.
            //request.BeginGetRequestStream(
            //    new AsyncCallback(EndGetStreamCallback),
            //    state
            //);

            //// Block the current thread until all operations are complete.
            ////waitObject.WaitOne();

            //// The operations either completed or threw an exception. 
            //if (state.OperationException != null)
            //{
            //    throw state.OperationException;
            //}
            //else
            //{
            //    Console.WriteLine("The operation completed - {0}", state.StatusDescription);
            //}
        }

        //private static void EndGetStreamCallback(IAsyncResult ar)
        //{
        //    FtpState state = (FtpState) ar.AsyncState;

        //    Stream requestStream = null;
        //    // End the asynchronous call to get the request stream. 
        //    try
        //    {
        //        requestStream = state.Request.EndGetRequestStream(ar);
        //        // Copy the file contents to the request stream. 
        //        const int bufferLength = 2048;
        //        byte[] buffer = new byte[bufferLength];
        //        int count = 0;
        //        int readBytes = 0;
        //        FileStream stream = File.OpenRead(state.FileName);
        //        do
        //        {
        //            readBytes = stream.Read(buffer, 0, bufferLength);
        //            requestStream.Write(buffer, 0, readBytes);
        //            count += readBytes;
        //        }
        //        while (readBytes != 0);
        //        Console.WriteLine ("Writing {0} bytes to the stream.", count);
        //        // IMPORTANT: Close the request stream before sending the request.
        //        stream.Close();
        //        requestStream.Close();
        //        // Asynchronously get the response to the upload request.
        //        state.Request.BeginGetResponse(
        //            new AsyncCallback (EndGetResponseCallback), 
        //            state
        //        );
        //    } 
        //    // Return exceptions to the main application thread. 
        //    catch (Exception e)
        //    {
        //        Console.WriteLine("Could not get the request stream.");
        //        state.OperationException = e;
        //        state.OperationComplete.Set();
        //        return;
        //    }

        //}

        //// The EndGetResponseCallback method   
        //// completes a call to BeginGetResponse. 
        //private static void EndGetResponseCallback(IAsyncResult ar)
        //{
        //    FtpState state = (FtpState) ar.AsyncState;
        //    FtpWebResponse response = null;
        //    try 
        //    {
        //        response = (FtpWebResponse) state.Request.EndGetResponse(ar);
        //        response.Close();
        //        state.StatusDescription = response.StatusDescription;
        //        // Signal the main application thread that  
        //        // the operation is complete.
        //        state.OperationComplete.Set();
        //        //System.Windows.Forms.MessageBox.Show(state.FileName);
        //        Console.WriteLine("File " + state.FileName + " sent.");
        //    }
        //    // Return exceptions to the main application thread. 
        //    catch (Exception e)
        //    {
        //        Console.WriteLine ("Error getting response.");
        //        state.OperationException = e;
        //        state.OperationComplete.Set();
        //    }
        //}
    }
}
