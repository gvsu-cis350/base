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

        //changeable things
        setSize(500, 375);
        setTitle("Stick Figure Demo");
        add("Center", figure);
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);

        //another way to center the screen
        setLocationRelativeTo(null);
        setVisible(true);
    }

    private class StickFigure extends Canvas {

        public void drawing (Graphics p){

            p.drawOval(50,50,100,25); // make the head

        }
    }
}
