using SharpBits.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanBackup
{
    class BitsTranferFile
    {
        LogWriter log = new LogWriter();
        static int i = 0;
        static bool result = true;
        static string confirm;
        private void notificationHandler_OnJobTransferredEvent(object sender, NotificationEventArgs e)
        {
           
            Console.WriteLine(" Progress BytesTransferred " + e.Job.Progress.BytesTransferred.ToString());
            log.LogWrite(" Progress BytesTransferred : " + e.Job.Progress.BytesTransferred);
           
            Console.WriteLine(" Progress  FilesTotal  " + e.Job.Progress.FilesTotal);
            Console.WriteLine(" Progress FilesTransferred " + e.Job.Progress.FilesTransferred);

            Console.WriteLine(" Progress  BytesTotal" + e.Job.Progress.BytesTotal);
            log.LogWrite(" Progress  BytesTotal : " + e.Job.Progress.BytesTotal);

            
            Console.WriteLine(" Job.State  " + e.Job.State);
            log.LogWrite(" Job.State  " + e.Job.State);

            Console.WriteLine(" CreationTime " + e.Job.JobTimes.CreationTime);
            log.LogWrite(" CreationTime " + e.Job.JobTimes.CreationTime);
            Console.WriteLine(" JobId " + e.Job.JobId);
            log.LogWrite(" JobId " + e.Job.JobId);
            if (e.Job.State == JobState.Transferred)
            {
                confirm = "OK";
                System.Threading.Thread.Sleep(100);
                e.Job.Complete();
                Console.WriteLine(" Job.Complete " );
                log.LogWrite(" Job.Complete >>>  " + e.Job.JobId);
                Console.WriteLine(" TransferCompletionTime " + e.Job.JobTimes.TransferCompletionTime);
                log.LogWrite(" TransferCompletionTime " + e.Job.JobTimes.TransferCompletionTime);
                result = false;
            }
        }
      
        private void notificationHandler_OnInterfaceError(object sender, BitsInterfaceNotificationEventArgs e)
        {
            confirm = "Error";
            result = false;
        }
        private void notificationHandler_OnJobRemoved(object sender, NotificationEventArgs e)
        {
            confirm = "Error";
            result = false;
        }

        public string bits(string inFilePath, string outFilePath )
        {
            result = true;
            BitsManager manager = new BitsManager();
            manager.EnumJobs(JobOwner.CurrentUser);
          
            BitsJob newJob = manager.CreateJob("TestJob", JobType.Download);
            
            
            if (File.Exists(inFilePath))
            {
                if (Directory.Exists(Path.GetDirectoryName(outFilePath)))
                {
                    if (File.Exists(outFilePath))
                    {
                        Console.WriteLine("outPathFile Exists  error");
                        log.LogWrite("outPathFile Exists  error : " + outFilePath);
                    }
                    else
                    {
                        i++;
                        Console.WriteLine("Job > " + i);
                        newJob.AddFile(inFilePath, outFilePath);
                        newJob.Resume();
                       
                        manager.OnJobTransferred += new EventHandler<NotificationEventArgs>(notificationHandler_OnJobTransferredEvent);

                        manager.OnInterfaceError += new EventHandler<BitsInterfaceNotificationEventArgs>(notificationHandler_OnInterfaceError);

                        
                        while(result)
                        {
                        }

                        Console.WriteLine("Job > " + i + "  OK");
                        Console.WriteLine(" confirm " + "Job > " + i + " : " +confirm);
                        return confirm;
                    }

                }
                else
                {
                    Console.WriteLine("Dirictory outPath  error");
                    log.LogWrite("Dirictory outPath  error : " + outFilePath);
                    confirm = "Error";
                    return confirm;
                }
                

            }
            else
            {
                
                log.LogWrite("Dirictory outPath  error : " + inFilePath);
                confirm = "Error";
                return confirm;
               
            }
            confirm = "Error";
            return confirm;
            
        }
    }
}
