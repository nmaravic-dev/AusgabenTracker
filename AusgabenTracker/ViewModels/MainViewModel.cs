using AusgabenTracker.Data;

namespace AusgabenTracker.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly DBHelper _dbHelper;
    public MainViewModel(DBHelper dbHelper)
    {
        _dbHelper = dbHelper;
    }

}