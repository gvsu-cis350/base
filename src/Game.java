import java.io.*;
import java.util.ArrayList;
import java.util.Map;
import java.util.NoSuchElementException;
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
                switch(lines.get(i).toLowerCase().substring( 0, 1 ) ) {
                    case("b"):
                        // Beginning script
                        break;
                    case("e"):
                        // Ending script
                        break;
                    case("r"): 
                    {
                        Room r = new Room();
                        r = parseRoom(lines.get(i));
                        rooms.add(r);
                        break;
                    }
                    case("k"):
                        keyStrings.add(lines.get( i ) );
                        break;
                }
            }
            addKeys( keyStrings );
        } catch(Exception e) {
            throw new NoSuchElementException("The file which has been entered cannot be utilized.");
        }
    }

    protected Room parseRoom( String line ) {
        line = line.substring( 6 );
        line = line.replace( "\n", "" );
        line = line.replace( ", ", "," );
        Room newRoom = new Room();
        
        String[] room = line.split(",");

        // Map<String, ArrayList<String>> m = new Map<String, ArrayList<String>>();

        ArrayList<String> connectedRooms = new ArrayList<String>();

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
                {
                    String[] s = room[i].split(" ");
                    for(int j = 0; j < s.length; i++ ) {
                        connectedRooms.add( s[i] );
                    }
                    break;
                }
                case 6:
                    break;
            }
        }

        // newRoom.setRooms(connectedRooms);;

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

    public ArrayList<Room> getRooms() {
        return rooms;
    }

    public ArrayList<Key> getKeys() {
        return keys;
    }

    public void saveEscapeRoom(){
        
    }
}