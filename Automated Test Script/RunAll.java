package info.itest.www;
import org.junit.runner.RunWith;
import org.junit.runners.Suite;
import org.junit.runners.Suite.SuiteClasses;
import info.itest.www.TestCreatePost;
import info.itest.www.TestDeletePost;
import info.itest.www.TestLogin;

@RunWith(Suite.class)
@SuiteClasses({TestCreatePost.class,TestDeletePost.class,TestLogin.class})
public class RunAll {

}
