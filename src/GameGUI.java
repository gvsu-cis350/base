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

    private String mapFile = "D:\\CodingTests\\GUITests\\src\\pics\\map.png";
    private String imageFile = "D:\\CodingTests\\GUITests\\src\\pics\\image.png";
    private String saveLoadFile;
    private String commandInput;
    private String escapeFile = null;

    private Color backgroundColor = new Color(0xCB4335);
    private Color textColor = new Color(0xFFFFFF);

    final JFileChooser fileLoader = new JFileChooser();

    private Game escapeGame = new Game();
    private EscapeRoom escapeRoom;

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
        // escapeRoom = escapeGame.buildEscapeRoom(filename);
        // }catch(Exception e){
        //     new GameGUI();
        //     this.dispose();
        // }

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

        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);

        terminal.add(outScreen);
        terminal.add(command);

        mapPanel.add(mapVisual);
        imagePanel.add(imageVisual);
        mapImage.addTab("Map", mapPanel);
        mapImage.addTab("Location", imagePanel);

        notesPanel.add(notes);
        inventoryPanel.add(inventory);
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
        setSize(750, 750);
    }

    public void actionPerformed(ActionEvent e) {
        Object comp = e.getSource();
        if (comp == mainMenu) {
            new StartGUI();
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
            if(commandInput != null && !commandInput.equals("")){
                Scanner sc = new Scanner(commandInput);
                String word = sc.next();
                word.toLowerCase();
                switch(word){
                    case "list":
                        JOptionPane.showMessageDialog(null, "You entered \"list\"");
                        break;
                    case "create":
                        commandOutput = commandInput.substring(7);
                        noteList.addElement(commandOutput);
                        command.setText(null);
                        break;
                    case "delete":
                        noteList.remove(notes.getSelectedIndex());
                        command.setText(null);
                        break;
                    case "help":
                        outList.addElement("This is a string where it specifies commands!");
                        command.setText(null);
                        break;
                    case "input":
                        JOptionPane.showMessageDialog(null, "You entered \"input\"");
                        break;
                    case "move":
                        commandOutput = escapeRoom.moveRoom(commandInput.substring(5));
                        if(commandOutput == null)
                            commandOutput = "You've moved to " + commandInput.substring(5);
                        outList.addElement(commandOutput);
                        command.setText(null);
                        break;
                    case "inspect":
                        commandOutput = escapeRoom.inspectRoom();
                        outList.addElement(commandOutput);
                        command.setText(null);
                        break;
                    default: 
                        outList.addElement("Looks like that command doesn't exist.  Try the \"help\" command!");
                        command.setText(null);
                        break;
                }
                sc.close();                
            }
        }
        if (comp == options){
            new OptionsGUI();
            this.dispose();
        }
    }
}
