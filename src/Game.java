import java.io.*;
import java.util.ArrayList;
import java.util.Scanner;

public class Game {
    private ArrayList<Room> rooms;
    private ArrayList<Key> keys;
    
    public Game() {
        rooms = new ArrayList<Room>();
        keys = new ArrayList<Key>();
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
            ArrayList<String> keyStrings = new ArrayList<String>();

            while(sc.hasNextLine()) {
                lines.add( sc.nextLine() );
            }
            
            sc.close();

            for( int i = 0; i < lines.size(); i++ ) {
                if( lines.get( i ).toLowerCase().substring( 0, 10 ).equals( "beginning: " ) ) {
                    //set the beginning script to be this
                } else if( lines.get( i ).toLowerCase().substring( 0, 4 ).equals("end: ") ) {
                    //set the end script to be this
                } else if( lines.get( i ).toLowerCase().substring( 0, 5 ).equals( "room: " ) ) {
                    Room newRoom = parseRoom( lines.get( i ) );
                    rooms.add( newRoom );
                } else if( lines.get( i ).toLowerCase().substring( 0, 4 ).equals("key: ") ) {
                    keyStrings.add( lines.get(i) );
                } else if( lines.get(i).equals("\n") ) {
                    lines.remove( i );
                }
            }



            addKeys( keyStrings );
        } catch(Exception e) {
            System.out.println("CYMBRE THIS IS BAD");
            //Going to need to send some of information to the user interface
        }
    }

    public Room parseRoom( String line ) {
        line = line.substring( 6 );
        line = line.replace( "\n", "" );
        line = line.replace( ", ", "," );
        Room newRoom = new Room();
        
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
                    // newRoom.setImage( room[i] );
                    break;
                case 4: 
                    newRoom.setCode( room[i] );
                    break;
                case 5: 
                    break;
                case 6:
                    break;
            }
        }
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
                roomsKeyUnlocks.add(getRoomByName( keyRooms[j] ) );
            }
            Key newKey = new Key( name, roomsKeyUnlocks );
            keys.add( newKey );
        }   
    }

    private Room getRoomByName( String name ) {
        for( int i = 0; i < rooms.size(); i++ ) {
            if( rooms.get( i ).getName().toLowerCase().equals( name.toLowerCase() ) ) {
                return rooms.get( i );
            }
        }
        System.out.println("So they tried to add a room that didn't exist to the key");
        return null;
    }

    public ArrayList<Room> getRooms() {
        return rooms;
    }

    public ArrayList<Key> getKeys() {
        return keys;
    }

    public void saveEscapeRoom(){
        
    }
}
