/*
 * Program - Entry point for Solar System Editor
 */

using System;
using System.Windows.Forms;

namespace EditorRestart
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
