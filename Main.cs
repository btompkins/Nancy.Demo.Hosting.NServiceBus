using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using NServiceBus;
using Nancy.Hosting.Self;
using log4net;

namespace NServiceBusHosting
{
    class Main : IWantToRunAtStartup
    {
        private static NancyHost _nancyHost;

        private static readonly ILog Logger =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
 
        #region Implementation of IWantToRunAtStartup

        /// <summary>
        /// Method called at startup.
        /// </summary>
        public void Run()
        {
            Logger.Info("Starting Nancy host on http://localhost:8888/nancy/");
            _nancyHost = new NancyHost(new Uri("http://localhost:8888/nancy/"));
            _nancyHost.Start();

        }

        /// <summary>
        /// Method called on shutdown.
        /// </summary>
        public void Stop()
        {
            Logger.Info("Stopping Nancy host");
            _nancyHost.Stop();
        }

        #endregion
    }
}
