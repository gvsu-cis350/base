import java.util.ArrayList;

public class Entity {
  private double x, y, vx, vy;
  private ArrayList<Accel> accelerations = new ArrayList<>();

  public Entity(int x, int y, double vx, double vy) {
    this.x = x;
    this.y = y;
    this.vx = vx;
    this.vy = vy;
  }

  // Squares
  public Entity(int x, int y, double vx, double vy, int w, int h) {

  }

  // Balls
  public Entity(int x, int y, double vx, double vy, int r) {

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

  // dimX and dimY are independent to entity

  // getCenter is independent to entity

  // getRadius is independent to entity

  public double getX() {
    return this.x;
  }

  public double getY() {
    return this.y;
  }

  // getX2 is unused and relies on dimX

  // getY2 is unused and relies on dimY

  public void setX(int x) {
    this.x = x;
  }

  public void setY(int y) {
    this.y = y;
  }
}
