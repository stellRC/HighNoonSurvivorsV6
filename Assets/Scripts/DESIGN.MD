# HighNoonSurvivors

## Game Loop

The game loop is fairly straightforward. Ultimately, the player has to stay alive 6 in-game hours to win, and the special skills unlocked are extremely helpful for doing so. The player completes objectives to unlock skills on the skill wheel. 

| Objectives | Unlocked Skill |
| -------- | ------- |
| Slay 5 brawlers | Increased attack range |
| Slay 10 gunmen | Lightning bolt |
| Destroy 15 projectiles | Death fog |
| Survive past noon | Invincibility |

## Time
Movement of in-game time is physically represented by the softly glowing clock in the center of the game view. Time is calculated based on the value I set for secondsPerGameDay. One possible upgrade I might add in the future is the ability to temporarily slow down or freeze time. 

## Skill Tree & Objectives

### Objectives

Objectives are stored within a public dictionary. Each key represents the player requirement needed to unlock a new skill. The value is a bool. Is the objective met? True if yes; false if not yet. The player requirements change depending on the level data, which is stored inside of a scriptable object. 
The following values can be found in the levelData objects. 

|| Test Level | Level One |
| --------| -------- | ------- |
| Max Brawler Count | 1 | 5|
| Max Gunman Count| 1| 10|
| Max Projectile Count|1| 15|
| Max Hour Count|1| 15|

After each play session, the objectives are updated to reflect whether or not these goals have been met. Each time an objective is met, whether in easy or normal mode, the objective is instantiated with a strikethrough.

### Skills
This is a fairly simple skill tree. Currently the branches aren't connected, meaning there is no direct linear progression to the unlocks. Although, it is technically impossible to access the death fog without unlocking either the lightning bolt or the increased attack range since the normal attack doesn't check for projectile collision. In theory as I add more levels, I will be able to build out the tree. For example, incrementing the attack range further, adding more bolts of lightning, etc.

The user tests whether or not a skill is unlocked by selecting buttons on the skill wheel. The buttons stay visually 'locked' until their corresponding objective is met.

## Canvas 

When any UI element changes take place, the entire canvas need to then be redrawn and recalculated. To avoid this, I decided to separate out my UI elements across seven different canvases. UI navigation is controlled via script in the MainNavigation class. This class could definitely do with a little bit of refactoring. I originally had a single method for deactivating all canvas objects. As the project grew and trouble ensued, typing out each SetActive() became extremely helpful.

I also put all of the canvas objects into an array I created in the class PersistentObjects. Doing so keeps the game objects from being destroyed when the scene changes. 

## Scene Changing
I use async to load individual game scenes. This helps with avoiding the player experiencing game freezes that can occur due to scene transitions. I use a slider to show loading progress. 


## Coroutine
I use coroutines more often than I probably should as they can be quite expensive. I use them for movement checks, scene loading, etc.

## Collision Detection
I use box colliders with activated triggers for enemy-player collision detection. When a trigger is activated, the triggering gameobject is checked if they can one receive damage (via the IDODAMAGE interface) and if they are the target of the collider. If both of these conditions are true, either the player dies and the game is over, or the enemy/projectile returns to their particular Object pool. Right now both the player and all the enemies die after one hit. I have a health check and health data-stat on their scriptable objects in case I decide to add health or revival as a future power-up.

## DATA

### Data Persistence 
I create a single instance of the gameManager to ensure persistent data across game scenes. 

## Object Pooling
I use object pooling to improve game performance. In doing so, I create and reuse objects so as to circumvent the need for creating and destroying a new projectile or a new enemy every time they are instantiated. I limit the size of each pool with a unique max value.

I have pools for: 

- sound effects
- specific enemies e.g. brawlers, gunmen, and rollers
- projectiles

## Camera & Lighting

### Cinemachine Virtual Camera
The camera follows a point slightly in front of the player so that the player is never center screen. I also use a confiner 2d to define the interior boundary to prevent the camera from moving beyond the designated game space and showing the map 'void'.

[Reference: Cinemachine](https://unity.com/features/cinemachine)

### Camera Shake
The camera does a little dance when a special skill is used. This was done by adding a cinemachine impulse source to the player object. 

## VFX & Particles

### Fog
I use three layers of fog. The first two layers are variations on a material I created using shader graph effects. The third is made up of collision detecting particles I created with a VFX shader graph. A sphere is drawn around the player so that when the fog clears when it collides with it. 

### Dust
There are two layers of dust, each with slightly different settings and on different z-axises to bring about an illusion of depth.

### Death Particles
After an enemy dies via collision, they undergo ParticleDeathAnim. On exiting their animation state, the particle component attached the game object starts emitting in bursts. 


## User Interface

### Display Settings

While the resolution dropdown currently only displays common sizes (that work for both HNS and the user), this can be later updated to include all user valid resolutions. 

[Reference: Steam Hardware & Software Survey](https://store.steampowered.com/hwsurvey/)

### Audio Settings

Users can adjust volume settings for sound effects, music, or both with the master slider. These levels are interpreted linearly so that changes in volume sound more natural.  

All of the audio files for the SFX were free from the Unity asset store. 

### Typography

I chose a Google font called *[Slackey](https://fonts.google.com/specimen/Slackey/about)* for its resemblance to the font used in the title sequence below. 

## Art

### Film Inspiration  

I was inspired by the [title sequence](https://www.youtube.com/watch?v=rnSU_qq7owA) to Sergio Leone's A Fistful of Dollars (1964) with its solid colors, soft edges, and sudden jolting sounds. 

### Sprites

I used sprite and animations created by [Dead Revolver](https://deadrevolver.thousand-pixel.com/) for both the player character and the enemies.

## Animation

### LeanTween & doTween
A useful tool for tweening aka filling in motion between two points. I use this for animating UI and player movement. 

[Source: LeanTween](https://assetstore.unity.com/packages/tools/animation/leantween-3595)




