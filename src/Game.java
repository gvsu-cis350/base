import java.io.*;
import java.util.ArrayList;
import java.util.NoSuchElementException;
import java.util.Scanner;

public class Game {
    private ArrayList<Room> rooms;
    private ArrayList<Key> keys;
    private String beginningScript;
    private String endingScript;
    private ArrayList<ArrayList<String>> roomStrings;
    private ArrayList<ArrayList<String>> keyStrings;
    
    /**
     * Default initializes all of the member variables of the 
     * Game class.
     */
    public Game() {
        rooms = new ArrayList<Room>();
        keys = new ArrayList<Key>();
        roomStrings = new ArrayList<ArrayList<String>>();
        keyStrings = new ArrayList<ArrayList<String>>();
        beginningScript = "";
        endingScript = "";
    }
    
    /**
     * This method takes the name of a file in the form of a 
     * String and parses it and populates the arraylist of 
     * rooms and the arraylist of keys in order to generate
     * a functional escape room
     * @param filename
     */
    public void buildEscapeRoom(String filename){
        try {
            Scanner sc = new Scanner(new File(filename));
            ArrayList<String> lines = new ArrayList<String>();
            ArrayList<String> k = new ArrayList<String>();

            while(sc.hasNextLine()) {
                lines.add( sc.nextLine() );
            }
            
            sc.close();

            for( int i = 0; i < lines.size(); i++ ) {
                switch(lines.get(i).toLowerCase().substring( 0, 1 ) ) {
                    case("b"):
                        beginningScript = lines.get(i);
                        break;
                    case("e"):
                        endingScript = lines.get(i);
                        break;
                    case("r"): 
                    {
                        Room r = new Room();
                        r = parseRoom(lines.get(i));
                        rooms.add(r);
                        break;
                    }
                    case("k"):
                        k.add(lines.get( i ) );
                        break;
                }
            }

            // Creates all of the keys listed
            addKeys( k );

            // At this point, all of the rooms and keys should be created
            // What I need to do is add all of the keys that I've stored the names for into my rooms

            ArrayList<Room> connectedRooms = new ArrayList<Room>();
            
            for( int i = 0; i < rooms.size(); i++ ) {
                for( int j = 0; j < roomStrings.get(i).size(); i++ ) {
                    connectedRooms.add( getRoomByName( roomStrings.get(i).get(j) ) );
                }
                rooms.get( i ).setRooms( connectedRooms );
            }

            ArrayList<Key> connectedKeys = new ArrayList<Key>();

            for( int i = 0; i < rooms.size(); i++ ) {
                for( int j = 0; j < keyStrings.get(i).size(); i++ ) {
                    connectedKeys.add( getKeyByName( keyStrings.get(i).get(j) ) );
                }
                rooms.get( i ).setKeys( connectedKeys ); 
            }

        } catch(Exception e) {
            throw new NoSuchElementException("The file which has been entered cannot be utilized.");
        }
    }

    /**
     * This is a protected helper method which parses a room line and turns
     * it into a new room object and returns it
     * @param line
     * @return the new room object
     */
    protected Room parseRoom( String line ) {
        line = line.substring( 6 );
        line = line.replace( "\n", "" );
        line = line.replace( ", ", "," );
        Room newRoom = new Room();
        ArrayList<String> newRoomString = new ArrayList<String>();
        ArrayList<String> newKeyString = new ArrayList<String>();
        
        String[] room = line.split(",");

        for( int i = 0; i < room.length; i++ ) {
            switch( i )
            {
                case 0:
                    newRoom.setName( room[ i ] );
                    break;
                case 1:
                    newRoom.setScript( room[i] );
                    break;
                case 2:
                    if( room[i].toLowerCase().equals("true") ) {
                        newRoom.setIsEnd(true);
                    } else {
                        newRoom.setIsEnd(false);
                    }
                    break;
                case 3:
                    if( room[i].toLowerCase().equals("true") ) {
                        newRoom.setReqKey(true);
                    } else {
                        newRoom.setReqKey(false);
                    }
                case 4:
                    newRoom.setImage( room[i] );
                    break;
                case 5: 
                    newRoom.setCode( room[i] );
                    break;
                case 6: 
                {
                    String[] str = room[i].split(" ");
                    for(int j = 0; j < str.length; j++ ) {
                        newRoomString.add( str[j] );
                    }
                    break;
                }
                case 7:
                {
                    String[] str = room[i].split(" ");
                    for(int j = 0; j < str.length; j++) {
                        newKeyString.add( str[j] );
                    }
                    break;
                }
            }
        }
        keyStrings.add( newKeyString );
        roomStrings.add( newRoomString );
        return newRoom;
    }

    public void addKeys( ArrayList<String> keyStrings ) {
        String name;
        ArrayList<Room> roomsKeyUnlocks = new ArrayList<Room>();
        
        for( int i = 0; i < keyStrings.size(); i++ ) {
            String line = keyStrings.get( i );

            line = line.substring( 5 );
            line.replace( " ", "" );
            
            name = line.split(":")[0];
            line = line.substring(name.length() + 1);

            String[] keyRooms = line.split(",");
            for( int j = 0; j < keyRooms.length; j++ ) {
                keyRooms[j] = keyRooms[j].replace(" ", "");
                roomsKeyUnlocks.add( getRoomByName( keyRooms[j] ) );
            }
            Key newKey = new Key( name, roomsKeyUnlocks );
            keys.add( newKey );
        }   
    }

    protected Room getRoomByName( String name ) {
        for( int i = 0; i < rooms.size(); i++ ) {
            if( rooms.get( i ).getName().toLowerCase().equals( name.toLowerCase() ) ) {
                return rooms.get( i );
            }
        }
        return null;
    }

    protected Key getKeyByName( String name ) {
        for( int i = 0; i < rooms.size(); i++ ) {
            if( keys.get( i ).getName().toLowerCase().equals( name.toLowerCase() ) ) {
                return keys.get( i );
            }
        }
        return null;
    }

    public ArrayList<Room> getRooms() {
        return rooms;
    }

    public ArrayList<Key> getKeys() {
        return keys;
    }

    /**
     * This method is just for testing so it is protected
     * @return
     */
    protected ArrayList<ArrayList<String>> getKeyStrings() {
        return keyStrings;
    }

    /**
     * This method is just for testing so it is protected
     * @return
     */
    protected ArrayList<ArrayList<String>> getRoomStrings() {
        return roomStrings;
    }

    /**
     * Returns the script for the beginning of the escape room. 
     */
    public String getBeginningScript() {
        return beginningScript;
    }

    /**
     * Returns the script for the end of the escape room
     * @return
     */
    public String getEndingScript() {
        return endingScript;
    }

    public void saveEscapeRoom(){
        
    }
}