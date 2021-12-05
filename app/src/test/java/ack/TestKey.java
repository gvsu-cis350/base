package ack;

import static org.junit.Assert.assertEquals;
import static org.junit.Assert.assertThrows;

import org.junit.Test;

public class TestKey {
    @Test
    public void test_constructor() {
        Key key = new Key("key", null);
        Room room1 = new Room("room1", "room1", false, false, null, null, null, null);
        Room room2 = new Room("room2", "room2", false, false, null, null, null, null);

        key.addRoomToUnlock(room1);
        key.addRoomToUnlock(room2);

        assertEquals("key", key.getName());
        assertEquals(room1, key.getUnlocks().get(0));
        assertEquals(room2, key.getUnlocks().get(1));
    }

    @Test
    public void test_getName() {
        Key key = new Key("key", null);

        assertEquals("key", key.getName());
    }

    @Test
    public void test_setName() {
        Key key = new Key("key", null);

        assertEquals("key", key.getName());

        key.setName("a new key name!");

        assertEquals("a new key name!", key.getName());

        Throwable exception = assertThrows(IllegalArgumentException.class, () -> {
            key.setName(null);
        });
        assertEquals("setName in class Key: null input", exception.getMessage());

        exception = assertThrows(IllegalArgumentException.class, () -> {
            key.setName("");
        });
        assertEquals("setName in class Key: empty string", exception.getMessage());
    }

    @Test
    public void test_getUnlocks() {
        Key key = new Key("key", null);
        Room room1 = new Room("room1", "room1", false, false, null, null, null, null);
        Room room2 = new Room("room2", "room2", false, false, null, null, null, null);

        key.addRoomToUnlock(room1);
        key.addRoomToUnlock(room2);

        assertEquals(room1, key.getUnlocks().get(0));
        assertEquals(room2, key.getUnlocks().get(1));
    }

    @Test
    public void test_addRoomToUnlock() {
        Key key = new Key("key", null);
        Room room1 = new Room("room1", "room1", false, false, null, null, null, null);
        Room room2 = new Room("room2", "room2", false, false, null, null, null, null);

        key.addRoomToUnlock(room1);
        key.addRoomToUnlock(room2);

        assertEquals(room1, key.getUnlocks().get(0));
        assertEquals(room2, key.getUnlocks().get(1));

        Throwable exception = assertThrows(IllegalArgumentException.class, () -> {
            key.addRoomToUnlock(null);
        });
        assertEquals("addRoomToUnlock in class Key: null input", exception.getMessage());
    }

    @Test
    public void test_delRoomToUnlock() {
        Key key = new Key("key", null);
        Room room1 = new Room("room1", "room1", false, false, null, null, null, null);
        Room room2 = new Room("room2", "room2", false, false, null, null, null, null);

        key.addRoomToUnlock(room1);
        key.addRoomToUnlock(room2);

        assertEquals(room1, key.getUnlocks().get(0));
        assertEquals(room2, key.getUnlocks().get(1));

        key.delRoomToUnlock(1);

        assertEquals(1, key.getUnlocks().size());

        Throwable exception = assertThrows(IndexOutOfBoundsException.class, () -> {
            key.delRoomToUnlock(1);
        });
        assertEquals("delRoomToUnlock in class Key: index out of bounds", exception.getMessage());
    }
}