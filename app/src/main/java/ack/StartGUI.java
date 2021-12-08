package ack;

import javax.swing.*;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
public class StartGUI extends JFrame implements ActionListener{
    private JPanel mainPanel;
    private JPanel buttonPanel;
    private JPanel titlePanel;
    private JPanel namePanel;

    private JLabel title;
    private JLabel names;

    private JButton exit;
    private JButton start;
    private JButton options;

    private Color backgroundColor = new Color(0xF2F2F2);
    private Color textColor = new Color(0x222222);
    private Color itemColor = new Color(0xC1C4C8);
    private Color terminalColor = new Color(0xDEEAFF);
    private Color selectedColor = new Color(0xC1CEE0);

    private Font font;

    private String escapeFile = null;
    private String escapeFolder = null;
    private String fontName = "Sans-Serif";
    private String colorName = "Light";

    private int ftSize = 12;

    public StartGUI(){
        mainPanel = new JPanel();
        buttonPanel = new JPanel();
        titlePanel = new JPanel();
        namePanel = new JPanel();
        title = new JLabel("Escape Room Simulator", SwingConstants.CENTER);
        names = new JLabel("Ack! - Anna, Cymbre, Kit");
        exit = new JButton("Quit Game");
        start = new JButton("Start Game");
        options = new JButton("Options");
        font = new Font(fontName, Font.PLAIN, ftSize);

        title.setFont(new Font(fontName, Font.PLAIN, 20));
        names.setFont(new Font(fontName, Font.PLAIN, 14));
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        mainPanel.setLayout(new BoxLayout(mainPanel, BoxLayout.Y_AXIS));

        exit.addActionListener(this);
        start.addActionListener(this);
        options.addActionListener(this);

        mainPanel.setBackground(backgroundColor);
        buttonPanel.setBackground(backgroundColor);
        titlePanel.setBackground(backgroundColor);
        namePanel.setBackground(backgroundColor);

        exit.setBackground(itemColor);
        start.setBackground(itemColor);
        options.setBackground(itemColor);

        exit.setForeground(textColor);
        start.setForeground(textColor);
        options.setForeground(textColor);
        title.setForeground(textColor);
        names.setForeground(textColor);

        exit.setFont(font);
        start.setFont(font);
        options.setFont(font);

        titlePanel.add(title);
        namePanel.add(names);
        buttonPanel.add(start);
        buttonPanel.add(options);
        buttonPanel.add(exit);
        mainPanel.add(titlePanel);
        mainPanel.add(namePanel);
        mainPanel.add(buttonPanel);
        add(mainPanel);

        setVisible(true);
        setSize(500,200);
        setLocationRelativeTo(null);
    }

    public StartGUI(String filename, String folderName, String name, Color b, Color txt, Color item, Color out, Color sel, String n, int sz){
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
        buttonPanel = new JPanel();
        titlePanel = new JPanel();
        namePanel = new JPanel();
        title = new JLabel("Escape Room Simulator", SwingConstants.CENTER);
        names = new JLabel("Ack! - Anna, Cymbre, Kit");
        exit = new JButton("Quit Game");
        start = new JButton("Start Game");
        options = new JButton("Options");
        font = new Font(fontName, Font.PLAIN, ftSize);

        title.setFont(new Font(fontName, Font.PLAIN, 20));
        names.setFont(new Font(fontName, Font.PLAIN, 14));
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        mainPanel.setLayout(new BoxLayout(mainPanel, BoxLayout.Y_AXIS));

        exit.addActionListener(this);
        start.addActionListener(this);
        options.addActionListener(this);

        mainPanel.setBackground(backgroundColor);
        buttonPanel.setBackground(backgroundColor);
        titlePanel.setBackground(backgroundColor);
        namePanel.setBackground(backgroundColor);

        exit.setBackground(itemColor);
        start.setBackground(itemColor);
        options.setBackground(itemColor);

        exit.setForeground(textColor);
        start.setForeground(textColor);
        options.setForeground(textColor);
        title.setForeground(textColor);
        names.setForeground(textColor);

        exit.setFont(font);
        start.setFont(font);
        options.setFont(font);

        titlePanel.add(title);
        namePanel.add(names);
        buttonPanel.add(start);
        buttonPanel.add(options);
        buttonPanel.add(exit);
        mainPanel.add(titlePanel);
        mainPanel.add(namePanel);
        mainPanel.add(buttonPanel);
        add(mainPanel);

        setVisible(true);
        setSize(500,200);
        setLocationRelativeTo(null);
    }

    public void actionPerformed(ActionEvent e){
        Object comp = e.getSource();
        if(exit == comp){
            this.dispose();
        }
        if(start == comp){
            if (escapeFile == null){
                new GameGUI(colorName, backgroundColor, textColor, itemColor, terminalColor, selectedColor, fontName, ftSize);
            }
            else{
                new GameGUI(escapeFile, escapeFolder, colorName, backgroundColor, textColor, itemColor, terminalColor, selectedColor, fontName, ftSize);
            }
            this.dispose();
        }
        if(options == comp){
            new OptionsGUI(escapeFile, escapeFolder, colorName, backgroundColor, textColor, itemColor, terminalColor, selectedColor, fontName, ftSize);
            this.dispose();
        }
    }

    public static void main(String[] args){
        new StartGUI();
    }
}
