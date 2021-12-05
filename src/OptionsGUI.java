import javax.swing.*;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.io.*;

public class OptionsGUI extends JFrame implements ActionListener{
    private JPanel mainPanel;
    private JPanel escapeRoomPanel;
    private JPanel escapeButtonsPanel;
    private JPanel comboPanel;
    private JPanel visualPanel;
    private JPanel examplePanel;
    private JPanel navigationPanel;

    private JList exampleList;

    private JComboBox fontSize;
    private JComboBox fontStyle;
    private JComboBox colors;

    private JLabel exampleLabel;
    private JLabel pathLabel;

    private JButton load;
    private JButton make;
    private JButton exampleButton;
    private JButton apply;
    private JButton defaultButton;
    private JButton ok;
    private JButton mainMenu;

    private String escapeFile = null;
    private String colorName = "Light";

    private String[] exampleString = {"This is how the terminal looks", "2"};
    private String[] sizeList = {"Small", "Medium", "Large"};
    private String[] styleList = {"Serif", "Sans-Serif"};
    private String[] colorList = {"Dark", "Light"};

    final JFileChooser fileLoader;

    private Color backgroundColor = new Color(0xF2F2F2);
    private Color textColor = new Color(0x222222);
    private Color itemColor = new Color(0xC1C4C8);
    private Color terminalColor = new Color(0xDEEAFF);
    private Color selectedColor = new Color(0xC1CEE0);

    private Font font;
    private String fontName = "Sans-Serif";

    private int ftSize = 12;

    public OptionsGUI(){
        mainPanel = new JPanel();
        escapeRoomPanel = new JPanel();
        escapeButtonsPanel = new JPanel();
        examplePanel = new JPanel();
        comboPanel = new JPanel();
        visualPanel = new JPanel();
        navigationPanel = new JPanel();

        exampleList = new JList(exampleString);

        fontSize = new JComboBox(sizeList);
        fontStyle = new JComboBox(styleList);
        colors = new JComboBox(colorList);

        exampleLabel = new JLabel("This is a label");
        if (escapeFile == null){
            pathLabel = new JLabel("No escape room chosen", SwingConstants.CENTER);
        }
        else{
            pathLabel = new JLabel(escapeFile, SwingConstants.CENTER);
        }


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
        mainPanel.setLayout(new BoxLayout(mainPanel, BoxLayout.Y_AXIS));
        visualPanel.setLayout(new BoxLayout(visualPanel, BoxLayout.Y_AXIS));
        escapeRoomPanel.setLayout(new BoxLayout(escapeRoomPanel, BoxLayout.Y_AXIS));

        fontStyle.setBackground(backgroundColor);
        fontStyle.setForeground(textColor);
        exampleButton.setBackground(itemColor);

        mainPanel.setBackground(backgroundColor);
        escapeRoomPanel.setBackground(backgroundColor);
        escapeButtonsPanel.setBackground(backgroundColor);
        examplePanel.setBackground(backgroundColor);
        comboPanel.setBackground(backgroundColor);
        visualPanel.setBackground(backgroundColor);
        navigationPanel.setBackground(backgroundColor);

        load.setBackground(itemColor);
        make.setBackground(itemColor);
        exampleButton.setBackground(itemColor);
        defaultButton.setBackground(itemColor);
        apply.setBackground(itemColor);
        ok.setBackground(itemColor);
        mainMenu.setBackground(itemColor);

        fontSize.setBackground(itemColor);
        fontStyle.setBackground(itemColor);
        colors.setBackground(itemColor);

        exampleList.setBackground(terminalColor);
        exampleList.setSelectionBackground(terminalColor);

        exampleLabel.setForeground(textColor);
        pathLabel.setForeground(textColor);

        load.setForeground(textColor);
        make.setForeground(textColor);
        exampleButton.setForeground(textColor);
        defaultButton.setForeground(textColor);
        apply.setForeground(textColor);
        ok.setForeground(textColor);
        mainMenu.setForeground(textColor);

        fontSize.setForeground(textColor);
        fontStyle.setForeground(textColor);
        colors.setForeground(textColor);

        exampleList.setForeground(textColor);
        exampleList.setSelectionForeground(textColor);

        exampleList.setFont(font);
        fontSize.setFont(font);
        colors.setFont(font);
        exampleLabel.setFont(font);
        pathLabel.setFont(font);
        load.setFont(font);
        make.setFont(font);
        exampleButton.setFont(font);
        apply.setFont(font);
        defaultButton.setFont(font);
        ok.setFont(font);
        mainMenu.setFont(font);

        escapeButtonsPanel.add(load);
        escapeButtonsPanel.add(make);

        escapeRoomPanel.add(pathLabel);
        escapeRoomPanel.add(escapeButtonsPanel);
        mainPanel.add(escapeRoomPanel);

        examplePanel.add(exampleLabel);
        examplePanel.add(exampleList);
        examplePanel.add(exampleButton);

        comboPanel.add(fontStyle);
        comboPanel.add(fontSize);
        comboPanel.add(colors);

        visualPanel.add(comboPanel);
        visualPanel.add(examplePanel);

        navigationPanel.add(mainMenu);
        navigationPanel.add(defaultButton);
        navigationPanel.add(apply);
        navigationPanel.add(ok);

        mainPanel.add(visualPanel);
        mainPanel.add(navigationPanel);

        add(mainPanel);

        setVisible(true);
        setSize(550,500);
        setLocationRelativeTo(null);
    }
    public OptionsGUI(String filename, String name, Color b, Color txt, Color item, Color out, Color sel, String n, int sz){
        this.escapeFile = filename;
        this.colorName = name;
        this.backgroundColor = b;
        this.textColor = txt;
        this.itemColor = item;
        this.terminalColor = out;
        this.selectedColor = sel;
        this.fontName = n;
        this.ftSize = sz;

        mainPanel = new JPanel();
        escapeRoomPanel = new JPanel();
        escapeButtonsPanel = new JPanel();
        examplePanel = new JPanel();
        comboPanel = new JPanel();
        visualPanel = new JPanel();
        navigationPanel = new JPanel();
        font = new Font(fontName, Font.PLAIN, ftSize);

        exampleList = new JList(exampleString);

        fontSize = new JComboBox(sizeList);
        fontStyle = new JComboBox(styleList);
        colors = new JComboBox(colorList);

        fontSize.setSelectedIndex(getFontSizeIndex());
        fontStyle.setSelectedIndex(getFontStyleIndex());
        colors.setSelectedIndex(getColorsIndex());

        exampleLabel = new JLabel("This is a label");
        if (escapeFile == null){
            pathLabel = new JLabel("No escape room chosen", SwingConstants.CENTER);
        }
        else{
            pathLabel = new JLabel(escapeFile, SwingConstants.CENTER);
        }


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
        mainPanel.setLayout(new BoxLayout(mainPanel, BoxLayout.Y_AXIS));
        visualPanel.setLayout(new BoxLayout(visualPanel, BoxLayout.Y_AXIS));
        escapeRoomPanel.setLayout(new BoxLayout(escapeRoomPanel, BoxLayout.Y_AXIS));

        fontStyle.setBackground(backgroundColor);
        fontStyle.setForeground(textColor);
        exampleButton.setBackground(itemColor);

        mainPanel.setBackground(backgroundColor);
        escapeRoomPanel.setBackground(backgroundColor);
        escapeButtonsPanel.setBackground(backgroundColor);
        examplePanel.setBackground(backgroundColor);
        comboPanel.setBackground(backgroundColor);
        visualPanel.setBackground(backgroundColor);
        navigationPanel.setBackground(backgroundColor);

        load.setBackground(itemColor);
        make.setBackground(itemColor);
        exampleButton.setBackground(itemColor);
        defaultButton.setBackground(itemColor);
        apply.setBackground(itemColor);
        ok.setBackground(itemColor);
        mainMenu.setBackground(itemColor);

        fontSize.setBackground(itemColor);
        fontStyle.setBackground(itemColor);
        colors.setBackground(itemColor);

        exampleList.setBackground(terminalColor);
        exampleList.setSelectionBackground(terminalColor);

        exampleLabel.setForeground(textColor);
        pathLabel.setForeground(textColor);

        load.setForeground(textColor);
        make.setForeground(textColor);
        exampleButton.setForeground(textColor);
        defaultButton.setForeground(textColor);
        apply.setForeground(textColor);
        ok.setForeground(textColor);
        mainMenu.setForeground(textColor);

        fontSize.setForeground(textColor);
        fontStyle.setForeground(textColor);
        colors.setForeground(textColor);

        exampleList.setForeground(textColor);
        exampleList.setSelectionForeground(textColor);

        exampleList.setFont(font);
        fontSize.setFont(font);
        fontStyle.setFont(font);
        colors.setFont(font);
        exampleLabel.setFont(font);
        pathLabel.setFont(font);
        load.setFont(font);
        make.setFont(font);
        exampleButton.setFont(font);
        apply.setFont(font);
        defaultButton.setFont(font);
        ok.setFont(font);
        mainMenu.setFont(font);

        escapeButtonsPanel.add(load);
        escapeButtonsPanel.add(make);

        escapeRoomPanel.add(pathLabel);
        escapeRoomPanel.add(escapeButtonsPanel);
        mainPanel.add(escapeRoomPanel);

        examplePanel.add(exampleLabel);
        examplePanel.add(exampleList);
        examplePanel.add(exampleButton);

        comboPanel.add(fontStyle);
        comboPanel.add(fontSize);
        comboPanel.add(colors);

        visualPanel.add(comboPanel);
        visualPanel.add(examplePanel);

        navigationPanel.add(mainMenu);
        navigationPanel.add(defaultButton);
        navigationPanel.add(apply);
        navigationPanel.add(ok);

        mainPanel.add(visualPanel);
        mainPanel.add(navigationPanel);

        add(mainPanel);

        setVisible(true);
        setSize(550,500);
        setLocationRelativeTo(null);
    }

    public void actionPerformed(ActionEvent e){
        Object comp = e.getSource();
        if (comp == mainMenu){
            if (escapeFile == null){
                new StartGUI();
            }
            else{
                new StartGUI(escapeFile, colorName, backgroundColor, textColor, itemColor, terminalColor, selectedColor, fontName, ftSize);
            }
            this.dispose();
        }
        if (comp == apply){
            this.colorName = (String)colors.getSelectedItem();
            setColors(colorName);
            this.fontName = (String)fontStyle.getSelectedItem();
            setFontSize((String)fontSize.getSelectedItem());

            fontSize.setSelectedIndex(getFontSizeIndex());
            fontStyle.setSelectedIndex(getFontStyleIndex());
            colors.setSelectedIndex(getColorsIndex());

            new OptionsGUI(escapeFile, colorName, backgroundColor, textColor, itemColor, terminalColor, selectedColor, fontName, ftSize);
            this.dispose();
        }
        if (comp == ok){
            setColors((String)colors.getSelectedItem());
            this.fontName = (String)fontStyle.getSelectedItem();
            setFontSize((String)fontSize.getSelectedItem());
            new StartGUI(escapeFile, colorName, backgroundColor, textColor, itemColor, terminalColor, selectedColor, fontName, ftSize);
            this.dispose();
        }
        if (comp == load){
            int returnVal = fileLoader.showOpenDialog(OptionsGUI.this);

            if(returnVal == JFileChooser.APPROVE_OPTION){
                File file = fileLoader.getSelectedFile();
                escapeFile = file.getAbsolutePath();
            }
            if (escapeFile != null && escapeFile.substring(escapeFile.lastIndexOf(".")).equals(".txt")){
                pathLabel.setText(escapeFile);
            }
            else{
                JOptionPane.showMessageDialog(this, "Please choose a .txt file");
                escapeFile = null;
            }
        }
        if (comp == make){
            new InstructionsGUI(escapeFile, colorName, backgroundColor, textColor, itemColor, terminalColor, selectedColor, fontName, ftSize);
           //JOptionPane.showMessageDialog(this, "<html><h1 id=\"how-to-create-a-custom-escape-room\">How to Create a Custom Escape Room</h1><p>In order to create an escape room, users must create a .txt file in a specific format. <br>It is very important that users use the exact formatting required when creating an escape room because the program does not have much handling for incorrect formatting. <br>It is the responsibility of the user to make sure they create the escape room file properly. Please refer to the end of this file for an example. </p><h2 id=\"escape-room-elements-descriptions\">Escape Room Elements Descriptions</h2><ul><li>An escape room is made up of several elements. These are:<ul><li>Beginning Script. These lines will start with &quot;Beginning: &quot; which will set the script that is displayed at the beginning of the escape room. </li><li>Ending Script. These lines will start with &quot;End: &quot; which will set the script that is displayed when the player escapes from the escape room. </li><li>Room. These lines will start with &quot;Room: &quot; which will tell the program to parse the line as a room. A room is made up of several parts. They must be entered in a specific order which I will list now. <br>Each part must be separated from the rest of the line with a pipe character &quot;|&quot;. Please note that none of these may contain a pipe character as part of the entry.<ul><li>Name. Enter the name of the room. </li><li>Script. This is a description of the room that you want to pop up when the player enters the room. </li><li>Requires Key. If this room requires a key to enter, type &quot;true&quot;, if not type &quot;false&quot;.</li><li>End. If this room is the last room in the escape room, type &quot;true&quot;, if not type &quot;false&quot;.</li><li>Image Path. Type the image path of the image that you would like displayed when the player is in this room. </li><li>Code. Type the key code for the room. If there is no code for this room, type null. Please note that you cannot have a key for a room be &quot;null&quot; as that is not compatible with this program. </li><li>Connected Rooms. Please enter the names of the rooms that can be accessed from within this room. Separate each name with a comma so that the program can distinguish between them. </li><li>Keys. Please enter the names of the keys that are within the room, separate each name with a space so that the program can distinguish between them. </li></ul></li><li>Key. These lines will start with &quot;Key: &quot;. Then you will enter the name of the key, followed a pipe character &quot;|&quot; and a list of the names of rooms that that key unlocks, separated by commas.</li><li>Map. These lines will start with &quot;Map: &quot; and then will have the filepath to a file that contains the image you would like displayed as the overall map for your escape room. </li></ul></li></ul><h2 id=\"example-escape-room\">Example Escape Room</h2><pre><code>Beginning: This is a description of the beginning of the escape room. It will describe <span class=\"hljs-keyword\">where</span> I am and how I came to be unfortunately trapped there.<span class=\"hljs-keyword\"><br>End</span>: This is a description of the <span class=\"hljs-keyword\">end</span>. Whew! Glad I got out of that place...<br>Room: Bathroom|<span class=\"hljs-type\">This</span> is a description of the bathroom|<span class=\"hljs-type\">false</span>|<span class=\"hljs-type\">false</span>|<span class=\"hljs-type\">/image</span>.png|<span class=\"hljs-type\">ABC</span>|<span class=\"hljs-type\">Library</span>,Bedroom,Kitchen|<span class=\"hljs-type\">Key1</span>,Key2|<span class=\"hljs-type\"><br>Room</span>: <span class=\"hljs-keyword\">Library</span>|<span class=\"hljs-type\">This</span> is <span class=\"hljs-keyword\">where</span> they keep all of the books|<span class=\"hljs-type\">true</span>|<span class=\"hljs-type\">false</span>|<span class=\"hljs-type\">/image</span>.png|<span class=\"hljs-type\">ABC</span>|<span class=\"hljs-type\">Library</span>,Bedroom,Kitchen|<span class=\"hljs-type\">Key1</span>,Key2|<span class=\"hljs-type\"><br>Room</span>: Kitchen|<span class=\"hljs-type\">This</span> is <span class=\"hljs-keyword\">where</span> they <span class=\"hljs-built_in\">do</span> all of the cooking|<span class=\"hljs-type\">false</span>|<span class=\"hljs-type\">false</span>|<span class=\"hljs-type\">/image</span>.png|<span class=\"hljs-type\">ABC</span>|<span class=\"hljs-type\">Library</span>,Bedroom,Kitchen|<span class=\"hljs-type\">Key1</span>,Key2|<span class=\"hljs-type\"><br>Room</span>: Bedroom|<span class=\"hljs-type\">This</span> is <span class=\"hljs-keyword\">where</span> the person sleeps|<span class=\"hljs-type\">true</span>|<span class=\"hljs-type\">true</span>|<span class=\"hljs-type\">/image</span>.png|<span class=\"hljs-type\">ABC</span>|<span class=\"hljs-type\">Library</span>,Bedroom,Kitchen|<span class=\"hljs-type\">Key1</span>,Key2|<span class=\"hljs-type\"><br>Key</span>: Key1|<span class=\"hljs-type\">Bathroom</span>,Bedroom<br>Key: Key2|<span class=\"hljs-type\">Kitchen</span><br>Map: /image.png</code></pre><h2 id=\"some-important-considerations\">Some Important Considerations</h2><ul><li>Please note that each line must start with one of the listed keys for the elements and nothing may contain any &quot;\\n&quot; newline characters.</li><li>You must follow these instructions exactly in order to create an effective escape room.</li><li>There must not be any blank lines in your file. </li><li>Make sure you have the spacing/punctuation in your file exactly the same as you would like it to show up in your escape room. </li><li>Please note that all line starting keywords must have a colon and a space before you write any other elements. </li></ul></html>");
        }
        if (comp == defaultButton){
            escapeFile = null;

            this.fontName = "Sans-Serif";
            this.ftSize = 12;
            this.colorName = "Light";

            this.backgroundColor = new Color(0xF2F2F2);
            this.textColor = new Color(0x222222);
            this.itemColor = new Color(0xC1C4C8);
            this.terminalColor = new Color(0xDEEAFF);
            this.selectedColor = new Color(0xC1CEE0); 

            new OptionsGUI(escapeFile, colorName, backgroundColor, textColor, itemColor, terminalColor, selectedColor, fontName, ftSize);
            this.dispose();
        }
    }

    protected void setColors(String s){
        switch(s){
            case "Dark":
                this.backgroundColor = new Color(0x222222);
                this.textColor = new Color(0xFFFFFF);
                this.itemColor = new Color(0x383B3F);
                this.terminalColor = new Color(0x2A3C5C);
                this.selectedColor = new Color(0x5F5F5F);
                break;
            case "Light":
                backgroundColor = new Color(0xF2F2F2);
                textColor = new Color(0x222222);
                itemColor = new Color(0xC1C4C8);
                terminalColor = new Color(0xDEEAFF);
                selectedColor = new Color(0xC1CEE0);
                break;
        }
    }
    protected void setFontSize(String s){
        switch(s){
            case "Small":
                this.ftSize = 10;
                break;
            case "Medium":
                this.ftSize = 12;
                break;
            case "Large":
                this.ftSize = 14;
                break;
        }
    }
    
    protected int getFontSizeIndex(){
        String s = "";
        int result = 0;
        switch(ftSize){
            case 10:
                s ="Small";
                break;
            case 12:
                s = "Medium";
                break;
            case 14:
                s = "Large";
                break;
        }

        for(int i = 0; i < sizeList.length; i++){
            if(s.equals(sizeList[i])){
                result = i;
            }
        }
        return result;
    }
    protected int getColorsIndex(){
        int result = 0;
        for(int i = 0; i < colorList.length;i++){
            if(colorName.equals(colorList[i])){
                result = i;
            }
        }
        return result;
    }
    protected int getFontStyleIndex(){
        int result = 0;
        for(int i = 0; i < styleList.length; i++){
            if(fontName.equals(styleList[i])){
                result = i;
            }
        }
        return result;
    }
}
