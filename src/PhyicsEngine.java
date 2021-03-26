import java.util.ArrayList;

public class PhyicsEngine extends Thread {
  private long timePassed = 0;
  private long curTime = 0;
  private long lastTime = 0;
  private double timeFraction = 0.0;
  private ArrayList<Accel> constForces = new ArrayList<>();

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

  private void updateTime() {
    lastTime = curTime;
    curTime = System.currentTimeMillis();
    timePassed = (curTime - lastTime);
    timeFraction = (timePassed / 1000.0);
  }

  private void initializeConstForces() {
    constForces.add(new Accel(0.0, Main.GRAVITY));
  }

  private synchronized void applyConstForces() {
    double xAccel = 0, yAccel = 0;

    // Find total acceleration of all const forces
    for (int i = 0; i < constForces.size(); i++) {
      xAccel += constForces.get(i).ax();
      yAccel += constForces.get(i).ay();
    }

    // Apply the sum acceleration to each entity
    for (int i = 0; i < Main.world.size(); i++) {
      Entity entity = Main.world.get(i);
      entity.addAccel(new Accel(xAccel, yAccel));
    }
  }

  // Calculates the forces/vectors on the objects
  private synchronized void sumForces() {
    for (int i = 0; i < Main.world.size(); i++) {
      Entity entity = Main.world.get(i);
      // Get the sum of all accelerations acting on object
      Accel theAccel = entity.sumAccel();
      // Apply the resulting change in velocity
      double vx = entity.vx() + (theAccel.ax() * timeFraction);
      double vy = entity.vy() + (theAccel.ay() * timeFraction);
      entity.updateVelocity(vx, vy);
      // Apply drag coefficient
      entity.applyDrag(1.0 - (timeFraction * Main.DRAG));
    }
  }

  // Calculates and moves the objects based on previously
  // calculated forces to the proper coordinates
  private synchronized void moveEnts() {
    for (int i = 0; i < Main.world.size(); i++) {
      Entity entity = Main.world.get(i);
      // get the initial x and y coordinates
      double oldX = entity.getX();
      double oldY = entity.getY();
      // calculate the new x and y coordinates
      double newX = oldX + (entity.vx() * timeFraction);
      double newY = oldY + (entity.vy() * timeFraction);
      entity.updatePos(newX, newY);
//      checkWallCollisions(entity);
    }
    // checkCollisions();
  }

//  private synchronized void checkWallCollisions(Entity entity) {
//    int maxX = Main.width - entity.
//  }
}
