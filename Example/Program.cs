using HermoItemManagers;

namespace Example
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            //Application.Run(new MainForm());

            //Application.Run(new ItemViewer(new DataExample() { Id = 0, Value = "Ciao"}));
            Application.Run(new ItemEditor(new DataExample() { Id = 0, Value = "Ciao" }));
        }
    }
}