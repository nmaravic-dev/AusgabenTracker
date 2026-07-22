# Ausgaben-Tracker (WPF / MVVM)

Desktop-Anwendung zur Erfassung und Auswertung persönlicher Ausgaben.
Entwickelt mit **C# / WPF** nach dem **MVVM-Muster**, mit Anbindung an eine **SQL-Server-Datenbank** (Dapper) und **Dependency Injection**.

## Funktionen
- Ausgaben erfassen (Beschreibung, Betrag, Kategorie, Datum)
- Übersicht aller Ausgaben inkl. Kategorie (JOIN über Fremdschlüssel)
- Einträge löschen (mit Bestätigungsdialog)
- Automatische Berechnung der Gesamtsumme
- Limit-Statusanzeige: Die Gesamtsumme wird farblich markiert (grün / orange / rot), je nachdem ob ein definiertes Ausgabenlimit über- oder unterschritten wird

## Technologien
- **C# / .NET**, **WPF** (XAML)
- **MVVM** (Model – View – ViewModel)
- **Dapper** (Datenzugriff) + **Microsoft SQL Server**
- **Dependency Injection** (Microsoft.Extensions.DependencyInjection)
- **Material Design in XAML** (UI-Design, DialogHost)
- Data Binding, `INotifyPropertyChanged`, `ICommand` (RelayCommand), `ObservableCollection`, `IValueConverter`
  
## Hinweis zur Implementierung
`RelayCommand` (ICommand) und `ViewModelBase` (INotifyPropertyChanged) wurden bewusst manuell implementiert – zu Lernzwecken, um die zugrunde liegenden Mechanismen von MVVM (Commands und Property-Change-Benachrichtigung) vollständig zu verstehen. In einem produktiven Projekt lassen sich diese Klassen durch das **CommunityToolkit.Mvvm** ersetzen (`[ObservableProperty]`, `[RelayCommand]`).

## Projektstruktur
```
Models/       Expense.cs, Category.cs        -> Datenmodelle
Data/         DBHelper.cs                    -> Datenzugriff (Dapper)
ViewModels/   ViewModelBase, RelayCommand,
              LimitStatus, MainViewModel     -> Logik
Helpers/      StatusToBrushConverter.cs      -> Statusfarbe (IValueConverter)
Views/        MainWindow.xaml                -> Oberfläche (View)
App.xaml.cs   -> Einrichtung des DI-Containers
```

## Einrichtung / Setup
Die Anwendung nutzt eine lokale **SQL-Server-Datenbank**. Vor dem ersten Start muss diese angelegt werden.

**1. Datenbank und Tabellen erstellen**

Folgendes Skript im SQL Server Management Studio (SSMS) ausführen:

```sql
CREATE DATABASE Ausgaben_Tracker;
GO

USE Ausgaben_Tracker;
GO

CREATE TABLE Category (
    Id   INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(128) NOT NULL
);

CREATE TABLE Expense (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    Description NVARCHAR(200),
    Amount      DECIMAL(10,2) NOT NULL,
    Date        DATETIME2 NOT NULL,
    CategoryId  INT FOREIGN KEY REFERENCES Category(Id)
);

-- Beispiel-Kategorien
INSERT INTO Category (Name) VALUES ('Lebensmittel'), ('Transport'), ('Freizeit');
```

**2. Connection-String anpassen**

In `App.xaml.cs` den Servernamen an die eigene Umgebung anpassen:

```csharp
string connString = "Server=DEIN_SERVER;Database=Ausgaben_Tracker;Trusted_Connection=True;TrustServerCertificate=True;";
```
Die Anwendung nutzt **Windows-Authentifizierung** (kein Passwort im Connection-String).

**3. Starten**

Projekt in Visual Studio öffnen und mit **F5** starten.

## Credits
Euro icon created by [Freepik - Flaticon](https://www.flaticon.com/free-icons/euro)
