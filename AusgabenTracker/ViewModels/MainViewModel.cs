using AusgabenTracker.Data;
using AusgabenTracker.Models;
using MaterialDesignThemes.Wpf;
using System.Collections.ObjectModel;
using System.Windows;

namespace AusgabenTracker.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly DBHelper _dbHelper;
    private readonly ObservableCollection<Expense> _expenses = [];
    private readonly ObservableCollection<Category> _categories = [];
    private string? _description;
    private string? _amount;
    private Expense? _selectedExpense;
    private Category? _selectedCategory;
    private DateTime _date = DateTime.Now;
    public ObservableCollection<Expense> Expenses => _expenses;
    public ObservableCollection<Category> Categories => _categories;
    public string? Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }
    public string? Amount
    {
        get => _amount;
        set => SetProperty(ref _amount, value);
    }
    public DateTime Date
    {
        get => _date;
        set => SetProperty(ref _date, value);
    }
    public Category? SelectedCategory
    {
        get => _selectedCategory;
        set => SetProperty(ref _selectedCategory, value);
    }

    public Expense? SelectedExpense
    {
        get => _selectedExpense;
        set => SetProperty(ref _selectedExpense, value);
    }
    public decimal TotalAmount => _expenses.Sum(e => e.Amount);

    public LimitStatus Status
    {
        get
        {
            if (TotalAmount > 400) return LimitStatus.Over;
            if (TotalAmount >= 200) return LimitStatus.Warning;
            return LimitStatus.Ok;
        }
    }

    public RelayCommand AddExpenseCommand { get; }
    public RelayCommand DeleteExpenseCommand { get; }
    public MainViewModel(DBHelper dbHelper)
    {
        _dbHelper = dbHelper;
        AddExpenseCommand = new RelayCommand(AddExpense, CanAddExpense);
        DeleteExpenseCommand = new RelayCommand(DeleteExpense, CanDeleteExpense);
        _ = LoadAsync();
    }

    private bool CanAddExpense()
    {
        return !string.IsNullOrWhiteSpace(Description)
            && SelectedCategory is not null
            && decimal.TryParse(Amount, out decimal betrag)
            && betrag > 0;
    }


    private async void AddExpense()
    {
        Expense newExpense = new()
        {
            Description = Description,
            Amount = decimal.Parse(Amount!),
            Date = Date,
            CategoryId = SelectedCategory!.Id
        };

        await _dbHelper.AddExpenseAsync(newExpense);

        Description = null;
        Amount = null;
        SelectedCategory = null;
        Date = DateTime.Now;

        await LoadAsync();
    }

    private bool CanDeleteExpense() => SelectedExpense is not null;

    private async void DeleteExpense()
    {
        if (SelectedExpense is null)
            return;

        var result = await DialogHost.Show(Application.Current.MainWindow.Resources["DeleteDialog"], "RootDialog");
        if (result is bool confirmed && confirmed)
        {
            await _dbHelper.DeleteExpenseAsync(SelectedExpense);
        }

        await LoadAsync();
    }

    private async Task LoadAsync()
    {
        var categories = await _dbHelper.GetCategoriesAsync();
        Categories.Clear();
        foreach (var category in categories) Categories.Add(category);

        var expenses = await _dbHelper.GetAllExpensesAsync();
        Expenses.Clear();
        foreach (var expense in expenses) Expenses.Add(expense);
        OnPropertyChanged(nameof(TotalAmount));
        OnPropertyChanged(nameof(Status));

    }
}