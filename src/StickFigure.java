import java.awt.*;

public class StickFigure {
   private int body;
   private int feet;
   private Color color;
   private int size;

   public void drawing (Graphics paper){
      int head = feet - size;

      paper.setColor(color); // this sets the color of the stick figure
      paper.drawOval(); // make the head
      paper.drawLine(); // make the body

      // make the legs
      paper.drawLine();
      paper.drawLine();

      // make the arms
      paper.drawLine();
      paper.drawLine();

   }
}
