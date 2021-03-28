using System;
using System.Collections.Generic;
using System.Text;

namespace RIANTI_System
{
    public class Commands
    {
        public static List<String> GetCommandList()
        {
            List<string> commands = new List<string>();

            foreach(string command in Elite.EliteCommands)
            {
                commands.Add(command);
            }

            return commands;
        }

        public static string ProcessCommand(string command)
        {
            string response = "";
            if (Elite.EliteCommands.Contains(command.ToUpper()))
            {
                response = Elite.ProcessEDCommand(command);
            }
            return response;
        }
    }
}
