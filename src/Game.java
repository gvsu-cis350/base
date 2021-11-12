import java.io.*;
import java.util.ArrayList;
import java.util.Scanner;

public class Game {
    private ArrayList<EscapeRoom> escapeRooms;
    
    public void buildEscapeRoom(String filename){
        try {
            Scanner sc = new Scanner(new File(filename));
            sc.useDelimiter(",");
            int index = 1;

            ArrayList<Room> rooms = new ArrayList<Room>();

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
                        key = sc.next();
                        break;
                    case 5:
                        if( sc.next().toLowerCase().equals( "true" ) ) {
                            isEnd = true;
                        } else {
                            isEnd = false;
                        }
                        break;
                    case 6:
                        image = sc.next();
                        break;
                    case 7: 
                        break;
                    case 8: 
                        keyNames = sc.next();
                        break;
                }

                Room newRoom = new Room(name, description, isLocked, isEnd, image, null, null );
                rooms.add(newRoom);
                index++;
                if(index > 8) {
                    index = 1;
                }
            }

            index = 1;
            int roomIndex = 0;
            while(sc.hasNext()) {
                if(index == 1) {
                    roomIndex++;
                }

                if(index == 7)
                {
                    String[] availableRooms = sc.next().split(" ");
                    
                    for(Room r : rooms) {
                        for( String s : availableRooms ) {
                            if( s.equals(r.getName() ) ) {
                                rooms.get(roomIndex).addRoom(r);
                            }
                        }
                    }
                }

                index++;
                if(index > 8) {
                    index = 1;
                }
            }

            sc.close();
        } catch(Exception e) {
            //really bad things happen
        }
    }

    public void saveEscapeRoom(){
        
    }
}
