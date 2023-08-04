using System;
using System.Collections.Generic;
using System.Text;
using RIANTI_System;

namespace EDITH_Core
{
    class CommandHandler
    {
        public static string RunCommand(string command)
        {
            command = command.Replace(".", "");
            command = command.Replace(",", "");
            command = command.Replace("!", "");
            command = command.Replace("?", "");

            string outputString = "";

            outputString = Commands.ProcessCommand(command);

            if (outputString == "")
            {
                switch (command.ToLower())
                {
                    case "time":
                        outputString = GetTime();
                        break;
                    case "date":
                        outputString = GetDate();
                        break;
                }
            }

            return outputString;
        }

        public static string GetTime()
        {
            string time;

            time = $"The current time is {DateTime.Now.Hour}, {DateTime.Now.Minute}, {DateTime.Now.TimeOfDay}";

            return time;
        }

        public static string GetDate()
        {
            string date;

            date = $"Today is {DateTime.Now.DayOfWeek}, {DateTime.Now.Day}, {DateTime.Now.Month}, {DateTime.Now.Year}";

            return date;
        }

    }
}
