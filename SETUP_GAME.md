<div align="center">
    <h1>
        <img width="20%" src="https://github.com/Juanen100/SBMI-Decomp/raw/4.37.00-Release/GitHub%20Assets/Icon-152.png" style="border-radius: 50%;" align="center">
        <br>
    </h1>
    <h3>Self‑host Setup Guide</h3>
    <h4>This project is completely free from the original game's assets / intellectual property.</h4>
    <h5>None of the repo, the tool, nor the repo owner is affiliated with or sponsored by any affiliates of the original game.</h5>
    <h1></h1>
</div>

## Welcome

If you’re ready to swing into SBMI-Decomp and host your own server emulator, this guide walks you through a clean Unity-based setup.

> [!IMPORTANT]
> Now that you've got the Unity project, I'd recommend starting with the [PREREQUISITES.md](https://github.com/Juanen100/SBMI-Decomp/blob/4.37.00-Release/PREREQUISITES.md) It contains everything you need to know to set the project up correctly.

## SBMI-Decomp Remake Unity Project

The game consists of two scenes: `startScenes` and `Scene0`.

`startScenes` acts as a loader, downloading the OBB assets and other required files from the Google Play Store. `Scene0`, on the other hand, is the main game scene. This means you can skip `startScenes` entirely and load `Scene0` directly for immediate gameplay, as well as the initial setup, such as creating `game.json`, generating your personal ID, and other initialisation tasks.

`Scene0_backup` is a backup copy of `Scene0` that I created myself. I came across an issue where some assets occasionally failed to unload properly, which completely broke the scene loading process. For the time being, I'd recommend keeping it in case you need to restore the original scene.

**Keep in mind that the project is still far from finished, and there are plenty of issues that still need to be resolved**. For example, buildings that are still under construction appear beneath the construction area, and some shaders are not working correctly.

**You may also notice significant lag when interacting with quests, characters, or UI elements that trigger the game to save**. This is caused by the logging performed by Soaring when the game is running as a Unity project. If you'd like to disable it, open `SoaringDebug.cs` and, inside `static SoaringDebug()`, set the following variables:

````
 LogToConsole = false;
````
````
 LogToFile = false;
````
````
 LogToHandler = LogToHandlerType.none;
````

## Game

- If you want to play ````5.01.00```` or latest (NOT RECOMMENDED), please use the *old and improved* [SpongeBob Moves In! 4.37.00 found here](https://github.com/Juanen100/SBMI-Decomp/tree/4.37.00-Release) instead for the latest features, fixes, and improvements.

## Disclaimer
We do not claim ownership of any trademarks. All trademarks are the property of their respective owners.

# Contributors:
## Developers:
* Juanen100 (Former Project lead).
* vurzk-dev (Markut) (Current project leader).
## Asset contributors:
* Undisclosed contributo.

# Commemorations

- <a href="https://github.com/Juanen100">Juanen100</a> - Mentor.
- <a href="https://github.com/vurzk-dev">vurzk-dev (Markut)</a> - Concept contributor & the big chief.
