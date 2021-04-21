import java.awt.*;
import java.awt.event.*;
import javax.swing.*;
import java.awt.geom.Ellipse2D;
import java.awt.geom.Rectangle2D;
import java.awt.geom.Line2D;
public class Dancer {

  public static void drawDancer(Graphics2D g2D, int r){

    g2D.setColor(Color.WHITE);

    if(r <= 0){
      drawDanceA(g2D);
    }
    else if(r <= 1){
      drawDanceB(g2D);
    }
    else if(r <= 2){
      drawDanceC(g2D);
    }
    else{
      drawDanceD(g2D);
    }
  }

  public static void drawDanceA(Graphics2D g2D) {
    g2D.draw(new Line2D.Double( 150 + 150,70+ 75, 150+ 150,80+ 75 ));  // Draw neck
    g2D.fill(new Rectangle2D.Double(130+ 150,30 + 75, 40 ,40 )); //draw head
    g2D.draw(new Line2D.Double( 150+ 150,80+ 75, 150+ 150,193+ 75 ));    // Draw body
    g2D.draw(new Line2D.Double( 150+ 150,130+ 75, 250+ 150,130+ 75 ));   // Draw right arm
    g2D.draw(new Line2D.Double( 250+ 150,130+ 75, 250+ 150,70+ 75 ));
    g2D.draw(new Line2D.Double( 100+ 150,130+ 75, 40+ 150,190+ 75 ));    // Draw left arm
    g2D.draw(new Line2D.Double( 100+ 150,130+ 75, 150+ 150,130+ 75 ));
    g2D.draw(new Line2D.Double( 150+ 150,190+ 75, 95+ 150,320+ 75 ));    // Draw left leg
    g2D.draw(new Line2D.Double( 95+ 150,320+ 75, 75+ 150,320 + 75));
    g2D.draw(new Line2D.Double( 150+ 150,190+ 75, 205+ 150,320+ 75 ));   // Draw right leg
    g2D.draw(new Line2D.Double( 205+ 150,320+ 75, 225+ 150,320+ 75 ));

  }

  public static void drawDanceB(Graphics2D g2D) {
    g2D.fill(new Rectangle2D.Double(130+ 150,50 + 75, 40 ,40 )); //draw head
    g2D.draw(new Line2D.Double( 150 + 150,150+ 75, 150+ 150,80+ 75 ));  // Draw neck
    g2D.draw(new Line2D.Double( 150+ 150,80+ 75, 150+ 150,193+ 75 ));    // Draw body
    g2D.draw(new Line2D.Double( 150+ 150,130+ 75, 250+ 150,130+ 75 ));   // Draw right arm
    g2D.draw(new Line2D.Double( 250+ 150,130+ 75, 250+ 150,190+ 75 ));
    g2D.draw(new Line2D.Double( 100+ 150,130+ 75, 40+ 150,190+ 75 ));    // Draw left arm
    g2D.draw(new Line2D.Double( 100+ 150,130+ 75, 150+ 150,130+ 75 ));
    g2D.draw(new Line2D.Double( 150+ 150,190+ 75, 95+ 150,320+ 75 ));    // Draw left leg
    g2D.draw(new Line2D.Double( 95+ 150,320+ 75, 75+ 150,320 + 75));
    g2D.draw(new Line2D.Double( 150+ 150,190+ 75, 205+ 150,320+ 75 ));   // Draw right leg
    g2D.draw(new Line2D.Double( 205+ 150,320+ 75, 225+ 150,300+ 75 ));

  }

  public static void drawDanceC(Graphics2D g2D) {
    g2D.fill(new Rectangle2D.Double(130+ 150,30 + 75, 40 ,40 ));        //draw head
    g2D.draw(new Line2D.Double( 150 + 150,70+ 75, 150+ 150,80+ 75 ));  // Draw neck
    g2D.draw(new Line2D.Double( 150+ 150,80+ 75, 150+ 150,193+ 75 ));    // Draw body
    g2D.draw(new Line2D.Double( 150+ 150,130+ 75, 250+ 150,130+ 75 ));   // Draw right arm
    g2D.draw(new Line2D.Double( 250+ 150,130+ 75, 250+ 150,70+ 75 ));
    g2D.draw(new Line2D.Double( 100+ 150,130+ 75, 40+ 150,70+ 75 ));    // Draw left arm
    g2D.draw(new Line2D.Double( 100+ 150,130+ 75, 150+ 150,130+ 75 ));
    g2D.draw(new Line2D.Double( 150+ 150,190+ 75, 95+ 150,320+ 75 ));    // Draw left leg
    g2D.draw(new Line2D.Double( 95+ 150,320+ 75, 75+ 150,320 + 75));
    g2D.draw(new Line2D.Double( 150+ 150,190+ 75, 205+ 150,320+ 75 ));   // Draw right leg
    g2D.draw(new Line2D.Double( 205+ 150,320+ 75, 225+ 150,300+ 75 ));
  }

  public static void drawDanceD(Graphics2D g2D) {
    g2D.fill(new Rectangle2D.Double(130+ 150,50 + 75, 40 ,40 ));        //draw head
    g2D.draw(new Line2D.Double( 150 + 150,70+ 75, 150+ 150,80+ 75 ));  // Draw neck
    g2D.draw(new Line2D.Double( 150+ 150,80+ 75, 150+ 150,193+ 75 ));    // Draw body
    g2D.draw(new Line2D.Double( 150+ 150,130+ 75, 250+ 150,130+ 75 ));   // Draw right arm
    g2D.draw(new Line2D.Double( 250+ 150,130+ 75, 250+ 150,70+ 75 ));
    g2D.draw(new Line2D.Double( 100+ 150,130+ 75, 40+ 150,150+ 75 ));    // Draw left arm
    g2D.draw(new Line2D.Double( 100+ 150,130+ 75, 150+ 150,130+ 75 ));
    g2D.draw(new Line2D.Double( 150+ 150,190+ 75, 95+ 150,320+ 75 ));    // Draw left leg
    g2D.draw(new Line2D.Double( 95+ 150,320+ 75, 75+ 150,320 + 75));
    g2D.draw(new Line2D.Double( 150+ 150,190+ 75, 205+ 150,320+ 75 ));   // Draw right leg
    g2D.draw(new Line2D.Double( 205+ 150,320+ 75, 225+ 150,300+ 75 ));
  }
}