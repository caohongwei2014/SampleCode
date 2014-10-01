package info.itest.www;

import static org.junit.Assert.*;

import org.junit.After;
import org.junit.Before;
import org.junit.Test;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.chrome.ChromeDriver;

import info.itest.www.pages.*;

public class TestCreatePost {
	WebDriver dr;
	final String homePageUrl="http://localhost/wordpress/";;
	final String loginPageUrl="http://localhost/wordpress/wp-login.php";
	
	@Before
	public void setUp() throws Exception {
		System.out.println("Begin test");		
		System.setProperty("webdriver.chrome.driver", "c:\\chromedriver.exe");
		this.dr= new ChromeDriver();		
	}

	@After
	public void tearDown() throws Exception {
		System.out.println("End test");		
		dr.quit();		
	}
	
	@Test
	public void testCreatePost() throws InterruptedException{
		String userName ="admin";
		String userPass = "admin";

		LoginPage theLoginPage = new LoginPage(this.dr,this.loginPageUrl);
		theLoginPage.login(userName, userPass);
		
		String title = "Test tile" + System.currentTimeMillis();
		PostFormPage createPostPage = new PostFormPage(this.dr);
		createPostPage.createPost(title);
		
		HomePage theHomePage = new HomePage(this.dr, this.homePageUrl);
		assertEquals(title,theHomePage.getFirstPostLink().getText());		
	}		
	
}
