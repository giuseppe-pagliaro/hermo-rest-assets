﻿using Commons;

namespace CustomItemManagers
{
    public partial class TextFieldEditor : Field
    {
        public TextFieldEditor()
        {
            InitializeComponent();

            togglable = true;
            active = false;

            Togglable = false;
        }

        private bool active;
        private bool togglable;

        public bool Togglable
        {
            get { return togglable; }
            set
            {
                if (togglable != value)
                {
                    togglable = value;

                    if (togglable)
                    {
                        buttonActive.Enabled = true;
                        buttonActive.Visible = true;
                        txtBoxValue.Width -= buttonActive.Width;

                        active = false;
                        buttonActive.BackColor = style.SecondaryInteractableColor;
                        txtBoxValue.Enabled = false;
                    }
                    else
                    {
                        buttonActive.Enabled = false;
                        buttonActive.Visible = false;
                        txtBoxValue.Width += buttonActive.Width;
                    }
                }
            }
        }

        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        public String Value
        {
            get { return txtBoxValue.Text; }
        }

        protected override void ApplyStyle()
        {
            base.ApplyStyle();

            StyleAppliers.TextBox(txtBoxValue, style);
            StyleAppliers.Button(buttonActive, style);

            if (!active)
            {
                buttonActive.BackColor = style.SecondaryInteractableColor;
            }
        }

        private void buttonActive_Click(object sender, EventArgs e)
        {
            if (active)
            {
                active = false;
                buttonActive.BackColor = style.SecondaryInteractableColor;
                txtBoxValue.Enabled = false;
            }
            else
            {
                active = true;
                buttonActive.BackColor = style.PrimaryInteractableColor;
                txtBoxValue.Enabled = true;
            }
        }
    }
}