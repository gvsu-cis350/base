import static org.junit.Assert.assertEquals;

import java.util.ArrayList;

import static org.junit.Assert.assertThrows;


import org.junit.Test;

public class TestEscapeRoom {
    @Test
    public void test_constructor() {
        ArrayList<Room> rooms = new ArrayList<>();
        Room room1 = new Room("room1", "room1", false, false, null, null, null, null);
        Room room2 = new Room("room2", "room2", false, false, null, null, null, null);
        Room room3 = new Room("room3", "room3", false, false, null, null, null, null);
        Room room4 = new Room("room4", "room4", false, false, null, null, null, null);
        rooms.add(room1);
        rooms.add(room2);
        rooms.add(room3);
        rooms.add(room4);
        Player player = new Player(null, null, null);
        EscapeRoom escapeRoom = new EscapeRoom("escapeRoom", player, rooms);

        assertEquals("escapeRoom", escapeRoom.getName());
        assertEquals(player, escapeRoom.getPlayer());
        assertEquals(room1, escapeRoom.getMap().get(0));
        assertEquals(room2, escapeRoom.getMap().get(1));
        assertEquals(room3, escapeRoom.getMap().get(2));
        assertEquals(room4, escapeRoom.getMap().get(3));
        assertEquals(room1, escapeRoom.getPlayer().getCurrentPosition());
    }

    @Test
    public void test_getName() {
        EscapeRoom escapeRoom = new EscapeRoom("escaperoom yay!!", null, null);

        assertEquals("escaperoom yay!!", escapeRoom.getName());
    }

   @Test
   public void test_setName() {
       EscapeRoom escapeRoom = new EscapeRoom("escaperoom", null, null);

       assertEquals("escaperoom", escapeRoom.getName());

       escapeRoom.setName("new escaperoom");

       assertEquals("new escaperoom", escapeRoom.getName());

       Throwable exception = assertThrows(IllegalArgumentException.class, () -> {
           escapeRoom.setName(null);
       });
       assertEquals("setName in class EscapeRoom: null input", exception.getMessage());

       exception = assertThrows(IllegalArgumentException.class, () -> {
           escapeRoom.setName("");
       });
       assertEquals("setName in class EscapeRoom: empty string", exception.getMessage());
   }

    @Test
    public void test_getPlayer() {
        Player player = new Player(null, null, null);
        EscapeRoom escapeRoom = new EscapeRoom("escaperoom", player, null);

        assertEquals(player, escapeRoom.getPlayer());
    }

    @Test
    public void test_setPlayer() {
        Player player = new Player(null, null, null);
        EscapeRoom escapeRoom = new EscapeRoom("escaperoom", player, null);

        assertEquals(player, escapeRoom.getPlayer());

        Player player2 = new Player(null, null, null);
        escapeRoom.setPlayer(player2);

        assertEquals(player2, escapeRoom.getPlayer());
    }

    @Test
    public void test_getMap() {
        ArrayList<Room> map = new ArrayList<>();
        Room room1 = new Room("room1", "room1", false, false, null, null, null, null);
        Room room2 = new Room("room2", "room2", false, false, null, null, null, null);
        Room room3 = new Room("room3", "room3", false, false, null, null, null, null);
        map.add(room1);
        map.add(room2);
        map.add(room3);

        EscapeRoom escapeRoom = new EscapeRoom("escaperoom", null, map);

        assertEquals(room1, escapeRoom.getMap().get(0));
        assertEquals(room2, escapeRoom.getMap().get(1));
        assertEquals(room3, escapeRoom.getMap().get(2));
    }

    @Test
    public void test_setMap() {
        ArrayList<Room> map = new ArrayList<>();
        Room room1 = new Room("room1", "room1", false, false, null, null, null, null);
        Room room2 = new Room("room2", "room2", false, false, null, null, null, null);
        Room room3 = new Room("room3", "room3", false, false, null, null, null, null);
        map.add(room1);
        map.add(room2);
        map.add(room3);

        EscapeRoom escapeRoom = new EscapeRoom("escaperoom", null, null);
        escapeRoom.setMap(map);

        assertEquals(room1, escapeRoom.getMap().get(0));
        assertEquals(room2, escapeRoom.getMap().get(1));
        assertEquals(room3, escapeRoom.getMap().get(2));
    }

    @Test
    public void test_saveProgress() {
        ArrayList<String> notes = new ArrayList<>();
        notes.add("first note");
        notes.add("second note");
        notes.add("third note");

        ArrayList<Room> key1Unlocks = new ArrayList<>();
        key1Unlocks.add(new Room("room1", "room1", false, false, null, null, null, null));
        key1Unlocks.add(new Room("room2", "room2", false, false, null, null, null, null));
        Key key1 = new Key("key1", key1Unlocks);

        ArrayList<Room> key2Unlocks = new ArrayList<>();
        key2Unlocks.add(new Room("room3", "room3", false, false, null, null, null, null));
        key2Unlocks.add(new Room("room4", "room4", false, false, null, null, null, null));
        Key key2 = new Key("key2", key2Unlocks);

        ArrayList<Room> key3Unlocks = new ArrayList<>();
        key3Unlocks.add(new Room("room5", "room5", false, false, null, null, null, null));
        key3Unlocks.add(new Room("room6", "room6", false, false, null, null, null, null));
        Key key3 = new Key("key3", key3Unlocks);

        ArrayList<Key> inventory = new ArrayList<>();
        inventory.add(key1);
        inventory.add(key2);
        inventory.add(key3);

        Room room7 = new Room("room7", "room7", false, false, null, null, null, null);

        Player player = new Player(notes, inventory, room7);

        EscapeRoom escapeRoom = new EscapeRoom("escaperoom", player, null);

        Throwable exception = assertThrows(IllegalArgumentException.class, () -> {
            escapeRoom.saveProgress(null);
        });
        assertEquals("saveProgress in class EscapeRoom: null filename", exception.getMessage());

        exception = assertThrows(IllegalArgumentException.class, () -> {
            escapeRoom.saveProgress("");
        });
        assertEquals("saveProgress in class EscapeRoom: empty string", exception.getMessage());

        escapeRoom.saveProgress("progress");
    }

    @Test
    public void test_loadProgress() {
        EscapeRoom escapeRoom = new EscapeRoom("escapeRoom", null, null);
    }

    @Test
    public void test_moveRoom() {
        ArrayList<Room> map = new ArrayList<>();
        Room upstairs = new Room("Upstairs", "This is the upstairs", false, false, null, null, null, null);
        Room bedroom = new Room("Bedroom", "This is the bedroom", false, false, null, "123", null, null);
        Room bathroom = new Room("Bathroom", "This is the bathroom", true, false, null, "123", null, null);
        map.add(upstairs);
        map.add(bedroom);
        map.add(bathroom);
        upstairs.addRoom(bedroom);
        bedroom.addRoom(upstairs);
        bedroom.addRoom(bathroom);
        bathroom.addRoom(bedroom);
        EscapeRoom escapeRoom = new EscapeRoom("The house", new Player(null, null, null), map);

        assertEquals("Bedroom requires a code to enter.", escapeRoom.moveRoom("bEdroom"));

        bedroom.setCode(null);
        bedroom.setReqKey(false);
        assertEquals(null, escapeRoom.moveRoom("bedROOM"));
        assertEquals(bedroom, escapeRoom.getPlayer().getCurrentPosition());

        assertEquals("Bathroom requires a key and a code to enter.", escapeRoom.moveRoom("bathroom"));
        assertEquals(bedroom, escapeRoom.getPlayer().getCurrentPosition());

        bathroom.setCode(null);
        ArrayList<Room> unlocks = new ArrayList<>();
        unlocks.add(bathroom);
        escapeRoom.getPlayer().addToInventory(new Key("unlocks bathroom", unlocks));

        assertEquals(null, escapeRoom.moveRoom("bathroom"));
        assertEquals(bathroom, escapeRoom.getPlayer().getCurrentPosition());

        assertEquals("kitchen is not a valid name!", escapeRoom.moveRoom("kitchen"));
        assertEquals(bathroom, escapeRoom.getPlayer().getCurrentPosition());
    }

    @Test
    public void test_unlock() {
        ArrayList<Room> map = new ArrayList<>();
        Room upstairs = new Room("Upstairs", "This is the upstairs", false, false, null, null, null, null);
        Room bedroom = new Room("Bedroom", "This is the bedroom", true, false, null, "123", null, null);
        map.add(upstairs);
        map.add(bedroom);
        upstairs.addRoom(bedroom);
        bedroom.addRoom(upstairs);
        EscapeRoom escapeRoom = new EscapeRoom("The house", new Player(null, null, null), map);

        assertEquals("Bedroom requires a key and a code to enter.", escapeRoom.moveRoom("bedroom"));
        assertEquals("abc is incorrect!", escapeRoom.unlock("bedroom", "abc"));
        assertEquals("Bedroom also requires a key!", escapeRoom.unlock("bedroom", "123"));
        assertEquals(upstairs, escapeRoom.getPlayer().getCurrentPosition());

        ArrayList<Room> unlocks = new ArrayList<>();
        unlocks.add(bedroom);
        escapeRoom.getPlayer().addToInventory(new Key("unlocks bedroom", unlocks));

        assertEquals(null, escapeRoom.unlock("bedroom", "123"));
        assertEquals(bedroom, escapeRoom.getPlayer().getCurrentPosition());

        assertEquals(null, escapeRoom.unlock("bathroom", "123"));
        assertEquals(bedroom, escapeRoom.getPlayer().getCurrentPosition());
    }

    @Test
    public void test_inspectRoom() {
        ArrayList<Room> map = new ArrayList<>();
        Room upstairs = new Room("Upstairs", "This is the upstairs", false, false, null, null, null, null);
        Room bedroom = new Room("Bedroom", "This is the bedroom", false, false, null, null, null, null);
        Room bathroom = new Room("Bathroom", "This is the bathroom", true, false, null, null, null, null);
        ArrayList<Room> unlocks = new ArrayList<>();
        unlocks.add(bathroom);
        bedroom.addKey(new Key("unlocks bathroom", unlocks));
        map.add(upstairs);
        map.add(bedroom);
        map.add(bathroom);
        upstairs.addRoom(bedroom);
        bedroom.addRoom(upstairs);
        bedroom.addRoom(bathroom);
        bathroom.addRoom(bedroom);
        EscapeRoom escapeRoom = new EscapeRoom("The house", new Player(null, null, null), map);

        assertEquals("This is the upstairs", escapeRoom.inspectRoom());
        escapeRoom.moveRoom("bedroom");
        assertEquals("You found the following keys:\nunlocks bathroom\nThis is the bedroom", escapeRoom.inspectRoom());
        assertEquals("unlocks bathroom", escapeRoom.getPlayer().getInventory().get(0).getName());
        escapeRoom.moveRoom("bathroom");
        assertEquals("This is the bathroom", escapeRoom.inspectRoom());
        escapeRoom.getPlayer().setCurrentPosition(null);
        assertEquals(null, escapeRoom.inspectRoom());
    }
}