# The Harvest

## Overview

This is a roguelike / soulslike game.

It has 9 weapons, 8 enemies, 21 "upgrades", 13 "items", 60 rooms and more.

__The code is in `Assets/Scripts`__

https://github.com/user-attachments/assets/7dcfc62a-523d-4bde-a4c8-a7f7916da63c

<img width="518" height="296" alt="Image" src="https://github.com/user-attachments/assets/be556285-c6f9-4cb7-81bd-df58d15c58c9" />

## Requirements

- Unity Editor version:2021.3.23f1
- Render pipeline: URP (if applicable)
- Packages: TextMeshPro, Cinemachine, (list others used)
- Platform: made for windows

## How to run / Build

1. Open Unity Hub and add this project folder.
2. Open with the specified Unity Editor version.
3. To run in Editor: open `Assets/Scenes` choose a scene and press Play .
4. Build: File → Build Settings → select platform → Build.

## Controls

- Keyboard: WASD to move, Space to dodge, Left Mouse to attack, Right Mouse to block, f for items and interacting

## Gameplay overview

- Kill all enemies to progress to the next room
- Randomly generated rooms
- Different ranged and melee weapons with a combo system
- items like spikes boñmbs and daggers that can be used by pressing f
- Upgrades that change the way you play: poisoning, healing, exploding attacks, freezing, etc

## Project layout

- `Assets/Scripts` — gameplay code
- `Assets/Scenes` — scenes and levels
- `Assets/AAA_ObjectPrefabs` — prefabs (weapons, enemies, rooms)
- `Assets/Graphics` — sprites, models, materials
- `Assets/Audio` — music and SFX

## Level & room generation

The room generation is a code inspired by binding of isaac, but it allows for any room size and shape. 

https://github.com/user-attachments/assets/59b8b40f-4b0c-4a2d-93d5-326af24deff8
## Graphs

__OOP Graph__

[OOP code diagram](code_diagram.md)

(ASpecificItem and ASpecificUpgrade aren't real classes they just refer to any specific Item or Upgrade)

__General Game loop__

<img width="570" height="682" alt="Image" src="https://github.com/user-attachments/assets/6c60cca1-e3c1-4db4-9c75-b7974cb4f80f" />

It is in catalan

## Packages unity/dependencies

- cinemachine 
- textmeshpro
- probuilder
- searcher
- shadergraph
- burst
- mathematics

## Known issues and future improvements

- enemy path finding through traps bugged in some rooms
- save system is temporary (and bad), making the player a dontdestroyonload can create issues when transitioning scenes
- Menu's were made in one day and are increadibly basic
