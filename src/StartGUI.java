import javax.swing.*;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
public class StartGUI extends JFrame implements ActionListener{
    private JPanel mainPanel;
    private JPanel buttonPanel;
    private JPanel titlePanel;

    private JLabel title;

    private JButton exit;
    private JButton start;
    private JButton options;

    public Color backgroundColor = new Color(0x222222);
    public Color textColor = new Color(0xFFFFFF);
    public Color itemColor = new Color(0x383B3F);
    public Color terminalColor = new Color(0x2A3C5C);
    public Color selectedColor = new Color(0x5F5F5F);

    private String escapeFile = null;

    public StartGUI(){
        mainPanel = new JPanel();
        buttonPanel = new JPanel();
        titlePanel = new JPanel();
        title = new JLabel("Escape Room Title", SwingConstants.CENTER);
        exit = new JButton("Quit Game");
        start = new JButton("Start Game");
        options = new JButton("Options");

        title.setFont(new Font("Sans-Serif", Font.PLAIN, 20));
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        mainPanel.setLayout(new BoxLayout(mainPanel, BoxLayout.Y_AXIS));

        exit.addActionListener(this);
        start.addActionListener(this);
        options.addActionListener(this);

        mainPanel.setBackground(backgroundColor);
        buttonPanel.setBackground(backgroundColor);
        titlePanel.setBackground(backgroundColor);

        exit.setBackground(itemColor);
        start.setBackground(itemColor);
        options.setBackground(itemColor);

        exit.setForeground(textColor);
        start.setForeground(textColor);
        options.setForeground(textColor);
        title.setForeground(textColor);

        titlePanel.add(title);
        buttonPanel.add(start);
        buttonPanel.add(options);
        buttonPanel.add(exit);
        mainPanel.add(titlePanel);
        mainPanel.add(buttonPanel);
        add(mainPanel);

        setVisible(true);
        setSize(500,500);
        setLocationRelativeTo(null);
    }

    public StartGUI(String filename, Color b, Color txt, Color item, Color out, Color sel){
        this.escapeFile = filename;
        
        this.backgroundColor = b;
        this.textColor = txt;
        this.itemColor = item;
        this.terminalColor = out;
        this.selectedColor = sel;

        mainPanel = new JPanel();
        buttonPanel = new JPanel();
        titlePanel = new JPanel();

        title = new JLabel("Escape Room Title", SwingConstants.CENTER);

        exit = new JButton("Quit Game");
        start = new JButton("Start Game");
        options = new JButton("Options");

        title.setFont(new Font("Sans-Serif", Font.PLAIN, 20));
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        mainPanel.setLayout(new BoxLayout(mainPanel, BoxLayout.Y_AXIS));

        exit.addActionListener(this);
        start.addActionListener(this);
        options.addActionListener(this);

        mainPanel.setBackground(backgroundColor);
        buttonPanel.setBackground(backgroundColor);
        titlePanel.setBackground(backgroundColor);

        exit.setBackground(itemColor);
        start.setBackground(itemColor);
        options.setBackground(itemColor);

        exit.setForeground(textColor);
        start.setForeground(textColor);
        options.setForeground(textColor);
        title.setForeground(textColor);

        titlePanel.add(title);
        buttonPanel.add(start);
        buttonPanel.add(options);
        buttonPanel.add(exit);
        mainPanel.add(titlePanel);
        mainPanel.add(buttonPanel);
        add(mainPanel);

        setVisible(true);
        setSize(500,500);
        setLocationRelativeTo(null);
    }

    public void actionPerformed(ActionEvent e){
        Object comp = e.getSource();
        if(exit == comp){
            this.dispose();
        }
        if(start == comp){
            if (escapeFile == null){
                new GameGUI();
            }
            else{
                new GameGUI(escapeFile, backgroundColor, textColor, itemColor, terminalColor, selectedColor);
            }
            this.dispose();
        }
        if(options == comp){
            new OptionsGUI(escapeFile, backgroundColor, textColor, itemColor, terminalColor, selectedColor);
            this.dispose();
        }
    }

    public static void main(String[] args){
        new StartGUI();
    }
}
