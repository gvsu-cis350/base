import javax.swing.*;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.io.*;

public class OptionsGUI extends JFrame implements ActionListener{
    private JPanel mainPanel;
    private JPanel escapeRoomPanel;
    private JPanel visualPanel;
    private JPanel examplePanel;
    private JPanel navigationPanel;

    //private JList escapeRooms;
    private JList exampleList;

    private JComboBox fontSize;
    private JComboBox fontStyle;
    private JComboBox colors;

    private JLabel exampleLabel;
    private JLabel pathLabel;

    private JTextArea exampleText;

    private JButton load;
    private JButton make;
    private JButton exampleButton;
    private JButton apply;
    private JButton defaultButton;
    private JButton ok;
    private JButton mainMenu;

    private String escapeFile = null;

    private String[] exampleString = {"test", "test2", "test3", "test4"};
    private String[] sizeList = {"one", "two", "three", "four", "five"};
    private String[] styleList = {"Style1", "Style2", "Style3", "Style4", "Style5"};
    private String[] colorList = {"color1", "color2", "color3", "color4", "color5"};

    final JFileChooser fileLoader;

    public Color backgroundColor = new Color(0xCB4335);
    public Color textColor = new Color(0xFFFFFF);
    public Color selectionColor = new Color(0x313131);
    public Color selectionText = new Color(0xFF00EBFF);

    public OptionsGUI(){
        mainPanel = new JPanel();
        escapeRoomPanel = new JPanel();
        examplePanel = new JPanel();
        visualPanel = new JPanel();
        navigationPanel = new JPanel();

        exampleList = new JList(exampleString);

        fontSize = new JComboBox(sizeList);
        fontStyle = new JComboBox(styleList);
        colors = new JComboBox(colorList);

        exampleLabel = new JLabel("This is a label");
        if (escapeFile == null){
            pathLabel = new JLabel("No escape room chosen");
        }
        else{
            pathLabel = new JLabel(escapeFile);
        }

        exampleText = new JTextArea("This is an area with text in it", 5, 30);

        load = new JButton("Load Escape Room");
        make = new JButton("Make your own!");
        exampleButton = new JButton("Button");
        defaultButton = new JButton("Restore default settings");
        apply = new JButton("Apply Changes");
        ok = new JButton("Ok");
        mainMenu = new JButton("Main Menu");

        fileLoader = new JFileChooser();

        load.addActionListener(this);
        make.addActionListener(this);
        fontSize.addActionListener(this);
        fontStyle.addActionListener(this);
        colors.addActionListener(this);
        defaultButton.addActionListener(this);
        apply.addActionListener(this);
        ok.addActionListener(this);
        mainMenu.addActionListener(this);

        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);

        escapeRoomPanel.add(pathLabel);
        escapeRoomPanel.add(load);
        escapeRoomPanel.add(make);
        mainPanel.add(escapeRoomPanel);

        examplePanel.add(exampleLabel);
        examplePanel.add(exampleText);
        examplePanel.add(exampleList);
        examplePanel.add(exampleButton);

        visualPanel.add(fontStyle);
        visualPanel.add(colors);
        visualPanel.add(examplePanel);

        mainPanel.add(visualPanel);
        navigationPanel.add(mainMenu);
        navigationPanel.add(defaultButton);
        navigationPanel.add(apply);
        navigationPanel.add(ok);
        mainPanel.add(navigationPanel);

        //fontStyle.setBackground(backgroundColor);
        //fontStyle.setForeground(textColor);
        //exampleList.setSelectionBackground(selectionColor);
        //exampleList.setSelectionForeground(selectionText);
       //mainPanel.setBackground(backgroundColor);

        add(mainPanel);

        setVisible(true);
        setSize(1000,500);
    }

    public void actionPerformed(ActionEvent e){
        Object comp = e.getSource();
        if (comp == mainMenu){
            if (escapeFile == null){
                new StartGUI();
            }
            else{
                new StartGUI(escapeFile);
            }
            this.dispose();
        }
        if (comp == apply){
            //Change color values and refresh
        }
        if (comp == ok){
            //create new constructors for GUI classes that take in colors and font styles
            if (escapeFile == null){
                new StartGUI();
            }
            else{
                new StartGUI(escapeFile);
            }
            this.dispose();
        }
        if (comp == load){
            int returnVal = fileLoader.showOpenDialog(OptionsGUI.this);

            if(returnVal == JFileChooser.APPROVE_OPTION){
                File file = fileLoader.getSelectedFile();
                escapeFile = file.getAbsolutePath();
            }
            pathLabel.setText(escapeFile);
        }
        if (comp == make){
            //Show dialog box with example of csv (web link to google drive artifact???)
        }
        if (comp == defaultButton){
            //change everything back to the default values
        }
    }
}
