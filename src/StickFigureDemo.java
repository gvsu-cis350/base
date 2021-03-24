import java.awt.*;
import java.awt.event.ActionListener;
import java.awt.event.ActionEvent;
import javax.swing.*;

public class StickFigureDemo extends JFrame implements ActionListener {
    private JPanel viewerPanel;
    private JPanel controlPanel;

    private Container contentPane;

    public StickFigureDemo(){
        super("GUI Dancer");

        contentPane = new Container();
        contentPane.setLayout(new GridLayout(1,2));

        viewerPanel = new JPanel();
        viewerPanel.setBackground(Color.white);
        viewerPanel.setBorder(BorderFactory.createLineBorder(Color.BLACK));
        contentPane.add(viewerPanel);

        add(contentPane);
    }

    @Override
    public void actionPerformed(ActionEvent e) {

    }

    public static void main(String[] args){
        StickFigureDemo frame = new StickFigureDemo();
        frame.setSize(new Dimension(500, 500));
        frame.setResizable(false);
        frame.setVisible(true);
        frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
    }
}
