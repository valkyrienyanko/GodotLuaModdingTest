# Godot Modules
## About
A collection of useful modules and scripts that can easily be ported into other C# Godot games.

The goal of this project is to figure out all the tedious / not-so-obvious stuff in game development to make game development much easier.

I was thinking to myself, I want to make a bullet hell game, but I'm also going to be making more then just one game in the future, so that's why I created this project, so I don't have to redo the same things over and over again. If I want multiplayer, I can just grab it from here. If I want a modloader, I can find it here. That is my motivation behind this project.

## Modules
### [ModLoader](https://github.com/valkyrienyanko/GodotModules/blob/main/MOD_LOADER.md)  
![image](https://user-images.githubusercontent.com/6277739/162651881-b8f98aa5-da2a-4499-b4dd-737a64dec4a9.png)  

### [Options](https://github.com/valkyrienyanko/GodotModules/blob/main/OPTIONS.md)  
![image](https://user-images.githubusercontent.com/6277739/163117944-e350b70c-aaaa-426f-8719-3c28648d5747.png)  

### ENet-CSharp (wip)
Realtime client-side checks  
![image](https://user-images.githubusercontent.com/6277739/163118366-42523efa-33ab-4b0e-939f-3fba74618c83.png)  
Server browser  
![image](https://user-images.githubusercontent.com/6277739/163118505-7f47f22e-94a8-44ab-ad56-18bafd44c149.png)  
Lobby  
![image](https://user-images.githubusercontent.com/6277739/163263124-ad230a6c-6253-4c89-914c-de1a40ef32e4.png)  

Todo
- [x] Thread safety, run client on one thread, server on another
- [ ] Server list scene (wip)
- [ ] Lobby scene (wip)
- [x] Post created servers to [NodeJS web server](https://github.com/valkyrienyanko/GodotListServers) / fetch all servers
- [ ] Add packets for pinging servers
- [ ] Demo netcode for game scene (playing around with syncing player positions, and perhaps other things)

### Popup Menu (coming soon)
In games usually when you press Esc a popup menu comes up asking if you want to go back to the main menu or edit the options

### Tech Tree (coming soon)
Tech tree where nodes in tree are positioned automatically via script

## Utils
On top of all the modules there are also useful manager / utility scripts in `res://Global/` and `res://Scripts/`

```cs
// Load and play music
MusicManager.Load("menu theme", pathToMusic);
MusicManager.Play("menu theme");
MusicManager.SetVolume(50) // value ranges from 0 to 100
MusicManager.Pause();
MusicManager.Resume();

// FileManager
FileManager.GetProjectPath(); // non-exported = "res://", exported = next to the game exe
FileManager.GetConfig(path);
FileManager.WriteConfig<T>(path);
FileManager.WriteConfig(path, data);

// Utils
var someValue = 24f;
someValue.Remap(0, 100, -40, 80); //remap someValue from range 0-100 to range -40-80

// Encryption (this is not meant to be secure, it's just another annoyance to add to make mischief slightly harder)
var encryptedPassword = EncryptionHelper.Encrypt("epicPa55w0rd");
var password = EncryptionHelper.Decrypt(encryptedPassword);

// Exit and do clean up
GameManager.Exit();
```

## Contributing
See [CONTRIBUTING.md](https://github.com/valkyrienyanko/GodotLuaModdingTest/blob/main/CONTRIBUTING.md)

Have a look at all the issues marked with -> [`Good First Issue`](https://github.com/valkyrienyanko/GodotModules/issues?q=is%3Aissue+is%3Aopen+label%3A%22good+first+issue%22)

A lot of this is overwhelming at times, just making a minor contribution here and there is a huge motivator for me. Talk to me through Discord valk#9904 if you need more info on something.

Another way to contact me is by joining this [Discord](https://discord.gg/866cg8yfxZ)
