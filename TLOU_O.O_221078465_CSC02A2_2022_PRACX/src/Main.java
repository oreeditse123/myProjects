import acsse.csc2a.gui.GamePane;

import javafx.application.Application;
import javafx.scene.Scene;
import javafx.stage.Stage;

/**
 * The class that contains the main method and extends Application
 * 
 * @author Oreeditse Tlou
 *
 */
public class Main extends Application {

	@Override
	public void start(Stage primaryStage) throws Exception {
		
		GamePane root = new GamePane();
		
		Scene scene = new Scene(root);
		
		primaryStage.setScene(scene);
		primaryStage.setTitle("Wizard advantures");
		primaryStage.setWidth(684);
		primaryStage.setHeight(648);

		primaryStage.show();
	}

	/**
	 * Main method for every java application
	 * 
	 * @param args
	 */
	public static void main(String[] args) {
		launch(args);
	}

}
