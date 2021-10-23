public class KeyRoom extends Room {

    private Key key;

    public KeyRoom(String name, String script, boolean isLocked) {
        super(name, script, isLocked);
        this.setKey(key);
    }

    public Key getKey() {
        return key;
    }

    public void setKey(Key key) {
        this.key = key;
    }
}