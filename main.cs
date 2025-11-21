using System;
using System.Windows.Forms;

class Program {
	static void Main(string[] args) {
		#if DEBUG
			Test.Init();
			Test.Run();
		#else
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new TerrainGrid());
		#endif
	}
}
