//import java.awt.*;
//import javax.swing.*;
//
//public class StickFigureDemo extends JFrame {
//
//    private StickFigure figure = new StickFigure();
//
//    public static void main(String[] args){
//        StickFigureDemo fr = new StickFigureDemo();
//    }
//
//    public StickFigureDemo(){
//        setLayout(new BorderLayout());
//
//        //changeable things
//        setSize(500, 375);
//        setTitle("Stick Figure Demo");
//        add("Center", figure);
//        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
//
//        //another way to center the screen
//        setLocationRelativeTo(null);
//        setVisible(true);
//    }
//
//    private class StickFigure extends Canvas {
//
//        public void paint (Graphics p){
//
//            p.drawOval(175,50,90,70); // make the head
//            p.drawLine( 220,120, 220,220 ); //body
//
//            p.drawLine( 220,130, 190,160 );   // Draw top left arm
//            p.drawLine( 220,130, 250,160 );   // Draw top right arm
//            p.drawLine( 200,190, 190,160 );   // Draw bottom left arm
//            p.drawLine( 240,190, 250,160 );   // Draw bottom right arm
//
//            p.drawLine( 220,220, 200,250 );   // Draw top left leg
//            p.drawLine( 220,220, 245,250 );   // Draw top right leg
//
//            p.drawLine( 200,280, 200,250 );   // Draw bottom left leg
//            p.drawLine( 245,280, 245,250 );   // Draw bottom right leg
//
//        }
//    }
//}
