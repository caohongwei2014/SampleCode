package info.itest.www.pages;

import org.openqa.selenium.By;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.WebElement;

public class HomePage extends BasePage {
	
	
	public HomePage(WebDriver driver, String url){
		super(driver);
		this.url =url;
		this.goTo();
	}
	
	By firstPostLocator = By.cssSelector(".entry-title a");
	
	public WebElement getFirstPostLink(){
		return this.dr.findElement(firstPostLocator);		
	}
}
