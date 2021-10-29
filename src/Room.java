import java.util.regex.Pattern;
import java.util.ArrayList;

public class Room {

    private String name;
    private String script;
    private boolean isLocked;
    private ArrayList<String> images;
    private ArrayList<Room> listOfRooms;

    public Room(String name, String script, boolean isLocked) {
        this.setName(name);
        this.setScript(script);
        this.setIsLocked(isLocked);
        this.images = new ArrayList<String>();
        this.listOfRooms = new ArrayList<Room>();
    }

    public Room(String name, String script, boolean isLocked, ArrayList<String> images, ArrayList<Room> rooms) {
        this.setName(name);
        this.setScript(script);
        this.setIsLocked(isLocked);
        this.images = new ArrayList<String>();
        this.listOfRooms = new ArrayList<Room>();

        for (int i = 0; i < images.size(); i++) {
            addImage(images.get(i));
        }

        for (int i = 0; i < rooms.size(); i++) {
            addRoom(rooms.get(i));
        }
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        if (name == null)
            throw new IllegalArgumentException("setName in class Room: null input");
        if (name.equals(""))
            throw new IllegalArgumentException("setName in class Room: empty string");

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

    public String getImage(int index) {
        try {
            images.get(index);
        } catch (IndexOutOfBoundsException e) {
            throw new IndexOutOfBoundsException("getImage in class Room: index out of bounds");
        }
        return images.get(index);
    }

    public void addImage(String image) {
        if (image == null)
            throw new IllegalArgumentException("addImage in class Room: null input");
        if (image.equals(""))
            throw new IllegalArgumentException("addImage in class Room: empty string");

        String regularExpression = "([a-zA-Z]:)?(/[a-zA-Z0-9_.-]+)+.pdf";

        if (!Pattern.matches(regularExpression, image))
            throw new IllegalArgumentException("addImage in class Room: invalid file path");

        images.add(image);
    }

    public void delImage(int index) {
        try {
            images.remove(index);
        } catch (IndexOutOfBoundsException e) {
            throw new IndexOutOfBoundsException("delImage in class Room: index out of bounds");
        }
    }

    public Room getRoom(int index) {
        try {
            listOfRooms.get(index);
        } catch (IndexOutOfBoundsException e) {
            throw new IndexOutOfBoundsException("getRoom in class Room: index out of bounds");
        }

        return listOfRooms.get(index);
    }

    public void addRoom(Room room) {
        if (room == null)
            throw new IllegalArgumentException("addRoom in class Room: null input");
        listOfRooms.add(room);
    }

    public void delRoom(int index) {
        try {
            listOfRooms.remove(index);
        } catch (IndexOutOfBoundsException e) {
            throw new IndexOutOfBoundsException("delRoom in class Room: index out of bounds");
        }
    }

    @Override
    public String toString() {
        return "Room{" +
                "name='" + name + '\'' +
                ", script='" + script + '\'' +
                ", isLocked=" + isLocked +
                ", images=" + images +
                ", listOfRooms=" + listOfRooms +
                '}';
    }
}
