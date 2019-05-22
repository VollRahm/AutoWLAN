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

        static string configFile = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "config.json"));

        static void Main(string[] args)
        {
            ShowWindow(GetConsoleWindow(), 0 /*hide*/);

            #region Get Connection Data
            WlanData wlanData;
            
            try
            {
                wlanData = JsonConvert.DeserializeObject<WlanData>(configFile); //deserializes the config into the config class
            }
            catch (FileNotFoundException)
            {
                File.WriteAllText("config.json", JsonConvert.SerializeObject(wlanData = new WlanData() { SSID = "SSID_HERE", Autostart = false }));
                Environment.Exit(0);
            }
            #endregion

            #region Set/Remove Autostart

            RegistryKey rk = Registry.CurrentUser.OpenSubKey
            ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);


            #endregion

            #region Connect to wifi

            AccessPoint targetWLAN = null;
            Wifi wifi = new Wifi();
            
            foreach (AccessPoint ap in wifi.GetAccessPoints())
            {
                if(ap.Name == wlanData.SSID)
                {
                    targetWLAN = ap;
                    break;
                }
            }

            AuthRequest authRequest = new AuthRequest(targetWLAN);
            targetWLAN.Connect(authRequest);
            #endregion

        }
    }
}
