#Unoffical RoomAlive Toolkit Unity Intergration README

This project intergrates the RoomAlive Toolkit created by Microsoft Research. It uses a modified version of the toolkit which can be found [here](https://github.com/Superdroidz/RoomAliveToolkitConsole) which is required to work. It allows developers to calibrate, scan and import the room object file all within Unity.

This project is still in development and therefore this README will be updated with any changes that affect the use of the plugin.

## Authors
This was a joint effort of the following team members :

* [Adam Gestwa] (https://github.com/Superdroidz)
* [Anton Alenov] (https://github.com/ABoxOfFoxes)
* Alan Han
* Louis Chan 
* Ben Shuttlework
* James Baldock

## Prerequisites
* Visual Studio 2015 Community Edition (or better)
* Kinect for Windows v2 SDK
* Unity V5.3.1 (or higher)

As mentioned in the  [RoomAlive Toolkit](https://github.com/Kinect/RoomAliveToolkit), the project uses SharpDX and Math.NET Numerics packages. These will be downloaded and installed automatically via NuGet when RoomAlive Toolkit is built.

The 'Shaders' project requires Visual C++. Note that in Visual Studio 2015, Visual C++ is not installed by default. You may be prompted to install the necessary components when building the 'Shaders' project of the RoomAlive Toolkit.

## Installation and Setup

###Build RoomAlive Toolkit
Before proceeding with Unity, the Toolkit needs to be built. This can be done by opening up the the project solution in Visual Studios and building the project.


**Easy**
The Unity Intergration can be easily be imported via the included Unity Package file.

Simply import the package via the Assets/Import Package/Custom Package menu item.

Note: Importing via this method will overwrite any files that share the same name and are in the same position in the file structure.

**Manual**
Simply clone or download the zip folder of the project. You are then free to place the files as you wish into your Unity Project.

Note: All files for this intergration must remain under the Editor folder, this is a requirement of Unity.
