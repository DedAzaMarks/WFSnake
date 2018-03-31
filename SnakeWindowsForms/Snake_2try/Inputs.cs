using System.Windows.Forms;
using System.Collections;

namespace Snake_2try
{
    class Inputs
    {
        // Load list of avalible Keyboard buttons
        private static Hashtable KeyTable = new Hashtable();

        // Perform a ckeck to see if a particular button is pressed.
        public static bool KeyPressed(Keys key)
        {
            if (KeyTable[key] == null)
                return false;

            return (bool)KeyTable[key];
        }

        // Detect if a keybord button is pressed
        public static void ChangState (Keys key, bool state)
        {
            KeyTable[key] = state;
        }
    }
}
