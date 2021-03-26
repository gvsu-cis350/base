import java.awt.geom.Point2D;

public class Square extends Entity {
  private double w, h;

  public Square(double x, double y, double vx, double vy, double w, double h) {
    super(x, y, vx, vy);
    this.w = w;
    this.h = h;
  }

  public Point2D getCenter() {
    double w = this.w;
    double h = this.h;
    double c;
    c = w*w + h*h;
    c = Math.sqrt(c);
    c = c/2;
    // casting as int because idk the other guy did
    return new Point2D.Double(this.x + (int)c, this.y + (int)c);
  }

  public double getWidth() {
    return this.w;
  }

  public double getHeight() {
    return this.h;
  }
}
