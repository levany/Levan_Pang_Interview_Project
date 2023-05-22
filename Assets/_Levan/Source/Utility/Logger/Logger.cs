using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using UnityEngine.U2D;


namespace LevanPangInterview
{
    public static class Logger
    {
        //// Properties

        private static LoggerCanvas LoggerCanvas;

        private static StringBuilder builder = new StringBuilder();

        //// API

        /// <summary>
        /// Logs a message to the unity editor's console,
        /// and also to the on screen GUI Console 
        /// 
        /// in case you wish to Build and run and see log messages
        /// 
        /// (on screen llog is disabled by default,
        /// you can enable the uiLogger  gameobject in the main scene)
        /// </summary>
        /// <param name="message"></param>
        [HideInCallstack]
        public static void Log(string message
                             ,[CallerMemberName] string memberName       = ""
                             ,[CallerFilePath]   string sourceFilePath   = ""
                             ,[CallerLineNumber] int    sourceLineNumber = 0 )

        {
            builder.Clear();
            builder.Append("<color=lightblue>");
            builder.Append(Path.GetFileNameWithoutExtension(sourceFilePath));
            builder.Append(':');
            builder.Append(sourceLineNumber);
            builder.Append('.');
            builder.Append(memberName);
            builder.Append('(');
            builder.Append(')');
            builder.Append(' ');
            builder.Append("</color>");
            builder.Append(message);

            string text = builder.ToString();

            Debug.Log(text);
            LoggerCanvas?.Log(text);
        }

        [HideInCallstack]
        public static void LogError(string message
                                   ,[CallerMemberName] string memberName       = ""
                                   ,[CallerFilePath]   string sourceFilePath   = ""
                                   ,[CallerLineNumber] int    sourceLineNumber = 0 )
        {
            builder.Clear();
            builder.Append("<color=white>");
            builder.Append("[ERROR] ");
            builder.Append(Path.GetFileNameWithoutExtension(sourceFilePath));
            builder.Append(':');
            builder.Append(sourceLineNumber);
            builder.Append('.');
            builder.Append(memberName);
            builder.Append('(');
            builder.Append(')');
            builder.Append(' ');
            builder.Append("</color>");
            builder.Append(message);

            Debug.LogError(builder.ToString());
            LoggerCanvas?.LogError(builder.ToString());
        }

        [HideInCallstack]
        public static void LogSeperator()
        {
            var message = "=================================================";
            Debug.Log(message);
            LoggerCanvas?.Log(message);
        }

        //// Utility 

        public static void SetLoggerCanvas(LoggerCanvas canvas)
        {
            LoggerCanvas = canvas;
        }
    }
}