﻿namespace UserMaking
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mainToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Add_button = new System.Windows.Forms.Button();
            this.Clear_button = new System.Windows.Forms.Button();
            this.Add_GroupRule_button = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.Add_PersonRule_button = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Delete = new System.Windows.Forms.CheckBox();
            this.CreateFiles = new System.Windows.Forms.CheckBox();
            this.ReadData = new System.Windows.Forms.CheckBox();
            this.FullControl = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 42);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 49;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(583, 433);
            this.dataGridView1.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(19, 19);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1153, 28);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mainToolStripMenuItem
            // 
            this.mainToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadToolStripMenuItem,
            this.saveToolStripMenuItem});
            this.mainToolStripMenuItem.Name = "mainToolStripMenuItem";
            this.mainToolStripMenuItem.Size = new System.Drawing.Size(56, 24);
            this.mainToolStripMenuItem.Text = "Main";
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(124, 26);
            this.loadToolStripMenuItem.Text = "Load";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(124, 26);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // Add_button
            // 
            this.Add_button.Location = new System.Drawing.Point(1018, 538);
            this.Add_button.Name = "Add_button";
            this.Add_button.Size = new System.Drawing.Size(111, 24);
            this.Add_button.TabIndex = 2;
            this.Add_button.Text = "Add";
            this.Add_button.UseVisualStyleBackColor = true;
            this.Add_button.Click += new System.EventHandler(this.Add_button_Click);
            // 
            // Clear_button
            // 
            this.Clear_button.Location = new System.Drawing.Point(484, 491);
            this.Clear_button.Name = "Clear_button";
            this.Clear_button.Size = new System.Drawing.Size(111, 24);
            this.Clear_button.TabIndex = 3;
            this.Clear_button.Text = "Clear";
            this.Clear_button.UseVisualStyleBackColor = true;
            this.Clear_button.Click += new System.EventHandler(this.Clear_button_Click);
            // 
            // Add_GroupRule_button
            // 
            this.Add_GroupRule_button.Location = new System.Drawing.Point(396, 53);
            this.Add_GroupRule_button.Name = "Add_GroupRule_button";
            this.Add_GroupRule_button.Size = new System.Drawing.Size(84, 24);
            this.Add_GroupRule_button.TabIndex = 4;
            this.Add_GroupRule_button.Text = "Добавить";
            this.Add_GroupRule_button.UseVisualStyleBackColor = true;
            this.Add_GroupRule_button.Click += new System.EventHandler(this.Add_GroupRule_button_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.MenuBar;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(24, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(164, 16);
            this.label1.TabIndex = 5;
            this.label1.Text = "Выдать права человеку";
            // 
            // Add_PersonRule_button
            // 
            this.Add_PersonRule_button.BackColor = System.Drawing.Color.Chocolate;
            this.Add_PersonRule_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Add_PersonRule_button.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Add_PersonRule_button.Location = new System.Drawing.Point(396, 20);
            this.Add_PersonRule_button.Name = "Add_PersonRule_button";
            this.Add_PersonRule_button.Size = new System.Drawing.Size(86, 27);
            this.Add_PersonRule_button.TabIndex = 6;
            this.Add_PersonRule_button.Text = "Добавить";
            this.Add_PersonRule_button.UseVisualStyleBackColor = false;
            this.Add_PersonRule_button.Click += new System.EventHandler(this.Add_PersonRule_button_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(147, 16);
            this.label2.TabIndex = 7;
            this.label2.Text = "Выдать права группе";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(368, 495);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(111, 16);
            this.label3.TabIndex = 8;
            this.label3.Text = "Отчистить поле";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(950, 543);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 16);
            this.label4.TabIndex = 9;
            this.label4.Text = "Создать";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.Delete);
            this.panel1.Controls.Add(this.CreateFiles);
            this.panel1.Controls.Add(this.ReadData);
            this.panel1.Controls.Add(this.FullControl);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.Add_GroupRule_button);
            this.panel1.Controls.Add(this.Add_PersonRule_button);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(622, 42);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(507, 256);
            this.panel1.TabIndex = 10;
            // 
            // Delete
            // 
            this.Delete.AutoSize = true;
            this.Delete.Location = new System.Drawing.Point(27, 198);
            this.Delete.Name = "Delete";
            this.Delete.Size = new System.Drawing.Size(91, 20);
            this.Delete.TabIndex = 11;
            this.Delete.Text = "Удаление";
            this.Delete.UseVisualStyleBackColor = true;
            // 
            // CreateFiles
            // 
            this.CreateFiles.AutoSize = true;
            this.CreateFiles.Location = new System.Drawing.Point(27, 161);
            this.CreateFiles.Name = "CreateFiles";
            this.CreateFiles.Size = new System.Drawing.Size(145, 20);
            this.CreateFiles.TabIndex = 10;
            this.CreateFiles.Text = "Создание файлов";
            this.CreateFiles.UseVisualStyleBackColor = true;
            // 
            // ReadData
            // 
            this.ReadData.AutoSize = true;
            this.ReadData.Location = new System.Drawing.Point(27, 124);
            this.ReadData.Name = "ReadData";
            this.ReadData.Size = new System.Drawing.Size(74, 20);
            this.ReadData.TabIndex = 9;
            this.ReadData.Text = "Чтение";
            this.ReadData.UseVisualStyleBackColor = true;
            // 
            // FullControl
            // 
            this.FullControl.AutoSize = true;
            this.FullControl.Location = new System.Drawing.Point(27, 89);
            this.FullControl.Name = "FullControl";
            this.FullControl.Size = new System.Drawing.Size(126, 20);
            this.FullControl.TabIndex = 8;
            this.FullControl.Text = "Полный доступ";
            this.FullControl.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1153, 588);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Clear_button);
            this.Controls.Add(this.Add_button);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mainToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.Button Add_button;
        private System.Windows.Forms.Button Clear_button;
        private System.Windows.Forms.Button Add_GroupRule_button;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Add_PersonRule_button;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox Delete;
        private System.Windows.Forms.CheckBox CreateFiles;
        private System.Windows.Forms.CheckBox ReadData;
        private System.Windows.Forms.CheckBox FullControl;
        public System.Windows.Forms.DataGridView dataGridView1;
    }
}

