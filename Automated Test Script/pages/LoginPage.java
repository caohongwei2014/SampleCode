package info.itest.www.pages;

import org.openqa.selenium.By;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.WebElement;

public class LoginPage extends BasePage {

	public LoginPage(WebDriver driver, String url) {
		super(driver);
		this.url = url;
		this.goTo();
	}

	By usernameLocator = By.id("user_login");
	By passwordlocator = By.id("user_pass");
	By loginButtonLocator = By.id("wp-submit");
	By loginErrorLocator = By.id("login_error");
	
	public WebElement getUserNameTextField(){
		return this.dr.findElement(usernameLocator);		
	}
	
	public WebElement getPassWordField(){
		return this.dr.findElement(passwordlocator);		
	}
	
	public WebElement getSubmitButton(){
		return this.dr.findElement(loginButtonLocator);		
	}
	
	public WebElement LoginErrorDiv(){
		return this.dr.findElement(loginErrorLocator);
	}
	
	public DashPage login(String userName, String passWord) throws InterruptedException{
		
		this.doLoginStep(userName, passWord);
		return new DashPage(this.dr);
	}
	
	public LoginPage loginFailed(String userName, String passWord) throws InterruptedException{
		this.doLoginStep(userName, passWord);
		return this;
	}
	
	
	private void doLoginStep(String userName, String passWord) throws InterruptedException{
		this.getUserNameTextField().sendKeys(userName);
		Thread.sleep(1000);
		this.getPassWordField().sendKeys(passWord);
		Thread.sleep(1000);
		this.getSubmitButton().click();
	}	

}
