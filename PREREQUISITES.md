# Prerequisites:

```
- You must have Unity 4.6.9f1 installed, as that's the version the game used.
```
```
- You must provide your own copy of the game and its associated asset bundles.
```
```
- You must provide your own copy of the game's OBB files. Which are to be added to the server itself on compile - ensuring this repo does not contain any copywritten code.
```

# Installation:

## Chapter One: Install Unity
To begin with, you'll need to install Unity 4.6.9f1. Although it has been delisted from the [Unity Archive](https://unity.com/releases/editor/archive), the original download link is still available in the Unity Discussions. You can [Click here!](https://discussions.unity.com/t/early-unity-versions-downloads/927331) to download it. 

## Chapter Two: Obtain required game files
Secondly, you'll need to manually extract a copy of the game and place it somewhere you can easily access. Then, navigate to the following path::
````
 OBB/com.mtvn.sbmigoogleplay/assets
````

## Chapter Three: External Downloads
Once you've completed steps one and two, make sure you've downloaded all the OBB versions we've managed to find so far. That way, you'll be able to switch between whichever versions you'd like to play. [Click here!](https://www.mediafire.com/file/ypdtkrfs6p2desw/Versiones_SpongeBob_moves_in.zip/file)

<div align="left">
    <h1>
        <img width="20%" src="https://github.com/Juanen100/SBMI-Decomp/raw/4.37.00-Release/GitHub%20Assets/screenshot-000.png" style="border-radius: 50%;" align="center">
        <br>
    </h1>
</div>

## Chapter Four: File Placement
> [!NOTE]
> I'd also like to point out that the game relies on additional assets contained within the OBB package. As a result, you'll need to extract them manually and place them in their respective folders.

Once you've completed steps two and three, copy the additional assets into the appropriate folders, as shown below:

<div align="left">
    <h1>
        <img width="40%" src="https://github.com/Juanen100/SBMI-Decomp/raw/4.37.00-Release/GitHub%20Assets/screenshot-001.jpg" style="border-radius: 50%;" align="center">
        <br>
    </h1>
</div>

- On Windows:
  ````
  %userprofile%\AppData\LocalLow\Tinyfun Studios\Spongebob\Contents
  ````

- On macOS:
  ````
  ~/Library/Application Support/Tinyfun Studios/Spongebob/Contents
  ````

- On Linux:
  ````
  ~/.config/unity3d/Tinyfun Studios/Spongebob/Contents`
  ````

Therefore If the folder `Contents` doesn't exists, **you must create it by yourself**.
