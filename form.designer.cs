partial class Form1 {
	private System.ComponentModel.IContainer components = null;

	protected override void Dispose(bool disposing) {
		if (disposing && (components != null)) {
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent() {
		this.SuspendLayout();
		this.ClientSize = new System.Drawing.Size(400, 400);
		this.Name = "Grid Game";
		this.Text = "Terrain Grid Game";
		this.ResumeLayout(false);
	}
}
