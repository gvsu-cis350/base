import java.awt.geom.Point2D;

public class Ball extends Entity {
  private double r;

  public Ball(double x, double y, double vx, double vy, double r) {
    super(x, y, vx, vy);
    this.r = r;
  }

  public int dimX() {
    return (int) (this.r * 2);
  }

  public int dimY() {
    return (int) (this.r * 2);
  }

  public Point2D getCenter() {
    return new Point2D.Double(this.x + (this.dimX() / 2), this.y + (this.dimX()));
  }

  public double getRadius() {
    return this.r;
  }
}
