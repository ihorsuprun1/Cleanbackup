using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CleanBackup
{
    public class CleanBackup
    {
        DateTime starttime = DateTime.Now;
        LogWriter log = new LogWriter();

        public CleanBackup(string pathDAY, string pathWEEK, string pathMOUNT, string pathYears)
        {
            clean(pathDAY, pathWEEK, pathMOUNT, pathYears);
        }

        private void clean(string pathDAY, string pathWEEK, string pathMOUNT, string pathYears)
        {
            var dirDay = new DirectoryInfo(pathDAY);
            if (!dirDay.Exists)
            {
                dirDay.Create();
            }
            log.LogWrite("#######################################################");
            Console.WriteLine("Поиск файлов в каталоге : " + pathDAY);
            log.LogWrite("Поиск файлов в каталоге : " + pathDAY);
            Console.WriteLine("Подождите....");
            foreach (FileInfo file in dirDay.GetFiles())
            {
                Console.WriteLine("Имя файла: {0}", file.Name);
                int Day = 16;
                Console.WriteLine("Поиск monthly backup....");
                log.LogWrite("Поиск monthly backup....");
                if (file.CreationTime.Day == Day)
                {

                    if (file.Exists)
                    {
                        Console.WriteLine("monthly backup найден");
                        log.LogWrite("monthly backup найден");
                        try
                        {
                            Console.WriteLine("Перемещаю его в " + pathMOUNT + " ...");
                            log.LogWrite("Перемещаю его в " + pathMOUNT + " ...");
                            file.CopyTo(pathMOUNT + "\\" + file.Name, true);
                            log.LogWrite("Перемещение завершено!");
                            Console.WriteLine("Перемещение завершено!");
                        }
                        catch (Exception ex)
                        {
                            log.LogWrite("Файл с таким именем сущиствует Error " + ex.Message.ToString());
                            Console.WriteLine("Файл с таким именем сущиствует Error " + " ...");

                            Console.WriteLine(ex.Message.ToString());
                        }
                    }
                }

                else
                {
                    Console.WriteLine("Monthly backups не найдено ...");
                    log.LogWrite("Monthly backups не найдено ...");
                }

                Console.WriteLine("Поиск weekly backup...");
                log.LogWrite("Поиск weekly backup...");
                if (file.CreationTime.DayOfWeek == DayOfWeek.Thursday)
                {
                    if (file.Exists)
                    {
                        Console.WriteLine("Weekly backup найден");
                        log.LogWrite("Weekly backup найден");
                        try
                        {
                            Console.WriteLine("Перемещаю его в " + pathWEEK + " ...");
                            log.LogWrite("Перемещаю его в " + pathWEEK + " ...");
                            file.CopyTo(pathWEEK + "\\" + file.Name);
                            Console.WriteLine("Перемещение завершено!");
                            log.LogWrite("Перемещение завершено!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Файл с таким именем сущиствует Error " + " ...");
                            log.LogWrite("Файл с таким именем сущиствует Error " + " ..." + ex.Message.ToString());
                            Console.WriteLine(ex.Message.ToString());
                        }
                    }
                }
                 else
                {
                    Console.WriteLine("Weekly backups не найдено ...");
                    log.LogWrite("Weekly backups не найдено ...");
                }

            }

            Console.WriteLine("Начинаем удаление backups в каталоге: " + pathDAY);
            log.LogWrite(" Начинаем удаление backups в каталоге: " + pathDAY);
           
            foreach (FileInfo file in dirDay.GetFiles())
            {

                if (DateTime.UtcNow - file.LastWriteTimeUtc > TimeSpan.FromDays(8))
                {
                    Console.WriteLine("Найденый файл старше 8 дней: {0}", file.Name);
                    log.LogWrite("Найденый файл старше 8 дней: {0}" + file.Name);
                    File.Delete(file.FullName);
                    Console.WriteLine("Удаление файлов завершено! ");
                    log.LogWrite("Удаление файлов завершено! ");
                }
                else
                {
                    Console.WriteLine("Файлов старше 8 дней не обнаружено завершено! ");
                    log.LogWrite("Файлов старше 8 дней не обнаружено завершено! ");
                }
                //}
            }

            Console.WriteLine("Начинаем удаление backups в каталоге: " + pathWEEK);
            log.LogWrite("Начинаем удаление backups в каталоге: " + pathWEEK);
            

            var dirWeek = new DirectoryInfo(pathWEEK);
            if (!dirWeek.Exists)
            {
                dirWeek.Create();
            }

            foreach (FileInfo file in dirWeek.GetFiles())
            {
                if (DateTime.UtcNow - file.LastWriteTimeUtc > TimeSpan.FromDays(30))
                {
                    Console.WriteLine("Найденый файл старше 30 дней: {0}", file.Name);
                    log.LogWrite("Найденый файл старше 30 дней: " + file.Name);
                    File.Delete(file.FullName);
                    Console.WriteLine("Удаление файлов завершено! ");
                    log.LogWrite("Удаление файлов завершено!");
                }
                else
                {
                    log.LogWrite("Файлов старше 30 дней не обнаружено завершено! ");
                    Console.WriteLine("Файлов старше 30 дней не обнаружено завершено! ");
                }
            }
           
            Console.WriteLine("Начинаем удаление backups в каталоге: " + pathMOUNT);
            log.LogWrite("Начинаем удаление backups в каталоге: " + pathMOUNT);
           
            var dirMount = new DirectoryInfo(pathMOUNT);
            if (!dirMount.Exists)
            {
                dirMount.Create();
            }

            var dirYear = new DirectoryInfo(pathYears);
            if (!dirYear.Exists)
            {
                dirYear.Create();
            }
            foreach (FileInfo file in dirMount.GetFiles())
            {
                if (DateTime.UtcNow - file.LastWriteTimeUtc > TimeSpan.FromDays(365))
                {
                    Console.WriteLine("Найденый файл старше 365 дней: {0}", file.Name);
                    log.LogWrite("Найденый файл старше 365 дней: " + file.Name);
                    File.Move(file.FullName, pathYears + "\\" + file.Name);
                    Console.WriteLine("Удаление файлов завершено! ");
                    log.LogWrite("Удаление файлов завершено!");
                }
                else
                {
                    Console.WriteLine("Файлов старше 365 дней не обнаружено завершено! ");
                    log.LogWrite("Файлов старше 365 дней не обнаружено завершено! ");
                }
            }

            DateTime endtime = DateTime.Now;

            Console.WriteLine("Секунды " + (endtime - starttime).Seconds + " Миллисекунды " + (endtime - starttime).Milliseconds);
            log.LogWrite("Секунды " + (endtime - starttime).Seconds + " Миллисекунды " + (endtime - starttime).Milliseconds);

            Console.WriteLine("Програма завершыла свою работу! ");
            Thread.Sleep(10000);

            log.LogWrite("Програма завершыла свою работу! ");
            log.LogWrite("#######################################################");
        }

    }
}