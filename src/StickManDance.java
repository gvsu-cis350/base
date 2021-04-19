//import java.awt.*;
//import java.awt.event.*;
//import javax.swing.*;
//import java.awt.event.MouseEvent;
//import java.awt.event.MouseListener;
//import java.util.Random;
///**
// * StickMan Drawing
// * @author - "Donald"
// * @version - 03/30/21
// */
//public class Drawing extends JPanel implements MouseListener {
//     @Override
//    public void mouseExited(MouseEvent e) {}
//    @Override
//    public void mouseEntered(MouseEvent e) {}
//    @Override
//    public void mouseReleased(MouseEvent e) {}
//    @Override
//    public void mousePressed(MouseEvent e) {}
//
//    public static void main(String[] a) {
//         JFrame f = new JFrame();
//        f.setContentPane(new Drawing());
//        f.setSize(600, 400);
//        f.setDefaultCloseOperation(WindowConstants.EXIT_ON_CLOSE);
//        f.setVisible(true);
//    }
//
//    public void paintComponent(Graphics g){
//
//        // this statement required
//        super.paintComponent(g);
//
//        // optional: paint the background color (default is white)
//        setBackground(Color.CYAN);
//        // display words
//        g.setColor(Color.BLACK);
//        g.drawString("DANCE!", 250, 20);
//
//        //rando num/
//        Random rnd = new Random();
//        int r = rnd.nextInt(4);
//        if(r == 0){
//            drawDanceA(g);
//        }
//        if(r == 1){
//            drawDanceB(g);
//        }
//        if(r == 2){
//            drawDanceC(g);
//        }
//        if(r == 3){
//            drawDanceD(g);
//        }
//        //if rand = 0 dance a 1b 2c 3d
//
//
//        addMouseListener(this);
//
//    }
//
//
//    public void drawDanceA(Graphics g) {
//        g.fillRect( 130,30, 40,40 );      // Draw head
//        g.drawLine( 150,70, 150,80 );     // Draw neck
//        g.drawLine( 150,80, 150,193 );    // Draw body
//        g.drawLine( 150,130, 250,130 );   // Draw right arm
//        g.drawLine( 250,130, 250,70 );
//        g.drawLine( 100,130, 40,190 );    // Draw left arm
//        g.drawLine( 100,130, 150,130 );
//        g.drawLine( 150,190, 95,320 );    // Draw left leg
//        g.drawLine( 95,320, 75,320 );
//        g.drawLine( 150,190, 205,320 );   // Draw right leg
//        g.drawLine( 205,320, 225,320 );
//    }
//
//    public void drawDanceB(Graphics g) {
//        g.fillRect( 130,50, 40,40 );      // Draw head
//        g.drawLine( 150,150, 150,80 );     // Draw neck
//        g.drawLine( 150,80, 150,193 );    // Draw body
//        g.drawLine( 150,130, 250,130 );   // Draw right arm
//        g.drawLine( 250,130, 250,200 );
//        g.drawLine( 100,130, 40,190 );    // Draw left arm
//        g.drawLine( 100,130, 150,130 );
//        g.drawLine( 150,190, 95,320 );    // Draw left leg
//        g.drawLine( 95,320, 75,320 );
//        g.drawLine( 150,190, 205,320 );   // Draw right leg
//        g.drawLine( 205,320, 225,300 );
//    }
//
//    public void drawDanceC(Graphics g) {
//        g.fillRect( 130,30, 40,40 );      // Draw head
//        g.drawLine( 150,70, 150,80 );     // Draw neck
//        g.drawLine( 150,80, 150,193 );    // Draw body
//        g.drawLine( 150,130, 250,130 );   // Draw right arm
//        g.drawLine( 250,130, 250,70 );
//        g.drawLine( 100,130, 40,70 );    // Draw left arm
//        g.drawLine( 100,130, 150,130 );
//        g.drawLine( 150,190, 95,320 );    // Draw left leg
//        g.drawLine( 95,320, 75,320 );
//        g.drawLine( 150,190, 205,320 );   // Draw right leg
//        g.drawLine( 205,320, 225,320 );
//    }
//
//    public void drawDanceD(Graphics g) {
//        g.fillRect( 130,50, 40,40 );      // Draw head
//        g.drawLine( 150,70, 150,80 );     // Draw neck
//        g.drawLine( 150,80, 150,193 );    // Draw body
//        g.drawLine( 150,130, 250,130 );   // Draw right arm
//        g.drawLine( 250,130, 250,70 );
//        g.drawLine( 100,130, 40,190 );    // Draw left arm
//        g.drawLine( 100,130, 150,130 );
//        g.drawLine( 150,190, 95,320 );    // Draw left leg
//        g.drawLine( 95,320, 75,320 );
//        g.drawLine( 150,190, 205,320 );   // Draw right leg
//        g.drawLine( 205,320, 225,300 );
//    }
//
//
//    public void mouseClicked(MouseEvent e) {
//       repaint();
//
//    }
//}
