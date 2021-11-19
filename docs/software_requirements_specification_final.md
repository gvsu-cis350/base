# Overview
This document details the software requirements specification for our project. Functional and non-functional requirements are organized by feature.

# Software Requirements

# Functional Requirements

1. Character Customization
   1. The game shall assign the player with an appearance when the player clicks a portrait when customizing their character.
   2. The player shall input a custom name for their character when prompted by the game.
   3. The player shall choose their character's personal pronouns when prompted by the game.
2. Progress Tracking
   1. The game shall create a save file when the player clicks “save”.
   2. The game shall load a previous save file when the player clicks "load" and a specific save file to be loaded.
   3. The player shall have 54 available save slots to save their progress in.
   4. The game shall allow the player to view the dialogue history by clicking “History”.
   5. The game shall offer quick save functionality.
3. Story Interaction
   1. The game shall display a new scene or line of dialogue when the player clicks the screen.
   2. The player shall earn favor points with a romanceable character upon choosing a specific dialogue option.
   3. The game shall provide response options to interact with romanceable characters.
   4. The player shall choose one dialogue option from several dialogue options when prompted by the game.
   5. Game dialogue shall be influenced by dialogue options chosen by the player.
4. Story Branching
   1. The game shall branch into different storylines according to chosen dialogue options.
   2. The player shall unlock the good ending of a romanceable character when the player has enough favor points with the character.
   3. The game shall commence the bad ending if not enough favor points were earned for any romanceable character.
   4. The game shall contain four "routes", one for each romanceable character.
   5. The game shall commence the character route for whichever romanceable character has the highest number of favor points earned.

# Non-Functional Requirements

1. Performance
    1. The game shall load a visual within 1 second of it being prompted by the player.
    2. A new line of dialogue shall appear within 1 second of the player clicking or pressing the dialogue box, dialogue option, or space bar on the keyboard.
    3. The destination area that the player clicks on the interactive world map shall load the appropriate scene within 2 seconds.
    4. The game shall load within forty seconds of startup.
    5. The game shall save progress within twenty seconds of clicking "Save".
    6. The game shall load chat history within 5 seconds of the "History" button being clicked.
    7. The custom animation that plays after a "favorable" dialogue option is chosen will last no longer than 2 seconds.
2. Story Interaction
    1. The content of response dialogue shall be relevant to responses chosen by the player.
    2. Romanceable characters shall have unique responses to player chosen dialogue options.
3. Character Customization
    1. The player shall have the options of “she/her”, “he/him”, or “they/them” as personal pronouns to choose from when prompted.
    2. The game shall update the player’s stats based on answers to the game’s personality questionnaire.
4. Presentation
    1. A custom animation shall display when the player selects a "favorable" dialogue option.
    2. Text buttons shall be readable (i.e. have light text on a dark background).
    3. Clickable buttons shall be centered in the middle of the game window.
    4. Image buttons shall display a different image when hovered over with the mouse.
    5. Music shall play during various game scenes.
    6. The game shall load different visual scenes for romanceable character interactions.
5. Images
   1. Game characters shall have unique character sprites.
   2. Character images shall be sized appropriately to fit the game window.
   3. Various background images shall display during different game scenes.