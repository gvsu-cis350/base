import static org.junit.Assert.assertEquals;

import java.util.ArrayList;

import static org.junit.Assert.assertThrows;


import org.junit.Test;

public class TestEscapeRoom {
    @Test
    public void test_constructor() {
        ArrayList<Room> rooms = new ArrayList<>();
        Room room1 = new Room("room1", "room1", false, null, null, null, null);
        Room room2 = new Room("room2", "room2", false, null, null, null, null);
        Room room3 = new Room("room3", "room3", false, null, null, null, null);
        Room room4 = new Room("room4", "room4", false, null, null, null, null);
        rooms.add(room1);
        rooms.add(room2);
        rooms.add(room3);
        rooms.add(room4);
        Player player = new Player(null, null, null);
        String image = "c:/this-is-an-image.png";
        EscapeRoom escapeRoom = new EscapeRoom("escapeRoom", player, image, rooms);

        assertEquals("escapeRoom", escapeRoom.getName());
        assertEquals(player, escapeRoom.getPlayer());
        assertEquals("c:/this-is-an-image.png", escapeRoom.getImage());
        assertEquals(room1, escapeRoom.getMap().get(0));
        assertEquals(room2, escapeRoom.getMap().get(1));
        assertEquals(room3, escapeRoom.getMap().get(2));
        assertEquals(room4, escapeRoom.getMap().get(3));
        assertEquals(room1, escapeRoom.getPlayer().getCurrentPosition());
    }

    @Test
    public void test_getName() {
        EscapeRoom escapeRoom = new EscapeRoom("escaperoom yay!!", null, null, null);

        assertEquals("escaperoom yay!!", escapeRoom.getName());
    }

   @Test
   public void test_setName() {
       EscapeRoom escapeRoom = new EscapeRoom("escaperoom", null, null, null);

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
        EscapeRoom escapeRoom = new EscapeRoom("escaperoom", player, null, null);

        assertEquals(player, escapeRoom.getPlayer());
    }

    @Test
    public void test_setPlayer() {
        Player player = new Player(null, null, null);
        EscapeRoom escapeRoom = new EscapeRoom("escaperoom", player, null, null);

        assertEquals(player, escapeRoom.getPlayer());

        Player player2 = new Player(null, null, null);
        escapeRoom.setPlayer(player2);

        assertEquals(player2, escapeRoom.getPlayer());
    }

    @Test
    public void test_getImages() {
        String path1 = "/Desktop/School/GVSU/pic.png";
        String path2 = "Z:/users/annac/docs/123.png";

        EscapeRoom escapeRoom = new EscapeRoom("escapeRoom", null, path1, null);

        assertEquals("/Desktop/School/GVSU/pic.png", escapeRoom.getImage());

        escapeRoom.setImage(path2);
        assertEquals("Z:/users/annac/docs/123.png", escapeRoom.getImage());
    }

    @Test
    public void test_setImage() {
        EscapeRoom escapeRoom = new EscapeRoom("escapeRoom", null, null, null);
        escapeRoom.setImage("/Desktop/1234/GVSU/pic.png");
        escapeRoom.setImage("Z:/users/annac/docs/123.png");
        escapeRoom.setImage("/Desktop/School/GVSU/\"fall 2021\"/\"CIS 350\"/GVSU_CIS350-ACK/image.png");
        escapeRoom.setImage("c:/folder1/\"folder 2-_\"/___file---.png");

        assertEquals("c:/folder1/\"folder 2-_\"/___file---.png", escapeRoom.getImage());

        assertEquals("image not found", escapeRoom.setImage(""));
        assertEquals("image not found", escapeRoom.setImage(null));

        Throwable exception = assertThrows(IllegalArgumentException.class, () -> {
            escapeRoom.setImage("Mult:/image.png");
        });
        assertEquals("setImage in class EscapeRoom: invalid file path", exception.getMessage());

        exception = assertThrows(IllegalArgumentException.class, () -> {
            escapeRoom.setImage("/image.pdf");
        });
        assertEquals("setImage in class EscapeRoom: invalid file path", exception.getMessage());

        exception = assertThrows(IllegalArgumentException.class, () -> {
            escapeRoom.setImage("/image/!/image.png");
        });
        assertEquals("setImage in class EscapeRoom: invalid file path", exception.getMessage());

        exception = assertThrows(IllegalArgumentException.class, () -> {
            escapeRoom.setImage("/image/a folder/image.png");
        });
        assertEquals("setImage in class EscapeRoom: invalid file path", exception.getMessage());
    }

    @Test
    public void test_getMap() {
        ArrayList<Room> map = new ArrayList<>();
        Room room1 = new Room("room1", "room1", false, null, null, null, null);
        Room room2 = new Room("room2", "room2", false, null, null, null, null);
        Room room3 = new Room("room3", "room3", false, null, null, null, null);
        map.add(room1);
        map.add(room2);
        map.add(room3);

        EscapeRoom escapeRoom = new EscapeRoom("escaperoom", null, null, map);

        assertEquals(room1, escapeRoom.getMap().get(0));
        assertEquals(room2, escapeRoom.getMap().get(1));
        assertEquals(room3, escapeRoom.getMap().get(2));
    }

    @Test
    public void test_setMap() {
        ArrayList<Room> map = new ArrayList<>();
        Room room1 = new Room("room1", "room1", false, null, null, null, null);
        Room room2 = new Room("room2", "room2", false, null, null, null, null);
        Room room3 = new Room("room3", "room3", false, null, null, null, null);
        map.add(room1);
        map.add(room2);
        map.add(room3);

        EscapeRoom escapeRoom = new EscapeRoom("escaperoom", null, null, null);
        escapeRoom.setMap(map);

        assertEquals(room1, escapeRoom.getMap().get(0));
        assertEquals(room2, escapeRoom.getMap().get(1));
        assertEquals(room3, escapeRoom.getMap().get(2));
    }

    @Test
    public void test_saveLoadProgress() {

        ArrayList<Room> map = new ArrayList<>();
        Room room1 = new Room("room1", "room1", false, null, null, null, null);
        Room room2 = new Room("room2", "room2", false, null, null, null, null);
        Room room3 = new Room("room3", "room3", false, null, null, null, null);
        Room room4 = new Room("room4", "room4", false, null, null, null, null);
        Room room5 = new Room("room5", "room5", false, null, null, null, null);
        Room room6 = new Room("room6", "room6", false, null, null, null, null);
        Room room7 = new Room("room7", "room7", false, null, null, null, null);
        map.add(room1);
        map.add(room2);
        map.add(room3);
        map.add(room4);
        map.add(room5);
        map.add(room6);
        map.add(room7);

        ArrayList<String> notes = new ArrayList<>();
        notes.add("first note");
        notes.add("second note");
        notes.add("third note");

        ArrayList<Room> key1Unlocks = new ArrayList<>();
        key1Unlocks.add(room1);
        key1Unlocks.add(room2);
        Key key1 = new Key("key1", key1Unlocks);

        ArrayList<Room> key2Unlocks = new ArrayList<>();
        key2Unlocks.add(room3);
        key2Unlocks.add(room4);
        Key key2 = new Key("key2", key2Unlocks);

        ArrayList<Room> key3Unlocks = new ArrayList<>();
        key3Unlocks.add(room5);
        key3Unlocks.add(room6);
        Key key3 = new Key("key3", key3Unlocks);

        ArrayList<Key> inventory = new ArrayList<>();
        inventory.add(key1);
        inventory.add(key2);
        inventory.add(key3);

        Player player = new Player(notes, inventory, room7);

        EscapeRoom escapeRoom = new EscapeRoom("escaperoom", player, null, map);

        escapeRoom.saveProgress("progress");

        escapeRoom.setPlayer(new Player(null, null, null));

        escapeRoom.loadProgress("progress");

        assertEquals("first note", escapeRoom.getPlayer().getNotes().get(0));
        assertEquals("second note", escapeRoom.getPlayer().getNotes().get(1));
        assertEquals("third note", escapeRoom.getPlayer().getNotes().get(2));

        assertEquals("key1", escapeRoom.getPlayer().getInventory().get(0).getName());
        assertEquals(room1, escapeRoom.getPlayer().getInventory().get(0).getUnlocks().get(0));
        assertEquals(room2, escapeRoom.getPlayer().getInventory().get(0).getUnlocks().get(1));

        assertEquals("key2", escapeRoom.getPlayer().getInventory().get(1).getName());
        assertEquals(room4, escapeRoom.getPlayer().getInventory().get(1).getUnlocks().get(1));

        assertEquals("key3", escapeRoom.getPlayer().getInventory().get(2).getName());
        assertEquals(room5, escapeRoom.getPlayer().getInventory().get(2).getUnlocks().get(0));
        assertEquals(room6, escapeRoom.getPlayer().getInventory().get(2).getUnlocks().get(1));

        assertEquals(room7, escapeRoom.getPlayer().getCurrentPosition());
    }

    @Test
    public void test_moveRoom() {
        ArrayList<Room> map = new ArrayList<>();
        Room upstairs = new Room("Upstairs", "This is the upstairs", false, null, null, null, null);
        Room bedroom = new Room("Bedroom", "This is the bedroom", false, null, "123", null, null);
        Room bathroom = new Room("Bathroom", "This is the bathroom", true, null, "123", null, null);
        map.add(upstairs);
        map.add(bedroom);
        map.add(bathroom);
        upstairs.addRoom(bedroom);
        bedroom.addRoom(upstairs);
        bedroom.addRoom(bathroom);
        bathroom.addRoom(bedroom);
        EscapeRoom escapeRoom = new EscapeRoom("The house", new Player(null, null, null), null, map);

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
        Room upstairs = new Room("Upstairs", "This is the upstairs", false, null, null, null, null);
        Room bedroom = new Room("Bedroom", "This is the bedroom", true, null, "123", null, null);
        map.add(upstairs);
        map.add(bedroom);
        upstairs.addRoom(bedroom);
        bedroom.addRoom(upstairs);
        EscapeRoom escapeRoom = new EscapeRoom("The house", new Player(null, null, null), null, map);

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
        Room upstairs = new Room("Upstairs", "This is the upstairs", false, null, null, null, null);
        Room bedroom = new Room("Bedroom", "This is the bedroom", false, null, null, null, null);
        Room bathroom = new Room("Bathroom", "This is the bathroom", true, null, null, null, null);
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
        EscapeRoom escapeRoom = new EscapeRoom("The house", new Player(null, null, null), null, map);

        assertEquals("This is the upstairs", escapeRoom.inspectRoom());

        assertEquals(1, bedroom.getKeys().size());
        escapeRoom.moveRoom("bedroom");
        assertEquals("You found the following keys:\nunlocks bathroom\nThis is the bedroom", escapeRoom.inspectRoom());
        assertEquals("unlocks bathroom", escapeRoom.getPlayer().getInventory().get(0).getName());
        assertEquals(0, bedroom.getKeys().size());

        escapeRoom.moveRoom("bathroom");
        assertEquals("This is the bathroom", escapeRoom.inspectRoom());

        escapeRoom.moveRoom("bedroom");
        escapeRoom.inspectRoom();
        assertEquals(1, escapeRoom.getPlayer().getInventory().size());

        escapeRoom.getPlayer().setCurrentPosition(null);
        assertEquals(null, escapeRoom.inspectRoom());
    }
}