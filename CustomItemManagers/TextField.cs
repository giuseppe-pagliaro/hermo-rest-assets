﻿using Commons;

namespace CustomItemManagers
{
    public partial class TextField : Field
    {
        public TextField() : base()
        {
            InitializeComponent();

            this.Value = "(empty)";
        }

        public TextField(String name, String value) : base(name)
        {
            InitializeComponent();

            this.Value = value;
        }

        public TextField(String name, String value, Style style) : base(name, style)
        {
            InitializeComponent();

            this.Value = value;
        }

        public String Value
        {
            get { return this.txtValue.Text; }
            set { this.txtValue.Text = value; }
        }

        protected override void ApplyStyle()
        {
            base.ApplyStyle();
            StyleAppliers.Label(this.txtValue, this.style, FontStyle.Regular);
        }
    }
}