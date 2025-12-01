using System;
using System.Windows.Forms;

class Program {
	static void Main(string[] args) {
		#if DEBUG
			Test.Init();
			Test.Run();
			Console.ReadLine();
		#endif

		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(false);
		Application.Run(new TerrainGridWindow());
	}
}
