namespace DBClip;

static class Program
{
    [STAThread]
    static void Main()
    {
        try
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new Views.MainForm());
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error: " + ex.Message + "\n\n" + ex.StackTrace, "Startup Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }    
}
