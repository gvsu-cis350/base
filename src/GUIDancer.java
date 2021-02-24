import java.awt.*;
import java.awt.event.ActionListener;
import java.awt.event.ActionEvent;
import javax.swing.*;
import javax.swing.border.Border;

public class GUIDancer extends JFrame implements ActionListener {

    private JPanel viewerPanel;
    private JPanel controlPanel;

    private Container contentPane;

    public GUIDancer(){
        super("GUI Dancer");

        contentPane = new Container();
        contentPane.setLayout(new GridLayout(1,2));

        viewerPanel = new JPanel();
        viewerPanel.setBackground(Color.GREEN);
        viewerPanel.setBorder(BorderFactory.createLineBorder(Color.BLACK));
        contentPane.add(viewerPanel);

        controlPanel = new JPanel();
        controlPanel.setBackground(Color.RED);
        controlPanel.setBorder(BorderFactory.createLineBorder(Color.BLACK));
        contentPane.add(controlPanel);

        add(contentPane);
    }

    @Override
    public void actionPerformed(ActionEvent e) {

    }

    public static void main(String[] args){
        GUIDancer frame = new GUIDancer();
        frame.setSize(new Dimension(500, 500));
        frame.setResizable(false);
        frame.setVisible(true);
        frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
    }
}
