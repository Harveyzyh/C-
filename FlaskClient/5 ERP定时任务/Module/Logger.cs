using System;
using System.IO;
using System.Reflection;

namespace HarveyZ
{
    public class Logger
    {
        #region Instance
        private static object logLock;

        private static Logger _instance;

        private string logFileName;

        private string basePath = null;

        public Logger() { }

        /// <summary>
        /// Logger instance
        /// </summary>
        public Logger Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Logger();
                    logLock = new object();
                }
                return _instance;
            }
        }
        #endregion

        /// <summary>
        /// Write log to log file
        /// </summary>
        /// <param name="logContent">Log content</param>
        /// <param name="logType">Log type</param>
        public void WriteLog(string logContent, LogType logType = LogType.Information)
        {
            try
            {
                basePath = Directory.GetCurrentDirectory();
                if (!Directory.Exists(basePath + "\\Log"))
                {
                    Directory.CreateDirectory(basePath + "\\Log");
                }

                string dateString = DateTime.Now.ToString("yyyy-MM-dd");

                string[] logText = new string[] { DateTime.Now.ToString("HH:mm:ss") + ": " + logType.ToString() + ": " + logContent };

                lock (logLock)
                {
                    File.AppendAllLines(basePath + @"\Log\" + dateString + @".txt", logText);
                }
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Write exception to log file
        /// </summary>
        /// <param name="exception">Exception</param>
        public void WriteException(Exception exception,string specialText= null)
        {
            if (exception != null)
            {
                Type exceptionType = exception.GetType();
                string text = string.Empty;
                if (!string.IsNullOrEmpty(specialText))
                {
                    text = text + specialText + Environment.NewLine;
                }
                text = "Exception: " + exceptionType.Name + Environment.NewLine;
                text += "               " + "Message: " + exception.Message + Environment.NewLine;
                text += "               " + "Source: " + exception.Source + Environment.NewLine;
                text += "               " + "StackTrace: " + exception.StackTrace + Environment.NewLine;
                WriteLog(text, LogType.Error);
            }
        }
    }
}