import java.awt.*;
import java.awt.event.ActionListener;
import java.awt.event.ActionEvent;
import javax.swing.*;

public class StickFigure extends JFrame implements ActionListener {

   private JPanel viewerPanel;
   private JPanel controlPanel;

   private Container contentPane;

   private int body;
   private int feet;
   private Color color;
   private int size;

   public StickFigure(){
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
      GUIDancer frame = new GUIDancer();
      frame.setSize(new Dimension(500, 500));
      frame.setResizable(false);
      frame.setVisible(true);
      frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
   }

}
