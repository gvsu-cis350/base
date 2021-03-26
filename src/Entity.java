import java.util.ArrayList;

public class Entity {
  protected double x, y, vx, vy;
  protected ArrayList<Accel> accelerations = new ArrayList<>();

  public Entity(double x, double y, double vx, double vy) {
    this.x = x;
    this.y = y;
    this.vx = vx;
    this.vy = vy;
  }

  // Squares
  public Entity(double x, double y, double vx, double vy, double w, double h) {

  }

  // Balls
  public Entity(double x, double y, double vx, double vy, double r) {

  }

  public void applyDrag(double drag) {
    this.vx = (drag * this.vx);
    this.vy = (drag * this.vy);
  }

  public Accel sumAccel() {
    double xAccel = 0, yAccel = 0;
    for (int i = 0; i < this.accelerations.size(); i++) {
      xAccel += this.accelerations.get(i).ax();
      yAccel += this.accelerations.get(i).ay();
    }
    this.accelerations.clear();
    return new Accel(xAccel, yAccel);
  }

  public void addAccel(Accel a) {
    this.accelerations.add(a);
  }

  public void updateVelocity(double vx, double vy) {
    this.vx = vx;
    this.vy = vy;
  }

  public void updatePos(double newX, double newY) {
    this.x = newX;
    this.y = newY;
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
