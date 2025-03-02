/**
 * 
 */
package acsse.csc2a.model;

import javafx.scene.canvas.GraphicsContext;

/**
 * A class implementing the IEntityVisitor interface and providing implementation of the provided methods
 * 
 * @author Oreeditse Tlou
 *
 */
public class DrawEntityVisitor implements IEntityVisitor {
	/**
	 * Tile size
	 */
	int TILE_SIZE = 36;
	
	/**
	 * A GrapgicsContext instance
	 */
	private GraphicsContext gc;
	
	/**
	 * default constructor
	 */
	public DrawEntityVisitor() {
		gc = null;
	}
	
	/**
	 * A method for setting the GraphicsContext instance
	 * 
	 * @param gc
	 */
	public void setGraphicsContext(GraphicsContext gc) {
		this.gc = gc;
	}


	@Override
	public void drawPlayer(Player player) {
		//gc.setFill(Color.WHITE);
		gc.drawImage(player.getImage(), player.getX(), player.getY(), TILE_SIZE, TILE_SIZE);
		
	}


	@Override
	public void drawWizard(Wizard wizard) {
		gc.drawImage(wizard.getImage(), wizard.getX(), wizard.getY(), TILE_SIZE, TILE_SIZE);
	}


	@Override
	public void drawObstacle(Obstacle obstacle) {
		gc.drawImage(obstacle.getImage(), obstacle.getX(), obstacle.getY(), TILE_SIZE, TILE_SIZE);
	}
	
	
}
