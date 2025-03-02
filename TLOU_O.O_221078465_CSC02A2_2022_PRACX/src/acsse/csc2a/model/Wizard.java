/**
 * 
 */
package acsse.csc2a.model;

/**
 * A wizard entity class which is a type of game entity
 * 
 * @author Oreeditse Tlou
 *
 */
public class Wizard extends GameEntity {

	public Wizard(int xPos, int yPos) {
		super(xPos, yPos);
		// TODO Auto-generated constructor stub
	}

	@Override
	public void accept(IEntityVisitor visitor) {
		visitor.drawWizard(this);
	}

	

	@Override
	public void healthBar(int healthStatus, int damagepoints) {
		// TODO Auto-generated method stub
		
	}

}
