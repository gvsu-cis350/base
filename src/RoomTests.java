package demo.tests;

import org.junit.Test;
import org.junit.Assert.*;
import java.util.ArrayList;

public class RoomTests {

    @Test
    public void constructor() {
        Room room1 = new Room("bedroom", "Anna's bedroom", true);
        Room room2 = new Room ("bathroom", "Anna's bathroom", false);
        Room room3 = new Room ("house", "Anna's house", true);

        room1.addRoom(room2);
        room3.addRoom(room1);
        room3.addRoom(room2);
        System.out.println(room3.toString());
    }
}