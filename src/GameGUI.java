import javax.imageio.ImageIO;
import javax.swing.*;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.image.BufferedImage;
import java.io.File;
import java.util.*;

public class GameGUI extends JFrame implements ActionListener {
    private JPanel mainPanel;
    private JPanel terminal;
    private JPanel mapPanel;
    private JPanel imagePanel;
    private JPanel notesPanel;
    private JPanel inventoryPanel;
    private JPanel help;

    private JTabbedPane mapImage;
    private JTabbedPane notesInventory;


    private JScrollPane notesScroll;
    private JScrollPane outScroll;
    private JScrollPane inventoryScroll;

    private JTextField command;

    private JTextArea helpInfo;

    private DefaultListModel<String> noteList;
    private DefaultListModel<String> outList;
    private DefaultListModel<Key> keyList;

    private JList outScreen;
    private JList notes;
    private JList inventory;

    private JLabel mapVisual;
    private JLabel imageVisual;

    private JButton saveProgress;
    private JButton loadProgress;
    private JButton options;
    private JButton mainMenu;

    private BufferedImage mapPicture;
    private BufferedImage imagePicture;

    private String mapFile = null;
    private String imageFile = null;
    private String saveLoadFile = null;
    private String commandInput = null;
    private String escapeFile = null;

    private Color backgroundColor = new Color(0xCB4335);
    private Color textColor = new Color(0xFFFFFF);

    final JFileChooser fileLoader = new JFileChooser();

    private Game escapeGame = new Game();
    private EscapeRoom escapeRoom;
    private Player player;

    public GameGUI(){
        mainPanel = new JPanel();
        helpInfo = new JTextArea("It looks like an escape room isn't properly loaded.  Please go to options to load one\n", 1, 50);
        options = new JButton("Options");
        mainMenu = new JButton("Main Menu");

        mainMenu.addActionListener(this);
        options.addActionListener(this);

        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);

        mainPanel.add(helpInfo);
        mainPanel.add(options);
        mainPanel.add(mainMenu);

        add(mainPanel);

        setVisible(true);
        setSize(750,750);
    }

    public GameGUI(String filename) {
        // try{
        //     escapeRoom = escapeGame.buildEscapeRoom(filename);
        // }catch(Exception e){
        //     new GameGUI();
        //     this.dispose();
        // }

        // player = escapeRoom.getPlayer();
        this.escapeFile = filename;

        mainPanel = new JPanel();
        terminal = new JPanel();
        mapPanel = new JPanel();
        imagePanel = new JPanel();
        notesPanel = new JPanel();
        inventoryPanel = new JPanel();
        help = new JPanel();

        mapImage = new JTabbedPane();
        notesInventory = new JTabbedPane();

        command = new JTextField(10);

        helpInfo = new JTextArea("Blurb about helpful things", 2, 30);

        outList = new DefaultListModel();
        noteList = new DefaultListModel();
        keyList = new DefaultListModel();

        outList.addElement("Welcome!");

        outScreen = new JList(outList);
        notes = new JList(noteList);
        inventory = new JList(keyList);

        outScroll = new JScrollPane(outScreen);
        notesScroll = new JScrollPane(notes);
        inventoryScroll = new JScrollPane(inventory);

        // mapFile = escapeRoom.getImage();
        // imageFile = player.getCurrentPosition().getImage();

        try {
            mapPicture = ImageIO.read(new File(mapFile));
            imagePicture = ImageIO.read(new File(imageFile));
            mapVisual = new JLabel(new ImageIcon(mapPicture));
            imageVisual = new JLabel(new ImageIcon(imagePicture));
        }catch(Exception e) {
            mapVisual = new JLabel("Map image not found");
            imageVisual = new JLabel("Area image not found");
        }

        saveProgress = new JButton("Save");
        loadProgress = new JButton("Load");
        mainMenu = new JButton("Main Menu");

        saveProgress.addActionListener(this);
        loadProgress.addActionListener(this);
        mainMenu.addActionListener(this);
        command.addActionListener(this);

        helpInfo.setEditable(false);
        outScroll.setPreferredSize(new Dimension(400, 650));
        inventoryScroll.setPreferredSize(new Dimension(200,100));
        notesScroll.setPreferredSize(new Dimension(200,100));
        outScroll.setHorizontalScrollBarPolicy(ScrollPaneConstants.HORIZONTAL_SCROLLBAR_NEVER);


        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);

        terminal.add(outScroll);
        terminal.add(command);

        mapPanel.add(mapVisual);
        imagePanel.add(imageVisual);
        mapImage.addTab("Map", mapPanel);
        mapImage.addTab("Location", imagePanel);

        notesPanel.add(notesScroll);
        inventoryPanel.add(inventoryScroll);
        notesInventory.addTab("Notes", notesPanel);
        notesInventory.addTab("Inventory", inventoryPanel);

        help.add(helpInfo);
        help.add(saveProgress);
        help.add(loadProgress);
        help.add(mainMenu);

        mainPanel.add(terminal);
        mainPanel.add(mapImage);
        mainPanel.add(notesInventory);
        mainPanel.add(help);

        add(mainPanel);

        setVisible(true);
        setSize(1000, 1000);
    }

    public void actionPerformed(ActionEvent e) {
        Object comp = e.getSource();
        if (comp == mainMenu) {
            if (escapeFile == null)
                new StartGUI();
            else
                new StartGUI(escapeFile);
            this.dispose();
        }
        if (comp == saveProgress) {
            int returnVal = fileLoader.showOpenDialog(GameGUI.this);

            if(returnVal == JFileChooser.APPROVE_OPTION){
                File file = fileLoader.getSelectedFile();
                saveLoadFile = file.getAbsolutePath();
                escapeRoom.saveProgress(saveLoadFile);
            }
        }
        if (comp == loadProgress) {
            int returnVal = fileLoader.showOpenDialog(GameGUI.this);

            if(returnVal == JFileChooser.APPROVE_OPTION){
                File file = fileLoader.getSelectedFile();
                saveLoadFile = file.getAbsolutePath();
                escapeRoom.loadProgress(saveLoadFile);
            }
        }
        if (comp == command){
            commandInput = command.getText();
            String commandOutput = null;
            Scanner sc = new Scanner(commandInput);
            if(commandInput != null && !commandInput.equals("") && sc.hasNext()){
                String word = sc.next();
                word.toLowerCase();
                switch(word){
                    case "list":
                        // Ask if this is the intended output of list command
                        commandOutput = "";
                        for (Room room : escapeRoom.getMap()){
                            commandOutput = commandOutput + ", " + room.getName();
                        }
                        outList.addElement(commandOutput);
                        command.setText(null);
                        break;
                    case "create":
                        // Error checking should be done
                        if (sc.hasNext()){
                            commandOutput = commandInput.substring(7);
                            noteList.addElement(commandOutput);
                        }
                        else{
                            outList.addElement("Please add a note to create");
                        }
                        command.setText(null);
                        break;
                    case "delete":
                        // Error checking should be done
                        if(sc.hasNext()){
                            commandOutput = commandInput.substring(7);
                            for(int i = 0; i < noteList.getSize(); i++){
                                if(commandOutput.equals(noteList.get(i))){
                                    noteList.remove(i);
                                    break;
                                }
                            }
                        }
                        else{
                            outList.addElement("Please add a note to remove");
                        }
                        command.setText(null);
                        break;
                    case "help":
                        // Takes no input, if it starts with help, should print help string
                        outList.addElement("This is a string where it specifies commands!");
                        command.setText(null);
                        break;
                    case "input":
                        // Error checking should be done
                        if(sc.hasNext()){
                            String code = sc.next();
                            if (sc.hasNext()){
                                String roomName = commandInput.substring(7 + code.length());
                                commandOutput = escapeRoom.unlock(roomName, code);
                                outList.addElement(commandOutput);
                            }
                            else{
                                outList.addElement("Input requires a room name, then the code");
                            }
                        }
                        else{
                            outList.addElement("Input requires a room name, then the code");
                        }
                        command.setText(null);
                        break;
                    case "move":
                        // Error checking should be done
                        if(sc.hasNext()){
                            commandOutput = escapeRoom.moveRoom(commandInput.substring(5));
                            if(commandOutput == null)
                                commandOutput = "You've moved to " + commandInput.substring(5);
                            outList.addElement(commandOutput);
                        }
                        else{
                            outList.addElement("Please enter a room to move to");
                        }
                        command.setText(null);
                        break;
                    case "inspect":
                        // Error checking should be done, no inputs
                        commandOutput = escapeRoom.inspectRoom();
                        outList.addElement(commandOutput);
                        command.setText(null);
                        break;
                    default: 
                        outList.addElement("Looks like that command doesn't exist.  Try the \"help\" command!");
                        command.setText(null);
                        break;
                }            
            }
            else{
                outList.addElement("Looks like we couldn't find a command.  Try typing \"help\"!");
            }
            sc.close();
            while (outList.getSize() > 50)
                outList.removeElementAt(0);
        }
        if (comp == options){
            new OptionsGUI();
            this.dispose();
        }
    }
}
