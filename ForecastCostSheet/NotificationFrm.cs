using DevExpress.CodeParser;
using DevExpress.XtraGrid.Views.WinExplorer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ForecastCostSheet
{
    public partial class NotificationFrm : Form
    {
        public NotificationFrm()
        {
            InitializeComponent();
        }

        public enum NumericButtons
        {
            None = 1,
            TwoButtons = 2,
            ThreeButtons = 3
        }

        public NumericButtons meNumericButtons = NumericButtons.None;

        public void Display(string Message, string Title, System.Windows.Forms.MessageBoxButtons Buttons, System.Windows.Forms.MessageBoxIcon Icon)
        {
            Image IconImage;

            try
            {

                if (Icon == System.Windows.Forms.MessageBoxIcon.Asterisk) IconImage = PictureBoxError.Image;
                else if (Icon == System.Windows.Forms.MessageBoxIcon.Question) IconImage = PictureBoxQuestion.Image;
                else if (Icon == System.Windows.Forms.MessageBoxIcon.Warning) IconImage = PictureBoxWarning.Image;
                else if (Icon == System.Windows.Forms.MessageBoxIcon.Stop) IconImage = PictureBoxStop.Image;
                else if (Icon == System.Windows.Forms.MessageBoxIcon.Exclamation) IconImage = PictureBoxExclamation.Image;
                else if (Icon == System.Windows.Forms.MessageBoxIcon.Information) IconImage = PictureBoxInformation.Image;
                else if (Icon == System.Windows.Forms.MessageBoxIcon.Error)
                {
                    IconImage = PictureBoxError.Image;
                    if (Title == "") Title = "Exception Message";
                }
                else IconImage = PictureBoxInformation.Image;                 

                NotificationIcon.Image = IconImage;
                NotificationTitle.Text = Title;
                NotificationMessage.Text = Message;

                switch (Buttons)
                {
                    case System.Windows.Forms.MessageBoxButtons.OK:
                        Button3.Text = "OK";
                        Button3.DialogResult = DialogResult.OK;
                        Button3.Visible = true;

                        Button3.TabStop = true;
                        Button3.TabIndex = 1;

                        break;

                    case System.Windows.Forms.MessageBoxButtons.OKCancel:
                        Button2.Text = "OK";
                        Button2.DialogResult = DialogResult.OK;
                        Button2.Visible = true;

                        Button2.TabStop = true;
                        Button2.TabIndex = 2;

                        Button3.Text = "Cancel";
                        Button3.DialogResult = DialogResult.Cancel;
                        Button3.Visible = true;

                        Button3.TabStop = true;
                        Button3.TabIndex = 1;

                        break;

                    case System.Windows.Forms.MessageBoxButtons.AbortRetryIgnore:
                        Button1.Text = "Abort";
                        Button1.DialogResult = DialogResult.Abort;
                        Button1.Visible = true;

                        Button1.TabStop = true;
                        Button1.TabIndex = 3;

                        Button2.Text = "Retry";
                        Button2.DialogResult = DialogResult.Retry;
                        Button2.Visible = true;

                        Button2.TabStop = true;
                        Button2.TabIndex = 2;

                        Button3.Text = "Ignore";
                        Button3.DialogResult = DialogResult.Ignore;
                        Button3.Visible = true;

                        Button3.TabStop = true;
                        Button3.TabIndex = 1;

                        break;

                    case System.Windows.Forms.MessageBoxButtons.YesNoCancel:

                        Button1.Text = "Yes";
                        Button1.DialogResult = DialogResult.Yes;
                        Button1.Visible = true;

                        Button1.TabStop = true;
                        Button1.TabIndex = 3;

                        Button2.Text = "No";
                        Button2.DialogResult = DialogResult.No;
                        Button2.Visible = true;

                        Button2.TabStop = true;
                        Button2.TabIndex = 2;

                        Button3.Text = "Cancel";
                        Button3.DialogResult = DialogResult.Cancel;
                        Button3.Visible = true;

                        Button3.TabStop = true;
                        Button3.TabIndex = 1;

                        break;

                    case MessageBoxButtons.YesNo:

                        Button2.Text = "Yes";
                        Button2.DialogResult = DialogResult.Yes;
                        Button2.Visible = true;

                        Button2.TabStop = true;
                        Button2.TabIndex = 2;

                        Button3.Text = "No";
                        Button3.DialogResult = DialogResult.No;
                        Button3.Visible = true;

                        Button3.TabStop = true;
                        Button3.TabIndex = 1;

                        break;

                    case MessageBoxButtons.RetryCancel:

                        Button2.Text = "Retry";
                        Button2.DialogResult = DialogResult.Retry;
                        Button2.Visible = true;

                        Button2.TabStop = true;
                        Button2.TabIndex = 2;

                        Button3.Text = "Cancel";
                        Button3.DialogResult = DialogResult.Cancel;
                        Button3.Visible = true;

                        Button3.TabStop = true;
                        Button3.TabIndex = 1;

                        break;
                }

                if (meNumericButtons == NumericButtons.ThreeButtons)
                {
                    Button1.Text = "1";
                    Button1.DialogResult = DialogResult.Yes;
                    Button1.Visible = true;

                    Button1.TabStop = true;
                    Button1.TabIndex = 3;

                    Button2.Text = "2";
                    Button2.DialogResult = DialogResult.No;
                    Button2.Visible = true;

                    Button2.TabStop = true;
                    Button2.TabIndex = 2;

                    Button3.Text = "3";
                    Button3.DialogResult = DialogResult.Cancel;
                    Button3.Visible = true;

                    Button3.TabStop = true;
                    Button3.TabIndex = 1;

                }

            }
            catch (Exception ex)
            { }

        }

        private void NotificationMessage_DoubleClick(Object sender, EventArgs e)
        {
            Clipboard.SetText(NotificationMessage.Text);
        }

        private void NotificationForm_KeyUp(Object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Y)
            {
                if (Button1.DialogResult == DialogResult.Yes) this.DialogResult = DialogResult.Yes;
                if (Button2.DialogResult == DialogResult.Yes) this.DialogResult = DialogResult.Yes;
                if (Button3.DialogResult == DialogResult.Yes) this.DialogResult = DialogResult.Yes;
            }

            if (e.KeyCode == Keys.N)
            {
                if(Button1.DialogResult == DialogResult.No) this.DialogResult = DialogResult.No;
                if(Button2.DialogResult == DialogResult.No) this.DialogResult = DialogResult.No;
                if(Button3.DialogResult == DialogResult.No) this.DialogResult = DialogResult.No;
            }

            if (e.KeyCode == Keys.Escape)
            {
                if (Button1.DialogResult == DialogResult.Cancel) this.DialogResult = DialogResult.Cancel;
                if (Button2.DialogResult == DialogResult.Cancel) this.DialogResult = DialogResult.Cancel;
                if (Button3.DialogResult == DialogResult.Cancel) this.DialogResult = DialogResult.Cancel;
            }

            if (e.KeyCode == Keys.Oem1 || e.KeyCode == Keys.NumPad1)
                {
                if (Button1.DialogResult == DialogResult.Yes) this.DialogResult = DialogResult.Yes;
                if (Button2.DialogResult == DialogResult.Yes) this.DialogResult = DialogResult.Yes;
                if (Button3.DialogResult == DialogResult.Yes) this.DialogResult = DialogResult.Yes;
            }

            if( e.KeyCode == Keys.Oem2 || e.KeyCode == Keys.NumPad2)
            {
                if (Button1.DialogResult == DialogResult.No) this.DialogResult = DialogResult.No;
                if (Button2.DialogResult == DialogResult.No) this.DialogResult = DialogResult.No;
                if (Button3.DialogResult == DialogResult.No) this.DialogResult = DialogResult.No;
            }

            if(e.KeyCode == Keys.Oem3 || e.KeyCode == Keys.NumPad3)
            {
                if (Button1.DialogResult == DialogResult.Cancel) this.DialogResult = DialogResult.Cancel;
                if (Button2.DialogResult == DialogResult.Cancel) this.DialogResult = DialogResult.Cancel;
                if (Button3.DialogResult == DialogResult.Cancel) this.DialogResult = DialogResult.Cancel;
            }

        }

    }
}
