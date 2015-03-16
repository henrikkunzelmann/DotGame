using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.Utils
{
    /// <summary>
    /// Stellt verschiedene Funktionen für den Log bereit.
    /// </summary>
    public static class Log
    {
        /// <summary>
        /// Das Level ab dem Nachrichten im Log angezeigt werden.
        /// Standard: LogLevel.Verbose
        /// </summary>
        public static LogLevel LevelMinimum { get; set; }

        /// <summary>
        /// Schreibt eine Nachricht in den Log.
        /// </summary>
        /// <param name="level">Das Level für die Nachricht</param>
        /// <param name="message">Die Nachricht</param>
        public static void Write(LogLevel level, string message)
        {
            if (message == null)
                throw new ArgumentNullException("message");

            if (level >= LevelMinimum)// alle Einträge die unter dem Minimum fallen ignoreren
            {
                string formattedMessage = string.Format("[{0}] {1} {2}\r\n", DateTime.Now.ToLongTimeString(), ("(" + level.ToString() + ")").PadRight(10), message);
                Console.Write(formattedMessage);
                System.Diagnostics.Debug.Write(formattedMessage);
            }
        }

        /// <summary>
        /// Schreibt eine formattierte Nachricht in den Log.
        /// </summary>
        /// <param name="level">Das Level für die Nachricht</param>
        /// <param name="format">Das Format der Nachricht</param>
        /// <param name="args">Die Argumente für das Format</param>
        public static void Write(LogLevel level, string format, params object[] args)
        {
            if (format == null)
                throw new ArgumentNullException("format");
            if (args == null)
                throw new ArgumentNullException("args");

            Write(level, string.Format(format, args));
        }

        /// <summary>
        /// Ruft Log.Write(LogLevel.Verbose, message) auf.
        /// </summary>
        /// <param name="message">Die Nachricht</param>
        public static void Verbose(string message)
        {
            if (message == null)
                throw new ArgumentNullException("message");

            Write(LogLevel.Verbose, message);
        }

        /// <summary>
        /// Ruft Log.Write(LogLevel.Debug, message) auf.
        /// </summary>
        /// <param name="message">Die Nachricht</param>
        public static void Debug(string message)
        {
            if (message == null)
                throw new ArgumentNullException("message");

            Write(LogLevel.Debug, message);
        }

        /// <summary>
        /// Ruft Log.Write(LogLevel.Info, message) auf.
        /// </summary>
        /// <param name="message">Die Nachricht</param>
        public static void Info(string message)
        {
            if (message == null)
                throw new ArgumentNullException("message");

            Write(LogLevel.Info, message);
        }

        /// <summary>
        /// Ruft Log.Write(LogLevel.Warning, message) auf.
        /// </summary>
        /// <param name="message">Die Nachricht</param>
        public static void Warning(string message)
        {
            if (message == null)
                throw new ArgumentNullException("message");

            Write(LogLevel.Warning, message);
        }

        /// <summary>
        /// Ruft Log.Write(LogLevel.Error, message) auf.
        /// </summary>
        /// <param name="message">Die Nachricht</param>
        public static void Error(string message)
        {
            if (message == null)
                throw new ArgumentNullException("message");

            Write(LogLevel.Error, message);
        }

        /// <summary>
        /// Ruft Log.Write(LogLevel.Verbose, format, args) auf.
        /// </summary>
        /// <param name="message">Die Nachricht</param>
        public static void Verbose(string format, params object[] args)
        {
            if (format == null)
                throw new ArgumentNullException("format");
            if (args == null)
                throw new ArgumentNullException("args");

            Write(LogLevel.Verbose, format, args);
        }

        /// <summary>
        /// Ruft Log.Write(LogLevel.Debug, format, args) auf.
        /// </summary>
        /// <param name="message">Die Nachricht</param>
        public static void Debug(string format, params object[] args)
        {
            if (format == null)
                throw new ArgumentNullException("format");
            if (args == null)
                throw new ArgumentNullException("args");

            Write(LogLevel.Debug, format, args);
        }

        /// <summary>
        /// Ruft Log.Write(LogLevel.Info, format, args) auf.
        /// </summary>
        /// <param name="message">Die Nachricht</param>
        public static void Info(string format, params object[] args)
        {
            if (format == null)
                throw new ArgumentNullException("format");
            if (args == null)
                throw new ArgumentNullException("args");

            Write(LogLevel.Info, format, args);
        }

        /// <summary>
        /// Ruft Log.Write(LogLevel.Warning, format, args) auf.
        /// </summary>
        /// <param name="message">Die Nachricht</param>
        public static void Warning(string format, params object[] args)
        {
            if (format == null)
                throw new ArgumentNullException("format");
            if (args == null)
                throw new ArgumentNullException("args");

            Write(LogLevel.Warning, format, args);
        }

        /// <summary>
        /// Ruft Log.Write(LogLevel.Error, format, args) auf.
        /// </summary>
        /// <param name="message">Die Nachricht</param>
        public static void Error(string format, params object[] args)
        {
            if (format == null)
                throw new ArgumentNullException("format");
            if (args == null)
                throw new ArgumentNullException("args");

            Write(LogLevel.Error, format, args);
        }


        /// <summary>
        /// Schreibt alle Felder des Objekts obj in den Log.
        /// </summary>
        /// <param name="level">Das LogLevel welches benutzt werden soll.</param>
        /// <param name="obj">Das Objekt mit den Feldern die in den Log geschrieben werden sollen.</param>
        public static void WriteFields(LogLevel level, object obj)
        {
            var fields = obj.GetType().GetFields();
            Log.Write(level, "All fields for {0} [n: {1}]:", obj.GetType().FullName, fields.Length);
            foreach (var field in fields)
                Log.Write(level, "\t{0} = {1}", field.Name, field.GetValue(obj));
        }
    }
}
