# How to Create a Custom Escape Room
In order to create an escape room, users must create a .txt file in a specific format. It is very important that users use the exact formatting required when creating an escape room because the program does not have much handling for incorrect formatting. It is the responsibility of the user to make sure they create the escape room file properly. Please refer to the end of this file for an example. 
## Escape Room Elements Descriptions
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
        * Connected Rooms. Please enter the names of the rooms that can be accessed from within this room. Separate each name with a comma so that the program can distinguish between them. 
        * Keys. Please enter the names of the keys that are within the room, separate each name with a space so that the program can distinguish between them. 
    * Key. These lines will start with "Key: ". Then you will enter the name of the key, followed by another ": " and a list of names of the rooms that that key unlocks, separated by commas.
    * Map. These lines will start with "Map: " and then will have the filepath to a file that contains the image you would like displayed as the overall map for your escape room. 


## Example Escape Room
```
Beginning: This is a description of the beginning of the escape room. It will describe where I am and how I came to be unfortunately trapped there.
End: This is a description of the end. Whew! Glad I got out of that place...
Room: Bathroom|This is a description of the bathroom|false|false|/image.png|ABC|Library Bedroom Kitchen|Key1 Key2|
Room: Library|This is where they keep all of the books|true|false|/image.png|ABC|Library Bedroom Kitchen|Key1 Key2|
Room: Kitchen|This is where they do all of the cooking|false|false|/image.png|ABC|Library Bedroom Kitchen|Key1 Key2|
Room: Bedroom|This is where the person sleeps|true|true|/image.png|ABC|Library Bedroom Kitchen|Key1 Key2|
Key: Key1: Bathroom| Bedroom
Key: Key2: Kitchen
Map: /image.png
```

## Some Important Considerations
* Please note that each line must start with one of the listed keys for the elements and nothing may contain any "\n" newline characters.
* You must follow these instructions exactly in order to create an effective escape room.
* There must not be any blank lines in your file. 
* Make sure you have the spacing/punctuation in your file exactly the same as you would like it to show up in your escape room. 
* Please note that all line starting keywords must have a colon and a space before you write any other elements. 