import java.util.ArrayList;

public class KeyRoom extends Room {

    private ArrayList<Key> keys;
    
    public KeyRoom(String name, String script, boolean isLocked, boolean isEnd, ArrayList<String> images, ArrayList<Room> rooms, ArrayList<Key> keys) {
        super(name, script, isLocked, isEnd, images, rooms);
        this.keys = new ArrayList<Key>();

        for (Key key : keys) {
            addKey(key);
        }
    }

    public ArrayList<Key> getKey() {
        return keys;
    }

    public void addKey(Key key) {
        if (key == null)
            throw new IllegalArgumentException("addKey in class KeyRoom: null input");
        keys.add(key);
    }

    public void delKey(int index) {
        try {
            keys.remove(index);
        } catch (IndexOutOfBoundsException e) {
            throw new IndexOutOfBoundsException("delKey in class KeyRoom: index out of bounds");
        }
    }
}