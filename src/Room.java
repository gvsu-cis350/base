import java.util.regex.Pattern;
import java.util.*;

public class Room {

    private String name;
    private String script;
    private boolean isLocked;
    private boolean isEnd;
    private ArrayList<String> images;
    private ArrayList<Room> rooms;

    public Room(String name, String script, boolean isLocked, boolean isEnd, ArrayList<String> images, ArrayList<Room> rooms) {
        this.setName(name);
        this.setScript(script);
        this.setIsLocked(isLocked);
        this.setIsEnd(isEnd);
        this.images = new ArrayList<String>();
        this.rooms = new ArrayList<Room>();

        if (images != null) {
            for (String image : images) {
                addImage(image);
            }
        }

        if (rooms != null) {
            for (Room room : rooms) {
                addRoom(room);
            }
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

    public boolean getIsEnd() {
        return isEnd;
    }

    public void setIsEnd(boolean isEnd) {
        this.isEnd = isEnd;
    }

    public ArrayList<String> getImages() {
        return images;
    }

    public void addImage(String path) {
        if (path == null)
            throw new IllegalArgumentException("addImage in class Room: null input");
        if (path.equals(""))
            throw new IllegalArgumentException("addImage in class Room: empty string");

        String regex = "([\\w]:)?((/[\\w\\s-.]+)|(/\"[\\w\\s-.]+\"))+.pdf";

        if (!Pattern.matches(regex, path))
            throw new IllegalArgumentException("addImage in class Room: invalid file path");

        images.add(path);
    }

    public void delImage(int index) {
        try {
            images.remove(index);
        } catch (IndexOutOfBoundsException e) {
            throw new IndexOutOfBoundsException("delImage in class Room: index out of bounds");
        }
    }

    public ArrayList<Room> getRooms() {
        return rooms;
    }

    public void addRoom(Room room) {
        if (room == null)
            throw new IllegalArgumentException("addRoom in class Room: null input");
        rooms.add(room);
    }

    public void delRoom(int index) {
        try {
            rooms.remove(index);
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
                ", listOfRooms=" + rooms +
                '}';
    }
}