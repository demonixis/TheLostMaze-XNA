# The Lost Maze 

The Lost Maze is an exploration game that takes place in many mazes. You've to get all crystals to make the best score.

![Preview](https://github.com/demonixis/TheLostMaze-XNA/blob/master/Images/preview.png)

This is the XNA version adapted to work with MonoGame. Sources can contains references to Windows Phone 7, 8 and Windows Metro.

This is the version available for Windows 8.0 and Windows Phone 8.0 users (without Windows' UI APIs of course). This port is not complete and contains bugs.

## Build
The first thing is to install the [MonoGame SDK](http://www.monogame.net/downloads/), the version 3.7 (dev) is currently used but it must work with the stable version.

### First Step: Windows (DirectX)
- Open the Maze3D.Windows solution
- Select the project Maze3D.Windows as starting project
- Generate the project Maze3D.Windows

### First Step: Windws/Linux/Mac (OpenGL)
- Open the Maze3D.DesktopGL solution
- Select the project Maze3D.DesktopGL as starting project
- Generate the project Maze3D.DesktopGL

### All
- Copy the `Data` folder located in `Maze3D/Content/Data`
- Past it to `bin/Windows/x86/[Debug|Release]/Content`
- Run the game from Visual Studio like any other apps

### 3dRudder
[3dRudder](https://www.3drudder.com/) is only supported on the Maze.Windows target for now.

## License
This project is released under the MIT License.
