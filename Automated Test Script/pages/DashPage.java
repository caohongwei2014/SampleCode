package info.itest.www.pages;

import org.openqa.selenium.By;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.WebElement;

public class DashPage extends BasePage {
	
	public DashPage(WebDriver driver){
		super(driver);		
	}
	
	By greetingLocator =By.cssSelector("#wp-admin-bar-my-account .ab-item");
	
	public WebElement getGreetingLink(){
		return this.dr.findElement(greetingLocator);		
	}
}
