import static org.junit.Assert.assertEquals;
import static org.junit.Assert.assertThrows;

import org.junit.Test;

public class TestPlayer {
    
    @Test
    public void test_constructor() {
        Player player = new Player(null, null, null);
        String note1 = "This is a note!";
        String note2 = "This is another note!";
        Key key1 = new Key("key1", null);
        Key key2 = new Key("key2", null);
        Room room = new Room("room", "room", false, null, null, null, null);

        player.addNote(note1);
        player.addNote(note2);
        player.addToInventory(key1);
        player.addToInventory(key2);
        player.setCurrentPosition(room);

        assertEquals("This is a note!", player.getNotes().get(0));
        assertEquals("This is another note!", player.getNotes().get(1));
        assertEquals(key1, player.getInventory().get(0));
        assertEquals(key2, player.getInventory().get(1));
        assertEquals(room, player.getCurrentPosition());
    }

    @Test
    public void test_getNotes() {
        Player player = new Player(null, null, null);

        player.addNote("note1");
        player.addNote("note2");

        assertEquals("note1", player.getNotes().get(0));
        assertEquals("note2", player.getNotes().get(1));
    }

    @Test
    public void test_addNote() {
        Player player = new Player(null, null, null);

        player.addNote("note1");
        player.addNote("note2");

        assertEquals("note1", player.getNotes().get(0));
        assertEquals("note2", player.getNotes().get(1));

        player.addNote("");
        player.addNote(null);

        assertEquals(2, player.getNotes().size());
    }

    @Test
    public void test_delNote() {
        Player player = new Player(null, null, null);

        player.addNote("note1");
        player.addNote("note2");

        assertEquals("note1", player.getNotes().get(0));
        assertEquals("note2", player.getNotes().get(1));

        player.delNote(1);

        assertEquals(1, player.getNotes().size());

        assertEquals("Index out of bounds", player.delNote(1));
    }

    @Test
    public void test_getInventory() {
        Player player = new Player(null, null, null);
        Key key1 = new Key("key1", null);
        Key key2 = new Key("key2", null);

        player.addToInventory(key1);
        player.addToInventory(key2);

        assertEquals(key1, player.getInventory().get(0));
        assertEquals(key2, player.getInventory().get(1));
    }

    @Test
    public void test_addToInventory() {
        Player player = new Player(null, null, null);
        Key key1 = new Key("key1", null);
        Key key2 = new Key("key2", null);

        player.addToInventory(key1);
        player.addToInventory(key2);

        assertEquals(key1, player.getInventory().get(0));
        assertEquals(key2, player.getInventory().get(1));

        Throwable exception = assertThrows(IllegalArgumentException.class, () -> {
            player.addToInventory(null);
        });
        assertEquals("addToInventory in class Player: null input", exception.getMessage());
    }

    @Test
    public void test_delFromInventory() {
        Player player = new Player(null, null, null);
        Key key1 = new Key("key1", null);
        Key key2 = new Key("key2", null);

        player.addToInventory(key1);
        player.addToInventory(key2);

        assertEquals(key1, player.getInventory().get(0));
        assertEquals(key2, player.getInventory().get(1));

        player.delFromInventory(1);

        assertEquals(1, player.getInventory().size());

        Throwable exception = assertThrows(IndexOutOfBoundsException.class, () -> {
            player.delFromInventory(1);
        });
        assertEquals("delFromInventory in class Player: index out of bounds", exception.getMessage());
    }

    @Test
    public void test_getCurrentPosition() {
        Player player = new Player(null, null, null);
        Room room = new Room("room", "room", false, null, null, null, null);

        player.setCurrentPosition(room);

        assertEquals(room, player.getCurrentPosition());
    }

    @Test
    public void test_setCurrentPosition() {
        Player player = new Player(null, null, null);
        Room room = new Room("room", "room", false, null, null, null, null);
        Room room2 = new Room("room2", "room2", false, null, null, null, null);

        player.setCurrentPosition(room);

        assertEquals(room, player.getCurrentPosition());

        player.setCurrentPosition(room2);

        assertEquals(room2, player.getCurrentPosition());

    }
}