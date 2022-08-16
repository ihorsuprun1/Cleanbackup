using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanBackup
{
   public class Settings
    {
        public string CompanyName { get; set; } = "Unknow";
        public string TypeCopy { get; set; } = "File";
        public bool TelegramIsNeed { get; set; } = true;
        public bool CleanBackupIsNeed { get; set; } = true;
        public string TelegramChtid { get; set; } 
        public string TelegramToken { get; set; }
        public string PathSearchDirectory { get; set; } = @"D:\backup\";
        public string OutPutDirectory { get; set; } = @"D:\backups\1.Dayly\";
        public string PathDAY { get; set; } = @"D:\backups\1.Dayly";
        public string PathWEEK { get; set; } = @"D:\backups\2.Weekly";
        public string PathMOUNT { get; set; } = @"D:\backups\3.Monthly";
        public string PathYears { get; set; } = @"D:\backups\4.Years";
     
    }
}
