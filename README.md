# High Noon Survivors
## *A game written in C# using the Unity platform*

### Submission Overview
I submitted this project in a folder titled `StellaArce_CS50_FinalProject`. Inside that folder are two more:

- `HighNoonSurvivorsBuild`
	- Contains the actual game
- `ProjectCode`
	- Contains the raw code I wrote for the game

### Launching the Game
Open the `HighNoonSurvivorsBuild` folder. Inside you will see an executable file called `HighNoonSurvivorsV6.exe`.  Double click it to launch the game.

***NB***: I built this on an Windows computer and tested it on two different Windows machines. Unfortunately, I don't have access to any Apple hardware, so while I did try to make sure it would run on MacOS, I'm not 100% confident. If you try to run it on Mac and run into issues, try it on Windows.
### Playing the Game

#### Menu Screen

Once the game launches you'll arrive at the menu screen where you'll have 4 options.
- Start
- Settings
- About
- Quit

Let's go through them one by one.

#### Start
When you hit start, you'll be taken to a screen that shows you what your objectives in the game are, as well as what skills you have unlocked so far. Once you have completed objectives, you can unlock special abilities by clicking on the question marks on the wheel. Click a power-up again to highlight it and select it for battle!

***How to Play*:**
- Each objective completed unlocks one special "power up" on the wheel to the right.
- You use the WASD keys or the arrow keys to control your character's movement.
- Press the spacebar while moving to dash in intended direction
- Press the spacebar while idling to dash upwards
- You use the mouse to control your attacks.
	- Left mouse click = Normal attack
	- Right mouse click = Special attack
- A power-up bar will appear below your character when the game begins. As you defeat enemies, this bar will fill up. Once it is full, you can use your right mouse click to execute your special attack.
- If an enemy, or an enemy's projectile reaches you, you lose.
- If you survive until noon a final wave of enemies swarms you—survive as long as you can!
- The clock in the middle of the playfield tells you what the in-game time is!

***Enemy types*:**
- Brawlers
	- These guys attack with their bodies
- Gunmen
	- These guys stand at a distance and shoot at you
- Rollers
    - These guys roll really quickly to a random position before imploding 

***Special attacks*:**
- Lightning Bolt
	- Connects a bolt of lightning from the player to the clocktower. Anything the lightning touches is defeated
-  Death Fog
	- A dark fog crawls onto the screen, destroying any enemy it touches
- Invincibility
- Increased Attack Range

While playing, you can press Esc, or the setting icon in the top left corner to pause the game. While paused you have the option to:
- Resume the game
- Return to the main menu
- Access the settings

When you die, you can see your game stats:
- Projectiles hit
- Enemies defeated 
- Time survived

Pressing the restart button will take you back to the objectives page.

#### Settings
- Resolution
	- Should be set automatically for your monitor, but can be adjusted if possible
- Windowed
	- Currently not working, but will allow you toggle between full screen and windowed mode
- Master
	- Adjust total volume
- Music
	- Adjust just the music volume
- SFX
	- Adjust just the sound effects volume
- Controls
	- Currently just explains the controls. In the future, I'd like to make it so you can also set your own controls here.

#### About 
Just says who made the game (me!) and will take you to the GitHub repo for the game if you click my name.

#### Quit
Closes the game software.