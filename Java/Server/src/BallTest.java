import org.junit.Assert;

public class BallTest {

    @org.junit.Before
    public void setUp() {
        Ball.setBallHolder(150);
    }

    @org.junit.Test
    public void getBallHolder() {
        Assert.assertEquals(150, Ball.getBallHolder());
    }

    @org.junit.Test
    public void getPrevBallHolder() {
        Ball.setBallHolder(250);
        Assert.assertEquals(150, Ball.getPrevBallHolder());
    }

    @org.junit.Test
    public void setBallHolder() {
        Ball.setBallHolder(250);
        Assert.assertEquals(250, Ball.getBallHolder());
    }
}
