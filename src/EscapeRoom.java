import java.util.*;

public class EscapeRoom {
    private String name;
    private ArrayList<Room> map;
    private Player player;

    public void saveProgress(){

    }

    public void loadProgress(File input){

    }

    public String getName(){
        return name;
    }

    public void setName(String input){
        name = input;
    }

    public String moveRoom(String roomName) {
        for (Room r : player.getCurrentPosition().getRooms()) {
            if (roomName.equals(r.getName())) {
                if (r.getIsLocked()) {
                    for (Key k : player.getInventory()) {
                        if (k.getUnlocks().contains(r)) {
                            player.setCurrentPosition(r);
                            return "";
                        }
                        return roomName + " is locked!";
                    }
                }
                player.setCurrentPosition(r);
                return "";
            }
        }
        return roomName + " is not a valid name!";
    }
}
