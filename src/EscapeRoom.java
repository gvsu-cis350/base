import java.io.*;
import java.util.*;
import java.util.Scanner;


public class EscapeRoom {
    private String name;
    private Player player;
    private ArrayList<Room> map;

    public EscapeRoom(String name, Player player, ArrayList<Room> map) {
        this.setName(name);
        this.setPlayer(player);
        this.map = new ArrayList<Room>();
        this.map = map;

        if (map != null && player != null)
            this.player.setCurrentPosition(map.get(0));
    }

    public String getName(){
        return name;
    }

    public void setName(String name){
        if (name == null)
            throw new IllegalArgumentException("setName in class EscapeRoom: null input");
        if (name.equals(""))
            throw new IllegalArgumentException("setName in class EscapeRoom: empty string");

        this.name = name;
    }

    public Player getPlayer() {
        return player;
    }

    public void setPlayer(Player player) {
        this.player = player;
    }

    public ArrayList<Room> getMap() {
        return map;
    }

    public void setMap(ArrayList<Room> map) {
        this.map = map;
    }

    public boolean saveProgress(String filename){
        if (filename == null)
            throw new IllegalArgumentException("saveProgress in class EscapeRoom: null filename");
        if (filename.equals(""))
            throw new IllegalArgumentException("saveProgress in class EscapeRoom: empty string");

        PrintWriter out = null;

        try {
            out = new PrintWriter(new BufferedWriter(new FileWriter(filename)));

            out.print("notes:\n");
            for (String note : player.getNotes()) {
                out.println(note);
            }

            out.println("\ninventory:");
            for (Key key : player.getInventory()) {
                out.print(key.getName() + ": ");
                for (Room room : key.getUnlocks()) {
                    if (!room.equals(key.getUnlocks().get(key.getUnlocks().size() - 1)))
                        out.print(room.getName() + ", ");
                    else
                        out.print(room.getName() + "\n");
                }
            }

            out.print("\ncurrentPosition: " + player.getCurrentPosition().getName());

            out.close();
            return true;
        } catch (Exception e) {
            return false;
        }
    }

    public void loadProgress(String filename){
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
            if (room.getName().equalsIgnoreCase(roomName)) {
                return room;
            }
        }
        return null;
    }

    public String moveRoom(String roomName) {
        Room room = this.searchMap(roomName);

        if (room != null) {
            if (room.getCode() == null && room.getReqKey()) {
                for (Key key : player.getInventory()) {
                    if (key.getUnlocks().contains(room)) {
                        player.setCurrentPosition(room);
                        return null;
                    }
                }
                return room.getName() + " requires a key to enter.";
            }
            if (room.getCode() != null && !room.getReqKey()) {
                return room.getName() + " requires a code to enter.";
            }
            if (room.getCode() != null && room.getReqKey()) {
                return room.getName() + " requires a key and a code to enter.";
            }
            player.setCurrentPosition(room);
            return null;
        }
        return roomName + " is not a valid name!";
    }

    public String unlock(String roomName, String code) {
        Room room = this.searchMap(roomName);

        if (room != null) {
            if (code.equals(room.getCode())) {
                if (room.getReqKey()) {
                    for (Key key : player.getInventory()) {
                        if (key.getUnlocks().contains(room)) {
                            player.setCurrentPosition(room);
                            return null;
                        }
                    }
                    return room.getName() + " also requires a key!";
                }
                player.setCurrentPosition(room);
                return null;
            }
            return code + " is incorrect!";
        }
        return null;
    }

    public String inspectRoom() {
        if (player.getCurrentPosition() == null)
            return null;

        String output = "";

        if (player.getCurrentPosition().getKeys().size() > 0) {
            output += "You found the following keys:\n";
            for (Key k : player.getCurrentPosition().getKeys()) {
                if (!player.getInventory().contains(k)) {
                    player.addToInventory(k);
                    output += k.getName() + "\n";
                }
            }
        }

        return output += player.getCurrentPosition().getScript();
    }
}
