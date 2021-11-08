namespace FNH.Reader
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.box_image = new System.Windows.Forms.PictureBox();
			this.button_savePNG = new System.Windows.Forms.Button();
			this.button_openFNH = new System.Windows.Forms.Button();
			this.dialog_openFNH = new System.Windows.Forms.OpenFileDialog();
			this.dialog_savePNG = new System.Windows.Forms.SaveFileDialog();
			this.textbox = new System.Windows.Forms.TextBox();
			this.spacing = new System.Windows.Forms.TextBox();
			this.check_shadow = new System.Windows.Forms.CheckBox();
			this.check_border = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.box_image)).BeginInit();
			this.SuspendLayout();
			// 
			// box_image
			// 
			this.box_image.Location = new System.Drawing.Point(0, 0);
			this.box_image.Name = "box_image";
			this.box_image.Size = new System.Drawing.Size(1448, 851);
			this.box_image.TabIndex = 0;
			this.box_image.TabStop = false;
			// 
			// button_savePNG
			// 
			this.button_savePNG.Location = new System.Drawing.Point(1326, 804);
			this.button_savePNG.Name = "button_savePNG";
			this.button_savePNG.Size = new System.Drawing.Size(110, 35);
			this.button_savePNG.TabIndex = 1;
			this.button_savePNG.Text = "Save PNG";
			this.button_savePNG.UseVisualStyleBackColor = true;
			this.button_savePNG.Click += new System.EventHandler(this.button_savePNG_clicked);
			// 
			// button_openFNH
			// 
			this.button_openFNH.Location = new System.Drawing.Point(1210, 804);
			this.button_openFNH.Name = "button_openFNH";
			this.button_openFNH.Size = new System.Drawing.Size(110, 35);
			this.button_openFNH.TabIndex = 2;
			this.button_openFNH.Text = "Open File";
			this.button_openFNH.UseVisualStyleBackColor = true;
			this.button_openFNH.Click += new System.EventHandler(this.button_openFile_clicked);
			// 
			// dialog_openFNH
			// 
			this.dialog_openFNH.FileName = "openFileDialog1";
			this.dialog_openFNH.FileOk += new System.ComponentModel.CancelEventHandler(this.OpenFile);
			// 
			// dialog_savePNG
			// 
			this.dialog_savePNG.FileOk += new System.ComponentModel.CancelEventHandler(this.savePNG);
			// 
			// textbox
			// 
			this.textbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.textbox.Location = new System.Drawing.Point(98, 805);
			this.textbox.Name = "textbox";
			this.textbox.Size = new System.Drawing.Size(1106, 34);
			this.textbox.TabIndex = 3;
			this.textbox.Text = "abcdef...";
			this.textbox.TextChanged += new System.EventHandler(this.text_TextChanged);
			// 
			// spacing
			// 
			this.spacing.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.spacing.Location = new System.Drawing.Point(12, 805);
			this.spacing.Name = "spacing";
			this.spacing.Size = new System.Drawing.Size(80, 34);
			this.spacing.TabIndex = 4;
			this.spacing.Text = "10";
			this.spacing.TextChanged += new System.EventHandler(this.spacing_TextChanged);
			// 
			// check_shadow
			// 
			this.check_shadow.AutoSize = true;
			this.check_shadow.Location = new System.Drawing.Point(12, 778);
			this.check_shadow.Name = "check_shadow";
			this.check_shadow.Size = new System.Drawing.Size(80, 21);
			this.check_shadow.TabIndex = 5;
			this.check_shadow.Text = "Shadow";
			this.check_shadow.UseVisualStyleBackColor = true;
			this.check_shadow.CheckedChanged += new System.EventHandler(this.check_shadow_CheckedChanged);
			// 
			// check_border
			// 
			this.check_border.AutoSize = true;
			this.check_border.Location = new System.Drawing.Point(98, 778);
			this.check_border.Name = "check_border";
			this.check_border.Size = new System.Drawing.Size(73, 21);
			this.check_border.TabIndex = 6;
			this.check_border.Text = "Border";
			this.check_border.UseVisualStyleBackColor = true;
			this.check_border.CheckedChanged += new System.EventHandler(this.check_border_CheckedChanged);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.ClientSize = new System.Drawing.Size(1448, 851);
			this.Controls.Add(this.check_border);
			this.Controls.Add(this.check_shadow);
			this.Controls.Add(this.spacing);
			this.Controls.Add(this.textbox);
			this.Controls.Add(this.button_openFNH);
			this.Controls.Add(this.button_savePNG);
			this.Controls.Add(this.box_image);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Form1";
			this.Text = "VNH.Reader";
			((System.ComponentModel.ISupportInitialize)(this.box_image)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox box_image;
        private System.Windows.Forms.Button button_savePNG;
        private System.Windows.Forms.Button button_openFNH;
        private System.Windows.Forms.OpenFileDialog dialog_openFNH;
        private System.Windows.Forms.SaveFileDialog dialog_savePNG;
		private System.Windows.Forms.TextBox textbox;
		private System.Windows.Forms.TextBox spacing;
		private System.Windows.Forms.CheckBox check_shadow;
		private System.Windows.Forms.CheckBox check_border;
	}
}

