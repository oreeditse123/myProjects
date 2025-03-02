/**
 * 
 */
package acsse.csc2a.model;

/**
 * An interface for providing the  methods for drawing game entities
 * 
 * @author Oreeditse Tlou
 *
 */
public interface IEntityVisitor {
	/**
	 * A method for drawing the player entity
	 */
	public void drawPlayer(Player player);
	
	/**
	 * A method for drawing the Wizard entity
	 */
	public void drawWizard(Wizard wizard);
	
	/**
	 * A method for drawing the obstacles
	 */
	public void drawObstacle(Obstacle obstacle);
}
