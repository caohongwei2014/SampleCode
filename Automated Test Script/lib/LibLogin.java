package info.itest.www.lib;

import org.openqa.selenium.By;
import org.openqa.selenium.WebDriver;

public class LibLogin {
	WebDriver dr;	
	public LibLogin(WebDriver dr){
		this.dr=dr;		
	}
	public void login(String userName, String userPass)
			throws InterruptedException {
		this.dr.get("http://localhost/wordpress/wp-login.php");
		this.dr.findElement(By.id("user_login")).sendKeys(userName);
		Thread.sleep(1000);
		this.dr.findElement(By.id("user_pass")).sendKeys(userPass);
		Thread.sleep(1000);
		this.dr.findElement(By.id("wp-submit")).click();
		}
}
