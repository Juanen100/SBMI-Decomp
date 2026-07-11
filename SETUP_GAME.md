# Opening in Unity
This project uses Unity version 4.6.9f1, which is currently delisted from Unity's download page, even though the actual download link is still available here: https://discussions.unity.com/t/early-unity-versions-downloads/927331

The game relies on extra assets that are found inside the OBB package, more specifically inside `OBB/com.mtvn.sbmigoogleplay/assets`, meaning you have to manually extract it and put it in it's corresponding folder:
- On Windows: `%userprofile%\AppData\LocalLow\Tinyfun Studios\Spongebob\Contents`
If no `Contents` folder exists, you must create it by yourself.

The game has 2 scenes, one named `startScenes` and another called `Scene0`, where `startScenes` is a loader for downloading the OBB assets and such from the Play Store and `Scene0` is the actual main game, meaning you can skip `startScenes` completely and load `Scene0` for immediate gameplay and initialization stuff like game.json creation and your personal id and such. `Scene0_backup` is indeed a backup scene of `Scene0` that I've created myself because I've come across assets not destroying properly sometimes and completely destroying how the scene loads so just keep it for the moment.

**Keep in mind that the project is far from finished and there's still a lot of stuff causing issues like the buildings in progress being under the construction area and the shaders not working properly**. There is a lot of lag when interacting with quests, characters or UI elements that trigger the game to save, which is caused by the logging made by soaring when the game is a Unity project, you can disable it in `SoaringDebug.cs` and on `static SoaringDebug()` set the following variables like this `LogToConsole = false; LogToFile = false; LogToHandler = LogToHandlerType.none;`. Some shaders are still AssetRipper's dummy shaders, however DevXDevelopment does seem to be able to extract the shaders so it's just a matter of rewritting them into Unity's proper syntax.
