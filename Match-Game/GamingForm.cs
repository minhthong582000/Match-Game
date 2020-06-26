using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Match_Game
{
    public partial class GamingForm : Form
    {
        //Initialise Variables 
        Random location = new Random(); //Changes Location randomly 
        List<Point> points = new List<Point>(); // To Store Location of Images
        PictureBox FlippedImage1;
        PictureBox FlippedImage2;
        int FlippedCount = 0;
        int LevelUp = 30;
        int TimeLevel = 60;
        int Score = 0;
        public GamingForm()
        {
            InitializeComponent();
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            //Resetting all values
            resetButton.BackColor = Color.Transparent;
            resetButton.Text = "Play Again";
            ScoreCounter.Text = "0";
            TimeLeft.Text = "60";
            LevelValue.Text = "1";

            //Restarting the game
            GamingForm_Load(sender, e);
        }

        private void GamingForm_Load(object sender, EventArgs e)
        {
            startCountdownLabel.Text = "3"; //Label Displaying the time before cards are flipped to Cover mode

            NextLevelScore.Text = LevelUp.ToString(); // Label Displaying total score to the next level

            // Randomize pictureBoxes's location 
            // Add pictureBox location to a list
            foreach (PictureBox picture in GamePanel.Controls)
            {
                picture.Enabled = false;
                points.Add(picture.Location);
            }
            // Then randomly asign a new location for each pictureBox
            foreach (PictureBox picture in GamePanel.Controls)
            {  
                //Randomisation of Images
                int next = location.Next(points.Count);
                Point p = points[next];

                picture.Location = p;
                points.Remove(p);
            }

            ScoreTimer.Start();
            CountdownTimer.Start();

            // Asign an image for a pair of pictureBoxes
            PicBox1.Image = Properties.Resources.img1;
            PicBox11.Image = Properties.Resources.img1;

            PicBox2.Image = Properties.Resources.img2;
            PicBox12.Image = Properties.Resources.img2;

            PicBox3.Image = Properties.Resources.img3;
            PicBox13.Image = Properties.Resources.img3;

            PicBox4.Image = Properties.Resources.img4;
            PicBox14.Image = Properties.Resources.img4;

            PicBox5.Image = Properties.Resources.img5;
            PicBox15.Image = Properties.Resources.img5;

            PicBox6.Image = Properties.Resources.img6;
            PicBox16.Image = Properties.Resources.img6;

            PicBox7.Image = Properties.Resources.img7;
            PicBox17.Image = Properties.Resources.img7;

            PicBox8.Image = Properties.Resources.img8;
            PicBox18.Image = Properties.Resources.img8;

            PicBox9.Image = Properties.Resources.img9;
            PicBox19.Image = Properties.Resources.img9;

            PicBox10.Image = Properties.Resources.img10;
            PicBox20.Image = Properties.Resources.img10;
        }

        private void ScoreTimer_Tick(object sender, EventArgs e)
        {
            ScoreTimer.Stop();
            foreach (PictureBox picture in GamePanel.Controls)
            {   
                // Switching all cards back to cover mode
                picture.Enabled = true;
                picture.Cursor = Cursors.Hand;
                picture.Image = Properties.Resources.cover;
            }
        }

        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            int timer = Convert.ToInt32(startCountdownLabel.Text);
            timer -= 1;
            startCountdownLabel.Text = Convert.ToString(timer);
            if (timer == 0)
            {
                CountdownTimer.Stop();
                TimeRemaining.Start();
                startCountdownLabel.Text = "Go!";
            }
        }

        private void TimeRemaining_Tick(object sender, EventArgs e)
        {
            //Timer to show how much time is left to complete the level
            int timer = Convert.ToInt32(TimeLeft.Text);
            timer -= 1;
            TimeLeft.Text = Convert.ToString(timer);
            if (timer == 0)
            {
                TimeRemaining.Stop();
                MessageBox.Show("You Scored " + ScoreCounter.Text + " at level : " + LevelValue.Text);
                ScoreCounter.Text = "0";
                resetButton.BackColor = Color.Red;
                resetButton.Text = "Play Again?";
            }
        }

        private void FlipTime_Tick(object sender, EventArgs e)
        {
            //Timer to flip back images to cover image
            FlipTime.Stop();

            FlippedImage1.Image = Properties.Resources.cover;
            FlippedImage2.Image = Properties.Resources.cover;
            FlippedImage1 = null;
            FlippedImage2 = null;
        }

        private void changeLevel()
        {
            //Increment level of the game by increasing required score and decreasing Time Limit
            Score += Convert.ToInt32(ScoreCounter.Text);
            MessageBox.Show("Next Level!");

            if (Convert.ToInt32(ScoreCounter.Text) >= LevelUp)
            {
                ScoreCounter.Text = "0";
                TimeLevel -= 5;
                TimeLeft.Text = Convert.ToString(TimeLevel);
                LevelValue.Text = Convert.ToString(Convert.ToInt32(LevelValue.Text) + 1);
                LevelUp += 5;
                if (TimeLevel <= 15)
                {
                    MessageBox.Show("Thanks for Playing! You've completed the game");
                    Application.Exit();
                }

                GamingForm_Load(this, null);
            }
            else
            {
                MessageBox.Show("Game Over! You didn't score enough to go to the next level. Total Score -> " + Score);
                resetButton.BackColor = Color.Red;
                resetButton.Text = "Play Again?";
            }
        }

        private void checkImages(PictureBox pic1, PictureBox pic2)
        {
            //Check if images are the same in both the PictureBoxes

            if (FlippedImage1 == null)
            {
                FlippedImage1 = pic1;
            }
            else if (FlippedImage1 != null && FlippedImage2 == null)
            {
                if (FlippedImage1 != pic1)
                    FlippedImage2 = pic1;
            }
            if (FlippedImage1 != null && FlippedImage2 != null)
            {
                if (FlippedImage1.Tag == FlippedImage2.Tag)
                {
                    FlippedImage1 = null;   // Reassigning to null for the next set of values
                    FlippedImage2 = null;   // Same as above
                    pic2.Enabled = false;   // To avoid clicking the image
                    pic1.Enabled = false;   // Same as above
                    ++FlippedCount;         // To check if the game is over by checking if all images have been flipped
                    ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 10); // Score Increment if there is a correct match
                }
                else
                {
                    FlipTime.Start();
                    ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) - 5); //Score Decrement if there is a wrong match
                }

            }

            if (FlippedCount == 10)
            {   
                // If all images are flipped over then reset the count value and call changeLevel() to check and go to the next level
                FlippedCount = 0;
                changeLevel();
            }
        }

        // Click events for all Pictureboxes
        // CheckImages with a pair of pictureBox asign above
        #region Cards
        private void PicBox1_Click(object sender, EventArgs e)
        {
            PicBox1.Image = Properties.Resources.img1;
            checkImages(PicBox1, PicBox11);
        }

        private void PicBox2_Click(object sender, EventArgs e)
        {
            PicBox2.Image = Properties.Resources.img2;
            checkImages(PicBox2, PicBox12);
        }

        private void PicBox3_Click(object sender, EventArgs e)
        {
            PicBox3.Image = Properties.Resources.img3;
            checkImages(PicBox3, PicBox13);
        }

        private void PicBox4_Click(object sender, EventArgs e)
        {
            PicBox4.Image = Properties.Resources.img4;
            checkImages(PicBox4, PicBox14);
        }

        private void PicBox5_Click(object sender, EventArgs e)
        {
            PicBox5.Image = Properties.Resources.img5;
            checkImages(PicBox5, PicBox15);
        }

        private void PicBox6_Click(object sender, EventArgs e)
        {
            PicBox6.Image = Properties.Resources.img6;
            checkImages(PicBox6, PicBox16);
        }

        private void PicBox7_Click(object sender, EventArgs e)
        {
            PicBox7.Image = Properties.Resources.img7;
            checkImages(PicBox7, PicBox17);
        }

        private void PicBox8_Click(object sender, EventArgs e)
        {
            PicBox8.Image = Properties.Resources.img8;
            checkImages(PicBox8, PicBox18);
        }

        private void PicBox9_Click(object sender, EventArgs e)
        {
            PicBox9.Image = Properties.Resources.img9;
            checkImages(PicBox9, PicBox19);
        }

        private void PicBox10_Click(object sender, EventArgs e)
        {
            PicBox10.Image = Properties.Resources.img10;
            checkImages(PicBox10, PicBox20);
        }


        private void PicBox11_Click(object sender, EventArgs e)
        {
            PicBox11.Image = Properties.Resources.img1;
            checkImages(PicBox11, PicBox1);
        }


        private void PicBox12_Click(object sender, EventArgs e)
        {
            PicBox12.Image = Properties.Resources.img2;
            checkImages(PicBox12, PicBox2);
        }

        private void PicBox13_Click(object sender, EventArgs e)
        {
            PicBox13.Image = Properties.Resources.img3;
            checkImages(PicBox13, PicBox3);
        }

        private void PicBox14_Click(object sender, EventArgs e)
        {
            PicBox14.Image = Properties.Resources.img4;
            checkImages(PicBox14, PicBox4);
        }

        private void PicBox15_Click(object sender, EventArgs e)
        {
            PicBox15.Image = Properties.Resources.img5;
            checkImages(PicBox15, PicBox5);
        }

        private void PicBox16_Click(object sender, EventArgs e)
        {
            PicBox16.Image = Properties.Resources.img6;
            checkImages(PicBox16, PicBox6);
        }

        private void PicBox17_Click(object sender, EventArgs e)
        {
            PicBox17.Image = Properties.Resources.img7;
            checkImages(PicBox17, PicBox7);
        }

        private void PicBox18_Click(object sender, EventArgs e)
        {
            PicBox18.Image = Properties.Resources.img8;
            checkImages(PicBox18, PicBox8);
        }

        private void PicBox19_Click(object sender, EventArgs e)
        {
            PicBox19.Image = Properties.Resources.img9;
            checkImages(PicBox19, PicBox9);
        }

        private void PicBox20_Click(object sender, EventArgs e)
        {
            PicBox20.Image = Properties.Resources.img10;
            checkImages(PicBox20, PicBox10);
        }
        #endregion
    }
}
