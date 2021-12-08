package ack;

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
    private String escapeFolder = null;

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
    private String colorName = "Light";

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
    public OptionsGUI(String filename, String folderName, String name, Color b, Color txt, Color item, Color out, Color sel, String n, int sz){
        this.escapeFile = filename;
        this.escapeFolder = folderName;
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
                new StartGUI(escapeFile, escapeFolder, colorName, backgroundColor, textColor, itemColor, terminalColor, selectedColor, fontName, ftSize);
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

            new OptionsGUI(escapeFile, escapeFolder, colorName, backgroundColor, textColor, itemColor, terminalColor, selectedColor, fontName, ftSize);
            this.dispose();
        }
        if (comp == ok){
            setColors((String)colors.getSelectedItem());
            this.fontName = (String)fontStyle.getSelectedItem();
            setFontSize((String)fontSize.getSelectedItem());
            new StartGUI(escapeFile, escapeFolder, colorName, backgroundColor, textColor, itemColor, terminalColor, selectedColor, fontName, ftSize);
            this.dispose();
        }
        if (comp == load){
            int returnVal = fileLoader.showOpenDialog(OptionsGUI.this);

            if(returnVal == JFileChooser.APPROVE_OPTION){
                File file = fileLoader.getSelectedFile();
                escapeFile = file.getAbsolutePath();
                escapeFolder = file.getParent();
            }
            if (escapeFile != null && escapeFile.substring(escapeFile.lastIndexOf(".")).equals(".txt")){
                pathLabel.setText(escapeFile);
            }
            else{
                JOptionPane.showMessageDialog(this, "Please choose a .txt file");
                escapeFile = null;
                escapeFolder = null;
            }
        }
        if (comp == make){
            new InstructionsGUI();
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

            new OptionsGUI(escapeFile, escapeFolder, colorName, backgroundColor, textColor, itemColor, terminalColor, selectedColor, fontName, ftSize);
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
