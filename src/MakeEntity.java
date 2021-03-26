import java.util.Random;

public class MakeEntity extends Thread {

  public void run() {
    while (Main.isRunning) {
        Main.giveBirth(new Ball((Main.width / 2), (Main.height / 2), Math.random() * 1000.0, Math.random() * 1000.0, 50));
        Main.giveBirth(new Square(Main.width / 2, Main.height / 2, Math.random() * 1000.0, Math.random() * 1000.0, 50, 50));
    }
    try {
      sleep(Main.RATE);
    } catch (InterruptedException e) {

    }
  }
}
