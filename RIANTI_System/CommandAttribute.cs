using System;

namespace RIANTI_System
{
    internal class CommandAttribute : Attribute
    {
        private string v;

        public CommandAttribute(string v)
        {
            this.v = v;
        }
    }
}