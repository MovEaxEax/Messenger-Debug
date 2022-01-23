# The-Messenger-Debug-Modification
This is a modification for the game "The Messenger" to access a debug mode, where you can call methods from the game, display values such as position of the player and record/play TAS scripts. It has some more functions, which are described below (Mod is Windows-OS only).

## How to install
Download the `Assembly-CSharp.dll` from release section, or create your own build of it with the source code. After that, replace the file with the original one in your games directory. By default it's something like `"C:\Program Files (x86)\steam\steamapps\common\The Messenger\TheMessenger_Data\Managed"`

## Get Started
When ingame, toggle the Debug-Menu with `NUMPAD_0` or `F10`. From here you activate debug functions. There are three main functions:
  - Activate Details: When enabled, this option allows you to display the selected values ingame (when Debug-Menu is closed) in the top-left corner
  - Activate Modes: When enabled, you can toggle inspector mode (drag objects with mouse, colorize Collider2D of objects) and TAS mode (not included yet)
  - Activate Hotkeys: When enabled, you can use hotkeys (when Debug-Menu is closed) and also set them with functions, if you just click on the key-slots)
Anything in the Debug-Menu is navigated with the cursor and mouse-clicks, sou just click on what you want to enable or set. When clicked on a hotkey-slot, you can choose a preset function to set with. You can also manipulate and set parameters to the function in the hotkey file.

## File Structure
First time the game starts, it initialize the file structure the mod uses in the main directory, where the `TheMessenger.exe` is located. Here is a overview of all files and directories used:
  - .\xNyuDebug: Main direct for the mod
  - .\xNyuDebug\key_settings.txt: This file is for the hotkey settings. If you want to pass functions parameters, you set them in the file
  - .\xNyuDebug\TAS: Directory where the TAS scripts are located
  - .\xNyuDebug\TAS\TestScript.tmt: A TAS script you can run, just as a little showcase
  - .\xNyuDebug\dump: (Optional directory) You can dump all active GameObjects in RAM with the `Debug.DumpObjects()` command, the result will be stored in this directory
If you want to set the files to default, just delete them and rerun the game.

## Hotkeys
Like described above, you have to edit the `key_settings.txt` to pass functions parameters. After that you can press `Reload Hotkeysettings from file` in the Debug-Menu to make the changes appear.
//TODO: List all special parameters for Hotkeys

## TAS
The TAS will run your script files, frame by frame, sou you can set the inputs for each frame, as you like. I recommend to NOT write those files manually, since the syntax isn't very userfriendly yet. Use the record function or TAS mode (not implemented yet) to create scripts. TAS module contains following commands:
  - TAS.OpenConsole(): Opens a Debug console (CMD) in additional window
  - TAS.CloseConsole(): Closes the Debug console
  - TAS.ClearConsole(): Clears the text in the Debug window
  - TAS.RecordScript(): Records every input you do ingame, and saves it as .tmt script in the TAS scripts directory
  - TAS.ExecuteScript([filename]): Runs the last modified .tmt script in the TAS scripts directory. If a filename is passed as parameter, it executes this one instead
  - TAS.StopScript(): Stops palying/record TAS

##Inspector mode
In this mode you can draw Collider2D components of objects, drag objects around and see details of them. Ingame, press the checkboxes in the top right corner to make the functions active:
  - Hittable Objects: Draw colliders of every objects which can be hitted
  - Environment: Draw colliders of every object, such as ground, walls spikes
  - Zones: seems like this does nothing, yet :( Should draw spawn and trigger zones, but it doesn't
  - Mouse Inspection: When enabled, you can right click an object to open a details menu on them. Hold left click and hover on objects until you catched them, to drag them around with the cursor (This is a bit buggy for enemies and player, so make sure you hold left click until you got it)
##Performance mode
In the top left corner of the Debug-Menu, there is a checkbox for performance mode. Check it, to skips some frames of the Debug-Mode to increase performance. I recommend to only use this, if your game get's real laggy
