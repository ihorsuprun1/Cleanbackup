using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanBackup
{
    class FileService
    {
        LogWriter log = new LogWriter();
        public string FileCopy(string inFilePath, string outFilePath)
        {
            string confirm;
            FileInfo fileInf = new FileInfo(inFilePath);
            if (fileInf.Exists)
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
                        fileInf.MoveTo(outFilePath);
                        confirm = "OK";
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
