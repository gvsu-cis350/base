import java.util.*;

public class Player {

    private ArrayList<String> notes;
    private ArrayList<Key> inventory;
    private Room currentPosition;

    public Player(ArrayList<String> notes, ArrayList<Key> inventory, Room currentPosition) {
        this.currentPosition = currentPosition;
        this.notes = new ArrayList<>();
        this.inventory = new ArrayList<>();
        this.notes = notes;
        this.inventory = inventory;
        
    }

    public void addNote(String note) {
        if (note == null) 
            throw new IllegalArgumentException("addNote in class Player: null input");
        if (note.equals(""))
            throw new IllegalArgumentException("addNote in class Player: empty string");
        notes.add(note);
    }

    public void delNote(int index) {
        try {
            notes.remove(index);
        } catch (IndexOutOfBoundsException e) {
            throw new IndexOutOfBoundsException("delNote in class Player: index out of bounds");
        }
    }

    public ArrayList<String> getNotes() {
        return notes;
    }

    public void addToInventory(Key key){
        if (key == null)
            throw new IllegalArgumentException("addToInventory in class Player: null input");

        inventory.add(key);
    }

    public void delFromInventory(int index) {
        try {
            inventory.remove(index);
        } catch (IndexOutOfBoundsException e) {
            throw new IndexOutOfBoundsException("delFromInventory in class Player: index out of bounds");
        }
    }

    public ArrayList<Key> getInventory() {
        
        return inventory;
    }

    public Room getCurrentPosition() {
        return currentPosition;
    }

    public void setCurrentPosition(Room room) {
        currentPosition = room;
    }
}