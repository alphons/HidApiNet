#PHONY : build clean

all : run

#build : HIDProject.exe
#	@# do nothing

HIDProject.exe : Program.cs HidApiNet.HidDevice.cs HidApiNet.HidDeviceInfo.cs HidApiNet.HidDeviceInfoCollection.cs HidApiNet.Interop.cs Weather.Data.cs Weather.IContract.cs Weather.Server.cs Weather.Weather.cs
	mcs -unsafe Program.cs HidApiNet.HidDevice.cs HidApiNet.HidDeviceInfo.cs HidApiNet.HidDeviceInfoCollection.cs HidApiNet.Interop.cs Weather.Data.cs Weather.IContract.cs Weather.Server.cs Weather.Weather.cs -r:System.ServiceModel -Out:HIDProject.exe

clean :
	rm -rf HIDProject.exe

run : HIDProject.exe
	mono HIDProject.exe
