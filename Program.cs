﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace MoveBottlesBottomToTopService
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            //ServiceBase[] ServicesToRun;
            //ServicesToRun = new ServiceBase[]
            //{
            //    new MainService()
            //};
            //ServiceBase.Run(ServicesToRun);

            MainService mainService = new MainService();
            mainService.RunService();
        }
    }
}
