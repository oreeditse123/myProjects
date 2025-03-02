/**
 * 
 */
package acsse.csc2a.gui;

import java.util.ArrayList;

import acsse.csc2a.model.DrawEntityVisitor;
import acsse.csc2a.model.GameEntity;
import javafx.scene.canvas.Canvas;
import javafx.scene.canvas.GraphicsContext;
import javafx.scene.paint.Color;

/**
 * A class for providing a canvas to draw game entities on 
 * 
 * @author Oreeditse Tlou
 *
 */
public class GameCanvas extends Canvas {
	
	/**
	 * DrawEntityVisitor instance for visiting the canvas and getting the GraphicsContext instance
	 */
	private DrawEntityVisitor visitor = null;
	
	/**
	 * An arrayList containing game entities
	 */
	private ArrayList<GameEntity> entity = null;
	
	
	//Variables for the game
	final int originalSize = 12;//Original tile size
	final int scaler = 3;//Scaler 
	private final int TILE_SIZE = originalSize * scaler;//36x36
	
	final int MAX_COLS = 18;
	final int MAX_ROWS = 16;
	private final int MAX_WIDTH = MAX_COLS * TILE_SIZE;//648
	private final int MAX_HEIGHT = MAX_ROWS * TILE_SIZE;//576
	
	/**
	 * Default constructor of the canvas
	 */
	public GameCanvas() {
		
		//Instantiating the visitor
		visitor = new DrawEntityVisitor();
		//Instantiating the ArrayList
		entity = new ArrayList<>();
		
		//Setting the width and height
		setWidth(MAX_WIDTH);
		setHeight(MAX_HEIGHT);
	}
	
	
	/**
	 * A method for drawing entities
	 * 
	 * @param entity
	 */
	public void drawEntity(ArrayList<GameEntity> entity) {
		this.entity = entity;
		redrawCanvas();
	}
	
	/**
	 * The redrawCanvas method for drawing the actual entities on the canvas
	 */
	private void redrawCanvas() {
		GraphicsContext gc = getGraphicsContext2D();
		
		gc.clearRect(0, 0, MAX_WIDTH, MAX_HEIGHT);
		gc.setFill(Color.BLACK);
		gc.fillRect(0, 0, MAX_WIDTH, MAX_HEIGHT);
		
		//setting the visitor's GraphicsContext
		visitor.setGraphicsContext(gc);
		
		for(GameEntity g : entity) {
			g.accept(visitor);
		}
		
	}
	
	/**
	 * An accesser for returning the tile size
	 * 
	 * @return TILE_SIZE
	 */
	public int getTileSize() {
		return TILE_SIZE;
	}
	
	/**
	 * An accesser for returning the max width
	 *  
	 * @return MAX_WIDTH
	 */
	public int getMaxWidth() {
		return MAX_WIDTH;
	}
	
	/**
	 * An accesser for returning the max height
	 * 
	 * @return MAX_HEIGHT
	 */
	public int getMaxHeight() {
		return MAX_HEIGHT;
	}

	
}

















