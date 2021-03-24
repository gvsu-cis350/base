import java.awt.*;

public class StickFigure {
   private int body;
   private int feet;
   private Color color;
   private int size;

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
}
