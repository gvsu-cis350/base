import static org.junit.Assert.assertEquals;
import java.util.ArrayList;
import java.util.NoSuchElementException;

import org.junit.Test;

public class TestGame extends Game {
    @Test
    public void sample_Test(){

    }

    @Test
    public void test_Contructor() {
        
    }

    @Test
    public void build_Escape_Room_Test() {
        Game g = new Game();
        g.buildEscapeRoom("bin/TestFile.txt");
        ArrayList<Room> expectedArray = new ArrayList<Room>();

        ArrayList<Room> connectedRooms1 = new ArrayList<Room>();
        ArrayList<Room> connectedRooms2 = new ArrayList<Room>();
        ArrayList<Room> connectedRooms3 = new ArrayList<Room>();
        ArrayList<Room> connectedRooms4 = new ArrayList<Room>();


        Room e1 = new Room( "Bathroom", "This is a description of the bathroom", false, null, "ABCD", null, null);
        Room e2 = new Room( "Library", "This is where they keep all of the books", true, null, "ABCD", null, null);
        Room e3 = new Room( "Kitchen", "This is where they do all of the cooking", false, null, "ABCD", null, null);
        Room e4 = new Room( "Bedroom", "This is where the person sleeps", true, null, "ABCD", null, null);
        
        connectedRooms1.add(e2);
        connectedRooms1.add(e4);
        connectedRooms1.add(e3);

        connectedRooms2.add(e1);

        connectedRooms3.add(e1);
        connectedRooms3.add(e4);
        
        connectedRooms4.add(e1);

        expectedArray.add(e1);
        expectedArray.add(e2);
        expectedArray.add(e3);
        expectedArray.add(e4);

        expectedArray.get(0).setRooms(connectedRooms1);
        expectedArray.get(1).setRooms(connectedRooms2);
        expectedArray.get(2).setRooms(connectedRooms3);
        expectedArray.get(3).setRooms(connectedRooms4);

        ArrayList<Room> rooms = g.getRooms();

        for( int i = 0; i < expectedArray.size(); i++ ) {
            assertEquals( rooms.get(i).getName(), expectedArray.get(i).getName());
            assertEquals( rooms.get(i).getScript(), expectedArray.get(i).getScript());
            assertEquals( rooms.get(i).getIsEnd(), expectedArray.get(i).getIsEnd());
            // assertEquals( rooms.get(i).getImage(), expectedArray.get(i).getImage());
            // for( int j = 0; j < rooms.size(); j++ ) {
            //     assertEquals( rooms.get(i).getRooms().get(j).getName(), expectedArray.get(i).getRooms().get(j).getName() );
            // }
            // assertEquals( rooms.get(i).getKeys(), expectedArray.get(i).getKeys());
        }
    }

    @Test
    public void parse_Room_Test() {
        Game g = new Game();
        String line = "Room: Bathroom, This is a description of the bathroom, false, image.png, ABC, Library Bedroom Kitchen, null";
        Room r = g.parseRoom( line );

        assertEquals( "Bathroom", r.getName() );
        assertEquals( "This is a description of the bathroom", r.getScript() );
        assertEquals( false, r.getIsEnd() );
        // assertEquals( "image.png", r.getImage() );
        assertEquals( "ABC", r.getCode() );
        assertEquals( null, r.getKeys() );
        assertEquals( null, r.getRooms() );
    }

    @Test
    public void add_Keys_Test() {
        Game g = new Game();
        ArrayList<String> keyRooms = new ArrayList<String>();

        g.buildEscapeRoom("bin/TestFile.txt");

        keyRooms.add("Key: Key1: Bathroom, Bedroom");

        g.addKeys( keyRooms );

        Key key = g.getKeys().get( 0 );

        System.out.println("This is the key: " + key);

        assertEquals( "Key1", key.getName() );

        System.out.println(key.getUnlocks());
        assertEquals( "Bathroom", key.getUnlocks().get( 0 ).getName() );
        assertEquals( "Bedroom", key.getUnlocks().get( 1 ).getName() );
    }

    @Test ( expected = NoSuchElementException.class )
    public void build_Escape_Room_Invalid_File_Test() {
        Game g = new Game();
        g.buildEscapeRoom("file");
    }
}
