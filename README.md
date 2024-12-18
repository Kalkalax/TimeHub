# TimeHub

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