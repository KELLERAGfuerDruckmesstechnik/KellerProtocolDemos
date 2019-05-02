# KellerProtocolDemos
The KellerProtocol dll compiled in .NETStandard 2.0 and used in a WPF and a UWP demo

The KellerProtocol.cs is a C# implementation of the KELLER Bus protocol that can be found here:  
http://www.keller-druck.ch/home_g/paprod_g/progrcode_g.asp

Both, the WPF and the UWP example show how the F48 command (device initialization) and the command 73 (get a value from the device) from the KellerProtocol.cs can be used.

# General Prerequirement:
- A KELLER device is needed that can communicate on a Serial Port
- ...with a USB-COM converter cable (eg.K114 series) AND the needed FTDI driver
- ...or directly with cable of the K-10X series
- .NET Standard 2.0
- System.IO.Ports from nuget

# UWP Prerequirement:
- Tested on a  Windows 10 OS (Version 10.0.17134 Build 17134)
- Tested with Visual Studio 2019 Preview v2 plus needed "Universal Windows Platform development" components
- Concurrently, it uses Target version "Windows 10, version 1803 (Build 17134)"
- It also runs on VS 2017 and it should be able to run on older Target versions, too. (-> Project Configuration -> Application -> Targeting)

# WPF example:
![alt text](https://github.com/KELLERAGfuerDruckmesstechnik/KellerProtocolDemos/blob/master/KellerProtocolWpfDemoExampleScreen.png "WPF example main screen")

# UWP example:
![alt text](https://github.com/KELLERAGfuerDruckmesstechnik/KellerProtocolDemos/blob/master/KellerProtocolUwpDemoExampleScreen.png "UWP example main screen")

# License
The SW is free to use but KELLER will not take any responsibility for errors or liability of any kind as stated in http://www.keller-druck.ch/picts/pdf/misc/KELLER%20Software%20Disclaimer.pdf
