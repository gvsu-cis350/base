# Overview
These are the functional and non-functional requirements for the next few weeks for the project using the spiral agile method.

# Functional Requirements
1. Weapons
    1. Users shall be able to switch weapons through number and scroll wheel inputs.
    2. Users shall be able to hit 'R' to reload weapons.
    3. Weapons shall play sounds through a PunRPC method.
    4. Users shall use the left mouse button to shoot.
    5. Weapons shall do a set amount of damage set in a GunInfo script.
    6. Master client of a room shall be able to modify weapon availability at the start of a match.
2. Vehicles
    1. Users shall hit 'E' to enter vehicles.
    2. Users shall be able to control vehicles with 'WASD'.
    3. Maps shall offer a minimum of 2 vehicles, for multiple users able to operate.
    4. Users shall be able to shoot when driving the vehicle.
    5. Control of vehicles shall be managed by Photon Ownership.
    6. Master client shall create vehicles as Photon room objects.
3. Levels
    1. Levels will use pre-placed spawn points to determine spawn locations.
    2. There shall be only one spawn manager per level.
4. UI
    1. UI shall allow users to create server rooms with a button press.
    2. UI shall allow users to change player settings from inside the game.
    3. UI shall allow users to exit and close the game within the application.
    4. Each user shall have access to the settings menu at all times.
    5. The software’s window shall vary in size according to user settings.
    6. Users shall be able to change their names from the player settings menu.
    7. Users shall use the player settings menu to set the application in fullscreen mode.
5. General
    1. The applications shall attempt to read user config file upon start.
    2. Each user’s game shall connect to Photon Fixed Region ID “US”.
    3. The game shall support up to 20 players online simultaneously.
    4. Users shall use normative inputs, “WASD” and arrow keys, to move character around.
    5. Users shall use the mouse to change camera direction.
    6. Player scripts shall use a mixture of RPCs and Photon Events to communicate state changes and new information.
    7. Users shall use the spacebar to cause their character to jump.
    8. The application shall automatically generate a config file upon the start of the application if no config file is found.
    9. The application shall save a preferred Field of View setting for the player camera.
    10. Master client shall set match length at the start of a match.
    11. Master client shall set a score threshold at the start of a match.

# Non-Functional Requirements
1. Weapons
    1. Weapons shall have sound effects for various actions.
    2. Users shall either have access to all weapons or only 2 weapons at a time.
    3. Each weapon shall be unique in look and operation.
    4. Non-projectile based weapons shall be raycast where as soon as you fire the bullet hits.
    5. Weapon scripts shall inherit from abstract classes.
2. Vehicles
    1. Vehicles shall have distinct animations for players in vehicles.
    2. Only passengers shall effectively shoot from vehicles.
3. Levels
    1. There shall be at least 3 different maps.
    2. There shall be maps centered around a more confined environment.
    3. There shall be maps centered around a more open environment.
    4. Maps shall be playable in both team deathmatch and free for all game modes.
4. UI
    1. The menu shall be intuitive to use.
    2. The menu shall be generic and use minimalist imagery.
    3. The HUD shall show the weapon and ammo of the weapon.
    4. The HUD shall show the health of the player.
    5. The leaderboard shall show the current score of the game.
    6. The HUD in a match shall change accordance to team deathmatch and free for all game modes.
    7. The HUD shall change in accordance to match lengths.
5. General
    1. There shall be a user interface controlled with the mouse and keyboard.
    2. The final product shall have a multiplayer where users can interact together.
    3. The software shall have minimal lag and jittering.
    4. The game shall include sound effects for each individual character and weapon.
    5. Player’s weapons shall remain at a constant field of view.


