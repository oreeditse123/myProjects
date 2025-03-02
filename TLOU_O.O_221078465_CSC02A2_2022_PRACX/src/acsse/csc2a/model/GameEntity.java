/**
 * 
 */
package acsse.csc2a.model;

import javafx.scene.image.Image;

/**
 * Abstract base class for all entities, implements the IVisitableEntity
 * 
 * @author Oreeditse Tlou
 *
 */
public abstract class GameEntity implements IVisitableEntity {
	
	protected int xPosition;
	protected int yPosition;
	protected int entitySpeed;
	protected int healthStatus;
	protected int damgePoints;
	
	Image image = null;
	
	String direction = "";
	
	public GameEntity() {
		setInitalValues();
	}
	
	public GameEntity(int xPos, int yPos) {
		this.xPosition= xPos;
		this.yPosition = yPos;
		setInitalValues();
	}
	
	public GameEntity(int xPos, int yPos, int speed, int status,int damage) {
		this.xPosition = xPos;
		this.yPosition = yPos;
		this.entitySpeed = speed;
		this.healthStatus = status;
		this.damgePoints = damage;
		setInitalValues();
	}
	
	
	//public abstract void moveEntity(int x, int y);
	public abstract void healthBar(int healthStatus, int damagepoints);
	
	
	public int getX() {
		return xPosition;
	}
	
	
	public int getY() {
		return yPosition;
	}
	
	
	public int gethealthStatus() {
		return healthStatus;
	}
	
	
	
	public void setInitalValues() {
		this.xPosition = 100;
		this.yPosition = 100;
		this.entitySpeed = 4;
		this.healthStatus = 100;
		this.damgePoints = 0;
	}
	
    public Image getImage() {
		return image;
	}
	
}















