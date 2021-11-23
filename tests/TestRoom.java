import static org.junit.Assert.assertEquals;
import static org.junit.Assert.assertFalse;
import static org.junit.Assert.assertTrue;
import static org.junit.Assert.assertThrows;

import org.junit.Test;

public class TestRoom {

    @Test
    public void test_constructor() {
        Room room = new Room("room", "room", true, false, "/image.png", null, null, null);
        Room room1 = new Room("room1", "room1", false, false, null, "12345", null, null);
        Room room2 = new Room("room2", "room2", false, false, null, "abcde", null, null);
        Key key1 = new Key("key1", null);
        Key key2 = new Key("key2", null);

        room.addRoom(room1);
        room.addRoom(room2);
        room.addKey(key1);
        room.addKey(key2);

        assertEquals("room", room.getName());
        assertEquals("room", room.getScript());
        assertTrue(room.getReqKey());
        assertFalse(room.getIsEnd());
        assertEquals("/image.png", room.getImage());
        assertEquals("12345", room1.getCode());
        assertEquals("abcde", room2.getCode());
        assertEquals(room1, room.getRooms().get(0));
        assertEquals(room2, room.getRooms().get(1));
        assertEquals(key1, room.getKeys().get(0));
        assertEquals(key2, room.getKeys().get(1));
    }

    @Test
    public void test_getName() {
        Room room = new Room("This is a room", "room", false, false, null, null, null, null);

        assertEquals("This is a room", room.getName());
    }

    @Test
    public void test_setName() {
        Room room = new Room("This is a room", "room", false, false, null, null, null, null);
        
        assertEquals("This is a room", room.getName());

        Throwable exception = assertThrows(IllegalArgumentException.class, () -> {
            room.setName(null);
        });
        assertEquals("setName in class Room: null input", exception.getMessage());

        exception = assertThrows(IllegalArgumentException.class, () -> {
            room.setName("");
        });
        assertEquals("setName in class Room: empty string", exception.getMessage());

    }

    @Test
    public void test_getScript() {
        Room room = new Room("room", "This is a room!", false, false, null, null, null, null);

        assertEquals("This is a room!", room.getScript());
    }

    @Test
    public void test_setScript() {
        Room room = new Room("room", "This is a room!", false, false, null, null, null, null);

        assertEquals("This is a room!", room.getScript());

        room.setScript("");

        assertEquals("", room.getScript());

        room.setScript(null);
        
        assertEquals(null, room.getScript());
    }

    @Test
    public void test_getIsLocked() {
        Room room = new Room("room", "room", false, false, null, null, null, null);

        assertFalse(room.getReqKey());
    }

    @Test
    public void test_setIsLocked() {
        Room room = new Room("room", "room", false, false, null, null, null, null);
        room.setReqKey(true);

        assertTrue(room.getReqKey());
    }

    @Test
    public void test_getIsEnd() {
        Room room = new Room("room", "room", false, false, null, null, null, null);

        assertFalse(room.getIsEnd());
    }

    @Test
    public void test_setIsEnd() {
        Room room = new Room("room", "room", false, false, null, null, null, null);
        room.setIsEnd(true);

        assertTrue(room.getIsEnd());
    }

    @Test
    public void test_getImages() {
        String path1 = "/Desktop/School/GVSU/pic.png";
        String path2 = "Z:/users/annac/docs/123.png";

        Room room = new Room("room", "room", false, false, path1, null, null, null);

        assertEquals("/Desktop/School/GVSU/pic.png", room.getImage());

        room.setImage(path2);
        assertEquals("Z:/users/annac/docs/123.png", room.getImage());
    }

    @Test
    public void test_setImage() {
        Room room = new Room("room", "this is a room", false, false, null, null, null, null);
        room.setImage("/Desktop/1234/GVSU/pic.png");
        room.setImage("Z:/users/annac/docs/123.png");
        room.setImage("/Desktop/School/GVSU/\"fall 2021\"/\"CIS 350\"/GVSU_CIS350-ACK/image.png");
        room.setImage("c:/folder1/\"folder 2-_\"/___file---.png");

        assertEquals("image not found", room.setImage(""));
        assertEquals("image not found", room.setImage(null));

        Throwable exception = assertThrows(IllegalArgumentException.class, () -> {
            room.setImage("Mult:/image.png");
        });
        assertEquals("setImage in class Room: invalid file path", exception.getMessage());

        exception = assertThrows(IllegalArgumentException.class, () -> {
            room.setImage("/image.pdf");
        });
        assertEquals("setImage in class Room: invalid file path", exception.getMessage());

        exception = assertThrows(IllegalArgumentException.class, () -> {
            room.setImage("/image/!/image.png");
        });
        assertEquals("setImage in class Room: invalid file path", exception.getMessage());

        exception = assertThrows(IllegalArgumentException.class, () -> {
            room.setImage("/image/a folder/image.png");
        });
        assertEquals("setImage in class Room: invalid file path", exception.getMessage());
    }

    @Test
    public void test_getCode() {
        Room room = new Room("room", "room", false, false, null, "this is the code", null, null);

        assertEquals("this is the code", room.getCode());
    }

    @Test
    public void test_setCode() {
        Room room = new Room("room", "room", false, false, null, null, null, null);
        room.setCode("this is the code");

        assertEquals("this is the code", room.getCode());

        room.setCode("");

        assertEquals(null, room.getCode());
    }

    @Test
    public void test_getRooms() {
        Room room = new Room("room", "room", false, false, null, null, null, null);
        Room room1 = new Room("room1", "room1", false, false, null, null, null, null);
        Room room2 = new Room("room2", "room2", false, false, null, null, null, null);

        room.addRoom(room1);
        room.addRoom(room2);

        assertEquals(room1, room.getRooms().get(0));
        assertEquals(room2, room.getRooms().get(1));
    }

    @Test
    public void test_addRoom() {
        Room room = new Room("room", "room", false, false, null, null, null, null);
        Room room1 = new Room("room1", "room1", false, false, null, null, null, null);
        Room room2 = new Room("room2", "room2", false, false, null, null, null, null);

        room.addRoom(room1);
        room.addRoom(room2);

        assertEquals(room1, room.getRooms().get(0));
        assertEquals(room2, room.getRooms().get(1));

        Throwable exception = assertThrows(IllegalArgumentException.class, () -> {
            room.addRoom(null);
        });
        assertEquals("addRoom in class Room: null input", exception.getMessage());
    }

    @Test
    public void test_delRoom() {
        Room room = new Room("room", "room", false, false, null, null, null, null);
        Room room1 = new Room("room1", "room1", false, false, null, null, null, null);
        Room room2 = new Room("room2", "room2", false, false, null, null, null, null);

        room.addRoom(room1);
        room.addRoom(room2);

        assertEquals(room1, room.getRooms().get(0));
        assertEquals(room2, room.getRooms().get(1));

        room.delRoom(1);
        
        assertEquals(1, room.getRooms().size());

        Throwable exception = assertThrows(IndexOutOfBoundsException.class, () -> {
            room.delRoom(1);
        });
        assertEquals("delRoom in class Room: index out of bounds", exception.getMessage());
        
    }

    @Test
    public void test_getKeys() {
        Room room = new Room("room", "room", false, false, null, null, null, null);
        Key key1 = new Key("key1", null);
        Key key2 = new Key("key2", null);

        room.addKey(key1);
        room.addKey(key2);

        assertEquals(key1, room.getKeys().get(0));
        assertEquals(key2, room.getKeys().get(1));
    }

    @Test
    public void test_addKey() {
        Room room = new Room("room", "room", false, false, null, null, null, null);
        Key key1 = new Key("key1", null);
        Key key2 = new Key("key2", null);

        room.addKey(key1);
        room.addKey(key2);

        assertEquals(key1, room.getKeys().get(0));
        assertEquals(key2, room.getKeys().get(1));

        Throwable exception = assertThrows(IllegalArgumentException.class, () -> {
            room.addKey(null);
        });
        assertEquals("addKey in class Room: null input", exception.getMessage());
    }

    @Test
    public void test_delKey() {
        Room room = new Room("room", "room", false, false, null, null, null, null);
        Key key1 = new Key("key1", null);
        Key key2 = new Key("key2", null);

        room.addKey(key1);
        room.addKey(key2);

        assertEquals(key1, room.getKeys().get(0));
        assertEquals(key2, room.getKeys().get(1));

        room.delKey(1);
        
        assertEquals(1, room.getKeys().size());

        Throwable exception = assertThrows(IndexOutOfBoundsException.class, () -> {
            room.delKey(1);
        });
        assertEquals("delKey in class Room: index out of bounds", exception.getMessage());
    }
}