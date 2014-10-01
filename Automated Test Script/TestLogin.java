package info.itest.www;
import static org.junit.Assert.*;
import org.junit.After;
import org.junit.Before;
import org.junit.Test;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.chrome.ChromeDriver;
import info.itest.www.pages.*;

public class TestLogin {
	
	WebDriver dr;
	String[][] data = {
			{"oo","xx","Error: invalid userName."},
			{"oo","","Error:password is empty"},
			{"","xx","Error:unerName is empty"},
			{"admin","","Error:wrong password"},	
	};
	String loginPageUrl;
		
	@Before
	public void setUp() throws Exception {
		System.out.println("Begin test");		
		System.setProperty("webdriver.chrome.driver", "c:\\chromedriver.exe");
		this.dr= new ChromeDriver();
		this.loginPageUrl="http://localhost/wordpress/wp-login.php";
	}

	@After
	public void tearDown() throws Exception {
		System.out.println("End test");		
		dr.quit();		
	}

	@Test
	public void testLogin() throws Exception {
		
		String userName ="admin";
		String userPass = "admin";

		LoginPage lPage = new LoginPage(this.dr,this.loginPageUrl);
		DashPage dashboardPage = lPage.login(userName, userPass);
		assertTrue(dashboardPage.currentUrl().contains("wp-admin"));
		assertTrue(dashboardPage.getGreetingLink().getText().contains(userName));
	}
	
	@Test
	public void testLoginFail() throws InterruptedException{
		for(String[] item : this.data){
			String userName = item[0];
			String password = item[1];
			String tip = item[2];

			LoginPage lPage = new LoginPage(this.dr,this.loginPageUrl);
			lPage = lPage.loginFailed(userName, password);
			
			assertTrue(lPage.currentUrl().contains("wp-login"));
			assertNotEquals(lPage.LoginErrorDiv().getText(),tip);
		}
	} 
}
