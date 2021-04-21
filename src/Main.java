import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.geom.AffineTransform;
import java.awt.geom.Ellipse2D;
import java.awt.geom.Rectangle2D;
import java.awt.image.BufferStrategy;
import java.awt.image.BufferedImage;
import java.util.ArrayList;
import javax.swing.*;

/**************************************************************************************
 *
 *     GUI-Dancer is a small physics simulator in which users can experiment with
 * small spheres with various different physical constants. This is our submission
 * for the CIS 350 Final Project.
 *
 * @author Evan Johns
 * @author Jacquelin Jimenez
 * @author Abigail McDonald
 * @author Donald Finn
 *
 * @version 05/20/2021
 *
 **************************************************************************************/
public class Main {

    public static final int MAX_SPAWN = 20;
    public static final int RATE = 20;
    public static final int GRAVITY = 0;
    public static final double DRAG = 0.00;
    public static final double BOUNCE = .99;
    public static boolean DRAW_DANCE = true;
    public static boolean DRAW_DEBUG = false;

    public static int width;
    public static int height;
    public static Point mPoint;
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
    public static boolean isRunning = false;

    private static JMenuBar menus;
    private static JMenu actionMenu;
    private static JMenuItem danceToggleItem;
    private static JMenuItem debugItem;

    /**
     * Initialize Frame, start threads, and begin animation loop.
     */
    public static void main(String[] args){
        isRunning = true;
        width = 600;
        height = 600;
        initializeFrame();
        Thread PhysicsEngine = new PhysicsEngine();
        PhysicsEngine.start();
        Thread makeLife = new MakeEntity();
        makeLife.start();
        runAnimation();
    }

    /**
     * Adds a Square to the ArrayList world
     * @param square Square to be added
     * @return if list is full, send 0
     */
    public static synchronized int giveBirth(Square square) {
        if (world.size() >= MAX_SPAWN) return 1;
        world.add(square);
        return 0;
    }

    /**
     * Adds a Ball to the ArrayList world
     * @param ball Ball to be added
     * @return if list is full, send 0
     */
    public static synchronized int giveBirth(Ball ball) {
        if (world.size() >= MAX_SPAWN) return 1;
        world.add(ball);
        return 0;
    }

    /**
     * Initializes the JFrame and graphics utilities used in drawing to the Frame
     */
    public static void initializeFrame() {
        frame = new JFrame("Demo");
        frame.setIgnoreRepaint(true); //what does this do?
        frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);

        // Menu bar setup
        menus = new JMenuBar();
        actionMenu = new JMenu("Settings");
        danceToggleItem = new JMenuItem("Toggle Dance");
        debugItem = new JMenuItem("Debug Stats");

        actionMenu.add(danceToggleItem);
        actionMenu.add(debugItem);
        menus.add(actionMenu);

        // Action Listener for danceToggleItem
        danceToggleItem.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                DRAW_DANCE = !DRAW_DANCE;
            }
        });

        debugItem.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                DRAW_DEBUG = !DRAW_DEBUG;
            }
        });



        // Add Menu bar to frame
        frame.setJMenuBar(menus);

        // Create canvas (used for painting to frame)
        canvas = new Canvas();
        canvas.setIgnoreRepaint(true); //again what is ignoreRepaint?
        canvas.setSize(width,height);
        canvas.setBackground(Color.DARK_GRAY);

        // Add to display
        Container content = new Container();
        content.add(canvas);
        content.setPreferredSize(new Dimension(width, height));
        frame.setContentPane(content);
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

    /**
     * Main loop of the program. Updates the Frame with each entity's new position every iteration
     */
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

                // Get mouse location

                if (frame.getMousePosition() != null) mPoint = frame.getMousePosition();

                // Adds adjustable walls, can be taken out if problematic
                width = frame.getWidth();
                height = frame.getHeight();
                canvas.setSize(width, height);
                buffer = gc.createCompatibleImage(width, height);

                // clear back buffer
                g2D = buffer.createGraphics();
                g2D.setColor(Color.GRAY);
                g2D.fillRect(0, 0, width, height);

                if (DRAW_DANCE) Dancer.drawDancer(g2D, frames/20);

                // draw entities
                for (int i = 0; i < world.size(); i++) {
                    at = new AffineTransform();
                    at.translate(world.get(i).getX(), world.get(i).getY()); // get coords of entity
                    Entity entity = world.get(i);
                    g2D.setColor(Color.BLACK);
                    // for circles
                    if (entity instanceof Ball) {
                        g2D.fill(new Ellipse2D.Double(entity.getX(), entity.getY(), ((Ball) entity).getRadius() * 2, ((Ball) entity).getRadius() * 2));
                    }
                    if (entity instanceof Square) {
                        g2D.fill(new Rectangle2D.Double(entity.getX(), entity.getY(), ((Square) entity).getWidth(), ((Square) entity).getHeight()));
                    }
                }

                // display debug stats in frame
                if (DRAW_DEBUG) {
                    g2D.setFont(new Font("Courier New", Font.PLAIN, 12));
                    g2D.setColor(Color.GREEN);
                    g2D.drawString(String.format("FPS: %s", fps), 20, 20);
                    g2D.drawString(String.format("X: %s", mPoint.x), 20, 30);
                    g2D.drawString(String.format("Y: %s", mPoint.y), 20, 40);
                }
                graphics = b.getDrawGraphics();
                graphics.drawImage(buffer, 0, 0, null);
                if (!b.contentsLost()) b.show();

                // Let the OS have a little time
                Thread.sleep(15);
            } catch (InterruptedException ie) {
            } catch (NullPointerException ne) {
            } finally {
                // release resources
                if (graphics != null) graphics.dispose();
                if (g2D != null) g2D.dispose();
            }
        }
    }
}
