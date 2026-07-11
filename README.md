<div align="center">
    <h1>
        <img width="20%" src="https://github.com/Juanen100/SBMI-Decomp/raw/4.37.00-Release/GitHub%20Assets/Icon-152.png" style="border-radius: 50%;" align="center">
        <br>
    </h1>
    <h3>SpongeBob Moves In! Decompilation</h3>
  <p>
        <a href="https://github.com/Juanen100/SBMI-Decomp/blob/4.37.00-Release/LICENSE">
            <img alt="License" src="https://img.shields.io/github/license/Juanen100/SBMI-Decomp?label=License&style=for-the-badge">
        </a>
        <a href="https://github.com/Juanen100/SBMI-Decomp/wiki">
            <img alt="Wiki" src="https://img.shields.io/badge/Wiki-View%20Docs-purple?style=for-the-badge">
        </a>
        <a href="https://github.com/Juanen100/SBMI-Decomp/blob/4.37.00-Release/README.md#opening-in-unity">
            <img alt="Setup" src="https://img.shields.io/badge/Setup-Read%20Guide-brightgreen?style=for-the-badge">
        </a>
        <a href="https://github.com/Juanen100/SBMI-Decomp/projects">
            <img alt="Wiki" src="https://img.shields.io/badge/To%20Do-View%20Projects-darkgreen?style=for-the-badge">
        </a>
        <a href="https://github.com/Juanen100/SBMI-Decomp/stargazers">
            <img alt="Stars" src="https://img.shields.io/github/stars/Juanen100/SBMI-Decomp?color=gold&style=for-the-badge">
        </a>
    </p>
    <h4>This project is a decompilation and recreation of the SpongeBob Moves In! (SBMI) client in C#. Its goal is to reproduce the original game as accurately as possible while providing a solid foundation for preservation, modding, and further development by the community.</h4>
    <h4>The inspiration for this decomp comes from Aubrey Holmes' video</h4>

[𝙸 𝚁𝚎𝚟𝚒𝚟𝚎𝚍 𝚝𝚑𝚎 𝙻𝚘𝚜𝚝 𝚂𝚙𝚘𝚗𝚐𝚎𝙱𝚘𝚋 𝙶𝚊𝚖𝚎 𝙽𝚒𝚌𝚔𝚎𝚕𝚘𝚍𝚎𝚘𝚗 𝙳𝚘𝚎𝚜𝚗'𝚝 𝚆𝚊𝚗𝚝 𝚈𝚘𝚞 𝚝𝚘 𝙿𝚕𝚊𝚢](https://www.youtube.com/watch?v=G8cIHsdifxU)
    <h5>Neither this organization nor any of its projects are affiliated with, endorsed by, or sponsored by the original game's developers, publishers, or any of their affiliates.</h4>
    <h1></h1>
</div>

> [!IMPORTANT]
> Looking to play the game? Read the new step‑by‑step setup guide: [SETUP.md](SETUP.md)

## Information

> [!NOTE]
> Found a bug or issue? Please report it through the appropriate project's issue tracker via [GitHub Issues](https://github.com/Juanen100/SBMI-Decomp/issues). Our team will review it and address it as soon as possible.

Have a question, want to report an issue, or just interested in the project? **Join our Discord community**: https://discord.gg/HwJJuFjRTM. Once you've joined, head over to the `#development-only` channel for project discussions.

## Contributing

If you'd like to contribute to the project, please read the [CONTRIBUTING.md](CONTRIBUTING.md) guide before getting started.

### Prerequisites

- You **must** provide your *own* copy of the game and its associated asset bundles.
- You **must** provide your *own* copy of the game's OBB files. Required game assets are not included with this repository and must be placed in the appropriate content directory. Please read the [SETUP.md]($null) guide before getting started.

## How to set up SpongeBob Moves In!

Please read the [developer guide here on how to set up SpongeBob Moves In!]($null).

For instructions on hosting and running a server emulator with Docker, see the step-by-step guide: [SETUP.md]($null).

If you want to play 4.37.00 or earlier (NOT RECOMMENDED), please use the *new and improved* [SpongeBob Moves In! 5.01.00 branch]($null) instead for the latest features, fixes, and improvements.

## Gameplay

The project's goal is to faithfully recreate the game as it was at the time of the targeted build.
While most features are implemented and the game is playable from start to finish, functionality or bugs may be missing.

When hosting a local server, you can access all commands by default.

## Architecture

SpongeBob Moves In! (SBMI) consists of the following components:

* A client containing the game runtime and the game's associated DLLs.
* An OBB package containing additional game assets and supporting data directories required at runtime.

### Other information

- You can use ILSpy to inspect the game's original dependencies, understand their behaviour, and analyse how they interact with the client for server-side implementation purposes.
- https://sourceforge.net/projects/ilspy.mirror

<!-- Do not delete this 🚫 --> 

# Contributors:
## Developers:
* Juanen100 (Project Founder & Leader) (Current project leader).
* vurzk-dev (Markut) (Head Developer) (Current project leader).
* Kauan1936 (SBMI: Back to Bikini Bottom, retired).
* akao (SBMI: Back to Bikini Bottom, retired).
* WellFire (SBMI: Back to Bikini Bottom, retired).
* Undisclosed contributor (SBMIA, retired).
* Undisclosed contributor (SBMI Retro, retired).
## Asset contributors:
* Juanen100.
* vurzk-dev (Markut).
* Kauan1936, retired.
* akao, retired.
* WellFire, retired.
* Undisclosed contributor, retired.
* Undisclosed contributor, retired.
# History of SpongeBob Moves In! Remakes
## SBMI: Back to Bikini Bottom (Closed down)
Earliest iteration focused on reverse engineering the game from the APK in an offline environment, attempting to reconstruct its systems and behaviour. It remained largely experimental, with no successful networking integration.
## SBMI: Again (Closed down)
An offshoot from SBMI: Back to Bikini Bottom, led by an undisclosed contributor, which aimed to recreate the game in Unity using assets extracted from the APK and OBB files. The approach proved complex and ultimately did not succeed.
## SBMI Retro (Closed down)
A parallel project that attempted to reimplement the game in Godot 4 with a different design approach, but it was eventually discontinued.
## SBMI-Decomp (SpongeBob Moves In! Decompilation, formerly SBMI-Decomp)
SBMI-Decomp initially started as a separate effort led by Juanen100, focused on reverse engineering game files from the OBB. Later, with the involvement of vurzk-dev (Markut), the project evolved under the same name into a Unity-based reconstruction effort, integrating extracted assets and iteratively improving game functionality.

### Commemorations

- <a href="https://github.com/Juanen100">Juanen100</a>
- <a href="https://github.com/vurzk-dev">vurzk-dev (Markut)</a>

Special thanks to everyone in the community whose contributions helped preserve and bring SpongeBob Moves In! back to life. ❤️

## License [![GPL v3](https://img.shields.io/badge/GPL%20v3-blue)](http://www.gnu.org/licenses/gpl-3.0)

```
SpongeBob Moves In! Decompilation preservation project.
Copyright (C) 2026 - 2027 The SpongeBob Moves In team
See the GNU General Public License <https://www.gnu.org/licenses/>.
```

# Opening in Unity
This project uses Unity version 4.6.9f1, which is currently delisted from Unity's download page, even though the actual download link is still available here: https://discussions.unity.com/t/early-unity-versions-downloads/927331

The game relies on extra assets that are found inside the OBB package, more specifically inside `OBB/com.mtvn.sbmigoogleplay/assets`, meaning you have to manually extract it and put it in it's corresponding folder:
- On Windows: `%userprofile%\AppData\LocalLow\Tinyfun Studios\Spongebob\Contents`
If no `Contents` folder exists, you must create it by yourself.

The game has 2 scenes, one named `startScenes` and another called `Scene0`, where `startScenes` is a loader for downloading the OBB assets and such from the Play Store and `Scene0` is the actual main game, meaning you can skip `startScenes` completely and load `Scene0` for immediate gameplay and initialization stuff like game.json creation and your personal id and such. `Scene0_backup` is indeed a backup scene of `Scene0` that I've created myself because I've come across assets not destroying properly sometimes and completely destroying how the scene loads so just keep it for the moment.

**Keep in mind that the project is far from finished and there's still a lot of stuff causing issues like the buildings in progress being under the construction area and the shaders not working properly**. There is a lot of lag when interacting with quests, characters or UI elements that trigger the game to save, which is caused by the logging made by soaring when the game is a Unity project, you can disable it in `SoaringDebug.cs` and on `static SoaringDebug()` set the following variables like this `LogToConsole = false; LogToFile = false; LogToHandler = LogToHandlerType.none;`. Some shaders are still AssetRipper's dummy shaders, however DevXDevelopment does seem to be able to extract the shaders so it's just a matter of rewritting them into Unity's proper syntax.
