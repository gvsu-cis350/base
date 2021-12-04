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

    private String[] exampleString = {"This is how the terminal looks", "2"};
    private String[] sizeList = {"Small", "Medium", "Large"};
    private String[] styleList = {"Serif", "Sans-Serif"};
    private String[] colorList = {"Dark", "Light"};

    final JFileChooser fileLoader;

    public Color backgroundColor = new Color(0x222222);
    public Color textColor = new Color(0xFFFFFF);
    public Color itemColor = new Color(0x383B3F);
    public Color terminalColor = new Color(0x2A3C5C);

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
    public OptionsGUI(String filename){
        this.escapeFile = filename;

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
            if (escapeFile.substring(escapeFile.lastIndexOf(".")).equals(".csv")){
                pathLabel.setText(escapeFile);
            }
            else{
                JOptionPane.showMessageDialog(this, "Please choose a .csv file");
                escapeFile = null;
            }
        }
        if (comp == make){
            //Show dialog box with example of csv (web link to google drive artifact???)
        }
        if (comp == defaultButton){
            //change everything back to the default values including colors
            escapeFile = null;
        }
    }
}
