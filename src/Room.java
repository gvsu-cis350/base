public class Room {

    private String name;
    private String script;
    private boolean isLocked;
    private ArrayList<String> images;
    private ArrayList<Room> listOfRooms;

    public Room(String name, String script, boolean isLocked) {
        this.setName(name);
        this.setScript(newScript);
        this.setIsLocked(locked);
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        if (name == null)
            throw new IllegalArgumentExceptopn("setName in class Room: null input");
        if (name.equals(""))
            throw new IllegalArgumentExceptopn("setName in class Room: empty string");

        this.name = name;
    }

    public String getScript() {
        return script;
    }

    public void setScript(String script) {
        if (script == null)
            throw new IllegalArgumentException("setScript in class Room: null input");

        this.script = script;
    }

    public boolean getIsLocked() {
        return isLocked;
    }

    public void setIsLocked(boolean isLocked) {
        this.isLocked = isLocked;
    }

    public void addImage(String image) {
        if (image == null)
            throw new IllegalArgumentExceptopn("addImage in class Room: null input");
        if (image.equals(""))
            throw new IllegalArgumentExceptopn("addImage in class Room: empty string");

        images.add(image);
    }
    
    public void delImage(int index) {
        try {
            images.remove(index);
        } catch (IndexOutOfBoundsException e) {
            throw new IndexOutOfBoundsException("delImage in class Room: index out of bounds");
        }
    }

    public String getImage(int index) {
        return images.get(index);
    }

    public Room getRoom(int index) {
        try {
            return listOfRooms.get(index);
        } catch (IndexOutOfBoundsException e) {
            throw new IndexOutOfBoundsException("getRoom in class Room: index out of bounds");
        }
    }

    public void addRoom(Room room) {
        listOfRooms.add(room);
    }

    public void delRoom(int index) {
        try {
            listOfRooms.remove(index);
        } catch (IndexOutOfBoundsException e) {
            throw new IndexOutOfBoundsException("delRoom in class Room: index out of bounds");
        }
    }
}
