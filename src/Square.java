/**
 * Square entity used in drawing square to Frame
 */
public class Square extends Entity {
  private double w, h;

  public Square(double x, double y, double vx, double vy, double r, double w, double h) {
    super(x, y, vx, vy, r);
    this.w = w;
    this.h = h;
  }

  public double getWidth() { return this.w; }

  public double getHeight() { return this.h; }
}
