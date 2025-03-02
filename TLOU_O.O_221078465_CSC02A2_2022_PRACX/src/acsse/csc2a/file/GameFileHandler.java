/**
 * 
 */
package acsse.csc2a.file;

import java.io.File;
import java.io.FileNotFoundException;
import java.util.ArrayList;
import java.util.Scanner;
import java.util.StringTokenizer;

import acsse.csc2a.model.GameEntity;
import acsse.csc2a.model.Player;
import acsse.csc2a.model.Wizard;

/**
 * The class for reading and writing to text files
 * 
 * @author Oreeditse Tlou
 *
 */
public class GameFileHandler {
	
	public static ArrayList<GameEntity> readEntities(File entityFile) {
		ArrayList<GameEntity> entity =  new ArrayList<>();
		
		try(Scanner input = new Scanner(entityFile)){
			String line = input.nextLine();
			
			StringTokenizer tokenizer = new StringTokenizer(line, "\t");
			
			if(tokenizer != null) {
				
				int playerX = Integer.parseInt(tokenizer.nextToken());
				int playerY = Integer.parseInt(tokenizer.nextToken());
				
				Player player = new Player(playerX, playerY);
				entity.add(player);
		
			}
				while(input.hasNextLine()) {
					line = input.nextLine();
					
					int wizardX = Integer.parseInt(tokenizer.nextToken());
					int wizardY = Integer.parseInt(tokenizer.nextToken());
					
					Wizard wizard = new Wizard(wizardX, wizardY);
					entity.add(wizard);
					
				}
				
		} 
		catch (FileNotFoundException e) {
			e.printStackTrace();
		}
		
		return entity;
	}
	
	
	
}








