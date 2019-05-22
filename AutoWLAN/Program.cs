using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using SimpleWifi;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Reflection;

namespace AutoWLAN
{
    class Program
    {
        #region WINAPI
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        #endregion

        static string configFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "config.json"); //set path to current path, otherwise it crashes at startup

        static void Main(string[] args)
        {
            ShowWindow(GetConsoleWindow(), 0 /*hide console*/);

            #region Get Connection Data

            Config config; //create a new Config class
           
            try
            {
                config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(configFile)); //deserializes the config into the config class
            }
            catch (FileNotFoundException)
            {
                
                File.WriteAllText(configFile, JsonConvert.SerializeObject(config = new Config() { SSID = "SSID_HERE", Autostart = false })); //create new config JSON File with standard values
                Environment.Exit(0); //exit
            }
            #endregion

            #region Set/Remove Autostart

            RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true); //open autostart key in registry

            if (config.Autostart) //if autostart is enabled
            {
                
                rk.SetValue("AutoWLAN", Assembly.GetExecutingAssembly().Location); //add to startup at Assembly location
            }
            else
            {
                if (rk.GetValue("AutoWLAN") == null)
                {
                    rk.DeleteValue("AutoWLAN");
                }
            }

            
            #endregion

            #region Connect to wifi

            AccessPoint targetWLAN = null;
            Wifi wifi = new Wifi();
            
            foreach (AccessPoint ap in wifi.GetAccessPoints()) //iterating through all wireless profiles
            {
                if(ap.Name == config.SSID)
                {
                    targetWLAN = ap; //if the right one is found assign it
                    break;
                }
            }

            if (targetWLAN != null)
            {
                AuthRequest authRequest = new AuthRequest(targetWLAN);
                targetWLAN.Connect(authRequest); //connect
            }
            #endregion

        }
    }
}
