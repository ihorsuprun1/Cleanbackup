using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CleanBackup
{
    class Program
    {
       private static Settings settings { get; set;}

        static  void  Main(string[] args)
        {
            LogWriter log = new LogWriter();
            JsonService json = new JsonService();
            settings = json.ReadJsonConf();
            //Telegram telegram = new Telegram(settings.TelegramToken, settings.TelegramChtid);
            Telegram telegram = new Telegram(settings);
            System.Diagnostics.Stopwatch myStopwatch = new System.Diagnostics.Stopwatch();
            myStopwatch.Start();
            try
            {
               var dirBackup = new DirectoryInfo(settings.PathSearchDirectory);
                if (!dirBackup.Exists)
                {
                    dirBackup.Create();
                }

                List<string> fileList = Directory.GetFiles(settings.PathSearchDirectory).ToList();

                Console.WriteLine(fileList.Count());
                if (fileList == null || fileList.Count <= 0)
                {
                    Console.WriteLine("fileList == null && fileList.Count <= 0");
                    telegram.SendMessageAsync($"Company : {settings.CompanyName} \n  Date: {DateTime.Now}  Backups no found ").Wait();
                    
                }
                else
                {
                    Console.WriteLine("!!fileList == null && fileList.Count <= 0");
                    foreach (var f in fileList)
                    {
                        string inFilePath = @"" + f;
                        string outFilePath = settings.OutPutDirectory + Path.GetFileName(f);
                        Console.WriteLine(inFilePath);
                        Console.WriteLine(outFilePath);
                        switch (settings.TypeCopy)
                        {
                            case "File":
                                FileService fileService = new FileService();
                                if (fileService.FileCopy(inFilePath, outFilePath).Contains("OK"))
                                {
                                    try
                                    {
                                        File.Delete(inFilePath);
                                        Console.WriteLine(" delete >> " + inFilePath);
                                        log.LogWrite(" delete >> " + inFilePath);
                                    }
                                    catch (Exception ex)
                                    {
                                        log.LogWrite(ex.Message);
                                        telegram.SendMessageAsync($"CompanyName: {settings.CompanyName} \n  Error: {ex.Message}").Wait();
                                       
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Перемещения файла (fileService) по какой-то причине  не выполнен , подробности смотрите в log файле ");
                                    log.LogWrite("Перемещения файла (fileService) по какой-то причине  не выполнен , подробности смотрите в log файле ");
                                }
                                break;

                            case "Bits":
                                BitsTranferFile bits = new BitsTranferFile();
                                if (bits.bits(inFilePath, outFilePath).Contains("OK"))
                                {
                                    try
                                    {
                                        File.Delete(inFilePath);
                                        Console.WriteLine(" delete >> " + inFilePath);
                                        log.LogWrite(" delete >> " + inFilePath);
                                    }
                                    catch (Exception ex)
                                    {
                                        log.LogWrite(ex.Message);
                                        telegram.SendMessageAsync($"CompanyName: {settings.CompanyName} \n  Error: {ex.Message}").Wait();
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Перемещения файла (Bits) по какой-то причине  не выполнен , подробности смотрите в log файле ");
                                    log.LogWrite("Перемещения файла (Bits) по какой-то причине  не выполнен , подробности смотрите в log файле ");
                                }

                                break;
                            default:
                                break;
                        }
                    }
                }

                if (settings.CleanBackupIsNeed)
                {
                    CleanBackup clean = new CleanBackup(settings.PathDAY, settings.PathWEEK, settings.PathMOUNT, settings.PathYears);
                }
            }
            catch (Exception ex)
            {
                    telegram.SendMessageAsync($"CompanyName: {settings.CompanyName} \n  Error: {ex.Message}").Wait();
             
                Environment.Exit(0);
            }

            myStopwatch.Stop();
            TimeSpan ts = myStopwatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);
            log.LogWrite("App RunTime: " + elapsedTime);
            Environment.Exit(0);
        }
    }
}
