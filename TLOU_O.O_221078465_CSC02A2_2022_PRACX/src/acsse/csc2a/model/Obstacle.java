/**
 * 
 */
package acsse.csc2a.model;

/**
 * A obstacle entity class which is a type of entity
 * 
 * @author Oreeditse Tlou
 *
 */
public class Obstacle extends GameEntity {

	@Override
	public void accept(IEntityVisitor visitor) {
		visitor.drawObstacle(this);
	}

	

	@Override
	public void healthBar(int healthStatus, int damagepoints) {
		// TODO Auto-generated method stub
		
	}

}
