using System;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using Windows.Storage;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;

namespace DemoLauncher
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var item in args)
            {
                Debug.WriteLine(item.ToString());
            }
            try
            {
                // process object to keep track of your child process
                Process newProcess = null;

                if (args.Length > 2)
                {
                    // launch process based on parameter
                    switch (args[2])
                    {
                        case "/mstsc":
                            newProcess = new Process
                            {
                                StartInfo = new ProcessStartInfo
                                {
                                    FileName = "mstsc.exe",
                                    //Arguments = "command line arguments to your executable",
                                    UseShellExecute = false,
                                    RedirectStandardOutput = true,
                                    RedirectStandardInput = true,
                                    CreateNoWindow = true
                                }
                            };
                            break;
                        case "/cmd":
                            string parameters = ApplicationData.Current.LocalSettings.Values["parameters"] as string;
                            newProcess = new Process
                            {
                                StartInfo = new ProcessStartInfo
                                {
                                    FileName = "cmd.exe",
                                    Arguments = parameters,
                                    UseShellExecute = false,
                                    RedirectStandardOutput = true,
                                    RedirectStandardInput = true,
                                    CreateNoWindow = true
                                }
                            };
                            break;
                    }
                    if (newProcess != null) {
                        newProcess.Start();
                        while (!newProcess.StandardOutput.EndOfStream)
                        {
                            string line = newProcess.StandardOutput.ReadLine();
                            // do something with line
                            Console.WriteLine(line);
                        }
                        newProcess.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }

        }
    }
}
