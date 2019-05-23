# AutoWLAN
With this tool you can connect to a Wifi Network using a config file (at Windows Startup for example)
Currently the tool only connect to Wifi Networks that are already registered.

## How to use it?
The usage is very simple:
1. Download the ZIP file from [releases](https://github.com/VollRahm/AutoWLAN/releases) and extract it to some folder
2. Start the executable (AutoWLAN.exe) for the first time
3. Edit the config.json with a text editor of your choice, replace `SSID_HERE` by your Wifi Name (e.q. `WLAN-Home`) and set the       `Autostart` value to `true` if you want it to autostart, or `false` if you don't.
4. After that start it again and the settings will be applied. You can change it anytime you want then, but start the exe to apply it!

## How does it work?
The entire code is inside [Program.cs](https://github.com/VollRahm/AutoWLAN/blob/master/AutoWLAN/Program.cs), the code to connect is in the `connect to wifi` region
```cs
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
           
```

* The `Config.cs` class is just the config class I created for Newtonsoft.Json to (de)serialize the config file.
  It contains the SSID (AP Name) and the autostart bool
  If the autostart is true, it will be set in the `#Set/Remove Autostart` region.
* The AccessPoint and Wifi object are both classes from the [SimpleWifi Library by DigiExam](https://github.com/DigiExam/simplewifi)

## How to build?
* Clone or download the repository and use `Visual Studio` with `.NET Framework` installed to open the solution and build it. 

  ***or***
  
* Clone or download the repository and use [MSBuild](https://docs.microsoft.com/en-us/visualstudio/msbuild/msbuild?view=vs-2019) to build it. \
(Example: `msbuild AutoWLAN.sln /t:Rebuild /p:Configuration=Release /p:Platform="any cpu"`)
