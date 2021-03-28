using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using WindowsInput.Native;
using WindowsInput;

namespace RIANTI_System
{
    class Elite
    {
        public static bool EliteModeEnabled = false;

        public static Random rand = new Random();

        public static InputSimulator Sim = new InputSimulator();

        public static List<String> EliteCommands = new List<string>()
        {
            "HELLO",
            "GOODBYE",
            "GOOD BYE",
            "ENABLE ELITE",
            "DISABLE ELITE",
            "JUMP",
            "BOOST",
            "SUPERCRUISE",
            "SUPER CRUISE",
            "LANDING GEAR",
            "MODE",
            "LIGHTS",
            "COUNTERMEASURES",
            "COUNTER MEASURES",
            "WEAPONS",
            "ACTIVATE WEAPONS",
            "DEACTIVATE WEAPONS",
            "MAP",
            "TARGET",
            "BALANCE POWER",
            "RESET POWER",
            "DEPLOY LANDING GEAR",
            "RETRACT LANDING GEAR",
            "LOADOUT",
            "FIRE GROUP",
            "LOCK THREAT",
            "STEALTH",
            "LOUD",
            "HEATSINK",
            "HEAT SINK",
            "SCOOP",
            "SHIELDS",
            "BOOST SHIELDS",
            "DEPLOY",
            "DOCK",
            "DARK",
            "DOC",
            "DEPLOY FIGHTER",
            "RECALL FIGHTER",
            "DEFEND",
            "ATTACK",
            "RECALL"
        };

        public static string ProcessEDCommand(string commandText)
        {

            string text = "Command Not Found";

            switch (commandText.ToUpper())
            {
                case "ENABLE ELITE":
                    text = SetEDStatus(true);
                    break;
                case "DISABLE ELITE":
                    text = SetEDStatus(false);
                    break;
                case "HELLO":
                    text = Greet(true);
                    break;
                case "GOODBYE":
                case "GOOD BYE":
                    text = Greet(false);
                    break;
                case "JUMP":
                    if (EliteModeEnabled) text = Jump();
                    break;
                case "BOOST":
                case "SUPERCRUISE":
                case "SUPER CRUISE":
                    if (EliteModeEnabled) text = SuperCruise();
                    break;
                case "BALANCE POWER":
                case "RESET POWER":
                    if (EliteModeEnabled) text = BalancePower();
                    break;
                case "WEAPONS":
                case "ACTIVATE WEAPONS":
                    if (EliteModeEnabled) text = ToggleWeapons(true);
                    break;
                case "DEACTIVATE WEAPONS":
                    if (EliteModeEnabled) text = ToggleWeapons(false);
                    break;
                case "LOADOUT":
                case "FIRE GROUP":
                    if (EliteModeEnabled) text = ToggleFireGroup();
                    break;
                case "LOCK THREAT":
                    if (EliteModeEnabled) text = LockOnThreat();
                    break;
                case "HEAT SINK":
                case "HEATSINK":
                    if (EliteModeEnabled) text = DeployHeatSink();
                    break;
                case "STEALTH":
                case "STEALTH ON":
                    if (EliteModeEnabled) text = ToggleStealth(true);
                    break;
                case "STEALTH OFF":
                    if (EliteModeEnabled) text = ToggleStealth(false);
                    break;
                case "DEPLOY LANDING GEAR":
                    if (EliteModeEnabled) text = ToggleLandingGear(true);
                    break;
                case "RETRACT LANDING GEAR":
                    if (EliteModeEnabled) text = ToggleLandingGear(false);
                    break;
                case "SCOOP":
                    if (EliteModeEnabled) text = ToggleScoop();
                    break;
                case "COUNTERMEASURES":
                case "COUNTER MEASURES":
                    if (EliteModeEnabled) text = DeployCounterMeasures();
                    break;
                case "LIGHTS":
                    if (EliteModeEnabled) text = ToggleLights();
                    break;
                case "MODE":
                    if (EliteModeEnabled) text = ToggleMode();
                    break;
                case "SHIELDS":
                case "BOOST SHIELDS":
                    if (EliteModeEnabled) text = BoostShields();
                    break;
                case "DEPLOY":
                case "DEPLOY FIGHTER":
                    if (EliteModeEnabled) text = DeployFighter();
                    break;
                case "RECALL":
                case "RECALL FIGHTER":
                    if (EliteModeEnabled) text = Recall();
                    break;
                case "DOCK":
                case "DARK":
                case "DOC":
                    if (EliteModeEnabled) text = AutoDock();
                    break;
                case "DEFEND":
                    if (EliteModeEnabled) text = Defend();
                    break;
                case "ATTACK":
                    if (EliteModeEnabled) text = Attack();
                    break;
            }
            return text;
        }

        private static string SetEDStatus(bool status)
        {
            string newStatus = status ? "Enabled" : "Disabled";

            if (EliteModeEnabled != status)
            {
                EliteModeEnabled = status;

                return $"Elite Dangerous Command Module {newStatus}.";

            }
            else
            {
                return $"Elite Dangerous Command Module was already {newStatus}.";
            }

        }

        private static string Greet(bool status)
        {
            if (status)
            {
                return "Greetings Commander.";
            }
            else
            {
                return "Goodbye.";
            }
        }

        private static string Jump()
        {
            PressKey(VirtualKeyCode.VK_J);

            if (rand.Next(15) == 5)
            {
                return "Friendship Drive Initiated."; //fun easter egg
            }
            else
            {
                return "Jump Drive Initiated.";
            }
        }

        private static string SuperCruise()
        {
            PressKey(VirtualKeyCode.VK_K);

            return "Super Cruise Initiated.";
        }

        [Command("Test")]
        private static string BalancePower()
        {
            PressKey(VirtualKeyCode.DOWN);

            return "Power Balanced.";
        }

        private static string ToggleWeapons(bool status)
        {
            PressKey(VirtualKeyCode.VK_U);

            if (status)
            {
                return "Weapons Activated.";
            }
            else
            {
                return "Weapons Deactivated.";
            }
        }

        private static string ToggleFireGroup()
        {
            PressKey(VirtualKeyCode.VK_N);

            return "N/A";
        }

        private static string LockOnThreat()
        {
            PressKey(VirtualKeyCode.VK_H);

            return "Tartgeting Threat.";
        }

        private static string ToggleStealth(bool status)
        {
            PressKey(VirtualKeyCode.DELETE);

            if (status)
            {
                if (rand.Next(10) == 5)
                {
                    return "Stealth Mode Enabled."; //crysis ref
                }
                else
                {
                    return "Cloak Activated.";
                }
            }
            else
            {
                return "Stealth Mode Disabled.";
            }
        }

        private static string ToggleLandingGear(bool status)
        {
            PressKey(VirtualKeyCode.VK_C);

            if (status)
            {
                return "Landing Gear Deployed.";
            }
            else
            {
                return "Landing Gear Retracted.";
            }
        }

        private static string ToggleScoop()
        {
            PressKey(VirtualKeyCode.HOME);

            return "Cargo Scoop Activated.";
        }

        private static string DeployHeatSink()
        {
            PressKey(VirtualKeyCode.VK_V);

            return "Heatsink Deployed.";
        }

        private static string BoostShields()
        {
            PressKey(VirtualKeyCode.VK_B);

            return "Shields Replenished.";
        }

        private static string ToggleLights()
        {
            PressKey(VirtualKeyCode.VK_L);

            return "N/A";
        }

        private static string ToggleMode()
        {
            PressKey(VirtualKeyCode.VK_M);

            return "N/A";
        }

        private static string DeployCounterMeasures()
        {
            PressKey(VirtualKeyCode.NUMPAD8);

            return "Countermeasures Deployed.";
        }

        private static string DeployFighter()
        {

            PressKey(VirtualKeyCode.VK_3);
            Thread.Sleep(500);

            PressKey(VirtualKeyCode.VK_W);
            Thread.Sleep(200);

            PressKey(VirtualKeyCode.VK_S);
            Thread.Sleep(200);

            PressKey(VirtualKeyCode.SPACE);
            Thread.Sleep(200);

            PressKey(VirtualKeyCode.VK_W);
            Thread.Sleep(200);

            PressKey(VirtualKeyCode.VK_W);
            Thread.Sleep(200);

            PressKey(VirtualKeyCode.VK_W);
            Thread.Sleep(200);

            PressKey(VirtualKeyCode.VK_S);
            Thread.Sleep(200);

            PressKey(VirtualKeyCode.SPACE);
            Thread.Sleep(200);

            PressKey(VirtualKeyCode.VK_W);
            Thread.Sleep(200);

            PressKey(VirtualKeyCode.VK_W);
            Thread.Sleep(200);

            PressKey(VirtualKeyCode.VK_W);
            Thread.Sleep(200);

            PressKey(VirtualKeyCode.VK_S);
            Thread.Sleep(200);

            PressKey(VirtualKeyCode.VK_S);
            Thread.Sleep(200);

            PressKey(VirtualKeyCode.SPACE);
            Thread.Sleep(200);

            PressKey(VirtualKeyCode.VK_W);
            Thread.Sleep(200);

            PressKey(VirtualKeyCode.VK_3);

            return "Fighter Deployed.";
        }


        private static string AutoDock()
        {

            PressKey(VirtualKeyCode.VK_1);
            Thread.Sleep(200);
            PressKey(VirtualKeyCode.VK_E);
            Thread.Sleep(200);
            PressKey(VirtualKeyCode.VK_E);
            Thread.Sleep(200);
            PressKey(VirtualKeyCode.VK_D);
            Thread.Sleep(200);
            PressKey(VirtualKeyCode.SPACE);
            Thread.Sleep(200);
            PressKey(VirtualKeyCode.ESCAPE);

            return "Docking Requested.";
        }

        private static string Attack()
        {
            var sim = new InputSimulator();
            PressKey(VirtualKeyCode.NUMPAD2);

            return "N/A";
        }

        private static string Defend()
        {
            var sim = new InputSimulator();
            PressKey(VirtualKeyCode.NUMPAD1);

            return "N/A";
        }

        private static string Recall()
        {
            var sim = new InputSimulator();
            PressKey(VirtualKeyCode.NUMPAD0);

            return "Recalling Fighter";
        }

        private static void PressKey(VirtualKeyCode key, int sleepTime = 20)
        {
            Sim.Keyboard.KeyDown(key);
            Thread.Sleep(sleepTime);
            Sim.Keyboard.KeyUp(key);
        }

    }
}
