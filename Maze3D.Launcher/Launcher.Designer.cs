namespace Maze3D.Launcher
{
    partial class Launcher
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Launcher));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.screenConfig = new System.Windows.Forms.GroupBox();
            this.useFullscreen = new System.Windows.Forms.CheckBox();
            this.screenHeight = new System.Windows.Forms.TextBox();
            this.screenWidth = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.detectAuto = new System.Windows.Forms.CheckBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.selectLevel = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.useSound = new System.Windows.Forms.CheckBox();
            this.selectMode = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.selectDifficulty = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.useGamepad = new System.Windows.Forms.CheckBox();
            this.useVirtualPad = new System.Windows.Forms.CheckBox();
            this.useMouse = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.playButton = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.rendererChoice = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.screenConfig.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(451, 180);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.screenConfig);
            this.tabPage2.Controls.Add(this.detectAuto);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(442, 171);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Affichage";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // screenConfig
            // 
            this.screenConfig.Controls.Add(this.useFullscreen);
            this.screenConfig.Controls.Add(this.screenHeight);
            this.screenConfig.Controls.Add(this.screenWidth);
            this.screenConfig.Controls.Add(this.label4);
            this.screenConfig.Controls.Add(this.label3);
            this.screenConfig.Location = new System.Drawing.Point(6, 40);
            this.screenConfig.Name = "screenConfig";
            this.screenConfig.Size = new System.Drawing.Size(265, 108);
            this.screenConfig.TabIndex = 2;
            this.screenConfig.TabStop = false;
            this.screenConfig.Text = "Paramètres avancés";
            // 
            // useFullscreen
            // 
            this.useFullscreen.AutoSize = true;
            this.useFullscreen.Location = new System.Drawing.Point(9, 76);
            this.useFullscreen.Name = "useFullscreen";
            this.useFullscreen.Size = new System.Drawing.Size(154, 17);
            this.useFullscreen.TabIndex = 5;
            this.useFullscreen.Text = "Activer le mode plein écran";
            this.useFullscreen.UseVisualStyleBackColor = true;
            this.useFullscreen.CheckedChanged += new System.EventHandler(this.useFullscreen_CheckedChanged);
            // 
            // screenHeight
            // 
            this.screenHeight.Location = new System.Drawing.Point(77, 50);
            this.screenHeight.Name = "screenHeight";
            this.screenHeight.Size = new System.Drawing.Size(100, 20);
            this.screenHeight.TabIndex = 4;
            this.screenHeight.Text = "768";
            // 
            // screenWidth
            // 
            this.screenWidth.Location = new System.Drawing.Point(77, 23);
            this.screenWidth.Name = "screenWidth";
            this.screenWidth.Size = new System.Drawing.Size(100, 20);
            this.screenWidth.TabIndex = 3;
            this.screenWidth.Text = "1024";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 53);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Hauteur";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Largeur";
            // 
            // detectAuto
            // 
            this.detectAuto.AutoSize = true;
            this.detectAuto.Location = new System.Drawing.Point(6, 17);
            this.detectAuto.Name = "detectAuto";
            this.detectAuto.Size = new System.Drawing.Size(265, 17);
            this.detectAuto.TabIndex = 0;
            this.detectAuto.Text = "Detecter automatiquement les meilleurs paramètres";
            this.detectAuto.UseVisualStyleBackColor = true;
            this.detectAuto.CheckedChanged += new System.EventHandler(this.detectAuto_CheckedChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.rendererChoice);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(442, 171);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Jeu";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.selectLevel);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.useSound);
            this.groupBox2.Controls.Add(this.selectMode);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.selectDifficulty);
            this.groupBox2.Location = new System.Drawing.Point(6, 16);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 140);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Base";
            // 
            // selectLevel
            // 
            this.selectLevel.FormattingEnabled = true;
            this.selectLevel.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.selectLevel.Location = new System.Drawing.Point(73, 80);
            this.selectLevel.Name = "selectLevel";
            this.selectLevel.Size = new System.Drawing.Size(121, 21);
            this.selectLevel.TabIndex = 8;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 83);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "Niveau";
            // 
            // useSound
            // 
            this.useSound.AutoSize = true;
            this.useSound.Checked = true;
            this.useSound.CheckState = System.Windows.Forms.CheckState.Checked;
            this.useSound.Location = new System.Drawing.Point(73, 113);
            this.useSound.Name = "useSound";
            this.useSound.Size = new System.Drawing.Size(59, 17);
            this.useSound.TabIndex = 3;
            this.useSound.Text = "Activer";
            this.useSound.UseVisualStyleBackColor = true;
            // 
            // selectMode
            // 
            this.selectMode.FormattingEnabled = true;
            this.selectMode.Items.AddRange(new object[] {
            "Nouveau",
            "Old school"});
            this.selectMode.Location = new System.Drawing.Point(73, 50);
            this.selectMode.Name = "selectMode";
            this.selectMode.Size = new System.Drawing.Size(121, 21);
            this.selectMode.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 114);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Son";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 53);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(34, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Mode";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Difficulté";
            // 
            // selectDifficulty
            // 
            this.selectDifficulty.FormattingEnabled = true;
            this.selectDifficulty.Items.AddRange(new object[] {
            "Très facile",
            "Facile",
            "Normal",
            "Difficile"});
            this.selectDifficulty.Location = new System.Drawing.Point(73, 21);
            this.selectDifficulty.Name = "selectDifficulty";
            this.selectDifficulty.Size = new System.Drawing.Size(121, 21);
            this.selectDifficulty.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.useGamepad);
            this.groupBox1.Controls.Add(this.useVirtualPad);
            this.groupBox1.Controls.Add(this.useMouse);
            this.groupBox1.Location = new System.Drawing.Point(215, 17);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(220, 100);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Contrôles";
            // 
            // useGamepad
            // 
            this.useGamepad.AutoSize = true;
            this.useGamepad.Checked = true;
            this.useGamepad.CheckState = System.Windows.Forms.CheckState.Checked;
            this.useGamepad.Location = new System.Drawing.Point(11, 19);
            this.useGamepad.Name = "useGamepad";
            this.useGamepad.Size = new System.Drawing.Size(117, 17);
            this.useGamepad.TabIndex = 4;
            this.useGamepad.Text = "Activer le gamepad";
            this.useGamepad.UseVisualStyleBackColor = true;
            // 
            // useVirtualPad
            // 
            this.useVirtualPad.AutoSize = true;
            this.useVirtualPad.Checked = true;
            this.useVirtualPad.CheckState = System.Windows.Forms.CheckState.Checked;
            this.useVirtualPad.Location = new System.Drawing.Point(11, 65);
            this.useVirtualPad.Name = "useVirtualPad";
            this.useVirtualPad.Size = new System.Drawing.Size(122, 17);
            this.useVirtualPad.TabIndex = 6;
            this.useVirtualPad.Text = "Activer le pad virtuel";
            this.useVirtualPad.UseVisualStyleBackColor = true;
            // 
            // useMouse
            // 
            this.useMouse.AutoSize = true;
            this.useMouse.Checked = true;
            this.useMouse.CheckState = System.Windows.Forms.CheckState.Checked;
            this.useMouse.Location = new System.Drawing.Point(11, 42);
            this.useMouse.Name = "useMouse";
            this.useMouse.Size = new System.Drawing.Size(100, 17);
            this.useMouse.TabIndex = 5;
            this.useMouse.Text = "Activer la souris";
            this.useMouse.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(13, 210);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(450, 197);
            this.tabControl1.TabIndex = 1;
            // 
            // playButton
            // 
            this.playButton.Location = new System.Drawing.Point(384, 414);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(75, 23);
            this.playButton.TabIndex = 2;
            this.playButton.Text = "Jouer";
            this.playButton.UseVisualStyleBackColor = true;
            this.playButton.Click += new System.EventHandler(this.playButton_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(223, 138);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(89, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "Rendu graphique";
            // 
            // rendererChoice
            // 
            this.rendererChoice.FormattingEnabled = true;
            this.rendererChoice.Items.AddRange(new object[] {
            "DirectX 11",
            "OpenGL 2.0"});
            this.rendererChoice.Location = new System.Drawing.Point(318, 135);
            this.rendererChoice.Name = "rendererChoice";
            this.rendererChoice.Size = new System.Drawing.Size(117, 21);
            this.rendererChoice.TabIndex = 10;
            // 
            // Launcher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 431);
            this.Controls.Add(this.playButton);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(480, 470);
            this.MinimumSize = new System.Drawing.Size(480, 470);
            this.Name = "Launcher";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "The Lost Maze : Beta Launcher";
            this.Load += new System.EventHandler(this.Launcher_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.screenConfig.ResumeLayout(false);
            this.screenConfig.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox screenConfig;
        private System.Windows.Forms.CheckBox useFullscreen;
        private System.Windows.Forms.TextBox screenHeight;
        private System.Windows.Forms.TextBox screenWidth;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox detectAuto;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox selectDifficulty;
        private System.Windows.Forms.CheckBox useSound;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox useGamepad;
        private System.Windows.Forms.CheckBox useVirtualPad;
        private System.Windows.Forms.CheckBox useMouse;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.ComboBox selectMode;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button playButton;
        private System.Windows.Forms.ComboBox selectLevel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox rendererChoice;
        private System.Windows.Forms.Label label7;

    }
}

