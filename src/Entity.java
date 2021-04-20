import java.awt.geom.Point2D;
import java.util.ArrayList;

/**
 * Stores location, size, and acceleration of each object in ArrayList world
 */
public class Entity {
  protected double x, y, vx, vy, r;
  protected ArrayList<Accel> accelerations = new ArrayList<>();

  /**
   * Initializes entity with location, velocity, and size
   * @param x x-coordinate
   * @param y y-coordinate
   * @param vx vector x-coordinate
   * @param vy vector y-coordinate
   * @param r radius of entity hitbox
   */
  public Entity(double x, double y, double vx, double vy, double r) {
    this.x = x;
    this.y = y;
    this.vx = vx;
    this.vy = vy;
    this.r = r;
  }

  /**
   * Creates a vector at given coordinates
   * @return new Vector
   */
  public Vector2D velVector() {
    return new Vector2D(vx, vy);
  }

  /**
   * Applies drag coefficient
   * @param drag drag coefficient
   */
  public void applyDrag(double drag) {
    this.vx = (drag * this.vx);
    this.vy = (drag * this.vy);
  }

  /**
   * Sums all accelerations
   * @return new Accelerations object
   */
  public Accel sumAccel() {
    double xAccel = 0, yAccel = 0;
    for (Accel acceleration : this.accelerations) {
      xAccel += acceleration.ax();
      yAccel += acceleration.ay();
    }
    this.accelerations.clear();
    return new Accel(xAccel, yAccel);
  }

  /**
   * Adds acceleration to list of all accelerations
   * @param a new acceleration
   */
  public void addAccel(Accel a) {
    this.accelerations.add(a);
  }

  /**
   * Updates velocity to new coordinates
   */
  public void updateVelocity(double vx, double vy) {
    this.vx = vx;
    this.vy = vy;
  }

  /**
   * Updates position of entity
   * @param newX new x-coordinate
   * @param newY new y-coordinate
   */
  public void updatePos(double newX, double newY) {
    this.x = newX;
    this.y = newY;
  }

  /**
   * Returns center of the entity as a 2D point objectdd
   * @return Center of entity
   */
  public Point2D getCenter() {
    return new Point2D.Double(this.x + (this.dimX() / 2.0), this.y + (this.dimY() / 2.0));
  }

  /**
   * Calculates diameter of entity as an integer
   *
   * Following two methods are created under consideration of
   * ellipse objects potentially being added
   * @return diameter
   */
  public int dimX() {
    return (int) (this.r * 2);
  }

  /**
   * Calculates diameter of entity as an integer
   * @return diameter
   */
  public int dimY() {
    return (int) (this.r * 2);
  }

  public double getRadius() {
    return this.r;
  }

  public double vx() {
    return this.vx;
  }

  public double vy() {
    return this.vy;
  }

  public double getX() {
    return this.x;
  }

  public double getY() {
    return this.y;
  }

  public void setX(int x) {
    this.x = x;
  }

  public void setY(int y) {
    this.y = y;
  }
}
