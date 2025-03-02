/**
 * 
 */
package acsse.csc2a.gui;

import java.io.File;
import java.util.ArrayList;

import acsse.csc2a.file.GameFileHandler;
import acsse.csc2a.model.GameEntity;
import acsse.csc2a.model.ObjectPool;
import acsse.csc2a.model.Obstacle;
import acsse.csc2a.model.Player;
import acsse.csc2a.model.Wizard;
import acsse.csc2a.utility.KeyHandler;
import javafx.animation.AnimationTimer;
import javafx.event.ActionEvent;
import javafx.event.EventHandler;
import javafx.scene.control.Menu;
import javafx.scene.control.MenuBar;
import javafx.scene.control.MenuItem;
import javafx.scene.control.SeparatorMenuItem;
import javafx.scene.input.KeyCode;
import javafx.scene.input.KeyEvent;
import javafx.scene.layout.BorderPane;
import javafx.scene.layout.StackPane;
import javafx.scene.layout.VBox;
import javafx.stage.FileChooser;

/**
 * The class that contains all the controls and game entities
 * 
 * @author Oreeditse Tlou
 *
 */
public class GamePane extends StackPane {
	
	/**
	 * A BorderPane instance to place all controls on the layout
	 */
	private BorderPane layout = null;
	
	/**
	 * A MenuBar instance for placing the menu items on
	 */
	private MenuBar menuBar = null;
	
	/**
	 * A GameCanvas instance for placing the entities on the canvas
	 */
	private GameCanvas canvas = null;
	
	/**
	 * Creating a Player instance to place the player on the canvas
	 */
	private Player player = null;
	
	/**
	 * Creating a Wizard instance to be placed on the canvas 
	 */
	private Wizard wizard = null;
	
	
	/**
	 * A Obstacle instance to be placed onto the canvas
	 */
	private Obstacle wall = null;
	
	/**
	 * A ObjectPool instance to add the game entities (player, wizard and wall) to the pool for later re-use
	 */
	private ObjectPool object = null;
	
	/**
	 * 
	 */
	private ArrayList<GameEntity> entity = null;
	
	/**
	 * An AnimationTimer for redrawing in specified intervals pre second
	 */
	private AnimationTimer timer = null;
	
	/**
	 * A keyHandler instance
	 */
	private KeyHandler keyHandler = null;
	
	
	/**
	 * A default constructor for the GamePane class
	 */
	public GamePane() {
		
		//Instantiating the BorderPane
		layout = new BorderPane();
		
		//Instantiating the GameCanvas
		canvas = new GameCanvas();
		
		player = new Player(0, 0);
		wizard = new Wizard(300, 300);
		wall = new Obstacle();
		
		entity = new ArrayList<>();
		
		entity.add(player);
		entity.add(wizard);
		entity.add(wall);
		
		//Instantiating the MenuBar
		menuBar = new MenuBar();
		createMenubar(menuBar);
		
		
		keyHandler = new KeyHandler();
		
		timer = new AnimationTimer() {
			
			@Override
			public void handle(long arg0) {
				canvas.requestFocus();
				
				update();
			}
		};
				
	}
	
	private void createMenubar(MenuBar menuBar) {
		
		//Crating a menu and adding it to the Menubar
		Menu menuOption = new Menu("Option");
		menuBar.getMenus().add(menuOption);
		
		//Creating menu items and adding them to the Menu
		MenuItem startItem = new MenuItem("Start Game");
		MenuItem saveItem = new MenuItem("Save Game");
		MenuItem endItem = new MenuItem("End Game");
		menuOption.getItems().addAll(startItem, new SeparatorMenuItem(), saveItem, new SeparatorMenuItem(), endItem);
		
		//Creating an action listener for starting the game
		startItem.setOnAction(e -> {
			drawEntity();
		});
		
		
		///Creating an action listener for saving a game
		saveItem.setOnAction(e -> {
			
			FileChooser fc = new FileChooser();
			fc.setInitialDirectory(new File("data"));
			fc.setTitle("Choose a file");
			
			final File file = fc.showOpenDialog(null);
			if(file != null) {
				entity = GameFileHandler.readEntities(file);
				System.out.println("Saved Successfully");
			}else {
				System.err.println("Invalid file!!!!");
			}
		});
		
		//Creating an action listener for ending a game
		endItem.setOnAction(new EventHandler<ActionEvent>() {
			
			@Override
			public void handle(ActionEvent event) {
				endGameState();
			}
		});
		
		
		VBox root = new VBox();//Creating a VBox
		root.getChildren().add(menuBar);//adding the canvas to the VBox
		root.getChildren().add(layout);//Adding the BorderPane to the VBox
		this.getChildren().add(root);//Adding the VBox to the StackPane
		
	}
	
	/**
	 * A method for drawing entities
	 */
	private void drawEntity() {
		layout.setCenter(canvas);

		
		//Delegating the drawing to the GameCanvas' redrawCanvas method to draw the entities
		canvas.drawEntity(entity);
	}
	
	/**
	 * 
	 */
	public void endGameState() {
		System.out.println("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~GAME ENDED~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
	}
	
	
	public void update() {
		MovePlayer();
	}
	
	/**
	 * A method for moving the player
	 */
	public void MovePlayer() {
		player.movePlayer();
	}
	
	
	
}


















