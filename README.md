# SpongeBob Moves In! Decompilation <img align="right" width="128" height="128" src="https://github.com/Juanen100/SBMI-Decomp/blob/main/ExportedProject/Assets/Resources/textures/appicons/Icon-152.png?raw=true" alt="SBMI Icon" />

A decompilation of Spongebob Moves In! This project aims to recreate as close as possible the client for SBMI to provide a base for people to modify this game however they want to. Keep in mind that, whilst I'd like it to be as close as possible to the original client, I've added some code to make it work better with the Unity Editor because sometimes the game would softlock just by getting out of the main focus of the Game tab which is not ideal.

This project was based off the Android client, specifically version 4.37.00 since the iOS version was compiled il2cpp meaning it would require far much more work to pull off. I would've liked to use the iOS build but realistically, I think it's best to just keep it to the Android version since the project is far from finished yet.

# Opening in Unity
This project uses Unity version 4.6.9f1, which is currently delisted from Unity's download page, even though the actual download link is still available here: https://discussions.unity.com/t/early-unity-versions-downloads/927331

The game relies on extra assets that are found inside the OBB package, more specifically inside `OBB/com.mtvn.sbmigoogleplay/assets`, meaning you have to manually extract it and put it in it's corresponding folder:
- On Windows: `%userprofile%\AppData\LocalLow\Tinyfun Studios\Spongebob\Contents`
If no `Contents` folder exists, you must create it by yourself.

The game has 2 scenes, one named `startScenes` and another called `Scene0`, where `startScenes` is a loader for downloading the OBB assets and such from the Play Store and `Scene0` is the actual main game, meaning you can skip `startScenes` completely and load `Scene0` for immediate gameplay and initialization stuff like game.json creation and your personal id and such. `Scene0_backup` is indeed a backup scene of `Scene0` that I've created myself because I've come across assets not destroying properly sometimes and completely destroying how the scene loads so just keep it for the moment.

**Keep in mind that the project is far from finished and there's still a lot of stuff causing issues like the buildings in progress being under the construction area and the shaders not working properly**. Apparently shaders couldn't be decompiled with AssetRipper so I just had to guess how to recreate them purely from the name of the shader and the exposed parameters, but they're definitely not perfect. Also, there seems to be a lot of lag on my end when interacting with quests, characters or UI elements that trigger the game to save, this is probably caused due to the local save game rewriting the entirety of the json file without multithreading (which, of course, phones of the era couldn't have realistically had) but I still need to look into it.

# Contributing
If you want to contribute to this project, feel free to fork or download it and make a pull request. This being said, I will only accept pull requests that help improve the overall feeling of the game to get it working similarly to the original client, once the project reaches a point of *completion* I won't accept any more pull requests.