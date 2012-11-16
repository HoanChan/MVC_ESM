﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Mvc_ESM.Static_Helper;
using Microsoft.VisualBasic.ApplicationServices;
namespace Mvc_ESM
{
    static class Program
    {
        public static AlgorithmRunner AlgorithmRunner;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            AlgorithmRunner = new AlgorithmRunner();
            AlgorithmRunner.Init();
            App myApp = new App();
            myApp.Run(args);
        }
        //[STAThread]
        //static void Main()
        //{
        //    AlgorithmRunner = new AlgorithmRunner();
        //    AlgorithmRunner.Init();
        //    Application.EnableVisualStyles();
        //    Application.SetCompatibleTextRenderingDefault(false);
        //    Application.Run(new MainForm());
        //}

        /// <summary>
        /// We inherit from the VB.NET WindowsFormApplicationBase class, which has the 
        /// single-instance functionality.
        /// </summary>
        class App : WindowsFormsApplicationBase
        {
            public App()
            {
                // Make this a single-instance application
                this.IsSingleInstance = true;
                this.EnableVisualStyles = true;

                // There are some other things available in the VB application model, for
                // instance the shutdown style:
                this.ShutdownStyle = Microsoft.VisualBasic.ApplicationServices.ShutdownMode.AfterMainFormCloses;

                // Add StartupNextInstance handler
                this.StartupNextInstance += new StartupNextInstanceEventHandler(this.SIApp_StartupNextInstance);
            }

            /// <summary>
            /// We are responsible for creating the application's main form in this override.
            /// </summary>
            protected override void OnCreateMainForm()
            {
                // Create an instance of the main form and set it in the application; 
                // but don't try to run it.
                this.MainForm = new MainForm();

                // We want to pass along the command-line arguments to this first instance

                // Allocate room in our string array
                ((MainForm)this.MainForm).Args = new string[this.CommandLineArgs.Count];

                // And copy the arguments over to our form
                this.CommandLineArgs.CopyTo(((MainForm)this.MainForm).Args, 0);
            }

            /// <summary>
            /// This is called for additional instances. The application model will call this 
            /// function, and terminate the additional instance when this returns.
            /// </summary>
            /// <param name="eventArgs"></param>
            protected void SIApp_StartupNextInstance(object sender,
                StartupNextInstanceEventArgs eventArgs)
            {
                // Copy the arguments to a string array
                string[] args = new string[eventArgs.CommandLine.Count];
                eventArgs.CommandLine.CopyTo(args, 0);

                // Create an argument array for the Invoke method
                object[] parameters = new object[2];
                parameters[0] = this.MainForm;
                parameters[1] = args;

                // Need to use invoke to b/c this is being called from another thread.
                this.MainForm.Invoke(new MainForm.ProcessParametersDelegate(
                    ((MainForm)this.MainForm).ProcessParameters),
                    parameters);
            }
        }
    }
}
