import java.awt.*;
import java.awt.event.ActionListener;
import java.awt.event.ActionEvent;
import javax.swing.*;

public class StickFigureDemo extends JFrame implements ActionListener {
    private JPanel viewerPanel;
    private JPanel controlPanel;

    private Container contentPane;

    public StickFigureDemo(){
        super("Sitck Figure Demo");

        contentPane = new Container();
        contentPane.setLayout(new GridLayout(1,1));

        viewerPanel = new JPanel();
        viewerPanel.setBackground(Color.white);
        viewerPanel.setBorder(BorderFactory.createLineBorder(Color.BLACK));
        contentPane.add(viewerPanel);

        add(contentPane);
    }
    public int body;
    public int feet;
    public Color color;
    public int size;

    public void drawing (Graphics paper){
        int head = feet - size;

        paper.setColor(color); // this sets the color of the stick figure
        paper.drawOval(body-10, head, 20, 20); // make the head
        paper.drawLine(body, head+20, head, feet - 30); // make the body

        // make the legs
        paper.drawLine(body, feet-30, body-15, feet);
        paper.drawLine(body, feet-30, body+15, feet);

        // make the arms
        paper.drawLine(body, feet-70, body-25, feet-70);
        paper.drawLine(body, feet-70, body+25, feet-85);

    }

    @Override
    public void actionPerformed(ActionEvent e) {

    }

    public static void main(String[] args){
        StickFigureDemo frame = new StickFigureDemo();
        frame.setSize(new Dimension(1000, 1000));
        frame.setResizable(false);
        frame.setVisible(true);
        frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
    }
}
