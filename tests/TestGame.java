import static org.junit.Assert.assertEquals;
import static org.junit.Assert.assertFalse;
import static org.junit.Assert.assertTrue;

import java.util.ArrayList;
import java.util.NoSuchElementException;

import org.junit.Test;

public class TestGame {
    @Test
    public void sample_Test(){

    }

    @Test
    public void build_Escape_Room_Test() {
        Game g = new Game();
        g.buildEscapeRoom("bin/TestFile.txt");
        ArrayList<Room> expectedArray = new ArrayList<Room>();

        Room e1 = new Room( "Bathroom", "This is a description of the bathroom", false, false, "image.png", null, null);
        Room e2 = new Room( "Library", "This is where they keep all of the books", true, false, "library.png", null, null);
        Room e3 = new Room( "Kitchen", "This is where they do all of the cooking", false, false, "kitchen.png", null, null);
        Room e4 = new Room( "Bedroom", "This is where the person sleeps", true, true, "bedroom.png", null, null);
        
        expectedArray.add(e1);
        expectedArray.add(e2);
        expectedArray.add(e3);
        expectedArray.add(e4);

        ArrayList<Room> rooms = g.getEscapeRooms();

        for( int i = 0; i < expectedArray.size(); i++ ) {
            assertEquals( rooms.get(i).getName(), expectedArray.get(i).getName());
            assertEquals( rooms.get(i).getScript(), expectedArray.get(i).getScript());
            assertEquals( rooms.get(i).getIsLocked(), expectedArray.get(i).getIsLocked());
            assertEquals( rooms.get(i).getIsEnd(), expectedArray.get(i).getIsEnd());
            assertEquals( rooms.get(i).getImage(), expectedArray.get(i).getImage());
            assertEquals( rooms.get(i).getRooms(), expectedArray.get(i).getRooms());
            assertEquals( rooms.get(i).getKeys(), expectedArray.get(i).getKeys());
        }
    }
}
