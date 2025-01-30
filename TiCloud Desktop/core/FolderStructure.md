## Struktura folderów

```plaintext
TiCloud/
│
├── App.xaml                 // Główny plik konfiguracji aplikacji WPF
├── App.xaml.cs              // Kod-behind dla App.xaml
├── MainWindow.xaml          // Główne okno aplikacji
├── MainWindow.xaml.cs       // Kod-behind dla MainWindow.xaml
├── NameInputWindow.xaml
├── NameInputWindow.xaml.cs
├── AppInitializer.cs 
├── IconManager.cs
├── FodyWeavers.xml
│
├── core/
│   ├── database/
│   │   ├── models/
│   │   |   ├── Project.cs           // Model projektu
│   │   |   ├── WorkPeriod.cs        // Model okresu pracy
|   |   |   └── WorkSession.cs       // Model sesji pracy
|   |   |
|   |   └── DatabaseManager.cs       // Zarządzanie bazą danych
|   |  
│   ├── security/
│   │   └── DatabaseEncryptionKeyManager.cs  // Zarządzanie kluczem szyfrowania bazy
│   │   
│   └── timers/
│       ├── RefreshTimer.cs          // Odświeżanie widoku czasu
│       └── WorkTimeTracker.cs       // Główna logika śledzenia czasu pracy    
│
└── resources/
    └── Images/
        └── Icon.png                 // Ikona aplikacji

```