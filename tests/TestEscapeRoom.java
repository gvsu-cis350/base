import static org.junit.Assert.assertEquals;
import static org.junit.Assert.assertFalse;
import static org.junit.Assert.assertTrue;

import java.util.ArrayList;

import static org.junit.Assert.assertThrows;


import org.junit.Test;

public class TestEscapeRoom {
    @Test
    public void test_constructor() {
        ArrayList<Room> rooms = new ArrayList<>();
        Room room1 = new Room("room1", "room1", false, false, null, null, null);
        Room room2 = new Room("room2", "room2", false, false, null, null, null);
        Room room3 = new Room("room3", "room3", false, false, null, null, null);
        Room room4 = new Room("room4", "room4", false, false, null, null, null);
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
        Room room1 = new Room("room1", "room1", false, false, null, null, null);
        Room room2 = new Room("room2", "room2", false, false, null, null, null);
        Room room3 = new Room("room3", "room3", false, false, null, null, null);
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
        Room room1 = new Room("room1", "room1", false, false, null, null, null);
        Room room2 = new Room("room2", "room2", false, false, null, null, null);
        Room room3 = new Room("room3", "room3", false, false, null, null, null);
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
        key1Unlocks.add(new Room("room1", "room1", false, false, null, null, null));
        key1Unlocks.add(new Room("room2", "room2", false, false, null, null, null));
        Key key1 = new Key("key1", key1Unlocks);

        ArrayList<Room> key2Unlocks = new ArrayList<>();
        key1Unlocks.add(new Room("room3", "room1", false, false, null, null, null));
        key1Unlocks.add(new Room("room4", "room2", false, false, null, null, null));
        Key key2 = new Key("key2", key1Unlocks);

        ArrayList<Key> inventory = new ArrayList<>();
        inventory.add(key1);
        inventory.add(key2);

        ArrayList<Room> map = new ArrayList<>();
        Room currentPos = new Room("current position", "current position", false, false, null, null, null);
        map.add(currentPos);
        
        Player player = new Player(notes, inventory, currentPos);

        EscapeRoom escapeRoom = new EscapeRoom("escaperoom", player, null);

        escapeRoom.saveProgress("progress");
    }

    @Test
    public void test_loadProgress() {

    }

    // @Test
    // public void test_searchMap() {
    //     ArrayList<Room> rooms = new ArrayList<>();
    //     Room room1 = new Room("This is room 1!!!", "room1", false, false, null, null, null);
    //     Room room2 = new Room("room2 is the coolest room", "room2", false, false, null, null, null);
    //     Room room3 = new Room("!!ROOM 2", "room3", false, false, null, null, null);
    //     rooms.add(room1);
    //     rooms.add(room2);
    //     rooms.add(room3);
    //     EscapeRoom escapeRoom = new EscapeRoom("escapeRoom", new Player(null, null, null), rooms);

    //     assertEquals(room1, escapeRoom.searchMap("This is room 1!!!"));
    //     assertEquals(room2, escapeRoom.searchMap("room2 is the coolest room"));
    //     assertEquals(room3, escapeRoom.searchMap("!!ROOM 2"));
    //     assertEquals(null, escapeRoom.searchMap("this room doesn't exist in the map"));
    // }

    @Test
    public void test_moveRoom() {
        ArrayList<Room> map = new ArrayList<>();
        Room upstairs = new Room("Upstairs", "This is the upstairs", false, false, null, null, null);
        Room bedroom = new Room("Bedroom", "This is the bedroom", false, false, null, null, null);
        Room bathroom = new Room("Bathroom", "This is the bathroom", true, false, null, null, null);
        map.add(upstairs);
        map.add(bedroom);
        map.add(bathroom);
        upstairs.addRoom(bedroom);
        bedroom.addRoom(upstairs);
        bedroom.addRoom(bathroom);
        bathroom.addRoom(bedroom);
        EscapeRoom escapeRoom = new EscapeRoom("The house", new Player(null, null, null), map);

        assertEquals(null, escapeRoom.moveRoom("bEdroom"));
        assertEquals(bedroom, escapeRoom.getPlayer().getCurrentPosition());
        assertEquals("Bathroom is locked!", escapeRoom.moveRoom("bathroom"));
        assertEquals(bedroom, escapeRoom.getPlayer().getCurrentPosition());

        ArrayList<Room> unlocks = new ArrayList<>();
        unlocks.add(bathroom);
        escapeRoom.getPlayer().addToInventory(new Key("unlocks bathroom", unlocks));

        assertEquals(null, escapeRoom.moveRoom("bathroom"));
        assertEquals(bathroom, escapeRoom.getPlayer().getCurrentPosition());

        assertEquals("kitchen is not a valid name!", escapeRoom.moveRoom("kitchen"));
        assertEquals(bathroom, escapeRoom.getPlayer().getCurrentPosition());
    }

    @Test
    public void test_inspectRoom() {
        ArrayList<Room> map = new ArrayList<>();
        Room upstairs = new Room("Upstairs", "This is the upstairs", false, false, null, null, null);
        Room bedroom = new Room("Bedroom", "This is the bedroom", false, false, null, null, null);
        Room bathroom = new Room("Bathroom", "This is the bathroom", true, false, null, null, null);
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
