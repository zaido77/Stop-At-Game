using Stop_The_Timer.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stop_The_Timer
{
    public partial class frmStopAtGame : Form
    {
        Color _DefaultRed = Color.FromArgb(192, 0, 0);

        enum enGameStatus : byte
        {
            Start = 0,
            InProgress = 1,
            End = 2
        }

        byte TimerSeconds = 0;
        byte TimerPartOfSeconds = 0;
        enGameStatus GameStatus;

        public frmStopAtGame()
        {
            InitializeComponent();
        }

        string FormatTimer(byte Seconds, byte PartOfSeconds)
        {
            return Seconds.ToString("D2") + ":" + PartOfSeconds.ToString("D2");
        }

        void PerformGameStartAction()
        {
            timer1.Start();
            
            lblStopAt.Visible = false;
            numericUpDown.Visible = false;

            GameStatus = enGameStatus.InProgress;
        }

        byte GetFractionPart(decimal Number)
        {
            return (byte)((Number * 100) % 100);
            return Convert.ToByte((Number - (byte)Number) * 100);
        }

        string GetStopAtValue()
        {
            byte SecondsPart = Convert.ToByte(numericUpDown.Value);
            byte PartOfSecondsPart = GetFractionPart(numericUpDown.Value);

            return FormatTimer(SecondsPart, PartOfSecondsPart);
        }

        void PerformGameInProgressAction()
        {
            timer1.Stop();

            if (lblTimer.Text == GetStopAtValue())
                lblTimer.ForeColor = Color.Green;

            GameStatus = enGameStatus.End;
        }

        void PerformGameEndAction()
        {
            lblTimer.Text = "00:00";
            TimerSeconds = 0;
            TimerPartOfSeconds = 0;
            lblTimer.ForeColor = _DefaultRed;

            lblStopAt.Visible = true;
            numericUpDown.Visible = true;

            GameStatus = enGameStatus.Start;
        }

        void ApplyGameLogic()
        {
            switch (GameStatus)
            {
                case enGameStatus.Start:
                    PerformGameStartAction();
                    break;
                case enGameStatus.InProgress:
                    PerformGameInProgressAction();
                    break;
                case enGameStatus.End:
                    PerformGameEndAction();
                    break;

                default: break;
            }
        }

        void UpdateTimerLabel()
        {
            TimerPartOfSeconds += 1;

            if (TimerPartOfSeconds == 99)
            {
                TimerSeconds++;
                TimerPartOfSeconds = 0;
            }

            lblTimer.Text = FormatTimer(TimerSeconds, TimerPartOfSeconds);
        }

        private void pbRedButton_MouseDown(object sender, MouseEventArgs e)
        {
            pbRedButton.Image = Resources.RedButtonPressed;

            ApplyGameLogic();
        }

        private void pbRedButton_MouseUp(object sender, MouseEventArgs e)
        {
            pbRedButton.Image = Resources.RedButtonUnpressed;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateTimerLabel();
        }

        private void frmStopAtGame_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Enter)
            {
                pbRedButton_MouseDown(pbRedButton, new MouseEventArgs(MouseButtons.Left, 0, 0, 0, 0));
                
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void frmStopAtGame_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Enter)
            {
                pbRedButton_MouseUp(pbRedButton, new MouseEventArgs(MouseButtons.Left, 0, 0, 0, 0));
                
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

    }
}
