import java.awt.geom.Point2D;
import java.util.ArrayList;

/**
 * Physics Engine handles all physics calculations for each entity in the Frame.
 *
 * Parts used from User wilkystyle on GitHub:
 * https://github.com/wilkystyle/java2dphysicsengine/blob/master/MoveEngine.java
 */
public class PhysicsEngine extends Thread {

  private long timePassed = 0;
  private long curTime = 0;
  private long lastTime = 0;
  private double timeFraction = 0.0;
  private ArrayList<Accel> constForces = new ArrayList<>();

  /**
   * Begins the PhysicsEngine Thread
   */
  public void run() {
    curTime = System.currentTimeMillis();
    initializeConstForces();
    while (Main.isRunning) {
      updateTime();
      applyConstForces();
      sumForces();
      moveEnts();
      try {
        sleep(1);
      } catch (InterruptedException e) {
      }
    }
  }

  /**
   * Updates time variables
   */
  private void updateTime() {
    lastTime = curTime;
    curTime = System.currentTimeMillis();
    timePassed = (curTime - lastTime);
    timeFraction = (timePassed / 1000.0);
  }

  /**
   * Creates a fundamental universal principle in one line of code; Kappa
   */
  private void initializeConstForces() {
    constForces.add(new Accel(0.0, Main.GRAVITY));
  }

  /**
   * Applies Gravity to each entity in ArrayList world
   */
  private synchronized void applyConstForces() {
    double xAccel = 0, yAccel = 0;

    // Find the total acceleration of all const forces.
    for (Accel constForce : constForces) {
      xAccel += constForce.ax();
      yAccel += constForce.ay();
    }

    // Apply the sum acceleration to each entity.
    for (int i = 0; i < Main.world.size(); i++) {
      Entity s = Main.world.get(i);
      s.addAccel(new Accel(xAccel, yAccel));
    }
  }

  /**
   * Calculates the forces/vectors on the objects
   */
  private synchronized void sumForces() {
    for (int i = 0; i < Main.world.size(); i++) {
      Entity s = Main.world.get(i);

      // Get the sum of all accelerations acting on object.
      Accel theAccel = s.sumAccel();

      // Apply the resulting change in velocity.
      double vx = s.vx() + (theAccel.ax() * timeFraction);
      double vy = s.vy() + (theAccel.ay() * timeFraction);
      s.updateVelocity(vx, vy);

      // Apply drag coefficient
      s.applyDrag(1.0 - (timeFraction * Main.DRAG));
    }
  }

  /**
   * Calculates and moves the objects based on the
   * previously calculated forces to the proper coordinates
   */
  private synchronized void moveEnts() {
    for (int i = 0; i < Main.world.size(); i++) {
      Entity s = Main.world.get(i);

      // get the initial x and y coordinates
      double oldX = s.getX();
      double oldY = s.getY();

      // calculate the new x and y coordinates
      double newX = oldX + (s.vx() * timeFraction);
      double newY = oldY + (s.vy() * timeFraction);
      s.updatePos(newX, newY);

      // Looks for any all collisions
      checkWallCollisions(s);
    }
    // Looks for entity-entity collisions
    checkCollisions();
  }

  /**
   * Checks for all collisions between entities
   */
  private synchronized void checkCollisions() {
    for (int i = 0; i < Main.world.size(); i++) {
      Entity s = Main.world.get(i);
      Point2D sCenter = s.getCenter();
      for (int j = i + 1; j < Main.world.size(); j++) {
        Entity t = Main.world.get(j);

        // If at end of list, break
        if (t == null) break;
        Point2D tCenter = t.getCenter();

        // Calculate distance between centers
        double distBetween = sCenter.distance(tCenter);

        // Check that distance between centers is larger than sum of radii
        double bigR = Math.max(s.getRadius(), t.getRadius());
        if (distBetween < (bigR * 2)) collide(s, t, distBetween);
      }
    }
  }

  /**
   * Calculates a collision between two entities.
   *
   * Calculations and some documentation inferred from
   * previously credited user wilkystyles on GitHub
   * @param e1 First entity
   * @param e2 Second entity
   * @param distBetween distance between centers
   */
  private synchronized void collide(Entity e1, Entity e2, double distBetween) {
    // Get the relative x and y dist between them.
    double relX = e1.getX() - e2.getX();
    double relY = e1.getY() - e2.getY();

    // Take the arc-tangent to find the collision angle
    double collisionAngle = Math.atan2(relY, relX);

    // Rotate the coordinate systems for each object's velocity to align
    // with the collision angle.
    Vector2D sVel = e1.velVector(), tVel = e2.velVector();
    sVel.rotateCoordinates(collisionAngle);
    tVel.rotateCoordinates(collisionAngle);

    // In the collision coordinate system, the contact normals lie on the x-axis.
    // Only the velocity values along this axis are affected. We can now apply a simple
    // 1D momentum equation where the new x-velocity of the first object equals
    // a negative times the x-velocity of the second
    double swap = sVel.x;
    sVel.x = tVel.x;
    tVel.x = swap;

    // Now we need to get the vectors back into normal coordinate space
    sVel.restoreCoordinates();
    tVel.restoreCoordinates();

    // Give each object its new velocity.
    e1.updateVelocity(sVel.x * Main.BOUNCE, sVel.y * Main.BOUNCE);
    e2.updateVelocity(tVel.x * Main.BOUNCE, tVel.y * Main.BOUNCE);

    // Back them up in the opposite angle so they are not overlapping.
    double minDist = e1.getRadius() + e2.getRadius();
    double overlap = minDist - distBetween;
    double toMove = overlap / 2;

    // Update e1 to new position
    double newX = e1.getX() + (toMove * Math.cos(collisionAngle));
    double newY = e1.getY() + (toMove * Math.sin(collisionAngle));
    e1.updatePos(newX, newY);

    // Update e2 to new position
    newX = e2.getX() - (toMove * Math.cos(collisionAngle));
    newY = e2.getY() - (toMove * Math.sin(collisionAngle));
    e2.updatePos(newX, newY);
  }

  /**
   * Checks for wall collisions between an entity and bounding wall
   * @param e1 Entity
   */
  private synchronized void checkWallCollisions(Entity e1) {
    // Subtract 61 from height because of title and menu bars
    int maxY = (Main.height - 61) - e1.dimY();
    int maxX = Main.width - e1.dimX();

    // Check top-most wall
    if (e1.getY() > maxY) {
      e1.updatePos(e1.getX(), maxY);
      e1.updateVelocity(e1.vx(), (e1.vy() * -Main.BOUNCE));
    }

    // Check right-most wall
    if (e1.getX() > maxX) {
      e1.updatePos(maxX, e1.getY());
      e1.updateVelocity((e1.vx() * -Main.BOUNCE), e1.vy());
    }

    // Check left-most wall
    if (e1.getX() < 1) {
      e1.updatePos(1, e1.getY());
      e1.updateVelocity((e1.vx() * -Main.BOUNCE), e1.vy());
    }

    // Check bottom-most wall
    if (e1.getY() < 1) {
      e1.updatePos(e1.getX(), 1);
      e1.updateVelocity(e1.vx(), (e1.vy() * -Main.BOUNCE));
    }
  }
}
