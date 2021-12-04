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

    public StartGUI(String escapeFile){
        mainPanel = new JPanel();
        buttonPanel = new JPanel();
        titlePanel = new JPanel();

        title = new JLabel("Escape Room Title", SwingConstants.CENTER);

        exit = new JButton("Quit Game");
        start = new JButton("Start Game");
        options = new JButton("Options");

        this.escapeFile = escapeFile;

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
                new GameGUI(escapeFile);
            }
            this.dispose();
        }
        if(options == comp){
            if (escapeFile == null)
                new OptionsGUI();
            else{
                new OptionsGUI(escapeFile);
            }
            this.dispose();
        }
    }

    public static void main(String[] args){
        new StartGUI();
    }
}
