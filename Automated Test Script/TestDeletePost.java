package info.itest.www;

import static org.junit.Assert.*;
import info.itest.www.pages.*;

import org.junit.After;
import org.junit.Before;
import org.junit.Test;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.chrome.ChromeDriver;


public class TestDeletePost {
	WebDriver dr;
	String loginPageUrl="http://localhost/wordpress/wp-login.php";

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
	public void testDeletePost() throws InterruptedException{
		String userName ="admin";
		String userPass = "admin";
		
		LoginPage theLoginPage = new LoginPage(this.dr, this.loginPageUrl);
		theLoginPage.login(userName, userPass);
		
		PostFormPage createPostPage = new PostFormPage(this.dr);
		String postId = createPostPage.createPost();
		
		PostListPage postListPage = new PostListPage(this.dr);
		
		postListPage.deletPost(postId);
		assertTrue(postListPage.findRowId(postId));
	
	}
	
}
