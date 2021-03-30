import java.awt.*;
import java.awt.event.ActionListener;
import java.awt.event.ActionEvent;
import javax.swing.*;

public class StickFigureDemo extends JFrame {

    private StickFigure figure = new StickFigure();

    public static void main(String[] args){
        StickFigureDemo fr = new StickFigureDemo();
    }

    public StickFigureDemo(){
        setLayout(new BorderLayout());
        setSize(500, 375);
    }

    private class StickFigure  extends figure{}

    //other stuff
    private JPanel viewerPanel;
    private JPanel controlPanel;

    private Container contentPane;



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


}
