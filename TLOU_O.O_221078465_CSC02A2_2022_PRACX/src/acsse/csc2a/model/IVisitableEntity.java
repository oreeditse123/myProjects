/**
 * 
 */
package acsse.csc2a.model;

/**
 * An interface for providing the accept method
 * 
 * @author Oreeditse Tlou
 *
 */
public interface IVisitableEntity {
	/**
	 * A method for accepting a IEntityVisitor
	 */
	public void accept(IEntityVisitor visitor);
}
