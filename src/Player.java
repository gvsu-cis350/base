import java.util.*;

public class Player {

    private ArrayList<String> notes;
    private ArrayList<Key> inventory;
    private Room currentPosition;

    public void addNote(String note) {
        notes.add(note);
    }

    public void delNote(int index) {

    }

    public String printNotes() {

    }

    public void addToInventory(){

    }

    public void delFromInventory() {

    }

    public ArrayList<Key> getInventory() {
        return inventory;
    }

    public String printInventory() {

    }

    public Room getCurrentPosition() {
        return currentPosition;
    }

    public void setCurrentPosition(Room room) {
        currentPosition = room;
    }
}