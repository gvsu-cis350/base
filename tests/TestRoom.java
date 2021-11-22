import static org.junit.Assert.assertEquals;
import static org.junit.Assert.assertFalse;
import static org.junit.Assert.assertTrue;

import java.util.ArrayList;
import java.util.NoSuchElementException;

import org.junit.Test;

public class TestRoom {

    @Test
    public void test_constructor() {
        Room room1 = new Room("bedroom", "Anna's bedroom", true, false, null, null, null);
        Room room2 = new Room("bathroom", "Anna's bathroom", false, false, null, null, null);

        Key key1 = new Key("D:/docs/files/pic.pdf", null);
        Key key2 = new Key("/users/docs/123.pdf", null);

        ArrayList<Room> rooms = new ArrayList<Room>();
        rooms.add(room1);
        rooms.add(room2);

        ArrayList<Key> keys = new ArrayList<Key>();
        keys.add(key1);
        keys.add(key2);

        Room room3 = new Room("house", "Anna's house", true, false,"file", rooms, keys);

        assertEquals("house", room3.getName());
        assertEquals("Anna's house", room3.getScript());
        assertTrue(room3.getIsLocked());
        // assertEquals("D:/docs/files/pic.pdf", room3.getImages().get(0));
        // assertEquals("/users/docs/123.pdf", room3.getImages().get(1));
        assertEquals(room1, room3.getRooms().get(0));
        assertEquals(room2, room3.getRooms().get(1));
    }

    @Test
    public void test_getName() {
        Room room = new Room("doghouse", "There's a dog in here!", false, false, null, null, null);

        assertEquals("doghouse", room.getName());
    }

    @Test
    public void test_setName() {
        Room room = new Room("doghouse", "There's a dog in here!", false, false, null, null, null);
        room.setName("doghouse2");

        assertEquals("doghouse2", room.getName());
    }

    @Test
    public void test_getScript() {
        Room room = new Room("doghouse", "There's a dog in here!", false, false, null, null, null);

        assertEquals("There's a dog in here!", room.getScript());
    }

    @Test
    public void test_setScript() {
        Room room = new Room("doghouse", "There's a dog in here!", false, false, null, null, null);
        room.setScript("Where is my dog?");

        assertEquals("Where is my dog?", room.getScript());
    }

    @Test
    public void getIsLocked() {
        Room room = new Room("doghouse", "There's a dog in here!", false, false, null, null, null);

        assertFalse(room.getIsLocked());
    }

    @Test
    public void test_setIsLocked() {
        Room room = new Room("doghouse", "There's a dog in here!", false, false, null, null, null);
        room.setIsLocked(true);

        assertTrue(room.getIsLocked());
    }

    @Test
    public void test_getImages() {
        ArrayList<String> paths = new ArrayList<String>();
        paths.add("/Desktop/School/GVSU/pic.pdf");
        paths.add("Z:/users/annac/docs/123.pdf");

        // Room room = new Room("room", "this is a room", false, false, paths, null, null);

        assertEquals("/Desktop/School/GVSU/pic.pdf", room.getImages().get(0));
        assertEquals("Z:/users/annac/docs/123.pdf", room.getImages().get(1));
    }

    @Test
    public void test_addImage() {
        Room room = new Room("room", "this is a room", false, false, null, null, null);
        // room.addImage("/Desktop/1234/GVSU/pic.pdf");
        // room.addImage("Z:/users/annac/docs/123.pdf");
        // room.addImage("/Desktop/School/GVSU/\"fall 2021\"/\"CIS 350\"/GVSU_CIS350-ACK/image.pdf");
        // room.addImage("Drive_Name123:/folder1/\"folder 2-_\"/___file---.pdf");
    }

    @Test
    public void test_delImage() {

    }

    @Test
    public void test_getRooms() {

    }

    @Test
    public void test_addRoom() {

    }

    @Test
    public void test_delRoom() {

    }

    @Test
    public void test_getKeys() {

    }

    @Test
    public void test_addKey() {

    }

    @Test
    public void test_delKey() {

    }
}