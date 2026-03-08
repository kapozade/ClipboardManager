# ClipboardManager

## Data Flow
Every 500ms
└─ MacClipboardMonitor polls NSPasteboard.changeCount
    └─ Count changed?
        └─ Read string content from pasteboard
            └─ Fire ClipboardChanged event
                └─ HistoryManager.OnClipboardChanged()
                    ├─ Deduplicate
                    ├─ Prepend to list
                    ├─ Trim to 5
                    ├─ Persist via IHistoryRepository
                    └─ Fire HistoryChanged → HistoryViewModel updates

User presses Cmd+Shift+V
└─ MacHotkeyService fires HotkeyTriggered
    └─ App shows HistoryWindow (Avalonia floating window)
        └─ User selects item (or presses 1-5)
            └─ Write item to NSPasteboard
                └─ Simulate Cmd+V via CGEvent P/Invoke
                    └─ Item pasted into active app
                        └─ HistoryWindow closes
