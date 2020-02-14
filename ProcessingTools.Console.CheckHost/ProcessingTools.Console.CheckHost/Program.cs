// <copyright file="Program.cs" company="ProcessingTools">
// Copyright (c) 2020 ProcessingTools. All rights reserved.
// </copyright>

namespace ProcessingTools.Console.CheckHost
{
    using System;
    using System.Globalization;
    using System.Net;
    using System.Net.Sockets;

    /// <summary>
    /// Main program.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Main method.
        /// </summary>
        /// <param name="args">Arguments.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Main method.")]
        public static void Main(string[] args)
        {
            if (args is null || args.Length < 2)
            {
                PrintHelp();
                Environment.Exit(1);
                return;
            }

            string portArgument = null;

            TcpClient tcpClient = null;

            try
            {
                string addressArgument = args[0];
                portArgument = args[1];

                int portNumber;
                portNumber = int.Parse(portArgument, CultureInfo.InvariantCulture);

                tcpClient = new TcpClient();
                tcpClient.ReceiveTimeout = tcpClient.SendTimeout = 2000;

                if (IPAddress.TryParse(args[0], out IPAddress address))
                {
                    var endPoint = new IPEndPoint(address, portNumber);
                    tcpClient.Connect(endPoint);
                }
                else
                {
                    tcpClient.Connect(addressArgument, portNumber);
                }

                PrintSuccess(portArgument);
            }
            catch (Exception e)
            {
                if (e is SocketException || e is TimeoutException)
                {
                    PrintFail(portArgument);
                }
                else
                {
                    PrintHelp();
                }
            }
            finally
            {
                if (tcpClient != null)
                {
                    tcpClient.Close();
                }
            }
        }

        private static void PrintFail(string portArgument)
        {
            Console.WriteLine($"Not listening on port {portArgument}.");
        }

        private static void PrintHelp()
        {
            Console.WriteLine($"Usage: {AppDomain.CurrentDomain.FriendlyName} [host|ip] [port]");
        }

        private static void PrintSuccess(string portArgument)
        {
            Console.WriteLine($"Port {portArgument} is listening.");
        }
    }
}
