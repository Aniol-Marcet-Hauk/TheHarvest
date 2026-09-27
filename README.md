# The Harvest

## Overview

This is a roguelike / soulslike game, focused on scalable design.

It has 9 weapons, 8 enemies, 21 upgrades, 13 items, 60 rooms and more.

__The code is in `Assets/Scripts`__

https://github.com/user-attachments/assets/7dcfc62a-523d-4bde-a4c8-a7f7916da63c

<img width="518" height="296" alt="Image" src="https://github.com/user-attachments/assets/be556285-c6f9-4cb7-81bd-df58d15c58c9" />

## Requirements

- Unity Editor version:2021.3.23f1
- Render pipeline: URP (if applicable)
- Packages: listed in next section
- Platform: made for windows

## Packages unity dependencies

- cinemachine 
- textmeshpro
- probuilder
- searcher
- shadergraph
- burst

## How to run / Build

``` bash
git clone https://github.com/Aniol-Marcet-Hauk/TheHarvest.git
```
1. Open Unity Hub and add this project folder.
2. Open with the specified Unity Editor version.
3. To run in Editor: open `Assets/Scenes` choose a scene and press Play .
4. Build: File -> Build Settings -> select platform -> Build.

If it gives you errors that say object reference not set to an instance try using the __old version__ of this project:

``` bash
git clone --branch vStable https://github.com/Aniol-Marcet-Hauk/TheHarvest.git
```

## Controls

- Keyboard: WASD to move, Space to dodge, Left Mouse to attack, Right Mouse to block, f for items and interacting, and q and e to change items.

## Gameplay overview

- Kill all enemies to progress to the next room
- Randomly generated rooms
- Different ranged and melee weapons with a combo system
- items like spikes boñmbs and daggers that can be used by pressing f
- Upgrades that change the way you play: poisoning, healing, exploding attacks, freezing, etc

## Level & room generation

The room generation is a code inspired by the original binding of isaac, but it allows for any room size and shape.
It focuses on making levels interesting, meaning: 
spreading the most special rooms far apart on the  map,
allowing different difficulty of rooms determine how challenging a level will be,
allowing for loops to be created making levels feel different instead of simply corridors.

https://github.com/user-attachments/assets/59b8b40f-4b0c-4a2d-93d5-326af24deff8

## Graphs

__OOP Graph__

OOP graph (only classes no attributes or methods)

[OOP code diagram](code_diagram.md)

(ASpecificItem and ASpecificUpgrade aren't real classes they just refer to any specific Item or Upgrade)

__General Game loop__

<img width="856" height="1024" alt="Image" src="https://github.com/user-attachments/assets/d829ca38-af27-465e-9485-c7a77b2ffdc8" />



## Known issues and future improvements

- enemy path finding through traps bugged in some rooms
- save system is temporary (and bad), making the player a dontdestroyonload can create issues when transitioning scenes
- Menu's were made in one day and are increadibly basic
