import java.io.*;
import java.util.*;
import java.util.Scanner;


public class EscapeRoom {
    private String name;
    private Player player;
    private ArrayList<Room> map;

    public EscapeRoom(String name, ArrayList<Room> map, Player player) {
        this.name = name;
        this.map = new ArrayList<Room>();
        this.map = map;
        this.player = player;
    }

    public String getName(){
        return name;
    }

    public void setName(String input){
        name = input;
    }

    public void saveProgress(String filename){
        if (filename == null) 
            throw new IllegalArgumentException("saveProgress in class EscapeRoom: null filename");

        PrintWriter out = null;

        try {
            out = new PrintWriter(new BufferedWriter(new FileWriter(filename)));
        } catch (Exception e) {
            throw new IllegalArgumentException("saveProgess in class EscapeRoom");
        }

        out.println("notes:");
        for (String note : player.getNotes()) {
            out.println(note);
        }

        // ***** need to also invlude rooms to unlock for each key!!
        // out.println("inventory:");
        // for (Key key : player.getInventory()) {
        //     out.println(key.getName());
        // }

        out.println("currentPosition:");
        out.println(player.getCurrentPosition());

        out.close();
    }

    public void loadProgress(String filename){
        if (filename == null)
            throw new IllegalArgumentException("loadProgress in class EscapeRoom: null filename");
        if (filename.equals(""))
            throw new IllegalArgumentException("loadProgress in class EscapeRoom: filename is empty");

        try {
            Scanner scanner = new Scanner(new File(filename));
            
            while (!scanner.nextLine().equals("inventory:")) {
                this.player.addNote(scanner.nextLine());
            }
            // while (!scanner.nextLine().equals("currentPosition:")) {
            //     this.player.addToInventory();
            // }
             
        } catch (FileNotFoundException e) {
            throw new RuntimeException("loadProgress in class EscapeRoom: file not found");
        }
    }

    private Room searchMap(String roomName) {
        for (Room room : map) {
            if (room.getName().equals(roomName)) {
                return room;
            }
        }
        return null;
    }

    public String moveRoom(String roomName) {
        for (Room r : player.getCurrentPosition().getRooms()) {
            if (roomName.equals(r.getName())) {
                if (r.getIsLocked()) {
                    for (Key k : player.getInventory()) {
                        if (k.getUnlocks().contains(r)) {
                            player.setCurrentPosition(r);
                            return null;
                        }
                        return roomName + " is locked!";
                    }
                }
                player.setCurrentPosition(r);
                return null;
            }
        }
        return roomName + " is not a valid name!";
    }

    public String inspectRoom() {
        String output = "";

        if (player.getCurrentPosition().getKeys() != null) {
            output += "You found the following keys:\n";
            for (Key k : player.getCurrentPosition().getKeys()) {
                player.addToInventory(k);
                output += k.getName() + "\n";
            }
        }

        return output += player.getCurrentPosition().getScript();
    }
}
