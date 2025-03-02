/**
 * 
 */
package acsse.csc2a.model;

import java.util.ArrayList;

/**
 * An ObjectPool class for acquiring and releasing game entities
 * 
 * @author Oreeditse Tlou
 *
 */
public class ObjectPool {
	/**
	 * Creating an instance of the ObjectPool
	 */
	private static ObjectPool object = null;
	
	/**
	 * Creating an instance of GameEntity 
	 */
	private ArrayList<GameEntity> entities = null;
	
	/**
	 * Default constructor
	 */
	private ObjectPool() {
		
		entities = new ArrayList<>();
	
	}
	
	/**
	 * A static method for getting the Instance of ObjectPool
	 * @return ObjectPool instance
	 */
	public static ObjectPool getInstance() {
		if(object == null) {
			object = new ObjectPool();
		}
		
		return object;
	}
	
	/**
	 * A method for getting a GameEntity instance
	 * 
	 * @return entity
	 */
	public GameEntity getEntity(){
		
		if(entities.isEmpty()){//No Reusables to give
		    return null;
		}else{
		   GameEntity temp = entities.get(entities.size() - 1); //Get lastReusable
		   
		   entities.remove(entities.size() -1); //Remove from pool
		   return temp; //Return
		   
		}
	}
	
	/**
	 * A method for releasing the GameEntity instance
	 * 
	 * @param entity
	 */
	public void removeEntity(GameEntity entity) {
		if(entity != null){
			
			entities.add(entity);//Add Reusable to the Pool
		 
		}else {
			 System.out.println("Can't add nothing to the Pool!");
	    }
	}
}

