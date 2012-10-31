using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Mvc_ESM.Static_Helper;
namespace Mvc_ESM
{
    static class Program
    {
        public static AlgorithmRunner AlgorithmRunner;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        
        [STAThread]
        static void Main()
        {
            AlgorithmRunner = new AlgorithmRunner();
            AlgorithmRunner.Init();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Progress());
        }
    }
}
