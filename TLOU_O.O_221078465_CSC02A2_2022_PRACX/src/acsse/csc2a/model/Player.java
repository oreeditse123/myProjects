/**
 * 
 */
package acsse.csc2a.model;


import acsse.csc2a.utility.KeyHandler;
import javafx.scene.image.Image;


/**
 * A player entity class which is a type of game entity
 * 
 * @author Oreeditse Tlou
 *
 */
public class Player extends GameEntity {
	
	/**
	 * Player x position
	 */
    int playerX;
    
    /**
     * Player y position
     */
	int playerY;
	
	private KeyHandler keyHandler=null;
	
	
	
	/**
	 * A constructor providing the x and y position of the player
	 * 
	 * @param xPos
	 * @param yPos
	 */
	public Player(int xPos, int yPos ) {
		super(xPos, yPos);
		this.playerX = xPos;
		this.playerY = yPos;
		setInitalValues();
		
		
		Image up1 = new Image("./sprite/boy_down_1.png");
		this.image = up1;
		
		keyHandler = new KeyHandler();
	}
	
	/**
	 * The accept method provided by the IVisitableEntity interface
	 */
	@Override
	public void accept(IEntityVisitor visitor) {
		visitor.drawPlayer(this);
	}       
	
	public void movePlayer() {
		this.direction = keyHandler.direction;
		if(KeyHandler.isKeyPressed()) {
			switch(direction) {
			case "up" :
				playerY += entitySpeed;
				break;
			case "down" :
				playerY -= entitySpeed;
				break;
			case "left" : 
				playerX -= entitySpeed;
				break;
			case "right" :
				playerX += entitySpeed;
				break;
			}
		}
	}
	
	
	/**
	 * An overridable method for determining the health of the player
	 */
	@Override
	public void healthBar(int healthStatus, int damagepoints) {
		
	}

}











