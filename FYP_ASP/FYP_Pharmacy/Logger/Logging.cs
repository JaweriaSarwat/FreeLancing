﻿using System;
using System.Configuration;
using System.IO;
using System.Text;

namespace Logger
{
    public class Logging
    {
        private StringBuilder Log;
        private StringBuilder FunctionalLog;
        private string LOG_PATH = ConfigurationManager.AppSettings["logpath"].ToString();
        private string LogFileName = "Log_" + DateTime.Now.ToString("yyyy’-‘MM’-‘dd’T’HH’:’mm’:’ss");

        public void LogException(string Context, Exception Ex, string WebPage = "")
        {
            FunctionalLog = new StringBuilder("[" + DateTime.Now.ToString("yyyy’-‘MM’-‘dd’T’HH’:’mm’:’ss") + "] - (LogType: Exception) ");

            FunctionalLog.Append("Context: " + Context);

            AppendSeparator();

            if (!string.IsNullOrWhiteSpace(WebPage))
            {
                FunctionalLog.Append("WebPage: " + WebPage);
                AppendSeparator();
            }

            FunctionalLog.Append("Exception: " + Ex.Message);
            FunctionalLog.Append(Environment.NewLine);
            Log.Append(FunctionalLog);

        }

        public void LogInfo(string Context, string Class, string Info, string WebPage = "")
        {
            FunctionalLog = new StringBuilder("[" + DateTime.Now.ToString("yyyy’-‘MM’-‘dd’T’HH’:’mm’:’ss") + "] - (LogType: Info) ");

            FunctionalLog.Append("Context: " + Context);
            AppendSeparator();

            if (!string.IsNullOrWhiteSpace(WebPage))
            {
                FunctionalLog.Append("WebPage: " + WebPage);
                AppendSeparator();
            }

            FunctionalLog.Append("Class: " + Class);
            AppendSeparator();

            FunctionalLog.Append("Message: " + Info);
            FunctionalLog.Append(Environment.NewLine);
            Log.Append(FunctionalLog);

        }
        public void LogFunction(string Context, string FunctionName, string WebPage = "")
        {
            FunctionalLog = new StringBuilder("[" + DateTime.Now.ToString("yyyy’-‘MM’-‘dd’T’HH’:’mm’:’ss") + "] (LogType: Functional) ");

            FunctionalLog.Append("Context: " + Context);
            AppendSeparator();

            if (!string.IsNullOrWhiteSpace(WebPage))
            {
                FunctionalLog.Append("WebPage: " + WebPage);
                AppendSeparator();
            }

            FunctionalLog.Append("FunctionName: " + FunctionName);
            FunctionalLog.Append(Environment.NewLine);
            Log.Append(FunctionalLog);

        }
        public void LogSql(string Context, string Query, string QueryType)
        {
            FunctionalLog = new StringBuilder("[" + DateTime.Now.ToString("yyyy’-‘MM’-‘dd’T’HH’:’mm’:’ss") + "] (LogType: Sql) ");

            FunctionalLog.Append("Context: " + Context);
            AppendSeparator();

            FunctionalLog.Append("SqlType: " + QueryType);
            AppendSeparator();

            FunctionalLog.Append("Query:" + Query);
            FunctionalLog.Append(Environment.NewLine);
            Log.Append(FunctionalLog);
        }

        public void PublishLog()
        {
            File.WriteAllText(LOG_PATH + LogFileName, Log.ToString());
        }

        #region HelperMethods
        public void AppendSeparator()
        {
            FunctionalLog.Append(" | ");
        }
        #endregion

    }
}
