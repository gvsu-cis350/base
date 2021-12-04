import java.util.*;

public class Player {

    private ArrayList<String> notes;
    private ArrayList<Key> inventory;
    private Room currentPosition;

    public Player(ArrayList<String> notes, ArrayList<Key> inventory, Room currentPosition) {
        this.setCurrentPosition(currentPosition);
        this.notes = new ArrayList<>();
        this.inventory = new ArrayList<>();

        if (notes != null) {
            for (String note : notes) {
                this.addNote(note);
            }
        }

        if (inventory != null) {
            for (Key key : inventory) {
                this.addToInventory(key);
            }
        }
        
    }

    public ArrayList<String> getNotes() {
        return notes;
    }

    public void addNote(String note) {
        if (note == null || note.equals("") || note.contains(System.getProperty("line.separator"))) 
            return;
        notes.add(note);
    }

    public String delNote(int index) {
        try {
            notes.remove(index);
        } catch (IndexOutOfBoundsException e) {
            return "Index out of bounds";
        }
        return "";
    }

    public ArrayList<Key> getInventory() {
        return inventory;
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

    public Room getCurrentPosition() {
        return currentPosition;
    }

    public void setCurrentPosition(Room room) {
        currentPosition = room;
    }
}