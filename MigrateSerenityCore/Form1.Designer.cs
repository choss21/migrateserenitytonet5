namespace MigrateSerenityCore
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnMigrar = new System.Windows.Forms.Button();
            this.txtDirectorioModulos = new System.Windows.Forms.TextBox();
            this.btnExaminar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnMigrar
            // 
            this.btnMigrar.Location = new System.Drawing.Point(156, 97);
            this.btnMigrar.Margin = new System.Windows.Forms.Padding(6);
            this.btnMigrar.Name = "btnMigrar";
            this.btnMigrar.Size = new System.Drawing.Size(347, 89);
            this.btnMigrar.TabIndex = 0;
            this.btnMigrar.Text = "Realizar Ajustes";
            this.btnMigrar.UseVisualStyleBackColor = true;
            this.btnMigrar.Click += new System.EventHandler(this.btnMigrar_Click);
            // 
            // txtDirectorioModulos
            // 
            this.txtDirectorioModulos.Location = new System.Drawing.Point(36, 15);
            this.txtDirectorioModulos.Margin = new System.Windows.Forms.Padding(6);
            this.txtDirectorioModulos.Name = "txtDirectorioModulos";
            this.txtDirectorioModulos.Size = new System.Drawing.Size(886, 29);
            this.txtDirectorioModulos.TabIndex = 1;
            this.txtDirectorioModulos.Text = "C:\\HOMECHOSS\\Blinc\\Web\\NubeBlinc\\NubeBlinc.Web\\Modules";
            // 
            // btnExaminar
            // 
            this.btnExaminar.Location = new System.Drawing.Point(926, 16);
            this.btnExaminar.Name = "btnExaminar";
            this.btnExaminar.Size = new System.Drawing.Size(131, 27);
            this.btnExaminar.TabIndex = 2;
            this.btnExaminar.Text = "Examinar";
            this.btnExaminar.UseVisualStyleBackColor = true;
            this.btnExaminar.Click += new System.EventHandler(this.btnExaminar_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1091, 210);
            this.Controls.Add(this.btnExaminar);
            this.Controls.Add(this.txtDirectorioModulos);
            this.Controls.Add(this.btnMigrar);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "Form1";
            this.Text = "Pasar de NetFrameWork a NetCore3.1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnMigrar;
        private System.Windows.Forms.TextBox txtDirectorioModulos;
        private System.Windows.Forms.Button btnExaminar;
    }
}

