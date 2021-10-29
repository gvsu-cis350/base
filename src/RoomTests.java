package demo.tests

import org.junit.Test;
import static org.junit.Assert.*;
import java.util.*;

public class RoomTests {

    @Test
    public void test_constructor() {
        Room room1 = new Room("bedroom", "Anna's bedroom", true, null, null);
        Room room2 = new Room("bathroom", "Anna's bathroom", false, null, null);

        String path1 = "D:/docs/files/pic.pdf";
        String path2 = "/users/docs/123.pdf";

        ArrayList<Room> rooms = new ArrayList<Room>();
        rooms.add(room1);
        rooms.add(room2);

        ArrayList<String> paths = new ArrayList<String>();
        paths.add(path1);
        paths.add(path2);

        Room room3 = new Room("house", "Anna's house", true, paths, rooms);

        assertEquals("house", room3.getName());
        assertEquals("Anna's house", room3.getScript());
        assertTrue(room3.getIsLocked());
        assertEquals("D:/docs/files/pic.pdf", room3.getImages().get(0));
        assertEquals("/users/docs/123.pdf", room3.getImages().get(1));
        assertEquals(room1, room3.getRooms().get(0));
        assertEquals(room2, room3.getRooms().get(1));
    }

    @Test
    public void test_getName() {
        Room room = new Room("doghouse", "There's a dog in here!", false, null, null);

        assertEquals("doghouse", room.getName());
    }

    @Test
    public void test_setName() {
        Room room = new Room("doghouse", "There's a dog in here!", false, null, null);
        room.setName("doghouse2");

        assertEquals("doghouse2", room.getName());
    }

    @Test
    public void test_getScript() {
        Room room = new Room("doghouse", "There's a dog in here!", false, null, null);

        assertEquals("There's a dog in here!", room.getScript());
    }

    @Test
    public void test_setScript() {
        Room room = new Room("doghouse", "There's a dog in here!", false, null, null);
        room.setScript("Where is my dog?");

        assertEquals("Where is my dog?", room.getScript());
    }

    @Test
    public void getIsLocked() {
        Room room = new Room("doghouse", "There's a dog in here!", false, null, null);

        assertFalse(room.getIsLocked());
    }

    @Test
    public void test_setIsLocked() {
        Room room = new Room("doghouse", "There's a dog in here!", false, null, null);
        room.setIsLocked(true);

        assertTrue(room.getIsLocked());
    }

    @Test
    public void test_getImages() {
        ArrayList<String> paths = new ArrayList<String>();
        paths.add("/Desktop/School/GVSU/pic.pdf");
        paths.add("Z:/users/annac/docs/123.pdf");

        Room room = new Room("room", "this is a room", false, paths, null);

        assertEquals("/Desktop/School/GVSU/pic.pdf", room.getImages().get(0));
        assertEquals("Z:/users/annac/docs/123.pdf", room.getImages().get(1));
    }

    @Test
    public void test_addImage() {
        ArrayList<String> paths = new ArrayList<String>();
        paths.add("/Desktop/1234/GVSU/pic.pdf");
        paths.add("Z:/users/annac/docs/123.pdf");
        paths.add("/Desktop/School/GVSU/\"fall 2021\"/\"CIS 350\"/GVSU-CIS350-ACK/image.pdf");
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
}
