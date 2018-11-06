using System;
using System.IO;
using System.Text;

namespace FileHandlerChangeRootApp
{
    public enum LogType
    {
        Error = 1,
        Info = 2,
        Warn = 3,
        Debug = 4,
        Reminder = 5
    }

    public static class FileLog
    {
        /// <summary>
        /// 自动删除当前的空文件夹，
        /// 每小时执行一次。
        /// </summary>
        /// <param name="dirPath"></param>
        public static void DelEmptyFile(string dirPath)
        {
            DateTime now = DateTime.Now;
            string[] files = System.IO.Directory.GetFiles(dirPath, "*.txt");
            if (files == null || files.Length == 0)
            {
                return;
            }
            try
            {
                foreach (string s in files)
                {
                    FileInfo f = new FileInfo(s);
                    if ((now - f.CreationTime).Hours < 1)
                    {
                        continue;
                    }
                    if (f.Length == 0 && f.Name.Length == 14)
                    {
                        f.Delete();
                    }
                }
            }
            catch (Exception ex)
            {
            }

        }

        /// <summary>
        /// 记录信息
        /// </summary>
        /// <param name="msg"></param>
        public static void Info(string msg, LogType type)
        {
            WriteLog(false, "Info", msg, null, type);
        }
        /// <summary>
        /// 记录：消耗时间|日志消息 [begintime: begintime, taketime: taketime] | msg
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="begintime"></param>
        public static void Info(string msg, DateTime begintime, LogType type)
        {
            msg = string.Format("taketime: {0} | {1}"
                , (DateTime.Now - begintime)
                , msg);
            WriteLog(false, "Info", msg, null, type);
        }
        /// <summary>
        /// 记录信息
        /// </summary>
        /// <param name="IsFormat">true: 默认格式输出</param>
        /// <param name="msg"></param>
        public static void Info(bool IsFormat, string msg, LogType type)
        {
            WriteLog(IsFormat, "Info", msg, null, type);
        }

        /// <summary>
        /// 警告信息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        public static void Warn(string msg, Exception ex, LogType type)
        {
            WriteLog(false, "Warn", msg, ex, type);
        }
        /// <summary>
        /// 警告信息
        /// </summary>
        /// <param name="IsFormat">true: 默认格式输出</param>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        public static void Warn(bool IsFormat, string msg, Exception ex, LogType type)
        {
            WriteLog(IsFormat, "Warn", msg, ex, type);
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        public static void Error(string msg, Exception ex, LogType type)
        {
            WriteLog(false, "Error", msg, ex, type);
        }
        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="IsFormat">true: 默认格式输出</param>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        public static void Error(bool IsFormat, string msg, Exception ex, LogType type)
        {
            WriteLog(IsFormat, "Error", msg, ex, type);
        }

        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="msg"></param>
        public static void Debug(string msg, LogType type)
        {
            WriteLog(false, "Debug", msg, null, type);
        }
        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="IsFormat">true: 默认格式输出</param>
        /// <param name="msg"></param>
        public static void Debug(bool IsFormat, string msg, LogType type)
        {
            WriteLog(IsFormat, "Debug", msg, null, type);
        }

        /// <summary>
        /// 日志信息写入日志文件
        /// </summary>
        /// <param name="isFormat">是否按照默认格式输出日志</param>
        /// <param name="logType">日志类别：Error/Warn/Info/Debug</param>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        private static void WriteLog(bool isFormat, string logType, string msg, Exception ex, LogType type)
        {
            byte[] data = null;
            if (isFormat)
            {
                // 默认格式输出
                #region more code
                if (object.Equals(ex, null))
                {
                    msg = string.Format(@"[DateTime] : {0}
                                            [LogType]  : {1}
                                            [Message]  : {2}
                                            [Exception]:
                                            "
                        , DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff")
                        , logType
                        , msg
                        );
                }
                else
                {
                    msg = string.Format(@"[DateTime] : {0}
                                            [LogType]  : {1}
                                            [Message]  : {2}
                                            [Exception]: {3}
                                            "
                           , DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff")
                           , logType
                           , msg
                           , ex.ToString()
                           );
                }
                #endregion
            }
            else
            {
                // 自定格式输出
                #region more code
                if (object.Equals(ex, null))
                {
                    msg = string.Format(@"[{0}][{1}] : msg={2}
"
                        , DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff")
                        , logType
                        , msg
                        );
                }
                else
                {
                    msg = string.Format(@"[{0}][{1}] : msg={2}
ex={3} 
"
                           , DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff")
                           , logType
                           , msg
                           , ex.ToString()
                           );
                }
                #endregion
            }

            try
            {
                // init file
                string rootPath = AppDomain.CurrentDomain.BaseDirectory + "LogFile\\";

                //日志分类
                string logFileInfo = "";
                int key = (int)type;
                switch (key)
                {
                    case 2:
                        logFileInfo = "Info\\";
                        break;
                    case 1:
                        logFileInfo = "Error";
                        break;
                    case 3:
                        logFileInfo = "Warn\\";
                        break;
                    case 4:
                        logFileInfo = "Debug\\";
                        break;
                    case 5:
                        logFileInfo = "Reminder\\";
                        break;
                    default:
                        break;
                }

                string subPath = rootPath + logFileInfo;
                if (!Directory.Exists(rootPath))
                {
                    Directory.CreateDirectory(rootPath);
                    if (!Directory.Exists(subPath))
                    {
                        Directory.CreateDirectory(subPath);
                    }
                }
                else
                {
                    if (!Directory.Exists(subPath))
                    {
                        Directory.CreateDirectory(subPath);
                    }
                }
                string fpath = subPath + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";

                if (!File.Exists(fpath))
                {
                    File.CreateText(fpath).Dispose();
                }


                using (FileStream fs = File.Open(fpath, FileMode.Append, FileAccess.Write))
                {
                    data = Encoding.UTF8.GetBytes(msg);
                    fs.Write(data, 0, data.Length);
                    fs.Close();
                    fs.Dispose();
                }
            }
            catch (Exception e)
            {
            }
            finally
            {
                // 资源释放
                data = null;
                msg = null;
            }
        }

        /// <summary>
        /// 文件上传下载日志
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="type"></param>
        public static void Reminder(string msg, LogType type)
        {
            WriteLog(false, "Reminder", msg, null, type);
        }

    }
}
