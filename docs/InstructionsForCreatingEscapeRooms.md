# How to Create a Custom Escape Room
In order to create an escape room, users must create a .txt file in a specific format. It is very important that users use the exact formatting required when creating an escape room because the program does not have must handling for incorrect formatting. It is the responsibility of the user to make sure they create the escape room file properly. Please refer to the end of this file for an example. 
* An escape room is made up of several elements. These are:
    * Beginning Script. These lines will start with "Beginning: " which will set the script that is displayed at the beginning of the escape room. 
    * Ending Script. These lines will start with "End: " which will set the script that is displayed when the player escapes from the escape room. 
    * Room. These lines will start with "Room: " which will tell the program to parse the line as a room. A room is made up of several parts. They must be entered in a specific order which I will list now. Each part must be separated from the rest of the line with a pipe character "|". Please note that none of these may contain a pipe character as part of the entry.
        * Name. Enter the name of the room. 
        * Script. This is a description of the room that you want to pop up when the player enters the room. 
        * Requires Key. If this room requires a key to enter, type "true", if not type "false".
        * End. If this room is the last room in the escape room, type "true", if not type "false".
        * Image Path. Type the image path of the image that you would like displayed when the player is in this room. 
        * Code. Type the key code for the room. If there is no code for this room, type null. Please note that you cannot have a key for a room be "null" as that is not compatible with this program. 
        * Connected Rooms. Please enter the names of the rooms that can be accessed from within this room. Separate each name with a space so that the program can distinguish between them. 