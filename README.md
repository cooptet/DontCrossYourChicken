# Don't Cross Your Chickens!

A small 3D driving game with a flock of independently moving chickens standing between you and the finish line. Reach the shop without making the chickens too angry.

> **Project type:** 4-person team project  
> **Engine:** Unity 6  
> **Project version:** `6000.6.7f1`

## Overview

In *Don't Cross Your Chickens!*, you drive a farm truck along a road from the barn at the south end of the map to the shop at the north. The road is shared by a flock of chickens: they wander around the surrounding grass and occasionally move into your path.

Your truck must stay on the road, so getting past the flock is a matter of timing and steering rather than simply driving around them. Hitting chickens costs health. Reach the shop before you run out of hearts.

## Gameplay

- Drive the truck from the barn to the shop.
- Stay within the road boundaries.
- Watch the flock: chickens move together, but their movement is not completely predictable.
- Avoid hitting chickens. The health bar has three hearts; losing all three takes you to the lose screen.
- Reach the goal to win.

### Controls

| Input | Action |
|---|---|
| `W` / `↑` | Accelerate |
| `S` / `↓` | Reverse |
| `A` / `←` | Steer left |
| `D` / `→` | Steer right |
| `Space` | Brake |

## Chicken AI: Flocking

The central technical feature is a flocking system implemented with Unity `ScriptableObject` behaviours. Instead of assigning every chicken a fixed path, each agent calculates its movement from the other agents in its local neighbourhood.

### Neighbour detection

For each chicken, `Flock` uses `Physics.OverlapSphere` with a configurable neighbour radius and layer mask to find nearby agents. The chicken's own collider is excluded from the resulting context.

### Weighted behaviours

Each behaviour is a `FlockBehavior` ScriptableObject that returns a movement vector. `CompositeBehavior` combines the configured behaviours using individual weights, allowing the flock's overall movement to be tuned in the Inspector.

The project includes behaviours for:

- **Alignment** – steers an agent toward the average forward direction of its neighbours.
- **Avoidance** – pushes an agent away from nearby flockmates inside a smaller avoidance radius.
- **Steered cohesion** – steers toward the neighbourhood's average position, smoothing the direction change with `Vector3.SmoothDamp`.
- **Stay in radius** – applies an inward steering force as an agent approaches the configured flock boundary.

The active flock prefab references a composite behaviour that combines steered cohesion, alignment, avoidance, and the stay-in-radius behaviour. The individual ScriptableObject assets and their weights can be adjusted without changing the flock manager code.

### Agent movement and obstacle response

`FlockAgent` smooths changes between its current and target velocities, rotates toward its movement direction, and applies movement through its Rigidbody in `FixedUpdate`.

When a chicken collides with the player or an object tagged `Obstacle`, `ChickenRepel` asks the agent to temporarily move away from the contact point. This briefly overrides its normal flock velocity; once the repulsion timer expires, the agent resumes following the flock behaviour.

## Truck and game logic

### Truck controller

`TruckController` handles forward and reverse speed, acceleration, coasting deceleration, braking, and steering. Steering is smoothed, and its strength is reduced at higher speeds. The controller also clamps the truck's position to the road bounds using the road collider's local coordinate space.

### Chicken collisions and health

`PlayerChickenHit` detects contact with objects tagged `Chicken`. A cooldown prevents continuous contact with one chicken from repeatedly draining health every physics frame. A valid hit reduces the health bar by one heart and plays the chicken-hit sound when the audio controller is assigned.

`HealthbarController` manages the three heart UI elements and loads the lose screen when health reaches zero. `ChickenHitFlash` provides visual feedback by pulsing the chicken's materials toward red after a player collision.

### Goal and scenes

The goal uses `WinCondition` to detect the player reaching the goal object and load the win screen. The build settings include these scenes:

1. Main Menu
2. Main Level
3. Win Screen
4. Lose Screen
5. Tutorial

The menu provides routes into the game and tutorial, while the end screens allow the player to return to the main menu.

### Audio

`FarmAudioController` creates separate audio sources for background music, ambient chicken sounds, and chicken-hit sounds. Ambient playback can be scheduled at random intervals, and the background and ambient sources pause when the game is paused.

## Development

This was a four-person team project built over a short production schedule:

- **3 days** of pre-production
- **7 days** of implementation
- **4 days** of testing and balancing

The project was built in Unity, with the gameplay systems and flocking logic implemented in C#. Third-party assets were used for selected models, textures, fonts, UI, and audio; the gameplay code was developed for the project.

## Tech stack

- Unity Editor `6000.6.7f1`
- C#
- Unity Physics and Rigidbody movement
- ScriptableObjects for modular flock behaviours
- Unity Input System package (project dependency)

## Getting started

1. Clone or download this repository.
2. Open the project using Unity Editor `6000.6.7f1`.
3. Allow Unity to resolve and import the project packages and assets.
4. Open `Assets/Scenes/Main Menu.unity`.
5. Press **Play**.

The Main Menu, Main Level, Win Screen, Lose Screen, and Tutorial scenes are already included in the project's build settings.

## Project structure

```text
Assets/
├── FlockingAlgorithm/
│   ├── Behavior Scripts/       # Modular flock behaviour implementations
│   ├── Behavior Objects/       # ScriptableObject behaviour configurations
│   └── Flock Prefab/           # Flock manager prefab
├── Game Logic/
│   └── Game Logic Scripts/     # Health and win-condition logic
├── Prefabs/                    # Player, chicken, and environment prefabs
├── Scenes/                     # Menu, gameplay, tutorial, and result scenes
└── Scripts/                    # Truck, collision, audio, and menu scripts
```

## Assets and acknowledgements

The project uses the following third-party Unity Asset Store packages:

- [Animals Free Animated Low Poly 3D Models](https://assetstore.unity.com/packages/3d/characters/animals/animals-free-animated-low-poly-3d-models-260727) – chicken model
- [Low Poly Vehicle Starter Pack](https://assetstore.unity.com/packages/p/low-poly-vehicle-starter-pack-407734) – truck model
- [Pandazole Farm Ranch Low Poly Pack](https://assetstore.unity.com/packages/p/pandazole-farm-ranch-low-poly-pack-206756) – farm environment props
- [Lowpoly Textures Pack](https://assetstore.unity.com/packages/2d/textures-materials/lowpoly-textures-pack-140717) – textures
- [Hand Painted Seamless Wood Texture Vol. 6](https://assetstore.unity.com/packages/2d/textures-materials/wood/hand-painted-seamless-wood-texture-vol-6-162145) – wood texture
- [Pixel Font Tripfive](https://assetstore.unity.com/packages/2d/fonts/pixel-font-tripfive-64734) – font
- [Simple Heart Health System](https://assetstore.unity.com/packages/tools/gui/simple-heart-health-system-120676) – health UI asset

Please refer to each package's Asset Store page for its licence and usage terms.

## Screenshots

_Add screenshots to a `Screenshots/` folder and update the paths below._

| Gameplay | Main menu |
|---|---|
| `Screenshots/gameplay.png` | `Screenshots/main-menu.png` |

| Chicken hit feedback | Tutorial |
|---|---|
| `Screenshots/chicken-hit.png` | `Screenshots/tutorial.png` |
