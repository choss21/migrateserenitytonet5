
namespace MigrateSerenityCore
{
    partial class FPassv3To5
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FPassv3To5));
            this.btnExaminar = new System.Windows.Forms.Button();
            this.txtDirectorioModulos = new System.Windows.Forms.TextBox();
            this.btnMigrar = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.chkIsNetFramework = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnExaminar
            // 
            this.btnExaminar.Location = new System.Drawing.Point(537, 42);
            this.btnExaminar.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnExaminar.Name = "btnExaminar";
            this.btnExaminar.Size = new System.Drawing.Size(99, 26);
            this.btnExaminar.TabIndex = 5;
            this.btnExaminar.Text = "Examinar";
            this.btnExaminar.UseVisualStyleBackColor = true;
            this.btnExaminar.Click += new System.EventHandler(this.btnExaminar_Click);
            // 
            // txtDirectorioModulos
            // 
            this.txtDirectorioModulos.Location = new System.Drawing.Point(22, 42);
            this.txtDirectorioModulos.Margin = new System.Windows.Forms.Padding(9);
            this.txtDirectorioModulos.Name = "txtDirectorioModulos";
            this.txtDirectorioModulos.Size = new System.Drawing.Size(511, 26);
            this.txtDirectorioModulos.TabIndex = 4;
            // 
            // btnMigrar
            // 
            this.btnMigrar.Location = new System.Drawing.Point(22, 73);
            this.btnMigrar.Margin = new System.Windows.Forms.Padding(9);
            this.btnMigrar.Name = "btnMigrar";
            this.btnMigrar.Size = new System.Drawing.Size(614, 46);
            this.btnMigrar.TabIndex = 3;
            this.btnMigrar.Text = "Realizar Ajustes";
            this.btnMigrar.UseVisualStyleBackColor = true;
            this.btnMigrar.Click += new System.EventHandler(this.btnMigrar_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(18, 12);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(204, 24);
            this.label1.TabIndex = 6;
            this.label1.Text = "Ubicacion del Proyecto";
            // 
            // txtStatus
            // 
            this.txtStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtStatus.Location = new System.Drawing.Point(22, 121);
            this.txtStatus.Multiline = true;
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ReadOnly = true;
            this.txtStatus.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtStatus.Size = new System.Drawing.Size(617, 207);
            this.txtStatus.TabIndex = 7;
            // 
            // chkIsNetFramework
            // 
            this.chkIsNetFramework.AutoSize = true;
            this.chkIsNetFramework.Location = new System.Drawing.Point(272, 12);
            this.chkIsNetFramework.Name = "chkIsNetFramework";
            this.chkIsNetFramework.Size = new System.Drawing.Size(276, 24);
            this.chkIsNetFramework.TabIndex = 8;
            this.chkIsNetFramework.Text = "El proyecto esta en NetFrameWork";
            this.chkIsNetFramework.UseVisualStyleBackColor = true;
            // 
            // FPassv3To5
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(654, 332);
            this.Controls.Add(this.chkIsNetFramework);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnExaminar);
            this.Controls.Add(this.txtDirectorioModulos);
            this.Controls.Add(this.btnMigrar);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FPassv3To5";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Migrar proyecto de NetCore v3.1 To v5";
            this.Load += new System.EventHandler(this.FPassv3To5_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnExaminar;
        private System.Windows.Forms.TextBox txtDirectorioModulos;
        private System.Windows.Forms.Button btnMigrar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.CheckBox chkIsNetFramework;
    }
}