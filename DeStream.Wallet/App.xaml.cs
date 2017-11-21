﻿using DeStream.Wallet.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Navigation;

namespace DeStream.Wallet
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private const string AppName = "DeStreamWallet";
        private const string ConfigFile = "config.json";

        protected override void OnStartup(StartupEventArgs e)
        {
            var curProcessName = Process.GetCurrentProcess().ProcessName;
            if (Process.GetProcesses().Count(x => x.ProcessName == curProcessName) > 1)
            {
                Application.Current.Shutdown();
            }

            string configPath = Path.Combine(Environment.CurrentDirectory, ConfigFile);
            if (!File.Exists(configPath))
                configPath = ProceedConfig();

            var config = File.ReadAllText(configPath);
            try
            {
                AppContext.Config = new JavaScriptSerializer().Deserialize<Config>(config);
            }
            catch (Exception exc)
            {
                configPath = ProceedConfig();
                AppContext.Config = new JavaScriptSerializer().Deserialize<Config>(config);
            }
            base.OnStartup(e);
        }

        private string ProceedConfig()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            var appFolder = Path.Combine(path, AppName);
            string configPath = Path.Combine(appFolder, ConfigFile);
            if (!Directory.Exists(appFolder))
            {
                Directory.CreateDirectory(appFolder);
                WriteDefaultConfig(configPath);
            }
            else if (!File.Exists(configPath))
                WriteDefaultConfig(configPath);
            return configPath;
        }

        private void WriteDefaultConfig(string destinationPath)
        {
            using (var configStream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{Assembly.GetExecutingAssembly().GetName().Name}.{ConfigFile}"))
            {
                if (configStream != null)
                {
                    byte[] bytes = new byte[configStream.Length];
                    configStream.Read(bytes, 0, (int)configStream.Length);
                    using (var fileStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write))
                    {
                        fileStream.Write(bytes, 0, bytes.Length);
                        fileStream.Flush();
                    }
                }
            }
        }
    }
}