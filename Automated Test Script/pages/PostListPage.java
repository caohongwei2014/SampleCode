package info.itest.www.pages;

import org.openqa.selenium.By;
import org.openqa.selenium.NoSuchElementException;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.interactions.Actions;

public class PostListPage extends BasePage {
	
	public PostListPage(WebDriver driver){
		super(driver);		
	}
	
	By greetingLocator =By.cssSelector("#wp-admin-bar-my-account .ab-item");
	
	public void deletPost(String postId){
		this.dr.get("http://localhost/wordpress/wp-admin/edit.php");
		String rowId= "post-"+postId;
		WebElement row = this.dr.findElement(By.id(rowId));
		Actions action = new Actions(this.dr);
		action.moveToElement(row).perform();
		row.findElement(By.cssSelector(".trash a")).click();
	}
	
	
	public Boolean findRowId(String rowId){
		this.dr.get("http://localhost/wordpress/wp-admin/edit.php");
		try{
			this.dr.findElement(By.id(rowId));
		}catch(NoSuchElementException e){
			return true;
		}
		return false;		
	}
}
