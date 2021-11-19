import java.io.*;
import java.util.ArrayList;
import java.util.Scanner;

public class Game {
    private ArrayList<Room> rooms = new ArrayList<Room>();
    
    public ArrayList<Room> buildEscapeRoom(String filename){
        try {
            Scanner sc = new Scanner(new File(filename));
            sc.useDelimiter(",");
            int index = 1;

            String name = "";
            String description = "";
            boolean isLocked = false;
            String key = "";
            boolean isEnd = false;
            String image = "";
            String connectingRooms = "";
            String keyNames = "";

            while(sc.hasNext()){
                switch(index)
                {
                    case 1:
                        name = sc.next();
                        name = name.replace("\n", "");
                        break;
                    case 2:
                        description = sc.next();
                        break;
                    case 3:
                        if( sc.next().toLowerCase().equals( "true" ) ) {
                            isLocked = true;
                        } else {
                            isLocked = false;
                        }
                        break;
                    case 4:
                        String input = sc.next().toLowerCase();
                        if( input.equals( "true" ) ) {
                            isEnd = true;
                        } else {
                            isEnd = false;
                        }
                        break;
                    case 5:
                        image = sc.next();
                        break;
                    case 6: 
                        sc.next();
                        break;
                    case 8: 
                        keyNames = sc.next();
                        break;
                }
                index++;
                if(index > 8) {
                    Room newRoom = new Room(name, description, isLocked, isEnd, image, null, null );
                    rooms.add(newRoom);
                    index = 1;
                }
            }

            index = 1;
            int roomIndex = 0;
            while(sc.hasNext()) {
                if(index == 1) {
                    roomIndex++;
                }

                if(index == 7) {
                    String[] availableRooms = sc.next().split(" ");
                    
                    for(Room r : rooms) {
                        for( String s : availableRooms ) {
                            if( s.equals(r.getName() ) ) {
                                rooms.get(roomIndex).addRoom(r);
                            }
                        }
                    }
                }

                if(index == 8) {
                    //addkeys
                }

                index++;
                if(index > 8) {
                    index = 1;
                }
            }

            sc.close();
            return rooms;
        } catch(Exception e) {
            System.out.println("CYMBRE THIS IS BAD");
            return null;
        }
    }

    public ArrayList<Room> getEscapeRooms() {
        return rooms;
    }

    public void saveEscapeRoom(){
        
    }
}
