/**
 * 
 */
package acsse.csc2a.utility;

import javafx.scene.input.KeyCode;
import javafx.scene.input.KeyEvent;

/**
 * A class for handling user input
 * 
 * @author Oreeditse Tlou
 *
 */
public class KeyHandler {
	
	
	private static boolean wPressed = false;
	private static boolean sPressed = false;
	private static boolean aPressed  = false;
	private static boolean dPressed = false;
	
	public String direction = "";
	
	/**
	 * A default constructor
	 */
	public KeyHandler() {
		
	}
	
	/**
	 * A method for handling key pressed
	 * 
	 * @param event
	 */
	public void keyPressedHandler(KeyEvent event) {
		
		if(event.getCode() == KeyCode.W) {
			wPressed = true;
			direction = "up";
		 }
		
		if(event.getCode() == KeyCode.S) {
			 sPressed = true;
				direction = "down";

		 }
		
		if(event.getCode() == KeyCode.A) {
			 aPressed = true;
				direction = "left";

		 }
		
		if(event.getCode() == KeyCode.D) {
			 dPressed = true;
				direction = "right";

		 }
		
	}
	
	/**
	 * A method for handling releasing input
	 * 
	 * @param event
	 */
	public void keyReleasedHandler(KeyEvent event) {
		
		if(event.getCode() == KeyCode.W) {
			wPressed = false;
		 }
		
		if(event.getCode() == KeyCode.S) {
			 sPressed = false;
		 }
		
		if(event.getCode() == KeyCode.A) {
			 aPressed = false;
		 }
		
		if(event.getCode() == KeyCode.D) {
			 dPressed = false;
		 }
		
	}
	
	public static boolean isKeyPressed() {
		if(wPressed == true ||sPressed == true || aPressed == true || dPressed == true) {
			return true;
		}
		else {
			return false;
		}
		
	}
	
	public static boolean isKeyReleased() {
		if(wPressed == false ||sPressed == false || aPressed == false || dPressed == false) {
			return true;
		}
		else {
			return false;
		}
	}
	
}
	
	