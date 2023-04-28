# survival-shooter-project

This is a pre-alpha survival shooter game developed using Unity 2019. The game features basic survival horor gameplay shooter. The player moves using Rigidbody with automatic weapon shooting when enemies are detected in front of the player. It also includes a dodge feature, item buffs, a score system, upgrade and equipment preparation features after each wave of attack, and AI enemies that track the player using NavMesh. The enemy behavior changes when they get close to the player to make it more challenging.

To minimize the CPU usage, the game use a small amount of Update() function. Scriptable Objects are used to manage PlayerData, Items, Equipment, and GameEvent. Controling game event by using Scriptable Object making the scripts run more independently.

The game has been built for Windows and Android platforms using URP and the New Input System.

# features:
- Basic survival gameplay
- Automatic shooting
- Dodge feature
- Item buffs
- Score system
- Upgrade and equipment preparation feature
- AI enemies using NavMesh
- CPU optimized

# limitation :
- No 3D art (not yet)
- UI design and flow are basic
- Some menus are only for samples (score menu, setting menu)
- No music

# note :
- android UI after build is too small and pause button is unresponsive
- sometime Enemy got stuck and cannot find the player position
- dodge feature in android is not consistence
