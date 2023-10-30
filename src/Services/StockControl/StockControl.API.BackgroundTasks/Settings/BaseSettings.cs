namespace StockControl.API.BackgroundTasks.Settings;

public class BaseSettings
{
    /// <summary>
    ///  На сколько засыпать между циклами выполнения фоновых задач (секунды)
    /// </summary>
    public int Delay { get; set; }
}