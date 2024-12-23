<h1 align="center">TiCloud - Your personal productivity assistant</h1>

<p align="center">
  <a href="https://github.com/kamilkalarus">Kamil Kalarus</a>
</p>

OPIS APLIKACJI DO CZEGO S�U�Y I PO CO 




<p align="center">
  <img src="docs/ezgif-frame-001.jpg" width="45%" />
  <img src="docs/demo.gif" width="45%" />
</p>

## News
- ????-??-??: Pierwsza wersja TimeHub



## Features of TimeHub
- Light/dark mode toggle


## Roadmap
- Additional browser support
- Add more integrations

## Installation
Install my-project with npm

```bash
  npm install my-project
  cd my-project
```

## Documentation

[Development documentation](https://linktodocumentation)
- Gdzie znajduj� si� pliki bazy danych, kod szyfrowania,

## License

[MIT](https://choosealicense.com/licenses/mit/)

## Feedback

If you have any feedback, please reach out to us at fake@fake.com









<details>
  <summary>Class diagram</summary>
```
@startuml
' Klasa DatabaseManager
class DatabaseManager {
    - _realmInstance : Realm?
    - _realmConfiguration : RealmConfiguration?
    - DatabaseFilePath : string
    + DatabaseExists() : bool
    + InitializeDatabase() : void
    + GetRealmInstance() : Realm
    + SaveWorkSession(workPeriods: List<WorkPeriod>) : void
    + GetAllWorkSessions() : IQueryable<WorkSession>
}

' Klasa WorkSession
class WorkSession {
    - SessionID : ObjectId
    - SessionDate : DateTimeOffset
    - SessionWorkPeriods : IList<WorkPeriod>
    - SessionTime : long
    + CalculateSessionTime() : void
}

' Klasa WorkPeriod
class WorkPeriod {
    - PeriodStart : DateTime
    - PeriodEnd: DateTime
    - PeriodTime : long
    + CalculatePeriodTime(): void
}

' Klasa DatabaseEncryptionKeyManager
class DatabaseEncryptionKeyManager {
    - DatabaseEncryptionKeyFilePath : string
    + KeyExists() : bool
    + GenerateAndStoreKey(): void
    + GetKey() : byte[]
}
' Klasa RefreshTimer
class RefreshTimer {
    - _timer : DispatcherTimer
    - _workTimeTracker : WorkTimeTracker
    - _updateAction : Action<TimeSpan>
    + RefreshTimer(workTimeTracker: WorkTimeTracker, updateAction : Action<TimeSpan>)
    + Start() : void

}

' Relacje
DatabaseManager "1" --> "0..*" WorkSession : "manages"
WorkSession "1" *-- "0..*" WorkPeriod : "contains"
DatabaseManager "1" ..> "0..*" DatabaseEncryptionKeyManager : "depends on"
RefreshTimer "1" ..> "1" WorkTimeTracker : "uses"
@enduml

```