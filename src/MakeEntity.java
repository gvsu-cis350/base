import java.awt.geom.Point2D;

/**
 * Make Entity is in charge of adding entities to the ArrayList world
 */
public class MakeEntity extends Thread {

  /**
   * Temporary solution to fill the world with entities. Will switch to other methods later
   */
  public void run() {
    while (Main.isRunning) {
      // Spawn a Ball
      Main.giveBirth(new Ball(Main.width / 2.0, Main.height / 2.0,
          Math.random() * 1000.0, Math.random() * 1000.0, 25));

      // Spawn a Square
      Main.giveBirth(new Square(Main.width / 2.0, Main.height / 2.0,
          Math.random() * 1000.0, Math.random() * 1000.0, 25, 50, 50));
    }
    try {
      // Sleeps for the given spawn rate in Main
      sleep(Main.RATE);
    } catch (InterruptedException e) {
    }
  }


  /**
   * Currently unused. Will be useful in spawning a Ball at mouse's position
   * @param p1 mouse position
   */
  public void makeBall(Point2D p1) {
    Main.giveBirth(new Ball(Main.width / 2.0, Main.height / 2.0,
        Math.random() * 1000.0, Math.random() * 1000.0, 25));
  }

  /**
   * Currently unused. Will be useful in spawning a Square at mouse's position
   * @param p1 mouse position
   */
  public void makeSquare(Point2D p1) {
    Main.giveBirth(new Square(Main.width / 2.0, Main.height / 2.0,
        Math.random() * 1000.0, Math.random() * 1000.0, 25, 50, 50));
  }
}
