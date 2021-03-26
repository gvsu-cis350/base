import java.awt.*;
import java.awt.geom.AffineTransform;
import java.awt.geom.Ellipse2D;
import java.awt.geom.Rectangle2D;
import java.awt.image.BufferStrategy;
import java.awt.image.BufferedImage;
import java.util.ArrayList;
import javax.swing.*;

public class Main {

    public static final int MAX_SPAWN = 2;
    public static final int RATE = 100;
    public static final int GRAVITY = 1000;
    public static final double DRAG = 0;

    public static int width;
    public static int height;
    private static JFrame frame;
    private static Canvas canvas;
    public static BufferStrategy b;
    private static GraphicsEnvironment ge;
    private static GraphicsDevice gd;
    private static GraphicsConfiguration gc;
    private static BufferedImage buffer;
    private static Graphics graphics;
    private static Graphics2D g2D;
    private static AffineTransform at;
    public static ArrayList<Entity> world = new ArrayList<>();
    public static boolean isRunning = true;

    public static void main(String[] args){
        width = 1000;
        height = 1000;
        initializeFrame();
        // Thread moveEngine = new MoveEngine();
        // moveEngine.start();
         Thread makeLife = new MakeEntity();
         makeLife.start();
        runAnimation();
    }

    public static synchronized int giveBirth(Square square) {
        if (world.size() >= MAX_SPAWN) return 1;
        world.add(square);
        return 0;
    }

    public static synchronized int giveBirth(Ball ball) {
        if (world.size() >= MAX_SPAWN) return 1;
        world.add(ball);
        return 0;
    }

    public static void initializeFrame() {
        frame = new JFrame("Demo");
        frame.setIgnoreRepaint(true); //what does this do?
        frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);


        // Create canvas (used for painting to frame)
        canvas = new Canvas();
        canvas.setIgnoreRepaint(true); //again what is ignoreRepaint?
        canvas.setSize(width,height);
        canvas.setBackground(Color.DARK_GRAY);

        // Add to display
        frame.add(canvas);
        frame.pack();

        // Center to display
        frame.setLocationRelativeTo(null);
        frame.setVisible(true);

        // Set up the BufferStrategy for double buffering.
        canvas.createBufferStrategy(2);
        b = canvas.getBufferStrategy();

        // Get graphics configuration
        ge = GraphicsEnvironment.getLocalGraphicsEnvironment(); // what is Graphics environment?
        gd = ge.getDefaultScreenDevice(); // what is graphics device
        gc = gd.getDefaultConfiguration(); // what is graphics config

        // Create off-screen drawing surface
        buffer = gc.createCompatibleImage(width, height);

        // Objects needed for rendering
        graphics = null;
        g2D = null;
    }

    public static void runAnimation() {

        // Vars for debug stats
        int fps = 0;
        int frames = 0;
        long totalTime = 0;
        long curTime = System.currentTimeMillis();
        long lastTime = curTime;

        // Main Loop
        while (isRunning) {
            try {
                // Calculate frames
                lastTime = curTime;
                curTime = System.currentTimeMillis();
                totalTime += curTime - lastTime;
                if (totalTime > 1000) {
                    totalTime -= 1000;
                    fps = frames;
                    frames = 0;
                }
                ++frames;

                // Adds adjustable walls, can be taken out if problematic
                width = frame.getWidth();
                height = frame.getHeight();
                canvas.setSize(width, height);
                buffer = gc.createCompatibleImage(width, height);

                // clear back buffer
                g2D = buffer.createGraphics();
                g2D.setColor(Color.WHITE);
                g2D.fillRect(0, 0, width, height);

                // draw entities
                for (int i = 0; i < world.size(); i++) {
                    at = new AffineTransform();
                     at.translate(world.get(i).getX(), world.get(i).getY()); // get coords of entity
                    Entity entity = world.get(i);
                    g2D.setColor(Color.GREEN);
                    // for circles
                    if (entity instanceof Ball) {
                        g2D.fill(new Ellipse2D.Double(entity.getX(), entity.getY(), ((Ball) entity).getRadius() * 2, ((Ball) entity).getRadius() * 2));
                    }
                    if (entity instanceof Square) {
                        g2D.fill(new Rectangle2D.Double(entity.getX(), entity.getY(), ((Square) entity).getWidth(), ((Square) entity).getHeight()));
                    }
                }

                // display debug stats in frame
                g2D.setFont(new Font("Courier New", Font.PLAIN, 12));
                g2D.setColor(Color.GREEN);
                g2D.drawString(String.format("FPS: %s", fps), 20, 20);
                graphics = b.getDrawGraphics();
                graphics.drawImage(buffer, 0, 0, null);
                if (!b.contentsLost()) b.show();
                // Let the OS have a little time
                Thread.sleep(15);
            } catch (InterruptedException ie) {
            } finally {
                // release resources
                if (graphics != null) graphics.dispose();
                if (g2D != null) g2D.dispose();
            }
        }
    }
}
